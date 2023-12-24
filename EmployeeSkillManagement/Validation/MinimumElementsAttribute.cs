using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeSkillManagement.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MinimumElementsAttribute : ValidationAttribute
    {
        private readonly int _minimumElements;

        public MinimumElementsAttribute(int minimumElements)
        {
            _minimumElements = minimumElements;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var list = value as IList<object>;

            if (list != null && list.Count >= _minimumElements)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult($"At least {_minimumElements} element(s) are required.");
        }
    }
}