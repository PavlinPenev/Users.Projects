using Microsoft.EntityFrameworkCore;
using Users.Projects.Api.Data.Models;
using Users.Projects.Api.Services.Models;

namespace Users.Projects.Api.Data.Repositories.ProjectsRepository
{
    public class ProjectsRepository : IProjectsRepository
    {
        private readonly IRepository<Project> repository;

        public ProjectsRepository(IRepository<Project> repository)
        {
            this.repository = repository;
        }

        public async Task<List<Project>> GetTopTenAsync(ChartRequest request)
        {
            return await repository
                .GetAll()
                .Include(x => x.TimeLogs)
                .OrderByDescending(x => x.TimeLogs.Sum(y => y.HoursWorked))
                .Take(request.Take)
                .ToListAsync();
        }
    }
}
