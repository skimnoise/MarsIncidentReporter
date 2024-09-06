using MarsIncidentReporter.Models;
using Microsoft.EntityFrameworkCore;

namespace MarsIncidentReporter.Data
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Report> Reports { get; set; }
    public DbSet<Launch> Launches { get; set; }
    public DbSet<LaunchPad> LaunchPads { get; set; }
  }
}


