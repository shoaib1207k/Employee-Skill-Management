using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeSkillManagement.Models
{
    public class Skill
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [DisplayName("Skill Name")]
        public string SkillName { get; set; } = string.Empty;

        public List<EmployeeSkillAndLevel> EmployeeSkillAndLevels{get; set;} = new List<EmployeeSkillAndLevel>();
    }
}