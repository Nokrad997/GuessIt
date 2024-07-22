using System.Reflection;
using Backend.Dtos;
using Backend.Entities;
using Backend.Exceptions;
using Backend.Repositories;
using Backend.Utility;

namespace Backend.Services;

public class UserService
{
    private readonly TokenUtil _tokenUtil;
    private readonly UserRepository _userRepository;
    private readonly PasswordHasher _passwordHasher;

    public UserService(TokenUtil tokenUtil, UserRepository userRepository, PasswordHasher passwordHasher)
    {
        _tokenUtil = tokenUtil;
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<IEnumerable<UserDto>> Retrieve()
    {
        var retrievedUsers = await _userRepository.GetAllUsers();
        var usersDtos = retrievedUsers.Select(user => user.ConvertToDto()).ToList();

        return usersDtos;
    }
    
    public async Task<UserDto> Retrieve(int id)
    {
        var user = await _userRepository.GetUserById(id);
        if (user is null)
        {
            throw new ArgumentException("No user with provided id");
        }

        return user.ConvertToDto();
    }

    public async Task AddUserAsUser(AuthUserDto userDto)
    {
        var existingUser = await _userRepository.GetUserByEmail(userDto.Email);
        if (existingUser is not null)
        {
            throw new BadCredentialsException("User with provided email already exists");
        }
        
        userDto.Password = _passwordHasher.HashPassword(userDto.Password);
        User user = userDto.ConvertToEntity();
        
        await _userRepository.AddUser(user);
    }
    public async Task AddUserAsAdmin(UserDto userDto)
    {
        var existingUser = await _userRepository.GetUserByEmail(userDto.Email);
        if (existingUser is not null)
        {
            throw new BadCredentialsException("User with provided email already exists");
        }
        
        userDto.Password = _passwordHasher.HashPassword(userDto.Password);
        User user = userDto.ConvertToEntity();
        
        await _userRepository.AddUser(user);
    }

    public async Task<UserDto> EditUserAsUser(int id, AuthUserDto userDto)
    {
        var existingUser = await _userRepository.GetUserById(id);
        if (existingUser is null)
        {
            throw new ArgumentException("User with provided id doesn't exist");
        }
        
        var excludedProperties = new[] { "UserId", "Verified", "IsAdmin", "CreatedAt", "UpdatedAt" };
        UpdatePropertiesIfNeeded(existingUser, userDto, excludedProperties);

        return existingUser.ConvertToDto();
    }
    public async Task<UserDto> EditUserAsAdmin(int id, UserDto userDto)
    {
        var existingUser = await _userRepository.GetUserById(id);
        if (existingUser is null)
        {
            throw new ArgumentException("User with provided id doesn't exist");
        }
        
        var excludedProperties = new[] { "UserId", "CreatedAt", "UpdatedAt" };
        UpdatePropertiesIfNeeded(existingUser, userDto, excludedProperties);

        return existingUser.ConvertToDto();
    }

    public async Task DeleteUser(int id)
    {
        await _userRepository.DeleteUserById(id);
    }

    private static void UpdatePropertiesIfNeeded<T>(User user, T userDto, string[] excludedProperties)
    {
        var userProperties = typeof(User).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(prop => prop.CanRead && prop.CanWrite && !excludedProperties.Contains(prop.Name));

        foreach (var prop in userProperties)
        {
            var sourceValue = prop.GetValue(userDto);
            var targetValue = prop.GetValue(user);
            if (!Equals(sourceValue, targetValue))
            {
                prop.SetValue(user, sourceValue);
            }
        }
    }
}