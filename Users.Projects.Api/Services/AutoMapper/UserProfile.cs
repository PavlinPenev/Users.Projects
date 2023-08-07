using AutoMapper;
using Users.Projects.Api.Data.Models;
using Users.Projects.Api.Services.Models;
using Users.Projects.Api.Services.Models.Users;

namespace Users.Projects.Api.Services.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile() 
        {
            CreateMap<User, ChartUser>()
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => $"{src.FirstName} {src.LastName}"));
            CreateMap<User, UserDto>();
            CreateMap<List<User>, PagedList<UserDto>>()
                .ForMember(dest => dest.Items, opts => opts.MapFrom(src => src));
        }
    }
}
