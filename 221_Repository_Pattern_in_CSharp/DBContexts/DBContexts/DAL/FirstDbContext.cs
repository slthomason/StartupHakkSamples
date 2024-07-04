using DBContexts.Models;

using Microsoft.EntityFrameworkCore;

namespace DBContexts.DAL
{
    public class FirstDbContext : DbContext
    {
        public FirstDbContext(DbContextOptions<FirstDbContext> options) : base(options)
        {
            
        }

        public DbSet<Book> Books { get; set; }
    }
}