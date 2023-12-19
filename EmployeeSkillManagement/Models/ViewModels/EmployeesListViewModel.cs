using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeSkillManagement.Models.ViewModels
{
    public class EmployeesListViewModel
    {
        public List<SelectListItem> Skills {get; set;}
        public List<Employee> Employees {get; set;}

        public EmployeesListViewModel(){
            Skills = new List<SelectListItem>();
            Employees = new List<Employee>();   
        }
    }
}