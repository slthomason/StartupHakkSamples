using ExampleDataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExampleDataLayer
{

    public class BloggingContext : DbContext
    {
        public BloggingContext(DbContextOptions<BloggingContext> options)
           : base(options)
        {
        }
        public DbSet<BlogEntity> Blogs { get; set; }
        public DbSet<PostEntity> Posts { get; set; }

       

       
    }
}
