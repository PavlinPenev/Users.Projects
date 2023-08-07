using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Users.Projects.Api.Services.Models;

namespace Users.Projects.Api.Controllers
{
    [ApiController]
    public class UsersProjectsBaseController : ControllerBase
    {
        protected IActionResult HandleResult<T>(ServiceResult<T> result)
        {
            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }

            return StatusCode(result.StatusCode, result.ErrorMessage);
        }
    }
}
