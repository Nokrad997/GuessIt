using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Entities;
using Backend.Exceptions;
using Backend.Repositories;
using Backend.Repositories.Interfaces;
using Backend.Services.Interfaces;
using Backend.Utility;
using Backend.Utility.Interfaces;

namespace Backend.Services;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordAndEmailHasher _passwordAndEmailHasher;
    private readonly ITokenUtil _tokenUtil;

    public AuthService(IUserRepository userRepository, IPasswordAndEmailHasher passwordAndEmailHasher, ITokenUtil tokenUtil)
    {
        _userRepository = userRepository;
        _passwordAndEmailHasher = passwordAndEmailHasher;
        _tokenUtil = tokenUtil;
    }

    public async Task RegisterUser(EditUserDto registerUserDto)
    {
        var existingUsers = await _userRepository.GetAllUsers();
        var existingUser = existingUsers.FirstOrDefault(user =>
            _passwordAndEmailHasher.VerifyEmail(registerUserDto.Email, user.Email));
        if (existingUser is not null)
        {
            throw new BadCredentialsException("User with provided email already exists");
        }
        
        registerUserDto.Password = _passwordAndEmailHasher.HashPassword(registerUserDto.Password);
        registerUserDto.Email = _passwordAndEmailHasher.HashEmail(registerUserDto.Email);
        User user = registerUserDto.ConvertToEntity();
        
        await _userRepository.AddUser(user);
    }

    public async Task<TokensDto> LoginUser(AuthUserDto authUserDto)
    {
        var existingUsers = await _userRepository.GetAllUsers();
        var existingUser = existingUsers.FirstOrDefault(user =>
            _passwordAndEmailHasher.VerifyEmail(authUserDto.Email, user.Email));
        if (existingUser is null)
        {
            throw new BadCredentialsException("Wrong email or password");
        }

        if (_passwordAndEmailHasher.VerifyPassword(authUserDto.Password, existingUser.Password))
        {
            var tokens = _tokenUtil.CreateTokenPair(existingUser);
            return new TokensDto
            {
                AccessToken = tokens["access"],
                RefreshToken = tokens["refresh"]
            };
        }

        throw new BadCredentialsException("Wrong email or password");
    }
}