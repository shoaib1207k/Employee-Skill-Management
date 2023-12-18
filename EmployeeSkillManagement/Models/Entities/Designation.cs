using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeSkillManagement.Models
{
    public class Designation
    {   
        [Key]
        public int Id { get; set; }
        public String DesignationName { get; set; } = string.Empty;
    }
}