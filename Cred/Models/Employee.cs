using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Cred.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Employee Number is required")]
        [Display(Name = "Employee No")]
        public string EmployeeNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of Birth is required")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; } = string.Empty;

        [Display(Name = "Place of Birth")]
        public string PlaceOfBirth { get; set; } = string.Empty;
        public string PlaceOfBirth2 { get; set; } = string.Empty;
        // Navigation property for previous employments
        public virtual ICollection<PreviousEmployment> PreviousEmployments { get; set; } = new List<PreviousEmployment>();
    }
}
