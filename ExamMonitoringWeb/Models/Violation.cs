using System;
using System.ComponentModel.DataAnnotations;

namespace ExamMonitoringWeb.Models
{
    public class Violation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ViolationType { get; set; }
		public string Direction { get; set; }

		[Required]
        public DateTime Timestamp { get; set; }


        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int ExamId { get; set; }
        public Exam Exam { get; set; }

        public int LabId { get; set; }
        public Lab Lab { get; set; }
    }
}
