namespace Users.Projects.Api.Services.Models
{
    public class PagedList<T>
        where T : class
    {
        public List<T> Items { get; set; }

        public int TotalItemsCount { get; set; }
    }
}
