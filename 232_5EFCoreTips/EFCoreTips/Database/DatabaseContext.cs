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
        modelBuilder.Entity<RawProductsResponse>().HasNoKey(); // Response registration for SQL RAW QUERY


        //QUERY FILTERS
        modelBuilder.Entity<Products>().HasQueryFilter(x => x.isActive == false);
        //QUERY FILTERS
    }
}
