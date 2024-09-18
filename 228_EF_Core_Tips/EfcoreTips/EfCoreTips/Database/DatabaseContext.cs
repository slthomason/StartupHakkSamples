using System;
using EfCoreTips.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace EfCoreTips.Database;

public class DatabaseContext : DbContext
{   
    public DatabaseContext(DbContextOptions options) :base (options){}

    public DbSet<Products> products {get;set;}
    public DbSet<Categories> categories {get;set;}
}
