using aspnet_grpc; // Add reference to the grpc project
using Grpc.Net.Client;

// Configure gRPC channel
using var channel = GrpcChannel.ForAddress("https://localhost:7047");
var client = new ToDoService.ToDoServiceClient(channel);

// Add new item ToDo
var addReply = await client.AddToDoAsync(new AddToDoRequest { Description = "Learn gRPC" });
Console.WriteLine(addReply.Message);

// Get ToDo items
var getReply = await client.GetToDosAsync(new GetToDosRequest());
foreach (var item in getReply.Items)
{
    Console.WriteLine($"{item.Id}: {item.Description} (Completed: {item.IsCompleted})");
}

// Complete ToDo item
var completeReply = await client.CompleteToDoAsync(new CompleteToDoRequest { Id = 1 });
Console.WriteLine(completeReply.Message);

// Get ToDo items again
getReply = await client.GetToDosAsync(new GetToDosRequest());
foreach (var item in getReply.Items)
{
    Console.WriteLine($"{item.Id}: {item.Description} (Completed: {item.IsCompleted})");
}
Console.ReadLine();