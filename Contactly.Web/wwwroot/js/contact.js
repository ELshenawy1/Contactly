function getContacts() {
	$.ajax({
		url: "/Contact/IndexContactPartial",
		contentType: "application/json",
		data: contactParams,
		success: function (result) {
			$("#ContactTable").html(result)
		}
	})
}

getContacts();

const connection = new signalR.HubConnectionBuilder()
	.withUrl(contactlyAPI + "/ContactHub")
	.build();

//start Connection
connection.start();

connection.on("NewContact", function () {
	getContacts();
})

connection.on("DeleteContact", function (cid) {
	var element = document.getElementById("R_" + cid);
	if (element) {
		if (selectedContactId != 0) {
			element.remove();
		} else {
			getContacts();
		}
	}
})
connection.on("LockContactForUpdating", function (cid) {
	var updateBtn = document.getElementById("updateBtn_" + cid);
	var removeBtn = document.getElementById("removeBtn_" + cid);
	if (updateBtn) updateBtn.style.display = "none";
	if (removeBtn) removeBtn.style.display = "none";
})

connection.on("UnlockContact", function (cid) {
	var updateBtn = document.getElementById("updateBtn_" + cid);
	var removeBtn = document.getElementById("removeBtn_" + cid);
	if (updateBtn) updateBtn.style.display = "inline-block";
	if (removeBtn) removeBtn.style.display = "inline-block";
})

connection.on("ContactUpdate", function (pageIndex, updatedContact) {
	if (pageIndex !== contactParams.PageIndex) reuturn;

	let spans = document.getElementsByClassName("s_" + updatedContact.id);
	let inputs = document.getElementsByClassName("i_" + updatedContact.id);
	spans[0].innerHTML = updatedContact.name;
	spans[1].innerHTML = updatedContact.phone;
	spans[2].innerHTML = updatedContact.address;
	spans[3].innerHTML = updatedContact.notes;
	inputs[0].value = updatedContact.name;
	inputs[1].value = updatedContact.phone;
	inputs[2].value = updatedContact.address;
	inputs[3].value = updatedContact.notes;
	document.getElementById("updateBtn_" + updatedContact.id).style.display = "inline-block";
	document.getElementById("removeBtn_" + updatedContact.id).style.display = "inline-block";
})

function ensureEditCompletion() {
	if (selectedContactId != 0) {
		Swal.fire({
			title: "Finish Editing First",
			text: "You are already editing a contact. Please save or cancel before editing another one.",
			icon: "warning",
			confirmButtonText: "OK"
		});
		return true;
	}
	return false;
}
function Reset() {
	if (ensureEditCompletion()) return;
	contactParams.Search = '';
	document.getElementById("SearchInput").value = "";
	Search();
}

function Search() {
	if (ensureEditCompletion()) return;
	contactParams.Search = document.getElementById("SearchInput")?.value;
	contactParams.PageIndex = 1;
	getContacts();
}
function loadPage(pageIndex) {
	if (ensureEditCompletion()) return;
	if (contactParams.PageIndex !== pageIndex) {
		contactParams.PageIndex = pageIndex;
		getContacts();
	}
}

function ValidateUpdateInputs(val) {
	let inputs = document.getElementsByClassName("i_" + val);
	var isValid = true;
	var phoneRegex = /^\d{10,15}$/;
	let phoneValue = inputs[1]?.value.replace(/\D/g, '');

	if (!phoneRegex.test(phoneValue)) {
		isValid = false;
		$("#phoneError_" + val).show();
	} else {
		$("#phoneError_" + val).hide();
	}

	if (inputs[0]?.value.length > 50 || inputs[0]?.value.length < 3) {
		isValid = false;
		$("#nameError_" + val).show();
	} else {
		$("#nameError_" + val).hide();
	}

	if (inputs[2]?.value.length > 200 || inputs[2]?.value.length <= 1) {
		isValid = false;
		$("#addressError_" + val).show();
	} else {
		$("#addressError_" + val).hide();
	}

	if (inputs[3]?.value.length > 500 || inputs[3]?.value.length <= 1) {
		isValid = false;
		$("#notesError_" + val).show();
	} else {
		$("#notesError_" + val).hide();
	}
	return isValid;
}

