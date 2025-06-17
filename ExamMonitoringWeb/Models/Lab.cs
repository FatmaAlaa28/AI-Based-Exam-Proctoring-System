using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ExamMonitoringWeb.Models
{
    public class Lab
    {
        [Key]
        public string LabId { get; set; }

        [Required]
        public string LabName { get; set; }

        public string Location { get; set; }

        // Navigation
        public ICollection<ExamLab> ExamLabs { get; set; }
    }
}
