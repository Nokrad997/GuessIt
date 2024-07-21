using System.ComponentModel.DataAnnotations;
using Backend.Entities;

namespace Backend.Dtos.Interfaces;

public interface IUserDto
{
    public User ConvertToEntity();
}