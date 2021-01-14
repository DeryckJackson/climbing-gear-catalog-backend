using ClimbingGearBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace ClimbingGearBackend.Tests
{
  public class InMemoryGearControllerTest : GearControllerTest
  {
    public InMemoryGearControllerTest()
      : base(
        new DbContextOptionsBuilder<ClimbingGearContext>()
          .UseInMemoryDatabase("TestDatabase")
          .Options)
    {
    }
  }
}
