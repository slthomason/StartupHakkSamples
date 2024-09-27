using System;
using Microsoft.EntityFrameworkCore;

namespace InMemoryCaching.Database;

public class DatabaseContext : DbContext
{
    public DbSet<Categories> categories {get;set;}
    public DatabaseContext(DbContextOptions options) : base(options){}
}
