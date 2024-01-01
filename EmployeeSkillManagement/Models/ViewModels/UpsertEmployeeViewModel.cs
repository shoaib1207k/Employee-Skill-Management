using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EmployeeSkillManagement.Models.ViewModels
{
    public class UpsertEmployeeViewModel
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [DisplayName("First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required")]
        [DisplayName("Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Designation is required")]
        [DisplayName("Designation")]
        public string DesignationId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [DisplayName("Email Address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of Joining is required")]
        [DisplayName("Date of Joining")]
        public DateOnly DateOfJoining { get; set; }

        [Required(ErrorMessage = "Please add at least one skill")]
        public List<EmployeeSkillAndLevel> EmployeeSkillsAndLevels { get; set; } = new List<EmployeeSkillAndLevel>();
        public List<SelectListItem> DesignationOptions { get; set; }

        public List<SelectListItem> SkillOptions { get; set; }


        // Constructor
        public UpsertEmployeeViewModel()
        {
            // Initialize lists if needed
            DesignationOptions = new List<SelectListItem>();
            SkillOptions = new List<SelectListItem>();
            // Employee = new Employee();
        }
    }
}
