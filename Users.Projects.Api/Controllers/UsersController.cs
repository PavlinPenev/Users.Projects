using static Users.Projects.Api.Common.Constants.Routes.UsersController;
using Microsoft.AspNetCore.Mvc;
using Users.Projects.Api.Services.UserService;
using Users.Projects.Api.Services.Models.Users;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using Users.Projects.Api.Services.Models;

namespace Users.Projects.Api.Controllers
{
    [Route(USERS_CONTROLLER_ROUTE)]
    public class UsersController : UsersProjectsBaseController
    {
        private readonly IUserService userService;

        public UsersController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet(GET_USER)]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(ChartUser))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get([FromQuery][Required] Guid id) 
        {
            var chartUser = await userService.GetAsync(id);

            return HandleResult(chartUser);
        }

        [HttpPost(GET_ALL_USERS)]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(UserDto))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll([FromBody][Required] UsersRequest request)
        {
            var users = await userService.GetAllPagedAsync(request);

            return HandleResult(users);
        }

        [HttpPost(GET_TOP_TEN)]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(List<ChartUser>))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetTopTen([FromBody][Required] ChartRequest request)
        {
            var users = await userService.GetTopTenAsync(request);

            return HandleResult(users);
        }

        [HttpGet(REFRESH_DATA)]
        [SwaggerResponse(StatusCodes.Status200OK, type: typeof(bool))]
        [SwaggerResponse(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RefreshData()
        {
            var result = await userService.RefreshDataAsync();

            return HandleResult(result);
        }
    }
}
