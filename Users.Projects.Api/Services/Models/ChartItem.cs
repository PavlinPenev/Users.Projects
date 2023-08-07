namespace Users.Projects.Api.Services.Models
{
    public abstract class ChartItem
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public float HoursWorked { get; set; }
    }
}
