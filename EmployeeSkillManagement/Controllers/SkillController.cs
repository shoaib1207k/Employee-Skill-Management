using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EmployeeSkillManagement.Data;
using EmployeeSkillManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;


namespace EmployeeSkillManagement.Controllers
{
    public class SkillController : Controller
    {
        private readonly ILogger<SkillController> _logger;
        private readonly ApplicationDbContext _db;

        public SkillController(ILogger<SkillController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            List<Skill> skillList = _db.Skills.ToList();
            return View(skillList);
        }

        public IActionResult Create() { 
            return View();
         }

        [HttpPost]
        public IActionResult Create(Skill skill){
            if(ModelState.IsValid){

                Skill? skillInDB = _db.Skills.FirstOrDefault(u=>u.SkillName == skill.SkillName);

                if(skillInDB==null){
                    _db.Add(skill);
                    _db.SaveChanges();
                    return RedirectToAction("Index");
                }
               
                return View(skill);

            }
            return View(skill);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Skill? skillFromDb = _db.Skills
                .Include(s => s.EmployeeSkillAndLevels)
                .FirstOrDefault(s => s.Id == id);

            if (skillFromDb == null)
            {
                return NotFound();
            }

            // Delete or update related records
            if (skillFromDb.EmployeeSkillAndLevels != null)
            {
                _db.EmployeeSkillAndLevels.RemoveRange(skillFromDb.EmployeeSkillAndLevels);
            }

            // Perform soft delete or hard delete
            _db.Remove(skillFromDb);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Update(Skill skill){

            if(ModelState.IsValid){
                Skill? exisitingSkill = _db.Skills.Find(skill.Id);
                if(exisitingSkill == null){
                    return NotFound();
                }else{
                    exisitingSkill.SkillName = skill.SkillName;
                    _db.SaveChanges();
                }
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index",skill);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}