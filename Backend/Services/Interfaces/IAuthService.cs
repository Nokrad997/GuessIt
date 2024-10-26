using Backend.Dtos;
using Backend.Dtos.EditDtos;

namespace Backend.Services.Interfaces;

public interface IAuthService
{
    Task RegisterUser(EditUserDto registerUserDto);
    Task<TokensDto> LoginUser(AuthUserDto authUserDto);
}