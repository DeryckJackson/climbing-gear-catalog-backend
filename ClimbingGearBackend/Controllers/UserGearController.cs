using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClimbingGearBackend.Models;

namespace ClimbingGearBackend.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UserGearController : ControllerBase
  {
    private readonly ClimbingGearContext _context;

    public UserGearController(ClimbingGearContext context)
    {
      _context = context;
    }

    // GET: api/UserGear
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserGearDTO>>> GetUserGear(long userId)
    {
      var userGear = await _context.UserGear
        .Where(x => x.UserId == userId)
        .Include(x => x.Gear)
        .ToListAsync();

      var userGearDTOList = new List<UserGearDTO>();
      foreach (UserGear gear in userGear)
      {
        userGearDTOList.Add(UserGearToDTO(gear));
      }

      return userGearDTOList;
    }

    // GET: api/UserGear/5
    [HttpGet("{id}")]
    public async Task<ActionResult<UserGearDTO>> GetSingleUserGear(long userGearId)
    {
      var userGear = await _context.UserGear
        .Include(x => x.Gear)
        .FirstOrDefaultAsync(x => x.UserGearId == userGearId);

      if (userGear == null)
      {
        return NotFound();
      }

      return UserGearToDTO(userGear);
    }

    // PUT: api/UserGear/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGear(long userGearId, UserGear putUserGear)
    {
      if (userGearId != putUserGear.UserGearId)
      {
        return BadRequest();
      }

      var userGear = await _context.UserGear.FindAsync(userGearId);
      if (userGear == null)
      {
        return NotFound();
      }

      userGear.GearId = putUserGear.GearId;
      userGear.UserId = putUserGear.UserId;


      try
      {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException) when (!UserGearExists(userGearId))
      {
        return NotFound();
      }

      return NoContent();
    }

    // POST: api/UserGear
    [HttpPost]
    public async Task<IActionResult> CreateUserGear(long gearId, long userId)
    {
      UserGear userGear = new UserGear
      {
        GearId = gearId,
        UserId = userId,
      };

      _context.UserGear.Add(userGear);
      await _context.SaveChangesAsync();

      return CreatedAtAction(nameof(GetUserGear), new { id = userGear.UserGearId }, userGear);
    }

    // DELETE: api/UserGear/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserGear(long userGearId)
    {
      var userGear = await _context.UserGear.FindAsync(userGearId);
      if (userGear == null)
      {
        return NotFound();
      }

      _context.UserGear.Remove(userGear);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool UserGearExists(long userGearId)
    {
      return _context.UserGear.Any(e => e.UserGearId == userGearId);
    }

    private static UserGearDTO UserGearToDTO(UserGear userGear)
    {
      return new UserGearDTO
      {
        UserGearId = userGear.UserGearId,
        GearId = userGear.Gear.Id,
        Name = userGear.Gear.Name,
        Quantity = userGear.Quantity,
        Description = userGear.Gear.Description,
        Brand = userGear.Gear.Brand,
        WeightGrams = userGear.Gear.WeightGrams,
        LengthMM = userGear.Gear.LengthMM,
        WidthMM = userGear.Gear.WidthMM,
        DepthMM = userGear.Gear.DepthMM,
        Locking = userGear.Gear.Locking
      };
    }
  }
}
