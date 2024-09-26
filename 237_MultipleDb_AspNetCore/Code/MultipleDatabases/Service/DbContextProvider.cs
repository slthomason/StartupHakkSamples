using System;
using Microsoft.EntityFrameworkCore;
using MultipleDatabases.Interface;

namespace MultipleDatabases.Service;

public class DbContextProvider : IDbContextProvider
{
    protected IServiceProvider ServiceProvider { get; }

        public DbContextProvider(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public T Get<T>() 
            where T : DbContext
        {
            return ServiceProvider.GetRequiredService<T>();
        }
}
