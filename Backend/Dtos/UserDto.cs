using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Backend.Dtos.Interfaces;
using Backend.Entities;
using static System.String;

namespace Backend.Dtos;

public class UserDto : IUserDto, IValidatableObject
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
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (IsNullOrEmpty(Username))
        {
            yield return new ValidationResult("Username is required", new[] { nameof(Username) });
        }
        else
        {
            if(Username.Length < 3 || Username.Length > 50)
                yield return new ValidationResult("Username must be between 3 and 50 characters long", new[] { nameof(Username) });
        }

        if (IsNullOrEmpty(Email))
        {
            yield return new ValidationResult("Email is required", new[] { nameof(Email) });
        }
        else
        {
            if(!new EmailAddressAttribute().IsValid(Email))
                yield return new ValidationResult("Invalid email format", new[] { nameof(Email) });
        }

        if (IsNullOrEmpty(Password))
        {
            yield return new ValidationResult("Password is required", new[] { nameof(Password) });
        }
        else
        {
            if(Password.Length < 6 || Password.Length > 50)
                yield return new ValidationResult("Password must be between 6 and 50 characters long", new[] { nameof(Password) });
        }
    }
}