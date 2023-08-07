namespace Users.Projects.Api.Services.Models.Users
{
    public class UsersRequest : TableFilterRequestItem
    {
        public DateTime? DateAddedFrom { get; set; }

        public DateTime? DateAddedTo { get; set; }
    }
}
