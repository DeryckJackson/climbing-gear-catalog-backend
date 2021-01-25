using Microsoft.EntityFrameworkCore;

namespace ClimbingGearBackend.Models
{
  public class ClimbingGearContext : DbContext
  {
    public ClimbingGearContext(DbContextOptions<ClimbingGearContext> options)
      : base(options)
    {
    }

    public DbSet<Gear> Gear { get; set; }
    public DbSet<UserGear> UserGear { get; set; }
  }
}
