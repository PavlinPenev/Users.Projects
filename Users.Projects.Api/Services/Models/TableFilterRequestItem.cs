namespace Users.Projects.Api.Services.Models
{
    public class TableFilterRequestItem
    {
        public string SearchTerm { get; set; }

        public string OrderBy { get; set; }

        public bool IsDescending { get; set; }

        public int Skip { get; set; }

        public int Take { get; set; }
    }
}
