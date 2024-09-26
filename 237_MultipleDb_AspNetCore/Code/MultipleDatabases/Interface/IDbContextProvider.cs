using System;
using Microsoft.EntityFrameworkCore;

namespace MultipleDatabases.Interface;

public interface IDbContextProvider
{
    T Get<T>() where T : DbContext;
}
