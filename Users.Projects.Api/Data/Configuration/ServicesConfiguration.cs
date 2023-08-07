using Users.Projects.Api.Data.Repositories.ProjectsRepository;
using Users.Projects.Api.Data.Repositories.TimeLogsRepository;
using Users.Projects.Api.Data.Repositories.UsersRepository;
using Users.Projects.Api.Data.Repositories;

namespace Users.Projects.Api.Data.Configuration
{
    public static class ServicesConfiguration
    {
        public static void AddRepositories(this IServiceCollection services) 
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUsersRepository, UsersRepository>();
            services.AddScoped<IProjectsRepository, ProjectsRepository>();
            services.AddScoped<ITimeLogsRepository, TimeLogsRepository>();
        }
    }
}
