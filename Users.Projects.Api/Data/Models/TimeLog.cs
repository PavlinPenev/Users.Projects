using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Users.Projects.Api.Common.Constants.DbTables;

namespace Users.Projects.Api.Data.Models
{
    [Table(TIME_LOGS_TABLE_NAME)]
    public class TimeLog
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ProjectId { get; set; }

        public DateTime DateAdded { get; set; }

        [Required]
        public float HoursWorked { get; set; }

        #region Navigational properties
        public virtual User User { get; set; }

        public virtual Project Project { get; set; }
        #endregion
    }
}
