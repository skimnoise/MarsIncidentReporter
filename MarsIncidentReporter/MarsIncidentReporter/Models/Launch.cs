using System.Text.Json.Serialization;

namespace MarsIncidentReporter.Models
{
  public class Launch
  {
    [JsonPropertyName("mission_name")]
    public string MissionName { get; set; }

    [JsonPropertyName("launch_date_utc")]
    public string LaunchDateUtc { get; set; }

    [JsonPropertyName("rocket")]
    public Rocket Rocket { get; set; }

    [JsonPropertyName("launch_site")]
    public LaunchSite LaunchSite { get; set; }

  }

  public class Rocket
  {
    [JsonPropertyName("rocket_name")]
    public string RocketName { get; set; }
  }

  public class LaunchSite
  {
    [JsonPropertyName("site_name")]
    public string SiteName { get; set; }
  }
}
