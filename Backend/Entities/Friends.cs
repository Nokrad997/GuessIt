using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.JavaScript;
using Backend.Dtos;
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
    
    public FriendsStatusTypes UserFriendshipStatus { get; set; }
    public FriendsStatusTypes FriendFriendshipStatus { get; set; }

    public FriendsDto ConvertToDto()
    {
        return new FriendsDto
        {
            FriendsId = FriendsId,
            UserIdFk = UserIdFk,
            FriendIdFk = FriendIdFk,
            UserFriendshipStatus = UserFriendshipStatus,
            FriendFriendshipStatus = FriendFriendshipStatus
        };
    }
}