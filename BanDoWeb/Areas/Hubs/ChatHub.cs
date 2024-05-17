using BanDoWeb.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Project.DataAccess.Repository.IRepository;

namespace BanDoWeb.Areas.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChatHub(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;

        }
        public override Task OnConnectedAsync()
        {
            if (Context.User.Identity.Name != null)
            {
                Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);
                return base.OnConnectedAsync();
            }
            return base.OnConnectedAsync();
        }
        public async Task SendMessage(string sender, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", sender, message);
        }

        public Task SendMessageToGroup(string sender, string receiver, string message)
        {
            return Clients.OthersInGroup(receiver).SendAsync("ReceiveMessage", sender, message);
        }
    }
}
