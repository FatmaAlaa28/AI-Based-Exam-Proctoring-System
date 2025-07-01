using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace ExamMonitoringWeb.Models
{
    public enum Role
    {
        Supervisor,
        TeachingAssistant,
        Student
    }

    public enum Level
    {
        First,
        Second,
        Third,
        Fourth
    }
    public enum Department
    {
        General,
        CS,
        IS,
        IT,
        MM,
        SE,
        AI
    }
    public class ApplicationUser : IdentityUser<int>
    {
        [Required]
        [MaxLength(50)]
        public string FullName { get; set; }

        [MaxLength(14)]
        public string? NationalId { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        public Level Level { get; set; }

        [Required]
        public Role Role { get; set; }

        public Department Department { get; set; }

        // Navigation
        public ICollection<StudentExam> StudentExams { get; set; }
        public ICollection<Violation> Violations { get; set; }
        public ICollection<ObserverLabs> ObserverLabs { get; set; }
    }
}