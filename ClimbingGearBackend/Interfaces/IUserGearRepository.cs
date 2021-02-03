using System.Collections.Generic;
using System.Threading.Tasks;
using ClimbingGearBackend.Models;
using System.Security.Claims;

namespace ClimbingGearBackend.Interfaces
{
  public interface IUserGearRepository
  {
    Task AddAsync(UserGear userGear);
    Task DeleteAsync(UserGear userGear);
    Task<List<UserGear>> ListAsync(ClaimsPrincipal currentUser);
    bool Exists(long id);
    Task<UserGear> GetByIdAsync(long id);
    Task SaveChangesAsync();
  }
}
