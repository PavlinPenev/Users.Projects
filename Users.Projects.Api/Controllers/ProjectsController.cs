using static Users.Projects.Api.Common.Constants.Routes.ProjectsController;
using Microsoft.AspNetCore.Mvc;
using Users.Projects.Api.Services.ProjectService;
using Users.Projects.Api.Services.Models.Projects;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using Users.Projects.Api.Services.Models;

namespace Users.Projects.Api.Controllers
{
    [Route(PROJECTS_CONTROLLER_ROUTE)]
    public class ProjectsController : UsersProjectsBaseController
    {
        private readonly IProjectService projectService;

        public ProjectsController(IProjectService projectService)
        {
            this.projectService = projectService;
        }

        [HttpPost(GET_TOP_TEN)]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(List<ChartProject>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTopTen([FromBody][Required] ChartRequest request)
        {
            var projects = await projectService.GetTopTenAsync(request);

            return HandleResult(projects);
        }
    }
}
