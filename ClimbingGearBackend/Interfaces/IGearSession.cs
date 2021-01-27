using System.Collections.Generic;
using System.Threading.Tasks;
using ClimbingGearBackend.Models;

namespace ClimbingGearBackend.Interfaces
{
  public interface IGearRepository
  {
    Task AddAsync(Gear gear);
    Task DeleteAsync(Gear gear);
    Task<List<GearDTO>> DTOListAsync();
    bool Exists(long id);
    Task<Gear> GetByIdAsync(long id);
    Task SaveChangesAsync();
  }
}
