using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EmployeeSkillManagement.Data;
using EmployeeSkillManagement.Models;
using EmployeeSkillManagement.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace EmployeeSkillManagement.Controllers
{
    [Authorize(Policy = "RequireAdminRole")]
    public class SkillController : Controller
    {
        private readonly ILogger<SkillController> _logger;

        private readonly ISkillRepository _skillRepository;

        public SkillController(ILogger<SkillController> logger, ISkillRepository skillRepository)
        {
            _logger = logger;
            _skillRepository = skillRepository;
        }

        public async Task<IActionResult> Index()
        {
            List<Skill> skillList = await _skillRepository.GetAllSkillsAsync();
            return View(skillList);
        }

        public IActionResult Create() { 
            return View();
         }

        [HttpPost]
        public async Task<IActionResult> Create(Skill skill){
            try{
                if(ModelState.IsValid){
                    await _skillRepository.AddSkillAsync(skill);
                    return RedirectToAction("Index");
                }
            } catch(System.Exception ex){
                TempData["ErrorMessage"] = ex.Message;
            }
            return View(skill);
        }

        public async Task<IActionResult> Delete(int id)
        {
            try{
                await _skillRepository.DeleteSkillAsync(id);
            }catch(Exception ex){
                TempData["ErrorMessage"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Update(Skill skill)
        {
            try{
                if (ModelState.IsValid)
                {
                    await _skillRepository.UpdateSkillAsync(skill);
                }
            }catch(System.Exception ex){
                TempData["ErrorMessage"] = ex.Message;
            }

            return RedirectToAction("Index", skill);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}