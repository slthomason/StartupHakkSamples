using System;
using EfCoreTips.Database.Entities;
using EFCoreTips.Database.RawQueryResponses;
using Microsoft.EntityFrameworkCore;

namespace EfCoreTips.Database;

public class DatabaseContext : DbContext
{   
    public DatabaseContext(DbContextOptions options) :base (options){}

    public DbSet<Products> products {get;set;}
    public DbSet<Categories> categories {get;set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
         modelBuilder.Entity<Products>()
            .HasOne(p => p.categories)
            .WithMany(c => c.products)
            .HasForeignKey(p => p.catId);

        var burgers = new Categories()
        {
            catId = 1,
            catName = "Burgers"
        };
        var pizzas = new Categories()
        {
            catId = 2,
            catName = "Pizza"
        };
        var pasta = new Categories()
        {
            catId = 3,
            catName = "Pasta"
        };
        var drinks = new Categories()
        {
            catId = 4,
            catName = "Drinks"
        };

        modelBuilder.Entity<Categories>().HasData(burgers);
        modelBuilder.Entity<Categories>().HasData(pizzas);
        modelBuilder.Entity<Categories>().HasData(pasta);
        modelBuilder.Entity<Categories>().HasData(drinks);

        for(int i = 0; i < 90; i++){
            modelBuilder.Entity<Products>().HasData(new Products()
            {
                productId = i+1,
                productName = $"Product {i}",
                catId = 2,
                isActive = i % 2 == 0 ? true : false,
            });
        }
    
        modelBuilder.Entity<RawProductsResponse>().HasNoKey(); // Response registration for SQL RAW QUERY


        //QUERY FILTERS
        modelBuilder.Entity<Products>().HasQueryFilter(x => x.isActive == false);
        //QUERY FILTERS
    }
}

