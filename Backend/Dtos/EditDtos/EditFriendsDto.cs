using System.ComponentModel.DataAnnotations;
using Backend.Utility.Enums;

namespace Backend.Dtos.EditDtos;

public class EditFriendsDto : IValidatableObject
{
    public FriendsStatusTypes UserFriendshipStatus { get; set; }
    public FriendsStatusTypes FriendFriendshipStatus { get; set; }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
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