function UpdateContactValues() {
	let spans = document.getElementsByClassName("s_" + selectedContactId);
	let inputs = document.getElementsByClassName("i_" + selectedContactId);
	spans[0].innerHTML = inputs[0]?.value;
	spans[1].innerHTML = inputs[1]?.value;
	spans[2].innerHTML = inputs[2]?.value;
	spans[3].innerHTML = inputs[3]?.value;

	$(".s_" + selectedContactId).show();
	$(".i_" + selectedContactId).hide();
	$("#updateBtn_" + selectedContactId).show();
	$("#removeBtn_" + selectedContactId).show();
	$("#saveBtn_" + selectedContactId).hide();
	$("#cancelBtn_" + selectedContactId).hide();

	selectedContactId = 0;
}
function Save() {
	let inputs = document.getElementsByClassName("i_" + selectedContactId);
	if (!ValidateUpdateInputs(selectedContactId))
		return;


	var updateDto = {
		ID: parseInt(selectedContactId),
		Name: inputs[0]?.value,
		Phone: inputs[1]?.value,
		Address: inputs[2]?.value,
		Notes: inputs[3]?.value,
		PageIndex: contactParams.PageIndex,
	}

	$.ajax({
		url: contactlyAPI + "/api/ContactApi/" + selectedContactId,
		type: "PUT",
		contentType: "application/json",
		headers: {
			"Authorization": "Bearer " + authToken
		},
		data: JSON.stringify(updateDto),
		success: function (result) {
			UpdateContactValues()
		},
		error: function (result) {
		}
	});

	connection.invoke("UnlockContact", selectedContactId)
}
function UpdateOn(val) {
	if (ensureEditCompletion()) return;
	connection.invoke("LockContactForUpdating", val)



	selectedContactId = val;
	$(".s_" + val).hide();
	$(".i_" + val).show();
	$("#updateBtn_" + val).hide();
	$("#removeBtn_" + val).hide();
	$("#saveBtn_" + val).show();
	$("#cancelBtn_" + val).show();
}
function UpdateOff() {
	connection.invoke("UnlockContact", selectedContactId)
	$(".s_" + selectedContactId).show();
	$(".i_" + selectedContactId).hide();
	$("#updateBtn_" + selectedContactId).show();
	$("#removeBtn_" + selectedContactId).show();
	$("#saveBtn_" + selectedContactId).hide();
	$("#cancelBtn_" + selectedContactId).hide();


	$("#nameError_" + selectedContactId).hide();
	$("#phoneError_" + selectedContactId).hide();
	$("#notesError_" + selectedContactId).hide();
	$("#addressError_" + selectedContactId).hide();
	selectedContactId = 0;
}
function Delete(val) {
	if (ensureEditCompletion()) return;
	Swal.fire({
		title: "Are you sure?",
		text: "You won't be able to revert this!",
		icon: "warning",
		showCancelButton: true,
		confirmButtonColor: "#3085d6",
		cancelButtonColor: "#d33",
		confirmButtonText: "Yes, delete it!"
	}).then((result) => {
		if (result.isConfirmed) {
			$.ajax({
				url: contactlyAPI + "/api/ContactApi/" + val,
				type: "DELETE",
				headers: {
					"Authorization": "Bearer " + authToken
				},
				success: function (result) {

					Swal.fire({
						title: "Deleted!",
						text: "Your contact has been deleted.",
						timer: 2000,
						timerProgressBar: true,
						icon: "success"
					});
				},
				error: function () {
					Swal.fire({
						position: 'top-end',
						icon: 'error',
						title: 'Error deleting contact',
						showConfirmButton: false,
						timer: 3000,
						timerProgressBar: true
					})
				}
			});
		}
	});
}