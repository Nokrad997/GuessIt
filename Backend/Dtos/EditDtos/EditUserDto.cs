using System.ComponentModel.DataAnnotations;
using Backend.Dtos.Interfaces;
using Backend.Entities;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using static System.String;

namespace Backend.Dtos.EditDtos;
public class EditUserDto : IValidatableObject
{
    public string Username { get; set; }
    public  string Email { get; set; }
    public string Password { get; set; }
    public bool Verified { get; set; }
    public bool IsAdmin { get; set; }
    
    public User ConvertToEntity()
    {
        return new User
        {
            Username = Username,
            Email = Email,
            Password = Password,
            Verified = Verified,
            IsAdmin = IsAdmin
        };
    }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        Console.WriteLine("?");
        if(IsNullOrEmpty(Username) && IsNullOrEmpty(Email) && IsNullOrEmpty(Password))
        {
            yield return new ValidationResult("No changes detected", new[] { nameof(Username), nameof(Email), nameof(Password) });
        }
        if(!IsNullOrEmpty(Password))
        {
            if(Password.Length < 6 || Password.Length > 50)
                yield return new ValidationResult("Password must be between 6 and 50 characters long", new[] { nameof(Password) });
        }
        if (!IsNullOrEmpty(Username))
        {
            if(Username.Length < 3 || Username.Length > 50)
                yield return new ValidationResult("Username must be between 3 and 50 characters long", new[] { nameof(Username) });
        }
        if (!IsNullOrEmpty(Email))
        {
            if(!new EmailAddressAttribute().IsValid(Email))
                yield return new ValidationResult("Invalid email format", new[] { nameof(Email) });
        }
    }
}