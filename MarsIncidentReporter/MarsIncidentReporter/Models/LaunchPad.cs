using System.Text.Json.Serialization;

namespace MarsIncidentReporter.Models
{
  public class LaunchPad
  {
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("site_id")]
    public string SiteId { get; set; }

    [JsonPropertyName("site_name")]
    public string SiteName { get; set; }

    [JsonPropertyName("site_name_long")]
    public string SiteNameLong { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("location")]
    public LaunchPadLocation Location { get; set; }

    [JsonPropertyName("vehicles_launched")]
    public string[] VehiclesLaunched { get; set; }

    [JsonPropertyName("attempted_launches")]
    public int AttemptedLaunches { get; set; }

    [JsonPropertyName("successful_launches")]
    public int SuccessfulLaunches { get; set; }

    [JsonPropertyName("wikipedia")]
    public string Wikipedia { get; set; }

    [JsonPropertyName("details")]
    public string Details { get; set; }
  }

  public class LaunchPadLocation
  {
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("region")]
    public string Region { get; set; }

    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }
  }
}
