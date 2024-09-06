using System.Text.Json.Serialization;

namespace MarsIncidentReporter.Models
{
  public class Launch
  {
    [JsonPropertyName("flight_number")]
    public int FlightNumber { get; set; }

    [JsonPropertyName("mission_name")]
    public string MissionName { get; set; }

    [JsonPropertyName("launch_year")]
    public string LaunchYear { get; set; }

    [JsonPropertyName("launch_date_utc")]
    public DateTime? LaunchDateUtc { get; set; }

    [JsonPropertyName("launch_date_unix")]
    public long? LaunchDateUnix { get; set; }

    [JsonPropertyName("rocket")]
    public Rocket Rocket { get; set; }

    [JsonPropertyName("launch_site")]
    public LaunchSite LaunchSite { get; set; }

    [JsonPropertyName("launch_success")]
    public bool? LaunchSuccess { get; set; }

    [JsonPropertyName("details")]
    public string Details { get; set; }

    [JsonPropertyName("upcoming")]
    public bool Upcoming { get; set; }

    [JsonPropertyName("links")]
    public LaunchLinks Links { get; set; }
  }

  public class Rocket
  {
    [JsonPropertyName("rocket_id")]
    public string RocketId { get; set; }

    [JsonPropertyName("rocket_name")]
    public string RocketName { get; set; }

    [JsonPropertyName("rocket_type")]
    public string RocketType { get; set; }
  }

  public class LaunchSite
  {
    [JsonPropertyName("site_id")]
    public string SiteId { get; set; }

    [JsonPropertyName("site_name")]
    public string SiteName { get; set; }

    [JsonPropertyName("site_name_long")]
    public string SiteNameLong { get; set; }
  }

  public class LaunchLinks
  {
    [JsonPropertyName("mission_patch")]
    public string MissionPatch { get; set; }

    [JsonPropertyName("article_link")]
    public string ArticleLink { get; set; }

    [JsonPropertyName("video_link")]
    public string VideoLink { get; set; }
  }
}
