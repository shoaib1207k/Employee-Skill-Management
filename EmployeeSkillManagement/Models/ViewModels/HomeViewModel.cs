using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeSkillManagement.Models.ViewModels
{
    public class HomeViewModel
    {
        public int TotalEmployees { get; set; }
        public int TotalSkills { get; set; }
        public Dictionary<string, int> SkillWithEmployeeCount { get; set; } = new Dictionary<string, int>();
    }
}