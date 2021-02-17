using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClimbingGearBackend.Models;
using ClimbingGearBackend.Interfaces;

namespace ClimbingGearBackend.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  // TODO: #5 Enable Authorization and have DB querys only return logged in User's Gear
  public class UserGearController : ControllerBase
  {
    private readonly IUserGearRepository _repository;

    public UserGearController(IUserGearRepository repository)
    {
      _repository = repository;
    }

    // GET: api/UserGear
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserGearDTO>>> GetUserGear()
    {
      var currentUser = this.User;
      var userGear = await _repository.ListAsync(currentUser);

      var userGearDTOList = new List<UserGearDTO>();
      foreach (UserGear gear in userGear)
      {
        userGearDTOList.Add(UserGearToDTO(gear));
      }

      return userGearDTOList;
    }

    // GET: api/UserGear/5
    [HttpGet("{id}")]
    public async Task<ActionResult<UserGearDTO>> GetUserGear(long id)
    {
      var userGear = await _repository.GetByIdAsync(id);

      if (userGear == null)
      {
        return NotFound();
      }

      return UserGearToDTO(userGear);
    }

    // PUT: api/UserGear/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGear(long id, UserGear putUserGear)
    {
      if (id != putUserGear.Id)
      {
        return BadRequest();
      }

      var userGear = await _repository.GetByIdAsync(id);
      if (userGear == null)
      {
        return NotFound();
      }

      userGear.GearId = putUserGear.GearId;
      userGear.UserId = putUserGear.UserId;
      userGear.Quantity = putUserGear.Quantity;

      try
      {
        await _repository.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException) when (!UserGearExists(id))
      {
        return NotFound();
      }

      return NoContent();
    }

    // POST: api/UserGear
    [HttpPost]
    public async Task<ActionResult<UserGear>> CreateUserGear(UserGear postGear)
    {
      UserGear userGear = new UserGear
      {
        GearId = postGear.GearId,
        UserId = postGear.UserId,
        Quantity = postGear.Quantity
      };

      await _repository.AddAsync(userGear);

      return CreatedAtAction(nameof(GetUserGear), new { id = userGear.Id }, userGear);
    }

    // DELETE: api/UserGear/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserGear(long id)
    {
      var userGear = await _repository.GetByIdAsync(id);
      if (userGear == null)
      {
        return NotFound();
      }

      await _repository.DeleteAsync(userGear);

      return NoContent();
    }

    private bool UserGearExists(long userGearId)
    {
      return _repository.Exists(userGearId);
    }

    private static UserGearDTO UserGearToDTO(UserGear userGear)
    {
      return new UserGearDTO
      {
        Id = userGear.Id,
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
