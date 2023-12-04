using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

// Required is default but with no error messasge

namespace SkyLine.Models
{
    public class Department
    {
        public int Id { get; set; }

        // Data validation Attribute
        [DisplayName("Department")]
        [Required(ErrorMessage ="You have to provide a valid name.")]
        [MinLength(2,ErrorMessage ="Name mustn't be less than 2 charachters.")]
        [MaxLength(20,ErrorMessage ="Name mustn't exceed 20 charachters.")]
        public string Name { get; set; }
        
        [Required(ErrorMessage = "You have to provide a valid description.")]
        [MinLength(10, ErrorMessage = "Description mustn't be less than 10 charachters.")]
        [MaxLength(50, ErrorMessage = "Description mustn't exceed 50 charachters.")]
        public string Description { get; set; }

        [DisplayName("Yearly Budget")]
        [Required(ErrorMessage ="You have to provid a valid Annual budget.")]
        [Range(0,500000,ErrorMessage ="Annual budget must be between 0 EGP and 500000 EGP.")]
        public decimal AnnualBudget { get; set; }

        [DisplayName("Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name="Is Active")]
        public bool IsActive { get; set; }

        [ValidateNever]
        public List<Employee> Employees { get; set; }

        [DisplayName("Image")]
        [ValidateNever]
        public string ImagePath { get; set; }
    }
}
