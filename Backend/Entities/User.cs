using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Dtos;
using Backend.Entities.Interfaces;

namespace Backend.Entities;

public class User : IHasTimeStamp
{
    public int UserId { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool Verified { get; set; }
    public bool IsAdmin { get; set; }

    public ICollection<Statistics> Statistics { get; set; }
    public ICollection<Friends> Friends { get; set; }
    public ICollection<Game> Games { get; set; }
    public ICollection<Leaderboard> Leaderboards { get; set; }
    public ICollection<UserAchievements> UserAchievements { get; set; }

    public UserDto ConvertToDto()
    {
        return new UserDto
        {
            UserId = UserId,
            Username = Username,
            Email = Email,
            Verified = Verified,
            IsAdmin = IsAdmin,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt
        };
    }
}