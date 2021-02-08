using System.Collections.Generic;
using System.Threading.Tasks;
using ClimbingGearBackend.Models;
using System.Security.Claims;

namespace ClimbingGearBackend.Interfaces
{
  public interface IUserGearRepository
  {
    // Adds new UserGear to the Database
    Task AddAsync(UserGear userGear);
    // Deletes selected UserGear from the Database
    Task DeleteAsync(UserGear userGear);
    /* Returns a list of current user's UserGear with related Gear loaded,
    if user has no gear returns empty list */
    Task<List<UserGear>> ListAsync(ClaimsPrincipal currentUser);
    // Checks if a UserGear exists by looking up it's ID, returns a boolean
    bool Exists(long id);
    // Looks up related UserGear by ID and returns it, if ID is invalid returns null. 
    Task<UserGear> GetByIdAsync(long id);
    // Saves changes to the Database, currently only used for PUT actions
    Task SaveChangesAsync();
  }
}
