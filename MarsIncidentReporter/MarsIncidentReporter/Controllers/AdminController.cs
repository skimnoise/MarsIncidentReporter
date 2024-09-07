using MarsIncidentReporter.Data;
using MarsIncidentReporter.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MarsIncidentReporter.Controllers
{
  [Authorize(Roles = "Admin")]
  [ApiController]
  [Route("api/[controller]")]
  public class AdminController : ControllerBase
  {
    private readonly AppDbContext _context;
    public AdminController(AppDbContext context)
    {
      _context = context;
    }

    [HttpPost("create")]
    public IActionResult CreateReport(Report report)
    {
      report.DateCreated = DateTime.UtcNow;
      _context.Reports.Add(report);
      _context.SaveChanges();
      return Ok("Report created successfully.");
    }

    [HttpPut("update/{id}")]
    public IActionResult UpdateReport(int id, Report report)
    {
      var existingReport = _context.Reports.Find(id);
      if (existingReport == null)
      {
        return NotFound($"Report with ID# {id} not found.");
      }
      existingReport.Title = report.Title;
      existingReport.Description = report.Description;
      _context.SaveChanges();
      return Ok($"Report ID# {id} updated successfully.");
    }

    [HttpDelete("delete/{id}")]
    public IActionResult DeleteReport(int id)
    {
      var report = _context.Reports.Find(id);
      if (report == null)
      {
        return NotFound($"Report with ID# {id} not found.");
      }
      _context.Reports.Remove(report);
      _context.SaveChanges();
      return Ok($"Report ID# {id} deleted successfully.");
    }

    [HttpGet("get")]
    public IActionResult GetReports()
    {
      var reports = _context.Reports.ToList();
      return Ok(reports);
    }
  }
}
