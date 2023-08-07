using Users.Projects.Api.Data.Models;
using Users.Projects.Api.Services.Models;

namespace Users.Projects.Api.Data.Repositories.ProjectsRepository
{
    public interface IProjectsRepository
    {
        /// <summary>
        /// Gets top ten projects ordered by the most hours worked
        /// </summary>
        /// <param name="request"><see cref="ChartRequest"/></param>
        /// <returns><see cref="List{T}"/> of <see cref="Project"/></returns>
        Task<List<Project>> GetTopTenAsync(ChartRequest request);
    }
}
