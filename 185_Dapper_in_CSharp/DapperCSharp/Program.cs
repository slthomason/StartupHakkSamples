using DapperCSharp;

class Program
{
    static void Main(string[] args)
    {
        var connectionString = "Data Source=customers.db";

        var orderDataAccess = new OrderDataAccess(connectionString);

        orderDataAccess.CreateOrder(new Order {
            Id = 1, 
            UserId = 123, 
            ProductId = 321, 
            OrderDate = DateTime.Now,
            OrderAmount = 19.99m
        });

        var order = orderDataAccess.GetOrderById(1);

        var userOrders = orderDataAccess.GetOrdersByUserId(123);

        var detailedOrders = orderDataAccess.GetUserOrdersWithDetails(1);
    }
}