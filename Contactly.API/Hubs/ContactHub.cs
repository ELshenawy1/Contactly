using Contactly.Core.DTOs;
using Contactly.Core.Entities;
using Contactly.Core.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
namespace Contactly.API.Hubs
{
    public class ContactHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        public ContactHub(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task LockContactForUpdating(int contactId)
        {
            var userConnection = await _unitOfWork.Repository<ConnectionTracker>().GetByIdAsync(Context.ConnectionId);
            userConnection.ContactId = contactId;
            _unitOfWork.Repository<ConnectionTracker>().Update(userConnection);
            await _unitOfWork.Complete();

            await Clients.All.SendAsync("LockContactForUpdating", contactId);
        }
        public async Task UnlockContact(int contactId)
        {
            var userConnection = await _unitOfWork.Repository<ConnectionTracker>().GetByIdAsync(Context.ConnectionId);
            userConnection.ContactId = null;
            _unitOfWork.Repository<ConnectionTracker>().Update(userConnection);
            await _unitOfWork.Complete();

            await Clients.All.SendAsync("UnlockContact", contactId);
        }


        public override async Task OnConnectedAsync()
        {
            _unitOfWork.Repository<ConnectionTracker>().Add(new ConnectionTracker()
            {
                ConnectionId = Context.ConnectionId
            });
            await _unitOfWork.Complete();
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var userConnection = await _unitOfWork.Repository<ConnectionTracker>().GetByIdAsync(Context.ConnectionId);
            if (userConnection != null)
            {
                _unitOfWork.Repository<ConnectionTracker>().Delete(userConnection);
                await _unitOfWork.Complete();
            }

            await base.OnDisconnectedAsync(exception);
        }

    }
}
