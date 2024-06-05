using Dapper;
using EFCoreVsDapperDemo.Models;
using Microsoft.Data.SqlClient;

namespace EFCoreVsDapperDemo.DataAccess
{
    public class ProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM Products";
                return await connection.QueryAsync<Product>(sql);
            }
        }
    }
}
