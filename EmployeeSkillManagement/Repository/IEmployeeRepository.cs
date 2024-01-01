using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeSkillManagement.Models;
using EmployeeSkillManagement.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeSkillManagement.Repository
{
    public interface IEmployeeRepository
    {
        public Task<List<Employee>> GetAllEmployeesAsync();

        public Task<Employee> GetEmployeeById(int id);
        public Task<List<Employee>> GetEmployeesByName(string employeeName);

        public Task<bool> IsEmployeeExistByEmailAsync(string email);

        public Task UpsertEmployeeFromCreateViewModelAsync(UpsertEmployeeViewModel viewModel);

        public Task DeleteEmployeeAsync(int id);

        public Task<List<SelectListItem>> GetDesignationOptions();

        public Task<List<SelectListItem>> GetSkillOptions();


    }
}