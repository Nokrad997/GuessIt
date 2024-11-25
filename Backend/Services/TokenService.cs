using Backend.Services.Interfaces;
using Backend.Utility;
using Backend.Utility.Interfaces;

namespace Backend.Services;

public class TokenService : ITokenService
{
    private readonly ITokenUtil _tokenUtil;

    public TokenService(ITokenUtil tokenUtil)
    {
        _tokenUtil = tokenUtil;
    }
    
    public bool Validate(string token)
    {
        return _tokenUtil.ValidateToken(token);
    }

    public Dictionary<string, string> RefreshToken(string token)
    {
        return _tokenUtil.RefreshAccessToken(token);
    }
}