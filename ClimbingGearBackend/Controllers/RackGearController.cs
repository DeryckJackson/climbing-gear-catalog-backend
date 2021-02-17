using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClimbingGearBackend.Models;
using ClimbingGearBackend.Interfaces;

namespace ClimbingGearBackend.Controllers
{
  [Route("api/rack/{rackId}/gear")]
  [ApiController]
  public class RackGearController : ControllerBase
  {
    private readonly IRackRepository _repository;

    public RackGearController(IRackRepository repository)
    {
      _repository = repository;
    }

    [HttpGet("{gearId}")]
    public async Task<ActionResult<RackGearDTO>> GetRackGear(long rackId, long gearId)
    {
      var gear = await _repository.GetRackGearByIdsAsync(rackId, gearId);

      if (gear == null)
      {
        return NotFound();
      }

      return RackGearToRackGearDTO(gear);
    }

    [HttpPut("{gearId}")]
    public async Task<IActionResult> UpdateRackGear(long rackId, long gearId, RackGear putRackGear)
    {
      if (rackId != putRackGear.RackId || gearId != putRackGear.GearId)
      {
        return BadRequest();
      }

      var gear = await _repository.GetRackGearByIdsAsync(rackId, gearId);
      if (gear == null)
      {
        return NotFound();
      }

      gear.Quantity = putRackGear.Quantity;

      try
      {
        await _repository.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException) when (!RackExists(rackId))
      {
        return NotFound();
      }

      return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<RackGearDTO>> CreateRackGear(RackGear rackGear)
    {
      if (!RackExists(rackGear.RackId))
      {
        return NotFound();
      }

      await _repository.AddRackGearAsync(rackGear);

      return NoContent();
    }

    [HttpDelete("{gearId}")]
    public async Task<IActionResult> DeleteRackGear(long rackId, long gearId)
    {
      var gear = await _repository.GetRackGearByIdsAsync(rackId, gearId);
      if (gear == null)
      {
        return NotFound();
      }

      await _repository.DeleteRackGearAsync(gear);

      return NoContent();
    }

    private bool RackExists(long rackId)
    {
      return _repository.Exists(rackId);
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
  }
}
