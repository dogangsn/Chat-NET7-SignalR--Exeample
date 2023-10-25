using ChatSignalR.API.Model;
using ChatSignalR.API.Services;
using Microsoft.AspNetCore.SignalR;

namespace ChatSignalR.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly ChatService _chatservice;
        public ChatHub(ChatService chatService)
        {
            _chatservice = chatService;
        }

        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, "Chat");
            await Clients.Caller.SendAsync("UserConnected");
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, "Chat");
            var user = _chatservice.GetUserByConnectionId(Context.ConnectionId);
            _chatservice.RemoveUsersFromList(user);
            var onlineUser = _chatservice.GetOnLineUsers();
            await Clients.Groups("Chat").SendAsync("OnLineUsers", onlineUser);
            await DisplayOnLineUsers();

            await base.OnDisconnectedAsync(exception);
        }

        public async Task AddUserConnectionId(string name)
        {
            _chatservice.AddUserConnectinId(name, Context.ConnectionId);
            var onlineUser = _chatservice.GetOnLineUsers();
            await Clients.Groups("Chat").SendAsync("OnLineUsers", onlineUser);
        }

        public async Task ReceiveMessage(MessageDto message)
        {
            await Clients.Groups("Chat").SendAsync("NewMessage", message);
        }

        public async Task CreatePrivateChat(MessageDto message)
        {
            string privateGroupName = GetPrivateGroupName(message.From, message.To);
            await Groups.AddToGroupAsync(Context.ConnectionId, privateGroupName);
            var toConnectionId = _chatservice.GetConnectionIdUserBy(message.To);

            await Clients.Client(toConnectionId).SendAsync("OpenPrivateChat", message);
        }

        public async Task RecivePrivateMessage(MessageDto message)
        {
            string privateGroupName = GetPrivateGroupName(message.From, message.To);
            await Clients.Groups(privateGroupName).SendAsync("NewPrivateMessage", message);
        }

        public async Task RemovePrivateChat(string from, string to)
        {
            string privateGroupName = GetPrivateGroupName(from, to);
            await Clients.Groups(privateGroupName).SendAsync("ClosePrivateMessage");

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, privateGroupName);
            var toConnectionId = _chatservice.GetConnectionIdUserBy(to);
            await Groups.RemoveFromGroupAsync(toConnectionId, privateGroupName);
        }

        private async Task DisplayOnLineUsers()
        {
            var onlineUsers = _chatservice.GetOnLineUsers();
            await Clients.Groups("Chat").SendAsync("OnLineUsers", onlineUsers);
        }

        private string GetPrivateGroupName(string from, string to)
        {
            var stringCompare = string.CompareOrdinal(from, to) < 0;
            return stringCompare ? $"{from}-{to}" :  $"{to}-{from}";
        }

    }
}
