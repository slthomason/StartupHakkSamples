using DapperCSharp;

class Program
{
    static void Main(string[] args)
    {
        var connectionString = "Data Source=customers.db";

        var orderDataAccess = new OrderDataAccess(connectionString);

        orderDataAccess.CreateOrder(new Order { /* set properties */ });

        var order = orderDataAccess.GetOrderById(1);

        var userOrders = orderDataAccess.GetOrdersByUserId(1);

        var detailedOrders = orderDataAccess.GetUserOrdersWithDetails(1);
    }
}