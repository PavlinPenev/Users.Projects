using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data.SqlClient;

namespace Users.Projects.Api.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        public Repository(UsersProjectsDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
            DbSet = this.Context.Set<TEntity>();
        }

        protected UsersProjectsDbContext Context { get; set; }

        protected DbSet<TEntity> DbSet { get; set; }

        public virtual Task AddAsync(TEntity entity) => DbSet.AddAsync(entity).AsTask();

        public void Delete(TEntity entity) => DbSet.Remove(entity);

        public virtual IQueryable<TEntity> GetAll() => DbSet;

        public virtual IAsyncEnumerable<TEntity> GetAsAsyncEnumerable() => DbSet.AsAsyncEnumerable();

        public Task<int> SaveChangesAsync() => Context.SaveChangesAsync();

        public void Update(TEntity entity)
        {
            var entry = Context.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        public async Task ExecuteStoredProcedure(string storedProcedureName, params SqlParameter[] parameters)
        {
            var sqlCommand = $"EXEC {storedProcedureName} {string.Join(", ", parameters.Select(p => p.ParameterName))}";

            await Context.Database.ExecuteSqlRawAsync(sqlCommand, parameters);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Context?.Dispose();
            }
        }
    }
}
