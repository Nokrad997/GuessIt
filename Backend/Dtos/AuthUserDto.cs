using System.ComponentModel.DataAnnotations;
using Backend.Dtos.Interfaces;
using Backend.Entities;

namespace Backend.Dtos;

public class AuthUserDto : IUserDto
{
    [EmailAddress]
    public  string Email { get; set; }
    
    [Required]
    // [MinLength(8)]
    public string Password { get; set; }
    
    public User ConvertToEntity()
    {
        return new User
        {
            Email = Email,
            Password = Password,
        };
    }
}