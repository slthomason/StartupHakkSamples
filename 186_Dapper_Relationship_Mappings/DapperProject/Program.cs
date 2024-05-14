using Dapper;
using DapperProject;
using Microsoft.Data.SqlClient;

var sql =
    "SELECT o.Id AS OrderId," +
    "li.Id AS LineItemId, li.OrderId," +
    "li.Price, li.Currency, li.Quantity " +
    "FROM Orders o JOIN LineItems " +
    "li ON li.OrderId = o.Id WHERE o.Id = @OrderId";

using var connection = new SqlConnection();

var ordersDictionary = new Dictionary<long, Order>();

var orderId = 123;

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


/*
//Simple Mapping
var sql =
    "SELECT Id AS LineItemId, OrderId, Price, Currency, Quantity" +
    "FROM LineItems" +
    "WHERE OrderId = @OrderId";
 * 
 * 
//Dapper One To One Relationship Mapping
var sql =
    "SELECT li.Id AS LineItemId, li.OrderId, li.Price, li.Currency, li.Quantity, p.Id AS ProductId, p.Name" +
    "FROM LineItems li" +
    "JOIN Products p ON p.Id = li.ProductId" +
    "WHERE li.OrderId = @OrderId";
 * 
 * 
//Dapper One To Many Relationship Mapping
var sql = 
    "SELECT o.Id AS OrderId," +
    "li.Id AS LineItemId, li.OrderId," +
    "li.Price, li.Currency, li.Quantity " +
    "FROM Orders o JOIN LineItems " +
    "li ON li.OrderId = o.Id WHERE o.Id = @OrderId";
 * 
 * 
 */ 