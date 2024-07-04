using DBContexts.Models;

using Microsoft.EntityFrameworkCore;

namespace DBContexts.DAL
{
    public class SecondDbContext : DbContext
    {
        public SecondDbContext(DbContextOptions<SecondDbContext> options) : base(options)
        {

        }

        public DbSet<Movie> Movies { get; set; }
    }
}