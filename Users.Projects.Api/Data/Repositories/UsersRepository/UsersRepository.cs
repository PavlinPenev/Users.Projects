using LinqKit;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;
using Users.Projects.Api.Data.Models;
using Users.Projects.Api.Services.Models;
using Users.Projects.Api.Services.Models.Users;
using static Users.Projects.Api.Common.Constants.ErrorMessages;

namespace Users.Projects.Api.Data.Repositories.UsersRepository
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IRepository<User> repository;

        public UsersRepository(IRepository<User> repository)
        {
            this.repository = repository;
        }

        public async Task<User> GetAsync(Guid id)
        {
            var user = await this.repository.GetAll().Include(x => x.TimeLogs).FirstOrDefaultAsync(x => x.Id == id);

            if (user == null) 
            {
                throw new NullReferenceException(USER_NOT_FOUND);
            }

            return user;
        }

        public async Task<(List<User>, int)> GetAllPagedAsync(UsersRequest request)
        {
            var users = repository.GetAll();

            int itemsCount;

            var result = FilterUsers(users, request, out itemsCount);

            return (result, itemsCount);
        }

        public async Task<List<User>> GetTopTenAsync(ChartRequest request)
        {
            return await repository
                .GetAll()
                .Include(x => x.TimeLogs)
                .OrderByDescending(x => x.TimeLogs.Sum(y => y.HoursWorked))
                .Take(request.Take)
                .ToListAsync();
        }

        public async Task<bool> RefreshDataAsync()
        {
            await repository.ExecuteStoredProcedure("usp_ClearAndPopulateData");

            return true;
        }

        private List<User> FilterUsers(IQueryable<User> users, UsersRequest request, out int totalItemsCount)
        {
            Expression<Func<User, bool>> filterTemplate = x => true;

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                filterTemplate = filterTemplate.And(x =>
                    x.FirstName.Contains(request.SearchTerm)
                    || x.LastName.Contains(request.SearchTerm));
            }

            if (request.DateAddedFrom.HasValue || request.DateAddedTo.HasValue)
            {
                if (request.DateAddedFrom.HasValue && !request.DateAddedTo.HasValue)
                {
                    filterTemplate = filterTemplate.And(x => x.DateAdded >= request.DateAddedFrom);
                }
                else if (!request.DateAddedFrom.HasValue && request.DateAddedTo.HasValue)
                {
                    filterTemplate = filterTemplate.And(x => x.DateAdded <= request.DateAddedTo);
                }
                else
                {
                    filterTemplate = filterTemplate.And(x =>
                        x.DateAdded >= request.DateAddedFrom && x.DateAdded <= request.DateAddedTo);
                }
            }

            users = users.Where(filterTemplate.Compile()).AsQueryable();

            if (!string.IsNullOrEmpty(request.OrderBy))
            {
                var flags = BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public;

                var orderByPropertyInfo = typeof(User).GetProperty(request.OrderBy, flags);

                if (!request.IsDescending)
                {
                    users = users
                        .OrderBy(x => orderByPropertyInfo.GetValue(x));
                }
                else
                {
                    users = users
                        .OrderByDescending(x => orderByPropertyInfo.GetValue(x));
                }
            }

            totalItemsCount = users.Count();

            return users.Skip(request.Skip).Take(request.Take).ToList();
        }
    }
}
