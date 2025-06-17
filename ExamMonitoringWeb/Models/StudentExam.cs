using System;
using System.ComponentModel.DataAnnotations;

namespace ExamMonitoringWeb.Models
{
    public class StudentExam
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string ExamId { get; set; }
        public Exam Exam { get; set; }
    }
}
