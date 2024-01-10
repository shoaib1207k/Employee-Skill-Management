using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeSkillManagement.Validations
{
    
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NoNumbersAttribute: ValidationAttribute
    {
        public override bool IsValid(object? value)
        {

            string? stringValue = value.ToString();

            // Check if the string contains any numeric characters
            return !stringValue?.Any(char.IsDigit) ?? true;
        }
    }
}