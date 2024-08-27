using System.ComponentModel.DataAnnotations;
using Backend.Dtos.EditDtos;
using Backend.Entities;
using Backend.Utility.Enums;

namespace Backend.Dtos;

public class FriendsDto : EditFriendsDto, IValidatableObject
{
    public int FriendsId { get; set; }
    public int UserIdFk { get; set; }
    public int FriendIdFk { get; set; }

    public Friends ConvertToEntity()
    {
        return new Friends
        {
            FriendsId = FriendsId,
            UserIdFk = UserIdFk,
            FriendIdFk = FriendIdFk,
            UserFriendshipStatus = UserFriendshipStatus,
            FriendFriendshipStatus = FriendFriendshipStatus
        };
    }
    
    public new IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if(UserIdFk == 0)
        {
            yield return new ValidationResult("UserIdFk cannot be 0", new []{ nameof(UserIdFk) });
        }
        if(FriendIdFk == 0)
        {
            yield return new ValidationResult("FriendIdFk cannot be 0", new []{ nameof(FriendIdFk) });
        }
        if (UserIdFk != 0 && FriendIdFk != 0 && UserIdFk == FriendIdFk)
        {
            yield return new ValidationResult("You cannot add yourself as a friend", new []{ nameof(UserIdFk), nameof(FriendIdFk) });
        }
        if(UserFriendshipStatus is < FriendsStatusTypes.Pending or > FriendsStatusTypes.Blocked)
        {
            yield return new ValidationResult(
                $"Invalid status, is {UserFriendshipStatus}, should be greater than {FriendsStatusTypes.Pending} and lesser than {FriendsStatusTypes.Blocked}",
                new[] { nameof(UserFriendshipStatus) });
        }
        if((FriendFriendshipStatus is < FriendsStatusTypes.Pending or > FriendsStatusTypes.Blocked))
        {
            yield return new ValidationResult(
                $"Invalid status, is {FriendFriendshipStatus}, should be greater than {FriendsStatusTypes.Pending} and lesser than {FriendsStatusTypes.Blocked}",
                new[] { nameof(FriendFriendshipStatus) });
        }
    }
}