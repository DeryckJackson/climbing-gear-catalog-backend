using System.Collections.Generic;
using System.Threading.Tasks;
using ClimbingGearBackend.Models;

namespace ClimbingGearBackend.Interfaces
{
  public interface IGearRepository
  {
    // Adds new Gear to the Database
    Task AddAsync(Gear gear);
    // Deletes selected Gear from the Database
    Task DeleteAsync(Gear gear);
    // Returns a list of Gear, if no gear exists returns empty list 
    Task<List<GearDTO>> DTOListAsync();
    // Checks if a Gear exists by looking up it's ID, returns a boolean
    bool Exists(long id);
    // Looks up related Gear by ID and returns it, if ID is invalid returns null. 
    Task<Gear> GetByIdAsync(long id);
    // Saves changes to the Database, currently only used for PUT actions
    Task SaveChangesAsync();
  }
}
