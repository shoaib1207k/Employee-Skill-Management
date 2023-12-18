using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeSkillManagement.Models
{
    public class EmployeeSkillAndLevel
    {
        [Key]
        public int Id { get; set; }
        public int SkillId { get; set; }
        [Range(1,10)]
        public int SkillLevel { get; set; }
    }
}