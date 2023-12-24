using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeSkillManagement.Models;

namespace EmployeeSkillManagement.Repository
{
    public interface ISkillRepository
    {
        public Task<List<Skill>> GetAllSkillsAsync();
        public Task AddSkillAsync(Skill skill);
        public Task DeleteSkillAsync(int id);
        public Task UpdateSkillAsync(Skill skill);
        
    }
}