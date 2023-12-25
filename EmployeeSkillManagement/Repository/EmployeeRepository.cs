using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeSkillManagement.Data;
using EmployeeSkillManagement.Models;
using EmployeeSkillManagement.Models.ViewModels;
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
            List<Employee> employees = await _db.Employees.Include(e => e.EmployeeSkillsAndLevels).ToListAsync();
            return employees;
        }

        public async Task<Employee> GetEmployeeById(int id){
            if(id != 0){
                Employee? employee = await _db.Employees.FirstOrDefaultAsync(e => e.Id == id);
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

        public async Task AddEmployeeFromCreateViewModelAsync(CreateEmployeeViewModel viewModel)
        {
            if(await IsEmployeeExistByEmailAsync(viewModel.Email)){
                throw new Exception("Employee already exist with this email");
            }else{

                var newEmployee = new Employee
                {
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    DesignationName = _db.Designations.FirstOrDefault(u=>u.Id==viewModel.DesignationId)!.DesignationName,
                    Email = viewModel.Email,
                    DateOfJoining = viewModel.DateOfJoining,
                    EmployeeSkillsAndLevels = viewModel.EmployeeSkillAndLevels
                };

                // Now, handle the skills
                // if(viewModel.EmployeeSkillAndLevels!=null ){
                //     int index = 0;
                //     foreach (var esl in viewModel.EmployeeSkillAndLevels)
                //     {
                //         var employeeSkillAndLevel = new EmployeeSkillAndLevel
                //         {
                //             SkillId = _db.Skills.FirstOrDefault(s => s.Id == esl.SkillId)!.Id,
                //             SkillName = _db.Skills.FirstOrDefault(s => s.Id == esl.SkillId)!.SkillName,
                //             SkillLevel = esl.SkillLevel
                //         };

                //         newEmployee.EmployeeSkillsAndLevels.Add(employeeSkillAndLevel);
                //         index++;
                //     }
                // }
                await _db.Employees.AddAsync(newEmployee);
                await _db.SaveChangesAsync(); // Save changes to get the newEmployee.Id
            }

        }

        public async Task DeleteEmployeeAsync(int id)
        {
            if(id==0){
                throw new Exception("Employee Not Found");
            }
            Employee? employee = await _db.Employees
                                .Include(e=>e.EmployeeSkillsAndLevels)
                                .FirstOrDefaultAsync(e=>e.Id == id);
            if(employee == null){
                throw new Exception("Employee Not Found");
            }

            if(employee.EmployeeSkillsAndLevels != null){
                _db.EmployeeSkillAndLevels.RemoveRange(employee.EmployeeSkillsAndLevels);
            }

            _db.Remove(employee);
            await _db.SaveChangesAsync();
        }

       
    }
}



// UPSERT METHOD for Employees
/*
public async Task UpsertEmployeeFromViewModelAsync(UpsertEmployeeViewModel viewModel)
{
    var existingEmployee = await _db.Employees.FirstOrDefaultAsync(e => e.Email == viewModel.Email);

    if (existingEmployee == null)
    {
        // Insert operation
        var newEmployee = new Employee
        {
            FirstName = viewModel.FirstName,
            LastName = viewModel.LastName,
            DesignationName = _db.Designations.FirstOrDefault(u => u.Id == viewModel.DesignationId)!.DesignationName,
            Email = viewModel.Email,
            DateOfJoining = viewModel.DateOfJoining,
            EmployeeSkillsAndLevels = new List<EmployeeSkillAndLevel>()
        };

        // Handle the skills
        if (viewModel.SkillIds != null && viewModel.SkillLevel != null)
        {
            int index = 0;
            foreach (var skillId in viewModel.SkillIds)
            {
                var employeeSkillAndLevel = new EmployeeSkillAndLevel
                {
                    SkillId = _db.Skills.FirstOrDefault(s => s.Id == skillId)!.Id,
                    SkillName = _db.Skills.FirstOrDefault(s => s.Id == skillId)!.SkillName,
                    SkillLevel = viewModel.SkillLevel[index]
                };

                newEmployee.EmployeeSkillsAndLevels.Add(employeeSkillAndLevel);
                index++;
            }
        }

        await _db.Employees.AddAsync(newEmployee);
    }
    else
    {
        // Update operation
        existingEmployee.FirstName = viewModel.FirstName;
        existingEmployee.LastName = viewModel.LastName;
        existingEmployee.DesignationName = _db.Designations.FirstOrDefault(u => u.Id == viewModel.DesignationId)!.DesignationName;
        existingEmployee.DateOfJoining = viewModel.DateOfJoining;

        // Handle the skills
        if (viewModel.SkillIds != null && viewModel.SkillLevel != null)
        {
            // Remove existing skills
            _db.EmployeeSkillsAndLevels.RemoveRange(existingEmployee.EmployeeSkillsAndLevels);

            // Add new skills
            int index = 0;
            foreach (var skillId in viewModel.SkillIds)
            {
                var employeeSkillAndLevel = new EmployeeSkillAndLevel
                {
                    SkillId = _db.Skills.FirstOrDefault(s => s.Id == skillId)!.Id,
                    SkillName = _db.Skills.FirstOrDefault(s => s.Id == skillId)!.SkillName,
                    SkillLevel = viewModel.SkillLevel[index]
                };

                existingEmployee.EmployeeSkillsAndLevels.Add(employeeSkillAndLevel);
                index++;
            }
        }

        _db.Employees.Update(existingEmployee);
    }

    await _db.SaveChangesAsync();
}



*/