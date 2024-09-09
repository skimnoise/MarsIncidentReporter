using MarsIncidentReporter.Data;
using MarsIncidentReporter.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MarsIncidentReporter.Controllers
{
    [Authorize(Policy = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Admin/AccidentReports
        [HttpGet("AccidentReports")]
        public async Task<ActionResult<IEnumerable<AccidentReport>>> GetAccidentReports()
        {
            return await _context.AccidentReports.ToListAsync();
        }

        // GET: api/Admin/AccidentReports/5
        [HttpGet("AccidentReports/{id}")]
        public async Task<ActionResult<AccidentReport>> GetAccidentReport(int id)
        {
            var report = await _context.AccidentReports.FindAsync(id);

            if (report == null)
            {
                return NotFound();
            }

            return report;
        }

        // POST: api/Admin/AccidentReports
        [HttpPost("AccidentReports")]
        public async Task<ActionResult<AccidentReport>> CreateAccidentReport(AccidentReport report)
        {
            _context.AccidentReports.Add(report);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAccidentReport), new { id = report.Id }, report);
        }

        // PUT: api/Admin/AccidentReports/5
        [HttpPut("AccidentReports/{id}")]
        public async Task<IActionResult> UpdateAccidentReport(int id, AccidentReport report)
        {
            if (id != report.Id)
            {
                return BadRequest();
            }

            _context.Entry(report).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccidentReportExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Admin/AccidentReports/5
        [HttpDelete("AccidentReports/{id}")]
        public async Task<IActionResult> DeleteAccidentReport(int id)
        {
            var report = await _context.AccidentReports.FindAsync(id);
            if (report == null)
            {
                return NotFound();
            }

            _context.AccidentReports.Remove(report);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AccidentReportExists(int id)
        {
            return _context.AccidentReports.Any(e => e.Id == id);
        }
    }
}
