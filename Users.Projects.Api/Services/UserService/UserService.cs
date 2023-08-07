using AutoMapper;
using Users.Projects.Api.Data.Repositories.UsersRepository;
using Users.Projects.Api.Services.Models;
using Users.Projects.Api.Services.Models.Users;

namespace Users.Projects.Api.Services.UserService
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository usersRepository;
        private readonly IMapper mapper;

        public UserService(
            IUsersRepository usersRepository,
            IMapper mapper)
        {
            this.usersRepository = usersRepository;
            this.mapper = mapper;
        }

        public async Task<ServiceResult<ChartUser>> GetAsync(Guid id)
        {
            try
            {
                var user = await usersRepository.GetAsync(id);

                var chartUser = mapper.Map<ChartUser>(user);

                chartUser.HoursWorked = user.TimeLogs.Sum(t => t.HoursWorked);

                return new ServiceResult<ChartUser>
                {
                    StatusCode = 200,
                    IsSuccess = true,
                    Data = chartUser
                };
            }
            catch (NullReferenceException ex)
            {
                return new ServiceResult<ChartUser>
                {
                    StatusCode = 400,
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<ChartUser>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<ServiceResult<PagedList<UserDto>>> GetAllPagedAsync(UsersRequest request)
        {
            try
            {
                var usersResult = await usersRepository.GetAllPagedAsync(request);

                var users = usersResult.Item1;
                var mappedUsers = mapper.Map<PagedList<UserDto>>(users);
                mappedUsers.TotalItemsCount = usersResult.Item2;

                return new ServiceResult<PagedList<UserDto>>
                {
                    StatusCode = 200,
                    IsSuccess = true,
                    Data = mappedUsers
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<PagedList<UserDto>>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<ServiceResult<List<ChartUser>>> GetTopTenAsync(ChartRequest request)
        {
            try
            {
                var users = await usersRepository.GetTopTenAsync(request);

                var mappedUsers = mapper.Map<List<ChartUser>>(users);

                foreach (var mappedUser in mappedUsers)
                {
                    var user = users.FirstOrDefault(x => x.Id == mappedUser.Id);

                    mappedUser.HoursWorked = user.TimeLogs.Sum(x => x.HoursWorked);
                }

                return new ServiceResult<List<ChartUser>>
                {
                    StatusCode = 200,
                    IsSuccess = true,
                    Data = mappedUsers
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<List<ChartUser>>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<ServiceResult<bool>> RefreshDataAsync()
        {
            try
            {
                var result = await usersRepository.RefreshDataAsync();

                return new ServiceResult<bool>
                {
                    StatusCode = 200,
                    IsSuccess = result,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult<bool>
                {
                    StatusCode = 500,
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}
