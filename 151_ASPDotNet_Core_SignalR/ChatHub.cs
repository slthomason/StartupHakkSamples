using Microsoft.AspNetCore.SignalR;

namespace SignalRChatBlazor.Server.Hubs;

public class ChatHub : Hub
{
    private static Dictionary<string, string> _users = new Dictionary<string, string>();

    public override async Task OnConnectedAsync()
    {
        var username = Context.GetHttpContext()?.Request.Query["username"] ?? string.Empty;
        _users.Add(Context.ConnectionId,username);

        await SendMessage(string.Empty, $"{username} has joined the chat !!");
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (_users.ContainsKey(Context.ConnectionId))
        {
            await SendMessage(string.Empty, $"{_users[Context.ConnectionId]} has left the chat !!");
        }
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(string user, string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", user, message);
    }
    
}