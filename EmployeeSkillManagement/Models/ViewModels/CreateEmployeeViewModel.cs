using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EmployeeSkillManagement.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeSkillManagement.Models.ViewModels
{
    public class CreateEmployeeViewModel
    {
        // public Employee Employee {get; set;}
        public int MyProperty { get; set; }
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public String Email { get; set; }  = string.Empty;

        [Required]
        public DateOnly DateOfJoining { get; set; }

        [Required]
        public int DesignationId{get; set;}

        [MinimumElements(1, ErrorMessage = "Please add at least one element.")]
        public List<EmployeeSkillAndLevel> EmployeeSkillAndLevels{get; set;} = new List<EmployeeSkillAndLevel>();

        public List<SelectListItem> DesignationOptions { get; set; }

        public List<SelectListItem> SkillOptions { get; set; }


        // Constructor
        public CreateEmployeeViewModel()
        {
            // Initialize lists if needed
            DesignationOptions = new List<SelectListItem>();
            SkillOptions = new List<SelectListItem>();
            // Employee = new Employee();
        }
    }
}
