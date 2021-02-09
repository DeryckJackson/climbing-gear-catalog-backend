using Microsoft.EntityFrameworkCore;

namespace ClimbingGearBackend.Models
{
  [Keyless]
  public class RackGear
  {
    public long RackId { get; set; }
    public Rack Rack { get; set; }
    public long GearId { get; set; }
    public Gear Gear { get; set; }
    public int Quantity { get; set; }
  }

  public class RackGearDTO
  {
    public long RackId { get; set; }
    public long GearId { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public string Description { get; set; }
    public string Brand { get; set; }
    public int WeightGrams { get; set; }
    public int LengthMM { get; set; }
    public int WidthMM { get; set; }
    public int DepthMM { get; set; }
    public bool Locking { get; set; }
  }
}
