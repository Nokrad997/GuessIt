using System.Reflection;
using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Entities;
using Backend.Exceptions;
using Backend.Repositories;
using Backend.Utility;

namespace Backend.Services;

public class UserService
{
    private readonly TokenUtil _tokenUtil;
    private readonly UserRepository _userRepository;
    private readonly PasswordAndEmailHasher _passwordAndEmailHasher;

    public UserService(TokenUtil tokenUtil, UserRepository userRepository, PasswordAndEmailHasher passwordAndEmailHasher)
    {
        _tokenUtil = tokenUtil;
        _userRepository = userRepository;
        _passwordAndEmailHasher = passwordAndEmailHasher;
    }
    
    public async Task<IEnumerable<UserDto>> Retrieve()
    {
        var retrievedUsers = await _userRepository.GetAllUsers();

        return retrievedUsers.Select(user => user.ConvertToDto()).ToList();
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
    
    public async Task<UserDto> Retrieve(string token)
    {
        var userId = _tokenUtil.GetIdFromToken(token);
        var user = await _userRepository.GetUserById(userId);
        if (user is null)
        {
            throw new ArgumentException("No user with provided id");
        }

        return user.ConvertToDto();
    }
    public async Task AddUserAsAdmin(UserDto userDto)
    {
        var existingUsers = await _userRepository.GetAllUsers();
        var existingUser = existingUsers.FirstOrDefault(user =>
            _passwordAndEmailHasher.VerifyEmail(userDto.Email, user.Email));
        if (existingUser is not null)
        {
            throw new BadCredentialsException("User with provided email already exists");
        }
        
        userDto.Password = _passwordAndEmailHasher.HashPassword(userDto.Password);
        userDto.Email = _passwordAndEmailHasher.HashEmail(userDto.Email);
        User user = userDto.ConvertToEntity();
        
        await _userRepository.AddUser(user);
    }
    public async Task<UserDto> EditUserAsUser(int id, EditUserDto userDto, string token)
    {
        if (!_tokenUtil.GetIdFromToken(token).Equals(id))
        {
            throw new ArgumentException("Denied, cannot edit other user data");
        }
        
        var existingUser = await _userRepository.GetUserById(id);
        var existingUsers = await _userRepository.GetAllUsers();
        var emailCheck = existingUsers.FirstOrDefault(user => _passwordAndEmailHasher.VerifyEmail(userDto.Email, user.Email));
        if (existingUser is null)
        {
            throw new ArgumentException("User with provided id doesn't exist");
        } 
        if (emailCheck is not null && !existingUser.Equals(emailCheck))
        {
            throw new ArgumentException("User with provided email, already exists");
        }
        
        var excludedProperties = new[] { "UserId", "Verified", "IsAdmin", "CreatedAt", "UpdatedAt" };
        UpdatePropertiesIfNeeded(existingUser, userDto, excludedProperties);
        await _userRepository.EditUser(existingUser);

        return existingUser.ConvertToDto();
    }
    public async Task<UserDto> EditUserAsAdmin(int id, EditUserDto userDto)
    {
        var existingUser = await _userRepository.GetUserById(id);
        var existingUsers = await _userRepository.GetAllUsers();
        var emailCheck = existingUsers.FirstOrDefault(user => _passwordAndEmailHasher.VerifyEmail(userDto.Email, user.Email));
        if (existingUser is null)
        {
            throw new ArgumentException("User with provided id doesn't exist");
        } 
        if (emailCheck is not null && !existingUser.Equals(emailCheck))
        {
            throw new ArgumentException("User with provided email, already exists");
        }
        
        var excludedProperties = new[] { "UserId", "CreatedAt", "UpdatedAt" };
        UpdatePropertiesIfNeeded(existingUser, userDto, excludedProperties);

        await _userRepository.EditUser(existingUser);
        
        return existingUser.ConvertToDto();
    }
    public async Task DeleteUser(int id, string token)
    {
        var userToDelete = await _userRepository.GetUserById(id);
        if (userToDelete is null)
        {
            throw new ArgumentException("User doesn't exist");
        }
        
        var idFromRequest = _tokenUtil.GetIdFromToken(token);
        var userFromRequest = await _userRepository.GetUserById(idFromRequest);
        if (userFromRequest is null)
        {
            throw new ArgumentException("User doesn't exist");
        }
        
        if (userFromRequest.IsAdmin || idFromRequest == id)
        {
            await _userRepository.DeleteUserById(id);
        }
        else
        {
            throw new ArgumentException("Denied, cannot delete other user accounts");
        }
    }
    
    private void UpdatePropertiesIfNeeded<T>(User user, T userDto, string[] excludedProperties)
    {
        var userProperties = typeof(User).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(prop => prop.CanRead && prop.CanWrite && !excludedProperties.Contains(prop.Name));
        var dtoProperties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(prop => !excludedProperties.Contains(prop.Name));
        
        foreach (var userProp in userProperties)
        {
            var dtoProp = dtoProperties.FirstOrDefault(p => p.Name == userProp.Name && p.PropertyType == userProp.PropertyType);
            if (dtoProp == null) continue;
            
            var sourceValue = dtoProp.GetValue(userDto);
            var targetValue = userProp.GetValue(user);
                
            if (userProp.Name.Equals("Password") && sourceValue != null)
            {
                sourceValue = _passwordAndEmailHasher.HashPassword(sourceValue.ToString()); 
            }

            if (userProp.Name.Equals("Email") && !String.IsNullOrEmpty(sourceValue.ToString()))
            {
                sourceValue = _passwordAndEmailHasher.HashEmail(sourceValue.ToString());
            }
            
            if (!Equals(sourceValue, targetValue) && sourceValue != null)
            {
                userProp.SetValue(user, sourceValue);
            }
        }
    }
}