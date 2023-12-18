using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using EmployeeSkillManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EmployeeSkillManagement.Data
{
    public class ApplicationDbContext: IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Employee> Employees { get; set; }
        
        public DbSet<EmployeeSkillAndLevel> EmployeeSkillAndLevels { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Designation> Designations { get; set; }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder){
            string[] roles = {
                "CEO",
                "CTO",
                "CIO/Chief Digital Officer/Chief Innovation Officer",
                "VP of Product Management/Head of Product",
                "Product Manager",
                "VP of Marketing",
                "VP of Engineering/Director of Engineering",
                "Chief Architect",
                "Software Architect",
                "Engineering Project Manager/Engineering Manager",
                "Technical Lead/Engineering Lead/Team Lead",
                "Principal Software Engineer",
                "Senior Software Engineer/Senior Software Developer",
                "Software Engineer",
                "Software Developer",
                "Junior Software Developer",
                "Intern Software Developer"
            };
            base.OnModelCreating(modelBuilder);

            for(int i = 0; i < roles.Length; i++)
            {
                modelBuilder.Entity<Designation>().HasData(
                    new Designation{Id=i+1, DesignationName=roles[i]}
                );
            }

            //  modelBuilder.Entity<Employee>()
            //     .HasMany(e => e.EmployeeSkillsAndLevels)
            //     .WithOne()
            //     .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Skill>()
                .HasMany(s => s.EmployeeSkillAndLevels)
                .WithOne()
                .OnDelete(DeleteBehavior.Cascade);

        }
            
    }

   
}