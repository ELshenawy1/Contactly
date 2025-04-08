using Contactly.Web.Models;
using Contactly.Web.Models.DTOs;
using Contactly.Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Contactly.Web.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;
        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }
        [Authorize]
        public async Task<IActionResult> IndexContact()
        {
            List<ContactDTO> list = new();
            var response = await _contactService.GetAllAsync<APIResponse>(new ContactSpecParams());
            if (response != null && response.IsSuccess)
            {
                response.Result = JsonConvert.DeserializeObject<List<ContactDTO>>(Convert.ToString(response.Result));
            }
            return View(response);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> IndexContactPartial(ContactSpecParams contactParams)
        {
            List<ContactDTO> list = new();
            var response = await _contactService.GetAllAsync<APIResponse>(new ContactSpecParams() { PageIndex = contactParams.PageIndex , Search = contactParams.Search});
            if (response != null && response.IsSuccess)
            {
                response.Result = JsonConvert.DeserializeObject<List<ContactDTO>>(Convert.ToString(response.Result));
            }
            return PartialView("_ContactsTable",response);
        }
        [Authorize]
        public async Task<IActionResult> CreateContact()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateContact(ContactCreateDTO model)
        {
            if (ModelState.IsValid)
            {
                var response = await _contactService.CreateAsync<APIResponse>(model);
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Contact Created Successfully";
                    return RedirectToAction(nameof(IndexContact));
                }
                else
                {
                    TempData["error"] = (response.ErrorMessages != null && response.ErrorMessages.Count > 0)
                        ? response.ErrorMessages[0]
                        : "Error Encountered";
                }

            }
            TempData["error"] = "Error encountered!";
            return View(model);
        }
        [Authorize]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var response = await _contactService.DeleteAsync<APIResponse>(id);

            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Contact Deleted Successfully";
                return Json(new { success = true, message = "Contact Deleted Successfully" });
            }
            TempData["error"] = "Error encountered!";
            return Json(new { success = false, message = "Error encountered while deleting the contact!" });
        }




    }
}
