using System.Text.Json.Serialization;

namespace MarsIncidentReporter.Models
{
  public class Capsule
  {
    [JsonPropertyName("capsule_serial")]
    public string CapsuleSerial { get; set; }

    [JsonPropertyName("capsule_id")]
    public string CapsuleId { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("original_launch")]
    public DateTime? OriginalLaunch { get; set; }

    [JsonPropertyName("original_launch_unix")]
    public long? OriginalLaunchUnix { get; set; }

    [JsonPropertyName("missions")]
    public List<CapsuleMission> Missions { get; set; }

    [JsonPropertyName("landings")]
    public int Landings { get; set; }

    [JsonPropertyName("type")]
    public string Type { get; set; }

    [JsonPropertyName("details")]
    public string Details { get; set; }

    [JsonPropertyName("reuse_count")]
    public int ReuseCount { get; set; }
  }

  public class CapsuleMission
  {
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("flight")]
    public int Flight { get; set; }
  }
}
