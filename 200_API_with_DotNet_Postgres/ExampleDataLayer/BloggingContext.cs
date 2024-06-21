using ExampleDataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExampleDataLayer
{

    public class BloggingContext : DbContext
    {
        public DbSet<BlogEntity> Blogs { get; set; }
        public DbSet<PostEntity> Posts { get; set; }

        public BloggingContext()
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql($"Host=localhost;Port=5432;Username=apiuser;Password=apipassword;");
        }
    }
}
