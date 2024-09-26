using System;
using Microsoft.EntityFrameworkCore;
using MultipleDatabases.Database.Entities;

namespace MultipleDatabases.Database;

public class RestaurantContext : DbContext
{
    public DbSet<Categories> categories {get;set;}
    public RestaurantContext(DbContextOptions<RestaurantContext> options) :base(options){}
}
