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
        public Task<Skill> GetSkillByIdAsync(int id);
        public Task AddSkillAsync(Skill skill);
        public Task DeleteSkillAsync(int id);
        public Task<bool> UpdateSkillAsync(Skill skill);
        public Task<bool> IsSkillExistByNameAsync(Skill skill);
    }
}