using Users.Projects.Api.Services.ProjectService;
using Users.Projects.Api.Services.UserService;

namespace Users.Projects.Api.Services.Configuration
{
    public static class ServicesConfiguration
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService.UserService>();
            services.AddScoped<IProjectService, ProjectService.ProjectService>();
        }
    }
}
