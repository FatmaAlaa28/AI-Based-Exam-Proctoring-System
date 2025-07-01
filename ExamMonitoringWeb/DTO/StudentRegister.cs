using ExamMonitoringWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace ExamMonitoringWeb.DTO
{
    public class StudentRegister
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

        [Required(ErrorMessage = "Department is required.")]
        public Department Department { get; set; }

        [Required(ErrorMessage = "National ID is required.")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "National ID must be exactly 14 digits.")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "National ID must consist of 14 digits only.")]
        public string NationalId { get; set; }
    }
}
