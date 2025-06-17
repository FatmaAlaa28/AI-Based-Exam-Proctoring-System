using System;
using System.ComponentModel.DataAnnotations;

namespace ExamMonitoringWeb.Models
{
    public class Violation
    {
        [Key]
        public int ViolationId { get; set; }

        [Required]
        public string ViolationType { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string ExamId { get; set; }
        public Exam Exam { get; set; }
    }
}
