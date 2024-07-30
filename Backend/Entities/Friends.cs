using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Entities.Interfaces;

namespace Backend.Entities;

public class Friends : IHasTimeStamp
{
    public int FriendsId { get; set; }
    
    public int UserIdFk { get; set; }
    public User User { get; set; }
    
    public int FriendIdFk { get; set; }
    public User Friend { get; set; }
    
    [AllowedValues("Pending", "Accepted", "Rejected", "Blocked")]
    public string Status { get; set; }
}