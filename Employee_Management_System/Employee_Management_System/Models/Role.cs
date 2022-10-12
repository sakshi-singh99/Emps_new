using System.ComponentModel.DataAnnotations;

namespace Employee_Management_System.Models
{
    public class Role
    {
        [Required]
        [Key]
        public int RoleId { get; set; }
        [Required]
        public string RoleName { get; set; }

    }
}
