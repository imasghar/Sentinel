using Sentinel.Enums;
using Sentinel.Models;
using System.ComponentModel.DataAnnotations;

namespace Sentinel.DTOs
{
    public class SentinelUserDTO
    {
        [Required]
        [StringLength(25), MinLength(3, ErrorMessage = "First name must be at least 3 characters.")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Only letters allowed")]
        public string FirstName { get; set; } = null!;


        [Required]
        [StringLength(25), MinLength(3, ErrorMessage = "Last name must be at least 3 characters.")]
        [RegularExpression(@"^[A-Za-z]+$", ErrorMessage = "Only letters allowed")]
        public string LastName { get; set; } = null!;

        [Range(1, 3, ErrorMessage = "Please select a valid gender.")]
        public GenderEnum Gender { get; set; }

        [Required]
        [CustomValidation(typeof(SentinelUser), nameof(ValidateDob))]
        public DateTime Dob { get; set; }


        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;



        [StringLength(500)]
        public string? Biography { get; set; }




        [Required]
        [RegularExpression(@"^\d{5}-\d{7}-\d{1}$", ErrorMessage = "CNIC must be in the format 12345-1234567-1.")]
        public string CNIC { get; set; } = null!;




        [Required]
        [RegularExpression(@"^03\d{9}$", ErrorMessage = "Invalid Pakistani phone number")]
        public string PhoneNumber { get; set; } = null!;


        [StringLength(500)]
        public string? Address { get; set; }

        public string? ProfilePicUrl { get; set; }

        public static ValidationResult ValidateDob(DateTime dob, ValidationContext context)
        {
            var today = DateTime.UtcNow.Date;

            if (dob > today)
            {
                return new ValidationResult("Date of Birth cannot be in the future.");
            }

            var age = today.Year - dob.Year;
            if (dob.Date > today.AddYears(-age))
            {
                age--;
            }
            if (age > 120)
            {
                return new ValidationResult("Date of Birth cannot be more than 120 years ago.");
            }
            if (age < 18)
            {
                return new ValidationResult("User must be at least 18 years old.");
            }
            return ValidationResult.Success!;
        }

    }
}
