using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MarsIncidentReporter.Services;

namespace MarsIncidentReporter.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class EventController : ControllerBase
  {
    private readonly SpaceXApiService _spaceXApiService;

    public EventController(SpaceXApiService spaceXApiService)
    {
      _spaceXApiService = spaceXApiService;
    }

    [HttpGet("launches")]
    public async Task<IActionResult> GetLaunches()
    {
      var launches = await _spaceXApiService.GetLaunchesAsync();
      return Ok(launches);
    }
  }
}
