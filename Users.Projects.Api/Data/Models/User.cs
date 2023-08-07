using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Users.Projects.Api.Common.Constants.DbTables;

namespace Users.Projects.Api.Data.Models
{
    [Table(USERS_TABLE_NAME)]
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        public DateTime DateAdded { get; set; }

        #region Navigational properties
        public virtual ICollection<TimeLog> TimeLogs { get; set;}
        #endregion
    }
}
