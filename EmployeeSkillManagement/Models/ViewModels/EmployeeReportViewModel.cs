using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace EmployeeSkillManagement.Models.ViewModels
{
    public class EmployeeReportViewModel
    {
        public List<Employee>? Employees { get; set; }
        public string PrimarySkill { get; set; } = string.Empty;
    }
}