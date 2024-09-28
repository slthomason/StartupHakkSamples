using System;
using Microsoft.EntityFrameworkCore;

namespace InMemoryCaching.Database;

public class DatabaseContext : DbContext
{
    public DbSet<Categories> categories {get;set;}
    public DatabaseContext(DbContextOptions options) : base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        for(int i = 0; i < 10; i++)
        {
            modelBuilder.Entity<Categories>().HasData(new Categories(){
                catId = i + 1,
                catName = $"Burgers {i}"
            });
        }
    }
}
