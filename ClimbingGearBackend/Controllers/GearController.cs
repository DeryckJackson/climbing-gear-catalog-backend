using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClimbingGearBackend.Models;
using ClimbingGearBackend.Infrastucture;

namespace ClimbingGearBackend.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class GearController : ControllerBase
  {
    private readonly EFGearRepository _repository;

    public GearController(EFGearRepository repository)
    {
      _repository = repository;
    }

    // GET: api/Gear
    [HttpGet]
    public async Task<ActionResult<IEnumerable<GearDTO>>> GetGear()
    {
      return await _repository.DTOListAsync();
    }

    // GET: api/Gear/5
    [HttpGet("{id}")]
    public async Task<ActionResult<GearDTO>> GetGear(long id)
    {
      var gear = await _repository.GetByIdAsync(id);

      if (gear == null)
      {
        return NotFound();
      }

      return Gear.GearToDTO(gear);
    }

    // PUT: api/Gear/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateGear(long id, GearDTO gearDTO)
    {
      if (id != gearDTO.Id)
      {
        return BadRequest();
      }

      var gear = await _repository.GetByIdAsync(id);
      if (gear == null)
      {
        return NotFound();
      }

      gear.Name = gearDTO.Name;
      gear.Description = gearDTO.Description;
      gear.Brand = gearDTO.Brand;
      gear.WeightGrams = gearDTO.WeightGrams;
      gear.LengthMM = gearDTO.LengthMM;
      gear.WidthMM = gearDTO.WidthMM;
      gear.DepthMM = gearDTO.DepthMM;
      gear.Locking = gearDTO.Locking;

      try
      {
        await _repository.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException) when (!_repository.Exists(id))
      {
        return NotFound();
      }

      return NoContent();
    }

    // POST: api/Gear
    [HttpPost]
    public async Task<ActionResult<GearDTO>> CreateGear(GearDTO gearDTO)
    {
      Gear gear = new Gear
      {
        Name = gearDTO.Name,
        Description = gearDTO.Description,
        Brand = gearDTO.Brand,
        WeightGrams = gearDTO.WeightGrams,
        LengthMM = gearDTO.LengthMM,
        WidthMM = gearDTO.WidthMM,
        DepthMM = gearDTO.DepthMM,
        Locking = gearDTO.Locking,
      };

      await _repository.AddAsync(gear);

      return CreatedAtAction(nameof(GetGear), new { id = gear.Id }, Gear.GearToDTO(gear));
    }

    // DELETE: api/Gear/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteGear(long id)
    {
      var gear = await _repository.GetByIdAsync(id);
      if (gear == null)
      {
        return NotFound();
      }

      await _repository.DeleteAsync(gear);

      return NoContent();
    }
  }
}
