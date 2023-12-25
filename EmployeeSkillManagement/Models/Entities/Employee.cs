using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeSkillManagement.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;
      
        [Required]
        public Designation? Designation { get; set; } = new Designation();
          [Required]
        [EmailAddress]
        public String Email { get; set; } = string.Empty;
        [Required]
  
        public DateOnly DateOfJoining { get; set; }
        public List<EmployeeSkillAndLevel> EmployeeSkillsAndLevels { get; set; } = new List<EmployeeSkillAndLevel>();
    }
}