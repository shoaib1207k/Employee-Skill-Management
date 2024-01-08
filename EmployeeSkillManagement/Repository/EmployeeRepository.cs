using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeSkillManagement.Data;
using EmployeeSkillManagement.Models;
using EmployeeSkillManagement.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EmployeeSkillManagement.Repository
{  
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ApplicationDbContext _db;

        public EmployeeRepository(ApplicationDbContext db){
            _db = db;
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            List<Employee> employees = await _db.Employees.Include(e=>e.Designation)
                            .Include(e => e.EmployeeSkillsAndLevels).ToListAsync();
            // Sort the EmployeeSkillsAndLevels for each employee
            foreach (var employee in employees)
            {
                employee.EmployeeSkillsAndLevels = employee.EmployeeSkillsAndLevels
                    .OrderByDescending(skill => skill.IsPrimary)
                    .ToList();
            }
            return employees;
        }

        public async Task<Employee> GetEmployeeById(int id){
            if(id != 0){
                Employee? employee = await _db.Employees.Include(e=>e.Designation)
                                .Include(e=>e.EmployeeSkillsAndLevels)
                                .FirstOrDefaultAsync(e => e.Id == id);
                if(employee == null){
                    throw new Exception("Employee Not Found");
                }
               
                return employee;
            }else{
                throw new Exception("Employee Not Found");
            }
            
        }

        public async Task<List<Employee>> GetEmployeesByName(string employeeName)
        {
            List<Employee> employeesFiltered = await GetAllEmployeesAsync();
            employeesFiltered =  employeesFiltered
                    .Where(e => (e.FirstName + " " + e.LastName)
                    .Contains(employeeName, StringComparison.OrdinalIgnoreCase)).ToList();
            return employeesFiltered;
        }

        public async Task<bool> IsEmployeeExistByEmailAsync(string email)
        {
            Employee? employee = await _db.Employees
                                    .FirstOrDefaultAsync(e=>e.Email==email);
            if(employee==null){
                return false;
            }else{
                return true;
            }

        }

        public async Task UpsertEmployeeFromCreateViewModelAsync(UpsertEmployeeViewModel viewModel)
        {   
            if(viewModel.EmployeeId==0){
                if(await IsEmployeeExistByEmailAsync(viewModel.Email)){
                    throw new Exception("Employee already exist with this email!");
                }else{

                    var newEmployee = new Employee
                    {
                        FirstName = viewModel.FirstName,
                        LastName = viewModel.LastName,
                        Designation = await _db.Designations
                                    .FirstOrDefaultAsync(u=>u.Id==int.Parse(viewModel.DesignationId)),
                        Email = viewModel.Email,
                        DateOfJoining = viewModel.DateOfJoining,
                        EmployeeSkillsAndLevels = viewModel.EmployeeSkillsAndLevels
                    };

                    await _db.Employees.AddAsync(newEmployee);
                    await _db.SaveChangesAsync(); // Save changes to get the newEmployee.Id
                }
            }else{
                Employee existingEmployee = await GetEmployeeById(viewModel.EmployeeId);
                existingEmployee.FirstName = viewModel.FirstName;
                existingEmployee.LastName = viewModel.LastName;
                existingEmployee.Email = viewModel.Email;
                existingEmployee.Designation = await _db
                        .Designations.FirstOrDefaultAsync(u=>u.Id==int.Parse(viewModel.DesignationId));
                
                _db.EmployeeSkillAndLevels.RemoveRange(existingEmployee.EmployeeSkillsAndLevels);
                _db.EmployeeSkillAndLevels.AddRange(viewModel.EmployeeSkillsAndLevels);

                existingEmployee.EmployeeSkillsAndLevels = viewModel.EmployeeSkillsAndLevels;

                _db.Update(existingEmployee);
                await _db.SaveChangesAsync();
            }
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            if(id==0){
                throw new Exception("Employee Not Found!");
            }
            Employee? employee = await _db.Employees
                                .Include(e=>e.EmployeeSkillsAndLevels)
                                .FirstOrDefaultAsync(e=>e.Id == id);
            if(employee == null){
                throw new Exception("Employee Not Found!");
            }

            if(employee.EmployeeSkillsAndLevels != null){
                _db.EmployeeSkillAndLevels.RemoveRange(employee.EmployeeSkillsAndLevels);
            }

            _db.Remove(employee);
            await _db.SaveChangesAsync();
        }

        public async Task<List<SelectListItem>> GetSkillOptions()
        {
            var skills = await _db.Skills.ToListAsync();
            return skills.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.SkillName.ToString() }).ToList();
        }

        public async Task<List<SelectListItem>> GetDesignationOptions()
        {
            var designations = await _db.Designations.ToListAsync();
            return designations.Select(s=>new SelectListItem{Value=s.Id.ToString(), Text=s.DesignationName!.ToString()}).ToList();
        }
    }
}