using MarsIncidentReporter.Models;

namespace MarsIncidentReporter.Services
{
  public class SpaceXApiService
  {
    private readonly HttpClient _httpClient;

    public SpaceXApiService(HttpClient httpClient)
    {
      _httpClient = httpClient;
    }

    public async Task<IEnumerable<Launch>> GetLaunchesAsync()
    {
      var response = await _httpClient.GetFromJsonAsync<IEnumerable<Launch>>("https://api.spacexdata.com/v3/launches");
      return response;
    }
  }
}
