using Microsoft.EntityFrameworkCore;

namespace DBContexts.Contracts
{
    public interface IDbContextProvider
    {
        T Get<T>()
            where T : DbContext;
    }
}