using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeSkillManagement.Repository
{
    public interface IHomeRepository
    {
        public Task<int> GetTotalEmployeesCount();
        public Task<int> GetTotalSkillsCount();

        public Task<Dictionary<String, int>> GetTopSkillWithEmployeesCount();
    }
}