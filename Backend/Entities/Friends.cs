using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.JavaScript;
using Backend.Entities.Interfaces;
using Backend.Utility.Enums;

namespace Backend.Entities;

public class Friends : IHasTimeStamp
{
    public int FriendsId { get; set; }
    
    public int UserIdFk { get; set; }
    public User User { get; set; }
    
    public int FriendIdFk { get; set; }
    public User Friend { get; set; }
    
    public FriendsStatusTypes Status { get; set; }
}