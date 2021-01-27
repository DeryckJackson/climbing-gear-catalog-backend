namespace ClimbingGearBackend.Models
{
  public class UserGear
  {
    public long UserGearId { get; set; }
    public int Quantity { get; set; }
    public long GearId { get; set; }
    public Gear Gear { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
  }

  public class UserGearDTO
  {
    public long UserGearId { get; set; }
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
