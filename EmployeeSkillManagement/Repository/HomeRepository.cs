using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeSkillManagement.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeSkillManagement.Repository
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;

        public HomeRepository(ApplicationDbContext db){
            _db = db;
        }

        public async Task<int> GetTotalEmployeesCount()
        {
            return await _db.Employees.CountAsync();
        }
        public async Task<Dictionary<string, int>> GetTopSkillWithEmployeesCount()
        {
            var skillCounts = await _db.EmployeeSkillAndLevels
                .GroupBy(esl => esl.SkillName)
                .Select(g => new { SkillName = g.Key, EmployeeCount = g.Count() })
                .OrderByDescending(x => x.EmployeeCount)
                .ToDictionaryAsync(x => x.SkillName, x => x.EmployeeCount);

            return skillCounts;
        }

        public async Task<int> GetTotalSkillsCount()
        {
            return await _db.Skills.CountAsync();
        }
    }
}