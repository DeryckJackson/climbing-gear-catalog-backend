using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ClimbingGearBackend.Controllers;
using ClimbingGearBackend.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace ClimbingGearBackend.Tests
{
  public abstract class GearControllerTest
  {
    protected GearControllerTest(DbContextOptions<ClimbingGearContext> contextOptions)
    {
      ContextOptions = contextOptions;

      Seed();
    }
    protected DbContextOptions<ClimbingGearContext> ContextOptions { get; }

    private void Seed()
    {
      using (var context = new ClimbingGearContext(ContextOptions))
      {
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        var one = new Gear
        {
          Id = 1,
          Name = "Cam",
          Description = "Just a cam",
          Brand = "TestCams"
        };

        var two = new Gear
        {
          Id = 2,
          Name = "Nut",
          Description = "Just a nut",
          Brand = "TestNuts"
        };

        var three = new Gear
        {
          Id = 3,
          Name = "Carabiner",
          Description = "Just a carabiner",
          Brand = "TestCarabiners"
        };

        context.AddRange(one, two, three);

        context.SaveChanges();
      }
    }
    [Fact]
    public async Task GetGear_CallGetGearWithNoId_ShouldReturnAllGear()
    {
      using (var context = new ClimbingGearContext(ContextOptions))
      {
        var controller = new GearController(context);

        var result = await controller.GetGear();

        var gearList = new List<GearDTO>(result.Value);
        Assert.Equal(3, gearList.Count);
        Assert.Equal("Cam", gearList[0].Name);
        Assert.Equal("Nut", gearList[1].Name);
        Assert.Equal("Carabiner", gearList[2].Name);
      }
    }

    [Fact]
    public async Task GetGear_CallWithAnIdThatExists_ShouldReturnCorrectGear()
    {
      using (var context = new ClimbingGearContext(ContextOptions))
      {
        var controller = new GearController(context);

        var result = await controller.GetGear(1);

        var gear = result.Value;
        Assert.Equal("Cam", gear.Name);
      }
    }

    [Fact]
    public async Task GetGear_CallWithIdThatDoesntExist_ShouldReturnNotFound()
    {
      using (var context = new ClimbingGearContext(ContextOptions))
      {
        var controller = new GearController(context);

        var result = await controller.GetGear(42);

        Assert.IsType<NotFoundResult>(result.Result);
      }
    }

    [Fact]
    public async Task UpdateGear_CallWithIdAndGearDTO_ShouldReturnNoContent()
    {
      using (var context = new ClimbingGearContext(ContextOptions))
      {
        var controller = new GearController(context);

        var gear = new GearDTO()
        {
          Id = 1,
          Name = "EditedGear",
        };

        var result = await controller.UpdateGear(1, gear);

        Assert.IsType<NoContentResult>(result);
      }
    }

    [Fact]
    public async Task UpdateGear_CallWithIdAndWrongGearId_ShouldReturnBadRequest()
    {
      using (var context = new ClimbingGearContext(ContextOptions))
      {
        var controller = new GearController(context);

        var gear = new GearDTO()
        {
          Id = 2,
          Name = "EditedGear",
        };

        var result = await controller.UpdateGear(1, gear);

        Assert.IsType<BadRequestResult>(result);
      }
    }

    [Fact]
    public async Task UpdateGear_CallWithIdThatDoesntExist_ShouldReturnNotFound()
    {
      using (var context = new ClimbingGearContext(ContextOptions))
      {
        var controller = new GearController(context);

        var gear = new GearDTO()
        {
          Id = 42,
          Name = "EditedGear",
        };

        var result = await controller.UpdateGear(42, gear);

        Assert.IsType<NotFoundResult>(result);
      }
    }

    [Fact]
    public async Task CreateGear_CallWithNewGearDTO_ShouldReturnNewGearAndCreated()
    {
      using (var context = new ClimbingGearContext(ContextOptions))
      {
        var controller = new GearController(context);

        var gear = new GearDTO()
        {
          Name = "NewGear",
        };

        var result = await controller.CreateGear(gear);
        var resultObject = result.Result as CreatedAtActionResult;
        var resultGear = resultObject.Value as GearDTO;

        Assert.Equal(201, resultObject.StatusCode);
        Assert.Equal(gear.Name, resultGear.Name);
      }
    }

    [Fact]
    public async Task DeleteGear_CallWithIdAndGearDTO_ShouldReturnNoContent()
    {
      using (var context = new ClimbingGearContext(ContextOptions))
      {
        var controller = new GearController(context);

        var result = await controller.DeleteGear(1);

        Assert.IsType<NoContentResult>(result);

        List<Gear> gearList = context.Gear.ToList();
        Assert.Equal(2, gearList.Count);
      }
    }

    [Fact]
    public async Task DeleteGear_CallWithAnIncorrectId_ShouldReturnNotFound()
    {
      using (var context = new ClimbingGearContext(ContextOptions))
      {
        var controller = new GearController(context);

        var result = await controller.DeleteGear(42);

        Assert.IsType<NotFoundResult>(result);
      }
    }
  }
}
