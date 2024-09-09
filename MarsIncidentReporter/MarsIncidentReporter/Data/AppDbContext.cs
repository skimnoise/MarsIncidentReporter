using MarsIncidentReporter.Models;
using Microsoft.EntityFrameworkCore;

namespace MarsIncidentReporter.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<AccidentReport> AccidentReports { get; set; }
    }
}
