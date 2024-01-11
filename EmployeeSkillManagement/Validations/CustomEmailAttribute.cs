using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EmployeeSkillManagement.Validations
{
    public class CustomEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var email = value as string;
            if (email == null)
            {
                return new ValidationResult("Email is required");
            }

            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            var regex = new Regex(pattern);

            if (!regex.IsMatch(email))
            {
                return new ValidationResult("Invalid email format");
            }

            return ValidationResult.Success;
        }
    }

}