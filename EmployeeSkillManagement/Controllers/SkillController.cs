using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EmployeeSkillManagement.Data;
using EmployeeSkillManagement.Models;
using EmployeeSkillManagement.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace EmployeeSkillManagement.Controllers
{
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
            if(ModelState.IsValid){
                try
                {
                    await _skillRepository.AddSkillAsync(skill);
                    return RedirectToAction("Index");
                }
                catch (InvalidOperationException)
                {
                    TempData["ErrorMessage"] = "Skill already exists.";
                    return View(skill);
                }
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
            if (ModelState.IsValid)
            {
                try{
                    await _skillRepository.UpdateSkillAsync(skill);
                }catch(InvalidOperationException){
                    TempData["ErrorMessage"] = "Something went wrong";
                }
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