using System;
using Microsoft.EntityFrameworkCore;

namespace MultipleDatabases.Interface;

public interface IRepository<TEntity, TContext>
        where TEntity : class
        where TContext : DbContext
    {
        TEntity? GetById(int id);
        IEnumerable<TEntity> GetAll();
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
