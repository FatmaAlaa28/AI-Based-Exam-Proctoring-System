using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamMonitoringWeb.Models
{
    public class StudentExam
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int ExamId { get; set; }
        public Exam Exam { get; set; }
    }
}
