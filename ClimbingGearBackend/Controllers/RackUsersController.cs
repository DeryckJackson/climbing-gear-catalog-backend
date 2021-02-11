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
  public class RackController : ControllerBase
  {
    private readonly IRackRepository _repository;

    public RackController(IRackRepository repository)
    {
      _repository = repository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserRackDTO>>> GetUserRacks()
    {
      var user = this.User;
      var rackUsers = await _repository.GetUserRacksByUserIdAsync(user);
      var userRacksList = new List<UserRackDTO>();

      foreach (RackUsers rack in rackUsers)
      {
        userRacksList.Add(RackUsersToUserRackDTO(rack));
      }

      return userRacksList;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RackUsersGearDTO>> GetRackUsersGear(long id)
    {
      var rack = await _repository.GetRackByIdAsync(id);

      if (rack == null)
      {
        return NotFound();
      }

      var rackGearList = await _repository.RackGearListAsync(id);

      if (rackGearList.Count == 0)
      {
        return new RackUsersGearDTO
        {
          RackId = rack.Id,
          Name = rack.Name
        };
      }

      return RackGearToUserRackDTO(rackGearList);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUserRack(long id, UserRackDTO putUserRack)
    {
      if (id != putUserRack.RackId)
      {
        return BadRequest();
      }

      var userRack = await _repository.GetRackByIdAsync(id);
      if (userRack == null)
      {
        return NotFound();
      }

      userRack.Name = putUserRack.Name;

      try
      {
        await _repository.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException) when (!RackExists(id))
      {
        return NotFound();
      }

      return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<UserRackDTO>> CreateUserRack(string name)
    {
      var newRack = new Rack
      {
        Name = name
      };

      await _repository.AddRackAsync(newRack);

      var user = this.User;
      var newUserRack = new RackUsers
      {
        RackId = newRack.Id
      };
      await _repository.AddRackUserAsync(user, newUserRack);

      var userRackDTO = new UserRackDTO
      {
        RackId = newUserRack.RackId,
        Name = newRack.Name
      };

      return CreatedAtAction(nameof(GetUserRacks), userRackDTO.RackId, userRackDTO);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUserRack(long id)
    {
      var rack = await _repository.GetRackByIdAsync(id);
      if (rack == null)
      {
        return NotFound();
      }

      await _repository.DeleteRackAsync(rack);

      return NoContent();
    }

    private bool RackExists(long rackId)
    {
      return _repository.Exists(rackId);
    }

    private RackUsersGearDTO RackGearToUserRackDTO(List<RackGear> rackGearList)
    {
      var rackUsersGearDTO = new RackUsersGearDTO();

      rackUsersGearDTO.RackId = rackGearList[0].RackId;
      rackUsersGearDTO.Name = rackGearList[0].Rack.Name;

      foreach (RackGear gear in rackGearList)
      {
        rackUsersGearDTO.RackGearDTOs.Add(RackGearToRackGearDTO(gear));
      }

      return rackUsersGearDTO;
    }

    private RackGearDTO RackGearToRackGearDTO(RackGear rackGear)
    {
      var rackGearDto = new RackGearDTO
      {
        RackId = rackGear.RackId,
        GearId = rackGear.GearId,
        Name = rackGear.Gear.Name,
        Quantity = rackGear.Quantity,
        Description = rackGear.Gear.Description,
        Brand = rackGear.Gear.Brand,
        WeightGrams = rackGear.Gear.WeightGrams,
        LengthMM = rackGear.Gear.LengthMM,
        WidthMM = rackGear.Gear.WidthMM,
        DepthMM = rackGear.Gear.DepthMM,
        Locking = rackGear.Gear.Locking
      };

      return rackGearDto;
    }

    private UserRackDTO RackUsersToUserRackDTO(RackUsers rackusers)
    {
      return new UserRackDTO
      {
        RackId = rackusers.RackId,
        Name = rackusers.Rack.Name
      };
    }
  }
}
