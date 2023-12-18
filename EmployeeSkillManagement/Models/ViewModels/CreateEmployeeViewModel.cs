using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeSkillManagement.Models
{
    public class CreateEmployeeViewModel
    {
        // public Employee Employee {get; set;}

        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }

        [Required]
        public String? Email { get; set; } 
      
        [Required]
        public DateOnly DateOfJoining { get; set; }

        [Required]
        public int DesignationId{get; set;}

        [Required]
        public List<int>? SkillIds{get; set;}
        [Required]
        public List<int>? SkillLevel{get; set;}

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
