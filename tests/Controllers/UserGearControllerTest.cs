using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ClimbingGearBackend.Controllers;
using ClimbingGearBackend.Models;
using ClimbingGearBackend.Interfaces;
using Xunit;
using Moq;

namespace ClimbingGearBackend.Tests
{
  public class UserGearControllerTest
  {
    public List<UserGear> MockUserGearList()
    {
      return new List<UserGear>()
      {
        new UserGear
        {
          UserGearId = 1,
          GearId = 1,
          Quantity = 1,
          Gear = new Gear
          {
            Id = 1,
            Name = "Cam",
            Description = "Just a cam",
            Brand = "TestCams"
          }
        },
        new UserGear
        {
          UserGearId = 2,
          GearId = 2,
          Quantity = 2,
          Gear = new Gear
          {
            Name = "Nut",
            Description = "Just a nut",
            Brand = "TestNuts"
          }
        },
        new UserGear
        {
          UserGearId = 3,
          GearId = 3,
          Quantity = 3,
          Gear = new Gear
          {
            Name = "Carabiner",
            Description = "Just a Carabiner",
            Brand = "TestCarabiners"
          }
        }
      };
    }

    public User MockUser()
    {
      return new User
      {
        Id = "TestUserId",
      };
    }

    public UserGear MockUserGear()
    {
      return new UserGear
      {
        UserGearId = 1,
        GearId = 1,
        Quantity = 1,
        Gear = new Gear
        {
          Id = 1,
          Name = "Cam",
          Description = "Just a cam",
          Brand = "TestCams"
        }
      };
    }

    [Fact]
    public async Task GetUserGearWithNoId_ShouldReturnAllCurrentUserGear()
    {
      var mockRepo = new Mock<IUserGearRepository>();
      mockRepo.Setup(repo => repo.ListAsync(It.IsAny<ClaimsPrincipal>()).Result)
        .Returns(MockUserGearList());

      var controller = new UserGearController(mockRepo.Object);

      var result = await controller.GetUserGear();

      var userGear = new List<UserGearDTO>(result.Value);
      Assert.Equal(3, userGear.Count);
      Assert.Equal("Cam", userGear[0].Name);
      Assert.Equal("Nut", userGear[1].Name);
      Assert.Equal("Carabiner", userGear[2].Name);
    }

    [Fact]
    public async Task GetUserGearWithId_ShouldReturnCorrectUserGear()
    {
      var mockRepo = new Mock<IUserGearRepository>();
      mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<long>()).Result)
        .Returns(MockUserGear());

      var controller = new UserGearController(mockRepo.Object);

      var result = await controller.GetUserGear(1);

      var userGear = result.Value;

      Assert.IsType<UserGearDTO>(userGear);
      Assert.Equal("Cam", userGear.Name);
    }

    [Fact]
    public async Task GetUserGearWithInvalidUserGearId_ShouldReturnNotFound()
    {
      var mockRepo = new Mock<IUserGearRepository>();
      var controller = new UserGearController(mockRepo.Object);
      var nonExistentGearId = 42;

      var result = await controller.GetUserGear(nonExistentGearId);

      Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task UpdateUserGearWithIdAndUserGear_ShouldReturnNoContent()
    {
      var mockRepo = new Mock<IUserGearRepository>();
      mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<long>()).Result)
        .Returns(MockUserGear());


      var controller = new UserGearController(mockRepo.Object);

      var mockUserGear = new UserGear()
      {
        UserGearId = 1,
        UserId = "TestUserId",
        GearId = 1,
        Quantity = 42
      };

      var result = await controller.UpdateGear(1, mockUserGear);

      Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateUserGearWithNonMatchingIds_ShouldReturnBadRequest()
    {
      var mockRepo = new Mock<IUserGearRepository>();
      mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<long>()).Result)
        .Returns(MockUserGear());

      var controller = new UserGearController(mockRepo.Object);

      var mockUserGear = new UserGear()
      {
        UserGearId = 666,
        UserId = "TestUserId",
        GearId = 1,
        Quantity = 42
      };

      var result = await controller.UpdateGear(1, mockUserGear);

      Assert.IsType<BadRequestResult>(result);
    }

    [Fact]
    public async Task UpdateUserGearCallWithInvalidId_ShouldReturnNotFound()
    {
      var mockRepo = new Mock<IUserGearRepository>();
      var controller = new UserGearController(mockRepo.Object);

      var mockUserGear = new UserGear()
      {
        UserGearId = 42,
        UserId = "TestUserId",
        GearId = 1,
        Quantity = 42
      };

      var result = await controller.UpdateGear(42, mockUserGear);

      Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task CreateUserGearWithNewUserGear_ShouldReturnNewUserGearAndCreated()
    {
      var mockRepo = new Mock<IUserGearRepository>();
      var controller = new UserGearController(mockRepo.Object);

      var mockUserGear = new UserGear()
      {
        UserId = "NewUserGear",
        GearId = 1,
        Quantity = 42
      };

      var result = await controller.CreateUserGear(mockUserGear);
      var resultObject = result.Result as CreatedAtActionResult;
      var resultGear = resultObject.Value as UserGear;

      Assert.Equal(201, resultObject.StatusCode);
      Assert.Equal(mockUserGear.UserId, resultGear.UserId);
    }

    [Fact]
    public async Task DeleteGearWithIdAndUserGear_ShouldReturnNoContent()
    {
      var mockRepo = new Mock<IUserGearRepository>();
      mockRepo.Setup(repo => repo.GetByIdAsync(It.IsAny<long>()).Result)
        .Returns(MockUserGear());

      var controller = new UserGearController(mockRepo.Object);

      var result = await controller.DeleteUserGear(1);

      Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteGearWithAnInvalidId_ShouldReturnNotFound()
    {
      var mockRepo = new Mock<IGearRepository>();
      var controller = new GearController(mockRepo.Object);

      var result = await controller.DeleteGear(42);

      Assert.IsType<NotFoundResult>(result);
    }
  }
}
