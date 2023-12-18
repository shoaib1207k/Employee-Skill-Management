using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeSkillManagement.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }
        [EmailAddress]
        [Required]
        public string? Email { get; set; }
        [PasswordPropertyText]
        [Required]
        public string? Password { get; set; }
    }
}