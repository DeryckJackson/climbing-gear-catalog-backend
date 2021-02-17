using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ClimbingGearBackend.Models
{
  [Keyless]
  public class RackUsers
  {
    public long RackId { get; set; }
    public Rack Rack { get; set; }
    public string UserId { get; set; }
    public User User { get; set; }
  }

  public class UserRackDTO
  {
    public long RackId { get; set; }
    public string Name { get; set; }
  }

  public class RackUsersGearDTO
  {
    public long RackId { get; set; }
    public string Name { get; set; }
    public List<RackGearDTO> RackGearDTOs { get; set; }
  }
}
