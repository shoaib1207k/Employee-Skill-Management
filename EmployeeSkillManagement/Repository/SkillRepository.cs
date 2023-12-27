using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeSkillManagement.Data;
using EmployeeSkillManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeSkillManagement.Repository
{
    
    public class SkillRepository : ISkillRepository
    {
        private readonly ApplicationDbContext _db;

        public SkillRepository(ApplicationDbContext db){
            _db = db;
        }

        public async Task<List<Skill>> GetAllSkillsAsync()
        {
            List<Skill> skillList = await _db.Skills.ToListAsync();

            return skillList;

        }

        public async Task AddSkillAsync(Skill skill)
        {
            if(await IsSkillExistByNameAsync(skill)){
                throw new Exception("Skill with this name already exists");
            }
           
            await _db.AddAsync(skill);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteSkillAsync(int id)
        {

            if (id == 0)
            {
                throw new InvalidDataException("Invalid Id");
            }

            Skill? skillFromDb = await _db.Skills
                .Include(s => s.EmployeeSkillAndLevels)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (skillFromDb == null)
            {
                throw new Exception("Not");
            }

            // Delete or update related records
            if (skillFromDb.EmployeeSkillAndLevels != null)
            {
                _db.EmployeeSkillAndLevels.RemoveRange(skillFromDb.EmployeeSkillAndLevels);
            }

            // Perform soft delete or hard delete
            _db.Remove(skillFromDb);
            await _db.SaveChangesAsync();
           
        }

        public async Task UpdateSkillAsync(Skill skill)
        {   
            if(await IsSkillExistByNameAsync(skill)){

                throw new Exception("Skill with this name already exist");
            }
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    Skill? existingSkill = await _db.Skills.FindAsync(skill.Id);

                    if (existingSkill == null)
                    {
                        throw new Exception("Skill Not Found");
                    }

                    // Update SkillName in Skills table
                    existingSkill.SkillName = skill.SkillName;
                    await _db.SaveChangesAsync();

                    // Update SkillName in EmployeeSkillAndLevels table
                    var employeeSkillAndLevels = await _db.EmployeeSkillAndLevels
                        .Where(esl => esl.SkillId == existingSkill.Id)
                        .ToListAsync();

                    foreach (var esl in employeeSkillAndLevels)
                    {
                        esl.SkillName = skill.SkillName;
                    }

                    await _db.SaveChangesAsync();

                    // Commit the transaction
                    transaction.Commit();

                    // return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    // Log or handle exceptions
                    transaction.Rollback();
                    // return View("Error");
                }
            }
        }

        public async Task<Skill> GetSkillByIdAsync(int id)
        {
            if(id==0){
                throw new Exception("Skill Not Found");
            }
            Skill? skill = await _db.Skills.FirstOrDefaultAsync(s=>s.Id == id);
            if(skill != null)
                return skill;
            else
                throw new Exception("Skill Not Found");
        }

        public async Task<bool> IsSkillExistByNameAsync(Skill skill){
            try{
                Skill? skillInDb = await _db.Skills
                            .FirstOrDefaultAsync(s=> s.SkillName.ToLower() == skill.SkillName.ToLower());
                if(skillInDb==null){
                    return false;
                }else if(skill.Id == skillInDb.Id){
                    return false;
                }else{
                    return true;
                }
            } catch(Exception){
                throw new Exception("Something went wrong!");
            }
        }
    }
}