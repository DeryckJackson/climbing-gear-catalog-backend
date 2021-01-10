using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClimbingGearBackend.Models;

namespace ClimbingGearBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GearController : ControllerBase
    {
        private readonly ClimbingGearContext _context;

        public GearController(ClimbingGearContext context)
        {
            _context = context;
        }

        // GET: api/Gear
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GearDTO>>> GetGear()
        {
            return await _context.Gear
                .Select(x => GearToDTO(x))
                .ToListAsync();
        }

        // GET: api/Gear/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GearDTO>> GetGear(long id)
        {
            var gear = await _context.Gear.FindAsync(id);

            if (gear == null)
            {
                return NotFound();
            }

            return GearToDTO(gear);
        }

        // PUT: api/Gear/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGear(long id, GearDTO gearDTO)
        {
            if (id != gearDTO.Id)
            {
                return BadRequest();
            }

            var gear = await _context.Gear.FindAsync(id);
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
            gear.Locking = gear.Locking;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!GearExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/Gear
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

            _context.Gear.Add(gear);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGear), new { id = gear.Id }, GearToDTO(gear));
        }

        // DELETE: api/Gear/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGear(long id)
        {
            var gear = await _context.Gear.FindAsync(id);
            if (gear == null)
            {
                return NotFound();
            }

            _context.Gear.Remove(gear);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GearExists(long id)
        {
            return _context.Gear.Any(e => e.Id == id);
        }

        private static GearDTO GearToDTO(Gear gear) =>
            new GearDTO
            {
                Id = gear.Id,
                Name = gear.Name,
                Description = gear.Description,
                Brand = gear.Brand,
                WeightGrams = gear.WeightGrams,
                LengthMM = gear.LengthMM,
                WidthMM = gear.WidthMM,
                DepthMM = gear.DepthMM,
                Locking = gear.Locking,
            };
    }
}
