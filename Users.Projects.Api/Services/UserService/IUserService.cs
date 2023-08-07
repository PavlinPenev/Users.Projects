using Users.Projects.Api.Services.Models;
using Users.Projects.Api.Services.Models.Users;

namespace Users.Projects.Api.Services.UserService
{
    public interface IUserService
    {
        /// <summary>
        /// Gets user info for displaying it in a chart
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <returns><see cref="ServiceResult{T}"/> of <see cref="ChartUser"/></returns>
        Task<ServiceResult<ChartUser>> GetAsync(Guid id);

        /// <summary>
        /// Gets a list of users(paginated)
        /// </summary>
        /// <param name="request"><see cref="UsersRequest"/></param>
        /// <returns><see cref="ServiceResult{T}"/> of <see cref="PagedList{T}"/> of <see cref="UserDto"/></returns>
        Task<ServiceResult<PagedList<UserDto>>> GetAllPagedAsync(UsersRequest request);

        /// <summary>
        /// Gets top ten users ordered by the most hours worked
        /// </summary>
        /// <param name="request"><see cref="ChartRequest"/></param>
        /// <returns><see cref="ServiceResult{T}"/> of <see cref="List{T}"/> of <see cref="ChartUser"/></returns>
        Task<ServiceResult<List<ChartUser>>> GetTopTenAsync(ChartRequest request);

        /// <summary>
        /// Deletes db table contents and repopulate them
        /// </summary>
        /// <returns><see cref="ServiceResult{T}"/> where if successful of <see langword="true"/> else <see langword="false"/></returns>
        Task<ServiceResult<bool>> RefreshDataAsync();
    }
}
