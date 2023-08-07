using AutoMapper;
using Users.Projects.Api.Data.Repositories.ProjectsRepository;
using Users.Projects.Api.Services.Models;
using Users.Projects.Api.Services.Models.Projects;

namespace Users.Projects.Api.Services.ProjectService
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectsRepository projectsRepository;
        private readonly IMapper mapper;

        public ProjectService(IProjectsRepository projectsRepository, IMapper mapper)
        {
            this.projectsRepository = projectsRepository;
            this.mapper = mapper;
        }

        public async Task<ServiceResult<List<ChartProject>>> GetTopTenAsync(ChartRequest request)
        {
            try
            {
                var projects = await projectsRepository.GetTopTenAsync(request);

                var mappedProjects = mapper.Map<List<ChartProject>>(projects);

                foreach (var mappedProject in mappedProjects)
                {
                    var project = projects.FirstOrDefault(x => x.Id == mappedProject.Id);

                    mappedProject.HoursWorked = project.TimeLogs.Sum(x => x.HoursWorked);
                }

                return new ServiceResult<List<ChartProject>>
                {
                    StatusCode = 200,
                    IsSuccess = true,
                    Data = mappedProjects
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<List<ChartProject>>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
