namespace Users.Projects.Api.Data.Repositories.TimeLogsRepository
{
    public interface ITimeLogsRepository
    {
        /// <summary>
        /// Gets hours worked per user for all the projects he worked at
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns>Hours worked in <see cref="float"/></returns>
        Task<float> GetUserTimeLog(Guid userId);
    }
}
