using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ExamMonitoringWeb.Models
{
    public class ExamLab
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ExamId { get; set; }
        public Exam Exam { get; set; }

        public int LabId { get; set; }
        public Lab Lab { get; set; }
    }
}
