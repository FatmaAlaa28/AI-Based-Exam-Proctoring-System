using System;
using System.ComponentModel.DataAnnotations;
namespace ExamMonitoringWeb.Models
{
    public class ExamLab
    {
        public string ExamId { get; set; }
        public Exam Exam { get; set; }

        public string LabId { get; set; }
        public Lab Lab { get; set; }
    }
}
