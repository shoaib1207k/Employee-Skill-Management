using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EmployeeSkillManagement.Data;
using EmployeeSkillManagement.Models;
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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create(){

            var viewModel = new CreateEmployeeViewModel{
                DesignationOptions = GetDesignationOptions(),
                SkillOptions = GetSkillOptions(),
                // Employee = new Employee()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Create(CreateEmployeeViewModel viewModel){
            if(ModelState.IsValid){
                var newEmployee = new Employee
                {
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName,
                    Designation = _db.Designations.FirstOrDefault(u=>u.Id==viewModel.DesignationId),
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
                            SkillLevel = viewModel.SkillLevel[index]
                        };

                        newEmployee.EmployeeSkillsAndLevels.Add(employeeSkillAndLevel);
                        index++;
                    }
                }
                

            _db.Employees.Add(newEmployee);
            _db.SaveChanges(); // Save changes to get the newEmployee.Id

            // Now, handle the skills
                return RedirectToAction("Index");
            }
            viewModel.SkillOptions = GetSkillOptions();
            viewModel.DesignationOptions = GetDesignationOptions();
            return View(viewModel);
        }


        //[HttpPost]
        // public IActionResult DeleteSkill(int skillId, int employeeId)
        // {
        //     // Retrieve the employee based on the employeeId
        //     var employee = _db.Employees.Include(e => e.EmployeeSkillsAndLevels)
        //                                 .SingleOrDefault(e => e.Id == employeeId);

        //     if (employee == null)
        //     {
        //         // Handle the case where the employee is not found
        //         return NotFound();
        //     }

        //     // Find the skill in the employee's skills
        //     var skillToRemove = employee.EmployeeSkillsAndLevels
        //                                 .SingleOrDefault(esl => esl.SkillId == skillId);

        //     if (skillToRemove == null)
        //     {
        //         // Handle the case where the skill is not found
        //         return NotFound();
        //     }

        //     // Remove the skill from the employee's skills
        //     employee.EmployeeSkillsAndLevels.Remove(skillToRemove);

        //     // Save changes to update the database
        //     _db.SaveChanges();

        //     return RedirectToAction("Index"); // Redirect to wherever you want
        // }


        private List<SelectListItem> GetSkillOptions(){
            var skills = _db.Skills.ToList();
            return skills.Select(s=>new SelectListItem{Value=s.Id.ToString(), Text=s.SkillName.ToString()}).ToList();
        }
        private List<SelectListItem> GetDesignationOptions(){
            var designations = _db.Designations.ToList();
            return designations.Select(s=>new SelectListItem{Value=s.Id.ToString(), Text=s.DesignationName!.ToString()}).ToList();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }

        
    }
}