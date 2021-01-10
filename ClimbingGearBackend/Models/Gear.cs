namespace ClimbingGearBackend.Models
{
  public class Gear
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public int Qty { get; set; }
    public string Desc { get; set; }
    public string Brand { get; set; }
    public int WeightGrams { get; set; }
    public int WidthMM { get; set; }
    public int DepthMM { get; set; }
    public bool Locking { get; set; }
  }
}
