using Users.Projects.Api.Services.Models;
using Users.Projects.Api.Services.Models.Projects;

namespace Users.Projects.Api.Services.ProjectService
{
    public interface IProjectService
    {
        /// <summary>
        /// Gets top ten projects ordered by the most hours worked
        /// </summary>
        /// <param name="request"><see cref="ChartRequest"/></param>
        /// <returns><see cref="ServiceResult{T}"/> of <see cref="List{T}"/> of <see cref="ChartProject"/></returns>
        Task<ServiceResult<List<ChartProject>>> GetTopTenAsync(ChartRequest request);
    }
}
