using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Users.Projects.Api.Common.Constants.DbTables;

namespace Users.Projects.Api.Data.Models
{
    [Table(PROJECTS_TABLE_NAME)]
    public class Project
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        #region Navigational properties
        public virtual ICollection<TimeLog> TimeLogs { get; set; }
        #endregion
    }
}
