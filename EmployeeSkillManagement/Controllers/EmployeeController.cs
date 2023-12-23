using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EmployeeSkillManagement.Data;
using EmployeeSkillManagement.Models;
using EmployeeSkillManagement.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmployeeSkillManagement.Controllers
{
    // [Route("[controller]")]
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly ApplicationDbContext _db;

        public EmployeeController(ILogger<EmployeeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            EmployeesListViewModel employeesViewModel = new EmployeesListViewModel
            {
                Skills = await GetSkillOptions(),
                Employees = await _db.Employees.Include(e => e.EmployeeSkillsAndLevels).ToListAsync()
            };

            return View(employeesViewModel);
        }


        public async Task<IActionResult> Create(){

            var viewModel = new CreateEmployeeViewModel{
                DesignationOptions = await GetDesignationOptions(),
                SkillOptions = await GetSkillOptions(),
                // Employee = new Employee()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeViewModel viewModel){
            if(ModelState.IsValid){
                var newEmployee = new Employee
                {
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    DesignationName = _db.Designations.FirstOrDefault(u=>u.Id==viewModel.DesignationId).DesignationName,
                    Email = viewModel.Email,
                    DateOfJoining = viewModel.DateOfJoining,
                    EmployeeSkillsAndLevels = new List<EmployeeSkillAndLevel>()
                    // Assign other properties accordingly
                };

                // Now, handle the skills
                if(viewModel.SkillIds!=null && viewModel.SkillLevel!=null){
                    int index = 0;
                    foreach (var skillId in viewModel.SkillIds)
                    {
                        // var skillId = skillViewModel.Skill.Id;  // Assuming Skill has an Id property

                        var employeeSkillAndLevel = new EmployeeSkillAndLevel
                        {
                            SkillId = _db.Skills.FirstOrDefault(s => s.Id == skillId).Id,
                            SkillName = _db.Skills.FirstOrDefault(s => s.Id == skillId).SkillName,
                            SkillLevel = viewModel.SkillLevel[index]
                        };

                        newEmployee.EmployeeSkillsAndLevels.Add(employeeSkillAndLevel);
                        index++;
                    }
                }
                

            await _db.Employees.AddAsync(newEmployee);
            await _db.SaveChangesAsync(); // Save changes to get the newEmployee.Id

            // Now, handle the skills
                return RedirectToAction("Index");
            }
            viewModel.SkillOptions = await GetSkillOptions();
            viewModel.DesignationOptions = await GetDesignationOptions();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id){
            if(id==0){
                return NotFound();
            }
            Employee? employee = await _db.Employees
                                .Include(e=>e.EmployeeSkillsAndLevels)
                                .FirstOrDefaultAsync(e=>e.Id == id);
            if(employee == null){
                return NotFound();
            }

            if(employee.EmployeeSkillsAndLevels != null){
                _db.EmployeeSkillAndLevels.RemoveRange(employee.EmployeeSkillsAndLevels);
            }

            _db.Remove(employee);
            await _db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> GetEmployeeDeleteModal(int employee_id){
            if(employee_id == 0){
                return NotFound();
            }

            Employee? employee = await _db.Employees.FirstOrDefaultAsync(e => e.Id == employee_id);
            
            if(employee == null){
                return NotFound();
            }
            var employeeDeleteModal = PartialView("_DeleteEmployeeModal", employee);
          
            return employeeDeleteModal;
        }

        [HttpPost]
        public async Task<IActionResult> SearchEmployee(string empNameOrId, int skillId, bool generateReport){
            List<Employee> employeesFiltered = await _db.Employees
                                    .Include(e=>e.EmployeeSkillsAndLevels).ToListAsync();

            string empName = "";
            if (int.TryParse(empNameOrId, out int empId)){
                if(empId!=0){
                    employeesFiltered = employeesFiltered.Where(e=>e.Id == empId).ToList();
                }
            }
            else if (!string.IsNullOrWhiteSpace(empNameOrId))
            {
              empName = empNameOrId;
              employeesFiltered =  employeesFiltered
                    .Where(e => (e.FirstName + " " + e.LastName).Contains(empName, StringComparison.OrdinalIgnoreCase)
                                // e.LastName.Contains(empName, StringComparison.OrdinalIgnoreCase)
                                
                        ).ToList();
            }
            if(skillId!=0){
                employeesFiltered = employeesFiltered
                    .Where(e => e.EmployeeSkillsAndLevels.Any(esl => esl.SkillId == skillId))
                    .ToList();
            }
            if(generateReport){
                
                string primarySkill = "";
                if(skillId!=0){
                    primarySkill = _db.Skills.FirstOrDefault(s => s.Id == skillId).SkillName;
                    employeesFiltered.ForEach(employee => employee.EmployeeSkillsAndLevels.RemoveAll(skill => skill.SkillId == skillId));
                }



                EmployeeReportViewModel employeeReportViewModel = new EmployeeReportViewModel{
                    Employees = employeesFiltered,
                    PrimarySkill = primarySkill
                };
                return View("GenerateReport", employeeReportViewModel);
            }
            var employeesFilteredHtml = PartialView("_EmployeeCard", employeesFiltered);

            return employeesFilteredHtml;
        }

        private async Task<List<SelectListItem>> GetSkillOptions()
        {
            var skills = await _db.Skills.ToListAsync();
            return skills.Select(s => new SelectListItem { Value = s.Id.ToString(), Text = s.SkillName.ToString() }).ToList();
        }

        private async Task<List<SelectListItem>> GetDesignationOptions()
        {
            var designations = await _db.Designations.ToListAsync();
            return designations.Select(s=>new SelectListItem{Value=s.Id.ToString(), Text=s.DesignationName!.ToString()}).ToList();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        
    }
}