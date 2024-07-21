using Backend.Utility;

namespace Backend.Services;

public class TokenService
{
    private readonly TokenUtil _tokenUtil;

    public TokenService(TokenUtil tokenUtil)
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