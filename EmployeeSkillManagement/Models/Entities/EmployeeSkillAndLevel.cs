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
        public string SkillName { get; set; } = string.Empty;
        [Range(1,10)]
        
        public int SkillLevel { get; set; }
        public int SkillExperience { get; set; }
        public bool IsPrimary { get; set; }
    }
}