using Backend.Dtos;
using Backend.Dtos.EditDtos;

namespace Backend.Services.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> Retrieve();
    Task<UserDto> Retrieve(int id);
    Task<UserDto> Retrieve(string token);
    Task AddUserAsAdmin(UserDto userDto);
    Task<UserDto> EditUserAsUser(int id, EditUserDto userDto, string token);
    Task<UserDto> EditUserAsAdmin(int id, EditUserDto userDto);
    Task DeleteUser(int id, string token);
}