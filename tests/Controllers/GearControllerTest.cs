using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ClimbingGearBackend.Controllers;
using ClimbingGearBackend.Models;
using ClimbingGearBackend.Interfaces;
using Xunit;
using Moq;

namespace ClimbingGearBackend.Tests
{
  public class GearControllerTest
  {
    public List<GearDTO> MockDTOList()
    {
      return new List<GearDTO>()
      {
        new GearDTO
        {
          Id = 1,
          Name = "Cam",
          Description = "Just a cam",
          Brand = "TestCams"
        },
        new GearDTO
        {
          Id = 2,
          Name = "Nut",
          Description = "Just a nut",
          Brand = "TestNuts"
        },
        new GearDTO
        {
          Id = 3,
          Name = "Carabiner",
          Description = "Just a Carabiner",
          Brand = "TestCarabiners"
        }
      };
    }

    public Gear MockGear()
    {
      return new Gear
      {
        Id = 1,
        Name = "Cam",
        Description = "Just a cam",
        Brand = "TestCams"
      };
    }

    [Fact]
    public async Task GetGear_CallGetGearWithNoId_ShouldReturnAllGear()
    {
      var mockList = MockDTOList();
      var mockRepo = new Mock<IGearRepository>();
      mockRepo.Setup(repo => repo.DTOListAsync().Result).Returns(mockList);


      var controller = new GearController(mockRepo.Object);

      var result = await controller.GetGear();

      var gearList = new List<GearDTO>(result.Value);
      Assert.Equal(3, gearList.Count);
      Assert.Equal(mockList[0], gearList[0]);
      Assert.Equal(mockList[1], gearList[1]);
      Assert.Equal(mockList[2], gearList[2]);
    }

    [Fact]
    public async Task GetGear_CallWithAnIdThatExists_ShouldReturnCorrectGear()
    {
      var mockRepo = new Mock<IGearRepository>();
      mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<long>()).Result)
        .Returns(MockGear());

      var controller = new GearController(mockRepo.Object);

      var result = await controller.GetGear(1);

      var gear = result.Value;
      Assert.IsType<GearDTO>(gear);
      Assert.Equal("Cam", gear.Name);
    }


    [Fact]
    public async Task GetGear_CallWithIdThatDoesntExist_ShouldReturnNotFound()
    {
      var mockRepo = new Mock<IGearRepository>();
      var controller = new GearController(mockRepo.Object);
      var nonExistentGearId = 42;

      var result = await controller.GetGear(nonExistentGearId);

      Assert.IsType<NotFoundResult>(result.Result);
    }


    [Fact]
    public async Task UpdateGear_CallWithIdAndGearDTO_ShouldReturnNoContent()
    {
      var mockRepo = new Mock<IGearRepository>();
      mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<long>()).Result)
        .Returns(MockGear());


      var controller = new GearController(mockRepo.Object);

      var mockGear = new GearDTO()
      {
        Id = 1,
        Name = "EditedGear",
      };

      var result = await controller.UpdateGear(1, mockGear);

      Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateGear_CallWithIdAndWrongGearId_ShouldReturnBadRequest()
    {
      var mockRepo = new Mock<IGearRepository>();
      mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<long>()).Result)
        .Returns(MockGear());

      var controller = new GearController(mockRepo.Object);

      var gear = new GearDTO()
      {
        Id = 2,
        Name = "EditedGear",
      };

      var result = await controller.UpdateGear(1, gear);

      Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task UpdateGear_CallWithIdThatDoesntExist_ShouldReturnNotFound()
    {
      var mockRepo = new Mock<IGearRepository>();
      var controller = new GearController(mockRepo.Object);

      var gear = new GearDTO()
      {
        Id = 42,
        Name = "EditedGear",
      };

      var result = await controller.UpdateGear(42, gear);

      Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateGear_CallWithNewGearDTO_ShouldReturnNewGearAndCreated()
    {
      var mockRepo = new Mock<IGearRepository>();
      var controller = new GearController(mockRepo.Object);

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

    [Fact]
    public async Task DeleteGear_CallWithIdAndGearDTO_ShouldReturnNoContent()
    {
      var mockRepo = new Mock<IGearRepository>();
      mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<long>()).Result)
        .Returns(MockGear());
      var controller = new GearController(mockRepo.Object);

      var result = await controller.DeleteGear(1);

      Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteGear_CallWithAnIncorrectId_ShouldReturnNotFound()
    {
      var mockRepo = new Mock<IGearRepository>();
      var controller = new GearController(mockRepo.Object);

      var result = await controller.DeleteGear(42);

      Assert.IsType<NotFoundResult>(result);
    }
  }
}
