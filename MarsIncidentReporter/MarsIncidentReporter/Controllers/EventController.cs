using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MarsIncidentReporter.Services;
using Microsoft.AspNetCore.Authorization;

namespace MarsIncidentReporter.Controllers
{
    [Authorize(Policy = "AdminOrReader")]
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly SpaceXApiService _spaceXApiService;

        public EventController(SpaceXApiService spaceXApiService)
        {
            _spaceXApiService = spaceXApiService;
        }

        [HttpGet("launches")]
        public async Task<IActionResult> GetLaunches(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string sortBy = "launch_date_utc",
            [FromQuery] bool isAscending = true,
            [FromQuery] string? missionName = null,
            [FromQuery] string? rocketName = null,
            [FromQuery] bool? launchSuccess = null)
        {
            try
            {
                // Get data from SpaceX API
                var launches = await _spaceXApiService.GetLaunchesAsync();

                if (!string.IsNullOrEmpty(missionName))
                {
                    launches = launches.Where(l => l.MissionName.Contains(missionName, StringComparison.OrdinalIgnoreCase));
                }

                if (launches == null)
                {
                    return NotFound("No launches found.");
                }

                if (!string.IsNullOrEmpty(missionName))
                {
                    launches = launches.Where(l => l.MissionName.Contains(missionName, StringComparison.OrdinalIgnoreCase));
                }

                if (!string.IsNullOrEmpty(rocketName))
                {
                    launches = launches.Where(l => l.Rocket?.RocketName.Contains(rocketName, StringComparison.OrdinalIgnoreCase) == true);
                }

                if (launchSuccess.HasValue)
                {
                    launches = launches.Where(l => l.LaunchSuccess == launchSuccess.Value);
                }

                launches = isAscending
                  ? launches.OrderBy(l => GetPropertyValue(l, sortBy))
                  : launches.OrderByDescending(l => GetPropertyValue(l, sortBy));

                var totalItems = launches.Count();
                var pagedLaunches = launches.Skip((pageNumber - 1) * pageSize).Take(pageSize);

                var response = new
                {
                    TotalItems = totalItems,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    Launches = pagedLaunches
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        private object GetPropertyValue(object obj, string propertyName)
        {
            return obj.GetType().GetProperty(propertyName)?.GetValue(obj, null);
        }


        [HttpGet("launchpads")]
        public async Task<IActionResult> GetLaunchPads()
        {
            var launchPads = await _spaceXApiService.GetLaunchPadsAsync();
            return Ok(launchPads);
        }

        [HttpGet("upcoming-launches")]
        public async Task<IActionResult> GetUpcomingLaunches()
        {
            var upcomingLaunches = await _spaceXApiService.GetUpcomingLaunchesAsync();
            return Ok(upcomingLaunches);
        }

        [HttpGet("capsules")]
        public async Task<IActionResult> GetCapsules()
        {
            var capsules = await _spaceXApiService.GetCapsulesAsync();
            return Ok(capsules);
        }
    }
}
