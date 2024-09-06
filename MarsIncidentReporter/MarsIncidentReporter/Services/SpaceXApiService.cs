using MarsIncidentReporter.Models;

namespace MarsIncidentReporter.Services
{
  public class SpaceXApiService
  {
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "https://api.spacexdata.com/v3/";

    public SpaceXApiService(HttpClient httpClient)
    {
      _httpClient = httpClient;
    }

    public async Task<IEnumerable<Launch>> GetLaunchesAsync()
    {
      return await GetItemsAsync<Launch>(_baseUrl + "launches");
    }

    public async Task<IEnumerable<LaunchPad>> GetLaunchPadsAsync()
    {
      return await GetItemsAsync<LaunchPad>(_baseUrl + "launchpads");
    }

    public async Task<IEnumerable<Launch>> GetUpcomingLaunchesAsync()
    {
      return await GetItemsAsync<Launch>(_baseUrl + "launches/upcoming");
    }

    public async Task<IEnumerable<Capsule>> GetCapsulesAsync()
    {
      return await GetItemsAsync<Capsule>(_baseUrl + "capsules");
    }
    private async Task<IEnumerable<T>> GetItemsAsync<T>(string url)
    {
      var response = await _httpClient.GetFromJsonAsync<IEnumerable<T>>(url);
      return response ?? Enumerable.Empty<T>();
    }
  }
}
