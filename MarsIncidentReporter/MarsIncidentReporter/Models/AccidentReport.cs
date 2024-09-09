namespace MarsIncidentReporter.Models
{
  public class AccidentReport
  {
        public int Id { get; set; }
        public string ReportedBy { get; set; }
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Severity { get; set; }
        public bool IsResolved { get; set; }
    }
}
