using ExamMonitoringWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace ExamMonitoringWeb.DTO
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Full name is required.")]
        [MaxLength(50, ErrorMessage = "Full name cannot exceed 50 characters.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "User name is required.")]
        [MaxLength(50, ErrorMessage = "User name cannot exceed 50 characters.")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Role is required.")]
        public Role Role { get; set; }

        [Required(ErrorMessage = "Phone number is required.")]
        [MaxLength(11, ErrorMessage = "Phone number cannot exceed 11 characters.")]
        [MinLength(10, ErrorMessage = "Phone number cannot less than 10 characters.")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
