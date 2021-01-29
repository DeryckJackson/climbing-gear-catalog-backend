using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ClimbingGearBackend.Models;
using ClimbingGearBackend.Interfaces;

namespace ClimbingGearBackend.Infrastucture
{
  public class EFGearRepository : IGearRepository
  {
    private readonly ClimbingGearContext _dbContext;

    public EFGearRepository(ClimbingGearContext dbContext)
    {
      _dbContext = dbContext;
    }

    public bool Exists(long id)
    {
      return _dbContext.Gear.Any(e => e.Id == id);
    }

    public Task AddAsync(Gear gear)
    {
      _dbContext.Gear.Add(gear);
      return _dbContext.SaveChangesAsync();
    }

    public Task DeleteAsync(Gear gear)
    {
      _dbContext.Gear.Remove(gear);
      return _dbContext.SaveChangesAsync();
    }

    public Task<List<GearDTO>> DTOListAsync()
    {
      return _dbContext.Gear
        .Select(x => Gear.GearToDTO(x))
        .ToListAsync();
    }

    public Task<Gear> GetByIdAsync(long id)
    {
      return _dbContext.Gear.FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task SaveChangesAsync()
    {
      return _dbContext.SaveChangesAsync();
    }
  }
}
