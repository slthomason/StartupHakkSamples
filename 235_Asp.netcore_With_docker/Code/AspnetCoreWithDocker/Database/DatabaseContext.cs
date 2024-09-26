using System;
using Microsoft.EntityFrameworkCore;

namespace AspnetCoreWithDocker.Database;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions options) : base(options){}
}
