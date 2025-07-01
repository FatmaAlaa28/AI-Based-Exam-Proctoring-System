namespace ExamMonitoringWeb.DTO
{
    public class Alert
    {
        public string Name { get; set; }
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string ExamName{ get; set; }
        public DateTime Time { get; set; }
    }
}
