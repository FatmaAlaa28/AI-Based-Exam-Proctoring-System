using System.ComponentModel.DataAnnotations;

namespace ExamMonitoringWeb.DTO
{
    public class ExamAnalysis
    {
        public string ExamName { get; set; }
        public DateOnly Date { get; set; }
        public int Duration { get; set; }
        public int ViolationCount { get; set; }
        public string Status { get; set; }
    }
}
