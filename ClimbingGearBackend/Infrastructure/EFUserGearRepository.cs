using System.Collections.Generic;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ClimbingGearBackend.Models;
using ClimbingGearBackend.Interfaces;

namespace ClimbingGearBackend.Infrastucture
{
  public class EFUserGearRepository : IUserGearRepository
  {
    private readonly ClimbingGearContext _dbContext;
    private UserManager<User> _userManager;

    public EFUserGearRepository(ClimbingGearContext dbContext,
      UserManager<User> userManager)
    {
      _dbContext = dbContext;
      _userManager = userManager;
    }

    public Task AddAsync(UserGear userGear)
    {
      _dbContext.UserGear.Add(userGear);
      return _dbContext.SaveChangesAsync();
    }

    public Task DeleteAsync(UserGear userGear)
    {
      _dbContext.UserGear.Remove(userGear);
      return _dbContext.SaveChangesAsync();
    }

    public bool Exists(long id)
    {
      return _dbContext.UserGear.Any(e => e.Id == id);
    }

    public Task<List<UserGear>> ListAsync(ClaimsPrincipal currentUser)
    {
      return _dbContext.UserGear
        .Where(x => x.UserId == _userManager.GetUserId(currentUser))
        .Include(x => x.Gear)
        .ToListAsync();
    }

    public Task<UserGear> GetByIdAsync(long id)
    {
      return _dbContext.UserGear.FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task SaveChangesAsync()
    {
      return _dbContext.SaveChangesAsync();
    }
  }
}
