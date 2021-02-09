using Microsoft.EntityFrameworkCore;
using IdentityServer4.EntityFramework.Options;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.Extensions.Options;
using ClimbingGearBackend.Models;

namespace ClimbingGearBackend.Infrastucture
{
  public class ClimbingGearContext : ApiAuthorizationDbContext<User>
  {
    public ClimbingGearContext(
      DbContextOptions<ClimbingGearContext> options,
      IOptions<OperationalStoreOptions> operationalStoreOptions)
      : base(options, operationalStoreOptions)
    {
    }

    public DbSet<Gear> Gear { get; set; }
    public DbSet<UserGear> UserGear { get; set; }
    public DbSet<User> User { get; set; }
    public DbSet<Rack> Rack { get; set; }
    public DbSet<RackGear> RackGear { get; set; }
    public DbSet<RackUsers> RackUsers { get; set; }
  }
}
