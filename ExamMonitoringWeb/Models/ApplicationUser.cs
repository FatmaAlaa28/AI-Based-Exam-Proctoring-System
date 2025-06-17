using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace ExamMonitoringWeb.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(14)]
        public string NationalId { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        public string Level { get; set; }   

        [Required]
        public string Role { get; set; }

        // Navigation
        public ICollection<StudentExam> StudentExams { get; set; }
        public ICollection<Violation> Violations { get; set; }
    }
}
