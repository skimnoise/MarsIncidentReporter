using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarsIncidentReporter.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class EventController : ControllerBase
  {
    [HttpGet("launches")]
    public IActionResult GetLaunches()
    {
      return Ok("Launches data returned.");
    }
  }
}
