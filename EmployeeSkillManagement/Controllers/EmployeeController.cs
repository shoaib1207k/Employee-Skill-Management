using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EmployeeSkillManagement.Data;
using EmployeeSkillManagement.Models;
using EmployeeSkillManagement.Models.ViewModels;
using EmployeeSkillManagement.Repository;
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
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ISkillRepository _skillRepository;

        private readonly ApplicationDbContext _db;
        public EmployeeController(ILogger<EmployeeController> logger, IEmployeeRepository employeeRepository, 
                                    ApplicationDbContext db, ISkillRepository skillRepository)
        {
            _logger = logger;
            _employeeRepository = employeeRepository;
            _db = db;
            _skillRepository = skillRepository;
        }

        public async Task<IActionResult> Index()
        {
            EmployeesListViewModel employeesViewModel = new EmployeesListViewModel
            {
                Skills = await GetSkillOptions(),
                Employees = await _employeeRepository.GetAllEmployeesAsync()
            };

            return View(employeesViewModel);
        }


        public async Task<IActionResult> Create(){

            var viewModel = new UpsertEmployeeViewModel{
                DesignationOptions = await GetDesignationOptions(),
                SkillOptions = await GetSkillOptions(),
                // Employee = new Employee()
            };

            return View("Upsert",viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id){
            try{
                Employee employee = await _employeeRepository.GetEmployeeById(id);
                var viewModel = new UpsertEmployeeViewModel{
                    EmployeeId = id,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Email = employee.Email,
                    DateOfJoining = employee.DateOfJoining,
                    DesignationId = employee.Designation!.Id.ToString(),
                    EmployeeSkillsAndLevels = employee.EmployeeSkillsAndLevels,
                    DesignationOptions = await GetDesignationOptions(),
                    SkillOptions = await GetSkillOptions()
                };
                return View("Upsert", viewModel);
            } catch(Exception ex){
                TempData["ErrorMessage"] = ex.Message;
                return View("Index"); 
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> Upsert(UpsertEmployeeViewModel viewModel){
            try
            { 
                if(ModelState.IsValid){
                    await _employeeRepository.UpsertEmployeeFromCreateViewModelAsync(viewModel);
                    return RedirectToAction("Index");
                }
            }
            catch (System.Exception ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            viewModel.SkillOptions = await GetSkillOptions();
            viewModel.DesignationOptions = await GetDesignationOptions();
            return View("Upsert",viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id){
            try
            {
                await _employeeRepository.DeleteEmployeeAsync(id);
            }
            catch (System.Exception)
            {
                TempData["ErrorMessage"] ="Something went wrong.";
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> GetEmployeeDeleteModal(int employee_id){
            if(employee_id == 0){
                return NotFound();
            }
            
            Employee? employee = await _employeeRepository.GetEmployeeById(employee_id);
            
            if(employee == null){
                return NotFound();
            }
            var employeeDeleteModal = PartialView("_DeleteEmployeeModal", employee);
          
            return employeeDeleteModal;
        }

        [HttpPost]
        public async Task<IActionResult> SearchEmployee(string empNameOrId, int skillId, bool generateReport){
            
            List<Employee> employeesFiltered = await _employeeRepository.GetAllEmployeesAsync();

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
                    .Where(e => (e.FirstName + " " + e.LastName)
                    .Contains(empName, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            if(skillId!=0){
                employeesFiltered = employeesFiltered
                    .Where(e => e.EmployeeSkillsAndLevels.Any(esl => esl.SkillId == skillId))
                    .ToList();
            }
            if(generateReport){
                
                string primarySkill = "";
                if(skillId!=0){
                    primarySkill = _skillRepository.GetSkillByIdAsync(skillId).Result.SkillName;
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