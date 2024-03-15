using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperCSharp
{
    public class OrderDataAccess
    {
        private readonly string _connectionString;
        public OrderDataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void CreateOrder(Order order)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "INSERT INTO Orders (UserId, ProductId, OrderDate, OrderAmount) VALUES (@UserId, @ProductId, @OrderDate, @OrderAmount)";
                connection.Execute(sql, order);
            }
        }

        public Order GetOrderById(int orderId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.QueryFirstOrDefault<Order>("SELECT * FROM Orders WHERE Id = @Id", new { Id = orderId });
            }
        }

        public IEnumerable<Order> GetOrdersByUserId(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<Order>("SELECT * FROM Orders WHERE UserId = @UserId", new { UserId = userId });
            }
        }

        public class OrderDetail
        {
            public int OrderId { get; set; }
            public DateTime OrderDate { get; set; }
            public decimal OrderAmount { get; set; }
            public string ProductName { get; set; }
            public string ProductDescription { get; set; }
            public string ProductSpecifications { get; set; }
            public string ProductImageLink { get; set; }
            public string UserFirstName { get; set; }
            public string UserLastName { get; set; }
            public string UserEmail { get; set; }
        }

        public IEnumerable<OrderDetail> GetUserOrdersWithDetails(int userId)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                return connection.Query<OrderDetail>("GetUserOrdersWithDetails", new { UserId = userId },
                                                     commandType: CommandType.StoredProcedure);
            }
        }


    }
}
