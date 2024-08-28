using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Backend.Dtos.EditDtos;
using Backend.Dtos.Interfaces;
using Backend.Entities;
using static System.String;

namespace Backend.Dtos;

public class UserDto : EditUserDto, IValidatableObject
{
    public int UserId { get; init; }

    [EmailAddress]
    public new string Email { get; init; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public new string Password { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime CreatedAt { get; init; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime UpdatedAt { get; init; }

    public new User ConvertToEntity()
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
    
    public new IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
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