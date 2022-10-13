using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace Employee_Management_System.Models
{
    public class Map
    {
        [Key]
        [Required]
        public int MapId { get; set; }

        [Required]
        [Display(Name = "EmployeeId")]
        public int EmpId { get; set; }

        [ForeignKey("EmpId")]
        public virtual Employee employee { get; set; }

        [Required]
        public int ProjectId { get; set; }
        [ForeignKey("ProjectId")]
        public virtual Project project { get; set; }
    }
}
