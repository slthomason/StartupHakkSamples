using System;
using Microsoft.EntityFrameworkCore;
using MultipleDatabases.Database.Entities;

namespace MultipleDatabases.Database;

public class UsersContext : DbContext
{
    public DbSet<Users> users {get;set;}
    public UsersContext(DbContextOptions<UsersContext> options) :base(options){}
}
