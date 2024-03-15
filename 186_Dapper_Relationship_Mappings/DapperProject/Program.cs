using Dapper;
using DapperProject;
using Microsoft.Data.SqlClient;

using var connection = new SqlConnection();

var ordersDictionary = new Dictionary<long, Order>();
var sql = "SELECT o.Id AS OrderId,li.Id AS LineItemId, li.OrderId," +
    " li.Price, li.Currency, li.Quantity FROM Orders o JOIN LineItems " +
    "li ON li.OrderId = o.Id WHERE o.Id = @OrderId";

await connection.QueryAsync<Order, LineItem, Order>(
    sql,
    (order, lineItem) =>
    {
        if (ordersDictionary.TryGetValue(order.OrderId, out var existingOrder))
        {
            order = existingOrder;
        }
        else
        {
            ordersDictionary.Add(order.OrderId, order);
        }

        order.LineItems.Add(lineItem);
        return order;
    },
    new { OrderId = orderId },
    splitOn: "LineItemId");

var mappedOrder = ordersDictionary[orderId];