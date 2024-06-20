using Grpc.Core;

namespace aspnet_grpc.Services
{
    public class ToDoService : aspnet_grpc.ToDoService.ToDoServiceBase
    {
        private static readonly List<ToDoItem> ToDoItems = new List<ToDoItem>();
        private static int _nextId = 1;

        public override Task<AddToDoReply> AddToDo(AddToDoRequest request, ServerCallContext context)
        {
            var newItem = new ToDoItem
            {
                Id = _nextId++,
                Description = request.Description,
                IsCompleted = false
            };
            ToDoItems.Add(newItem);
            return Task.FromResult(new AddToDoReply
            {
                Message = $"ToDo item '{request.Description}' added with ID {newItem.Id}."
            });
        }

        public override Task<GetToDosReply> GetToDos(GetToDosRequest request, ServerCallContext context)
        {
            var reply = new GetToDosReply();
            reply.Items.AddRange(ToDoItems);
            return Task.FromResult(reply);
        }

        public override Task<CompleteToDoReply> CompleteToDo(CompleteToDoRequest request, ServerCallContext context)
        {
            var item = ToDoItems.FirstOrDefault(x => x.Id == request.Id);
            if (item != null)
            {
                item.IsCompleted = true;
                return Task.FromResult(new CompleteToDoReply
                {
                    Message = $"ToDo item with ID {request.Id} marked as completed."
                });
            }

            return Task.FromResult(new CompleteToDoReply
            {
                Message = $"ToDo item with ID {request.Id} not found."
            });
        }
    }
}