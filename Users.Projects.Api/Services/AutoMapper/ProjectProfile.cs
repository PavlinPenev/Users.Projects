using AutoMapper;
using Users.Projects.Api.Data.Models;
using Users.Projects.Api.Services.Models.Projects;

namespace Users.Projects.Api.Services.AutoMapper
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile() 
        {
            CreateMap<Project, ChartProject>();
        }
    }
}
