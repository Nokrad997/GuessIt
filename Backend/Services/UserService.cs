using Backend.Dtos;
using Backend.Entities;
using Backend.Repositories;
using Backend.Utility;

namespace Backend.Services;

public class UserService
{
    private readonly UserRepository _userRepository;
    private readonly PasswordHasher _passwordHasher;
    private readonly TokenUtil _tokenUtil;

    public UserService(UserRepository userRepository, PasswordHasher passwordHasher, TokenUtil tokenUtil)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenUtil = tokenUtil;
    }

    public async Task<bool> RegisterUser(AuthUserDto authUserDto)
    {
        var existingUser = await _userRepository.GetUserByEmailOrUsername(authUserDto.Email, authUserDto.Username);
        if (!existingUser.Equals(null))
        {
            return false;
        }
        
        authUserDto.Password = _passwordHasher.HashPassword(authUserDto.Password);
        User user = authUserDto.ConvertToEntity();
        
        await _userRepository.AddUser(user);

        return true;
    }

    public async Task<Dictionary<string, string>?> LoginUser(AuthUserDto authUserDto)
    {
        var existingUser = await _userRepository.GetUserByEmailOrUsername(authUserDto.Email, authUserDto.Username);
        if (existingUser.Equals(null))
        {
            return null;
        }
    
        bool passwordMatches = _passwordHasher.VerifyPassword(authUserDto.Password, existingUser.Password);

        return passwordMatches ? _tokenUtil.CreateTokenPair(existingUser) : null;
    }
}