using System.Linq;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ClimbingGearBackend.Models;
using ClimbingGearBackend.Infrastucture;

namespace ClimbingGearBackend.Interfaces
{
  public class EFRackRepository : IRackRepository
  {
    private readonly ClimbingGearContext _dbContext;
    private UserManager<User> _userManager;
    public EFRackRepository(ClimbingGearContext dbContext,
      UserManager<User> userManager)
    {
      _dbContext = dbContext;
      _userManager = userManager;
    }
    public Task AddRackAsync(Rack rack)
    {
      _dbContext.Rack.Add(rack);
      return _dbContext.SaveChangesAsync();
    }

    public Task AddRackGearAsync(RackGear rackGear)
    {
      _dbContext.RackGear.Add(rackGear);
      return _dbContext.SaveChangesAsync();
    }

    public Task AddRackUserAsync(ClaimsPrincipal currentUser, long rackId)
    {
      var rackUser = new RackUsers
      {
        RackId = rackId,
        UserId = _userManager.GetUserId(currentUser)
      };

      _dbContext.RackUsers.Add(rackUser);
      return _dbContext.SaveChangesAsync();
    }

    public Task DeleteRackAsync(Rack rack)
    {
      _dbContext.Rack.Remove(rack);
      return _dbContext.SaveChangesAsync();
    }

    public Task DeleteRackGearAsync(RackGear rackGear)
    {
      _dbContext.RackGear.Remove(rackGear);
      return _dbContext.SaveChangesAsync();
    }

    public Task DeleteRackUserAsync(RackUsers rackUser)
    {
      _dbContext.RackUsers.Remove(rackUser);
      return _dbContext.SaveChangesAsync();
    }

    public Task<Rack> GetRackByIdAsync(long id)
    {
      return _dbContext.Rack.FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<RackGear> GetRackGearByIdsAsync(long rackId, long gearId)
    {
      return _dbContext.RackGear
        .FirstOrDefaultAsync(x => x.RackId == rackId && x.GearId == gearId);
    }

    public Task<List<RackUsers>> GetUserRacksByUserId(ClaimsPrincipal currentUser)
    {
      return _dbContext.RackUsers
        .Where(x => x.UserId == _userManager.GetUserId(currentUser))
        .ToListAsync();
    }

    public Task<List<RackGear>> RackGearListAsync(long rackId)
    {
      return _dbContext.RackGear
        .Where(x => x.RackId == rackId)
        .Include(x => x.Gear)
        .ToListAsync();
    }

    public Task SaveChangesAsync()
    {
      return _dbContext.SaveChangesAsync();
    }
  }
}
