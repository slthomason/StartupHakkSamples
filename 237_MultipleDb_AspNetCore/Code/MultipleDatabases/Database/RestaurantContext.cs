using System;
using Microsoft.EntityFrameworkCore;
using MultipleDatabases.Database.Entities;

namespace MultipleDatabases.Database;

public class RestaurantContext : DbContext
{
    public DbSet<Categories> categories {get;set;}
    public RestaurantContext(DbContextOptions<RestaurantContext> options) :base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categories>().HasData(new Categories()
        {
            catId = 1,
            catName = "Burgers",
        });
    }
}
