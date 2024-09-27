using System;
using Microsoft.EntityFrameworkCore;

namespace Hangfire.Models;

public class DatabaseContext : DbContext
{
    public DbSet<Categories> categories {get;set;}
    public DatabaseContext(DbContextOptions options) : base(options){}
}
