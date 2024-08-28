using System.ComponentModel.DataAnnotations;
using Backend.Dtos.Interfaces;
using Backend.Entities;
using static System.String;

namespace Backend.Dtos;

public class AuthUserDto : IValidatableObject
{
    public  string Email { get; set; }
    public string Password { get; set; }
    
    public User ConvertToEntity()
    {
        return new User
        {
            Email = Email,
            Password = Password,
        };
    }
    
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if(IsNullOrEmpty(Email))
        {
            yield return new ValidationResult("Email is required", new[] { nameof(Email) });
        }
        if(IsNullOrEmpty(Password))
        {
            yield return new ValidationResult("Password is required", new[] { nameof(Password) });
        }
    }
}