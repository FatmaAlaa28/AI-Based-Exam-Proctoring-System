using ExamMonitoringWeb.Models;

namespace ExamMonitoringWeb.DTO
{
    public class StudentAnalysis
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int NumofExams { get; set; }
        public int Alerts { get; set; }
        public Department Department { get; set; }

        public ExamAnalysis[] exams { get; set; }

    }
}
