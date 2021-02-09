using System;
using ClimbingGearBackend.Infrastucture;
using ClimbingGearBackend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using IdentityServer4.EntityFramework.Options;
using Xunit;

namespace ClimbingGearBackend.Tests
{
  /* TODO(#7): I can't get this DbContext test working with Xunit, There's something about the operationalStoreOptions it doesn't like. Keeps throwing "Parameters don't have matching fixture data" errors. So until I can figure out why the extra options I need to get my Database working with the Api options the Repositories will remain untested. Link to Xunit Collections:
  
  https://xunit.net/docs/shared-context#collection-fixture
  */
  public class EfFixture : IDisposable
  {
    public ClimbingGearContext DbContext { get; private set; }
    public EfFixture(IOptions<OperationalStoreOptions> operationalStoreOptions)
    {
      // Initialize the YourDbContext property, run migrations or scripts to create
      // the schema, seed the database.
      string dbName = Guid.NewGuid().ToString();
      var options = new DbContextOptionsBuilder<ClimbingGearContext>()
        .UseInMemoryDatabase(databaseName: dbName).Options;

      ClimbingGearContext context = new ClimbingGearContext(options, operationalStoreOptions);
      context.Database.EnsureCreated();
      SeedData();
      DbContext = context;

    }

    public void Dispose()
    {
      DbContext.Database.EnsureDeleted();
    }

    public void SeedData()
    {
      var gear1 = new Gear
      {
        Id = 1,
        Name = "Cam",
        Description = "Just a cam",
        Brand = "TestCams"
      };
      var gear2 = new Gear
      {
        Id = 2,
        Name = "Nut",
        Description = "Just a nut",
        Brand = "TestNuts"
      };
      var gear3 = new Gear
      {
        Id = 3,
        Name = "Carabiner",
        Description = "Just a Carabiner",
        Brand = "TestCarabiners"
      };

      DbContext.Gear.AddRange(gear1, gear2, gear3);

      var userGear1 = new UserGear
      {
        UserGearId = 1,
        GearId = 1,
        UserId = "TestUserId",
        Quantity = 1
      };
      var userGear2 = new UserGear
      {
        UserGearId = 2,
        GearId = 2,
        UserId = "TestUserId",
        Quantity = 2,
      };
      var userGear3 = new UserGear
      {
        UserGearId = 3,
        GearId = 3,
        UserId = "NotTestUserId",
        Quantity = 3,
      };

      DbContext.UserGear.AddRange(userGear1, userGear2, userGear3);

      var testUser = new User
      {
        Id = "TestUserId",
        UserName = "TestUser",
        Email = "testuser@test.com"
      };

      DbContext.User.Add(testUser);
      DbContext.SaveChanges();
    }
  }

  [CollectionDefinition("EfCollection")]
  public class EfCollection : ICollectionFixture<EfFixture>
  {
  }
}
