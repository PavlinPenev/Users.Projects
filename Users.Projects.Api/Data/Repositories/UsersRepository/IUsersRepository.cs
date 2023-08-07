using Users.Projects.Api.Data.Models;
using Users.Projects.Api.Services.Models;
using Users.Projects.Api.Services.Models.Users;

namespace Users.Projects.Api.Data.Repositories.UsersRepository
{
    public interface IUsersRepository
    {
        /// <summary>
        /// Gets user from the db with a given Id
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <returns><see cref="User"/></returns>
        Task<User> GetAsync(Guid id);

        /// <summary>
        /// Gets all the users paginated
        /// </summary>
        /// <param name="request"><see cref="UsersRequest"/></param>
        /// <returns><see cref="List{T}"/> of <see cref="User"/> and <see langword="int"/></returns>
        Task<(List<User>, int)> GetAllPagedAsync(UsersRequest request);

        /// <summary>
        /// Gets a list of top ten users ordered by most hours worked
        /// </summary>
        /// <param name="request"><see cref="ChartRequest"/></param>
        /// <returns><see cref="List{T}"/> of <see cref="User"/></returns>
        Task<List<User>> GetTopTenAsync(ChartRequest request);

        /// <summary>
        /// Deletes db table contents and repopulate them
        /// </summary>
        /// <returns>If successful <see langword="true"/> else <see langword="false"/></returns>
        Task<bool> RefreshDataAsync();
    }
}
