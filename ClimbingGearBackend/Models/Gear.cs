namespace ClimbingGearBackend.Models
{
  public class Gear
  {
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Brand { get; set; }
    public int WeightGrams { get; set; }
    public int LengthMM { get; set; } 
    public int WidthMM { get; set; }
    public int DepthMM { get; set; }
    public bool Locking { get; set; }
    public string Secret { get; set; }
  }

  public class GearDTO
  {
    public long Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Brand { get; set; }
    public int WeightGrams { get; set; }
    public int LengthMM { get; set; } 
    public int WidthMM { get; set; }
    public int DepthMM { get; set; }
    public bool Locking { get; set; }
  }
}
