using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace EmployeeSkillManagement.Models.ViewModels
{
    public class EmployeeReportViewModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public Designation? Designation { get; set; } = new Designation();
        public String Email { get; set; } = string.Empty;
        public DateOnly DateOfJoining { get; set; }
        public List<EmployeeSkillAndLevel> EmployeeSkillsAndLevels { get; set; } = new List<EmployeeSkillAndLevel>();
        public List<Skill>? PrimarySkills { get; set; } 
        public List<Skill>? SecondarySkills { get; set; } 
    }
}