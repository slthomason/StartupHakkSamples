using DBContexts.Contracts;

using Microsoft.EntityFrameworkCore;

namespace DBContexts.DAL
{
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
}