using System.Data.SqlClient;
using Users.Projects.Api.Data.Models;

namespace Users.Projects.Api.Data.Repositories
{
    public interface IRepository<TEntity> : IDisposable
        where TEntity : class
    {
        IQueryable<TEntity> GetAll();

        IAsyncEnumerable<TEntity> GetAsAsyncEnumerable();

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        Task ExecuteStoredProcedure(string storedProcedureName, params SqlParameter[] parameters);

        void Delete(TEntity entity);

        Task<int> SaveChangesAsync();
    }
}
