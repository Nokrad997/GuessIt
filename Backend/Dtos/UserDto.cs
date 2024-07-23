using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Backend.Dtos.Interfaces;
using Backend.Entities;

namespace Backend.Dtos;

public class UserDto : IUserDto
{
    public int UserId { get; set; }
    public string Username { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string Password { get; set; }
    
    public bool Verified { get; set; }
    public bool IsAdmin { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime CreatedAt { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime UpdatedAt { get; set; }

    public User ConvertToEntity()
    {
        return new User
        {
            UserId = UserId,
            Username = Username,
            Email = Email,
            Password = Password,
            Verified = Verified,
            IsAdmin = IsAdmin,
            CreatedAt = CreatedAt,
            UpdatedAt = UpdatedAt
        };
    }
}