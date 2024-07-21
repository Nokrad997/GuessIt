using System.ComponentModel.DataAnnotations;
using Backend.Dtos.Interfaces;
using Backend.Entities;

namespace Backend.Dtos;

public class AuthUserDto : IUserDto
{
    public string? Username { get; set; }

    [EmailAddress]
    public  string? Email { get; set; }
    
    [Required]
    // [MinLength(8)]
    public  string Password { get; set; }
    
    public User ConvertToEntity()
    {
        var usr = new User();

        usr.Username = Username;
        usr.Email = Email;
        usr.Password = Password;

        return usr;
    }
}