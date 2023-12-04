using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SkyLine.Models
{
    // cards and tables
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage="This Field Cannot be empty")]
        [MinLength(2,ErrorMessage ="Name must be between 2 and 50 charachters")]
        [MaxLength(50,ErrorMessage = "Name must be between 2 and 50 charachters")]
        [DisplayName("Employee Name")]
        public string FullName { get; set; }

        [Required(ErrorMessage ="This Property Cannot be empty!")]
        [Range(0,80000,ErrorMessage ="Salary must be between 0 and 80000 EGP")]
        [DisplayName("Salary")]
        public decimal Salary { get; set; }

        [Required(ErrorMessage ="Birth date cannot be empty")]
        [DisplayName("Birth Date")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "Join date cannot be empty")]
        [DisplayName("Join Date")]
        public DateTime JoinDate { get; set; }
        public bool IsActive { get; set; }

        // foreign Key
        [Range(1, double.MaxValue, ErrorMessage ="Select a valid department!")]
        [DisplayName("Department")]
        public int DepartmentId { get; set; }

        [ValidateNever] // Don't Validate
        // Navigation Property (tells about a possible relationship)
        public Department Department { get; set; }

        [Required(ErrorMessage="Position cannot be empty!")]
        public string Position { get; set; }

        [ValidateNever]
        [DisplayName("Image")]
        public string ImagePath { get; set; }
    }
}
