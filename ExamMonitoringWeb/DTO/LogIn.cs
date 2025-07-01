using ExamMonitoringWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace ExamMonitoringWeb.DTO
{
    public class LogIn
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters.")]
        public string Password { get; set; }

    }
}
