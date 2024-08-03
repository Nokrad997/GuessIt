using Backend.Dtos;
using Backend.Dtos.EditDtos;
using Backend.Entities;
using Backend.Exceptions;
using Backend.Repositories;
using Backend.Utility;

namespace Backend.Services;

public class AuthService
{
    private readonly UserRepository _userRepository;
    private readonly PasswordHasher _passwordHasher;
    private readonly TokenUtil _tokenUtil;

    public AuthService(UserRepository userRepository, PasswordHasher passwordHasher, TokenUtil tokenUtil)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenUtil = tokenUtil;
    }

    public async Task<bool> RegisterUser(EditUserDto registerUserDto)
    {
        var existingUser = await _userRepository.GetUserByEmail(registerUserDto.Email);
        if (existingUser is not null)
        {
            throw new BadCredentialsException("User with provided email already exists");
        }
        
        registerUserDto.Password = _passwordHasher.HashPassword(registerUserDto.Password);
        User user = registerUserDto.ConvertToEntity();
        
        await _userRepository.AddUser(user);

        return true;
    }

    public async Task<TokensDto> LoginUser(AuthUserDto authUserDto)
    {
        var existingUser = await _userRepository.GetUserByEmail(authUserDto.Email);
        if (existingUser is null)
        {
            throw new BadCredentialsException("Wrong email or password");
        }

        if (_passwordHasher.VerifyPassword(authUserDto.Password, existingUser.Password))
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