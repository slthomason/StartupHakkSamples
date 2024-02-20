using Microsoft.AspNetCore.SignalR;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowAnyOrigin", p => p
        .WithOrigins("null") // Origin of an html file opened in a browser
        .AllowAnyHeader()
        .AllowCredentials()); 
});
builder.Services.AddSignalR();

var app = builder.Build();
app.UseCors("AllowAnyOrigin");
app.MapHub<ChatHub>("/chatHub");
app.Run();

public class ChatHub : Hub
{
    public async Task SendMessage(string message)
    {
        await Clients.All.SendAsync("ReceiveMessage", message);
    }
}

public record User(string Name, string Room);
public record Message(string User, string Text);

public class ChatHub : Hub
{
    private static ConcurrentDictionary<string, User> _users = new();

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        if (_users.TryGetValue(Context.ConnectionId, out var user))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, user.Room);
            await Clients.Group(user.Room).SendAsync("UserLeft", user.Name);
        }
    }
    
    public async Task JoinRoom(string userName, string roomName)
    {
        _users.TryAdd(Context.ConnectionId, new User(userName, roomName));
        await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        await Clients.Group(roomName).SendAsync("UserJoined", userName);
    }
    
    public async Task SendMessageToRoom(string roomName, string content)
    {
        var message = new Message(_users[Context.ConnectionId].Name, content);
        await Clients.Group(roomName).SendAsync("ReceiveMessage", message);
    }
}