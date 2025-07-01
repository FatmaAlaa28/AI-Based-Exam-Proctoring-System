using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace ExamMonitoringWeb.Models
{
    public class Lab
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string LabName { get; set; }

        public Level Location { get; set; }

        // Navigation
        public ICollection<ExamLab> ExamLabs { get; set; }
        public ICollection<ObserverLabs> ObserverLabs { get; set; }
    }
}
