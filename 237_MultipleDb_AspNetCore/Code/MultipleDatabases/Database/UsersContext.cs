using System;
using Microsoft.EntityFrameworkCore;
using MultipleDatabases.Database.Entities;

namespace MultipleDatabases.Database;

public class UsersContext : DbContext
{
    public DbSet<Users> users {get;set;}
    public UsersContext(DbContextOptions<UsersContext> options) :base(options){}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Users>().HasData(new Users()
        {
            userId = 1,
            customerName = "Jon Doe",
            phoneNumber = "+168025600884",
        });
    }
}
