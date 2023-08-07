using Microsoft.EntityFrameworkCore;
using Users.Projects.Api.Data.Models;

namespace Users.Projects.Api.Data.Repositories.TimeLogsRepository
{
    public class TimeLogsRepository : ITimeLogsRepository
    {
        private readonly IRepository<TimeLog> repository;

        public TimeLogsRepository(IRepository<TimeLog> repository)
        {
            this.repository = repository;
        }

        public async Task<float> GetUserTimeLog(Guid userId)
        {
            return await repository.GetAll().Where(x => x.UserId == userId).SumAsync(x => x.HoursWorked);
        }
    }
}
