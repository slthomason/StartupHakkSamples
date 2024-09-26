using System;
using Microsoft.EntityFrameworkCore;
using MultipleDatabases.Interface;

namespace MultipleDatabases.Service;

public class Repository<TEntity, TContext> : IRepository<TEntity, TContext>
        where TEntity : class
        where TContext : DbContext
    {
        protected TContext Context { get; private set; }

        public Repository(IDbContextProvider dbContextProvider)
        {
            Context = dbContextProvider.Get<TContext>() 
                ?? throw new InvalidOperationException($"DbContext of type {typeof(TEntity).Name} is not registered");
        }

        public TEntity? GetById(int id)
        {
            return Set().Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Set().ToList();
        }

        public void Add(TEntity entity)
        {
            Set().Add(entity);
        }

        public void Update(TEntity entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return Context.SaveChangesAsync(cancellationToken);
        }

        public void Delete(TEntity entity)
        {
            Set().Remove(entity);
        }

        protected DbSet<TEntity> Set()
        {
            return Context.Set<TEntity>();
        }
    }