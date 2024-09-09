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
        public string LaunchId { get; set; } 
        public string LaunchName { get; set; }
        public DateTime LaunchDate { get; set; }
        public string LaunchpadName { get; set; }
        public string CapsuleSerial { get; set; }
        public string CapsuleName { get; set; }
    }
}
