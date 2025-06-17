using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExamMonitoringWeb.Models
{
    public class Exam
    {
        [Key]
        public string ExamId { get; set; }

        [Required]
        public string ExamName { get; set; }

        [Required]
        public string ExamLink { get; set; }

        [Required]
        public byte[] ExamPhotosFolder { get; set; }

        public string ExamPklPath { get; set; }

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        // Navigation
        public ICollection<ExamLab> ExamLabs { get; set; }
        public ICollection<StudentExam> StudentExams { get; set; }
        public ICollection<Violation> Violations { get; set; }
    }
}
