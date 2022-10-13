using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employee_Management_System.Models
{
    public class Employee
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EmpId { get; set; }
        [Required(ErrorMessage = "Please enter First Name")]
        public string EmpFirstName { get; set; }
        [Required(ErrorMessage = "Please enter Last Name")]
        public string EmpLastName { get; set; }
        [Required]
        public string EmpEmail { get; set; }
        [Required]
        public string EmpPassword { get; set; }
        [Required]
        public string EmpAddress { get; set; }
        [Required]
        public string EmpPhoneNumber { get; set; }
        [Required]
        [Display(Name ="Role")]
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public virtual Role Role { get; set; }
        public DateTime EmpRegisteredDate { get; set; }

    }
}
