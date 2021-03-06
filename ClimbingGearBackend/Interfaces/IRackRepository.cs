using System.Threading.Tasks;
using ClimbingGearBackend.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace ClimbingGearBackend.Interfaces
{
  public interface IRackRepository
  {
    Task AddRackAsync(Rack rack);
    Task AddRackGearAsync(RackGear rackGear);
    Task AddRackUserAsync(ClaimsPrincipal currentUser, RackUsers rackUser);
    Task DeleteRackAsync(Rack rack);
    Task DeleteRackGearAsync(RackGear rackGear);
    Task DeleteRackUserAsync(RackUsers rackUser);
    bool Exists(long id);
    Task<Rack> GetRackByIdAsync(long id);
    Task<RackGear> GetRackGearByIdsAsync(long rackId, long gearId);
    Task<List<RackUsers>> GetUserRacksByUserIdAsync(ClaimsPrincipal currentUser);
    Task<List<RackGear>> RackGearListAsync(long rackId);
    Task SaveChangesAsync();
  }
}
