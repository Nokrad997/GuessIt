using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Entities;
using Backend.Exceptions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Utility;

public class TokenUtil
{
    private readonly string _privateKey;
    private readonly JwtSecurityTokenHandler _handler;

    public TokenUtil(IOptions<Config> settings)
    {
        _privateKey = settings.Value.TokenKey;
        _handler = new JwtSecurityTokenHandler();
    }
    public Dictionary<string, string> CreateTokenPair(User user)
    {
        var claims = GenerateClaims(user);
        var accessTokenDescriptor = CreateToken(true, claims);
        var refreshTokenDescriptor = CreateToken(false, claims);
        
        var refreshToken = _handler.CreateToken(refreshTokenDescriptor);
        var accessToken = _handler.CreateToken(accessTokenDescriptor);
        var tokenPair = new Dictionary<string, string>
        {
            { "access", _handler.WriteToken(accessToken) },
            { "refresh", _handler.WriteToken(refreshToken) }
        };
        
        return tokenPair;
    }

    public Dictionary<string, string> RefreshAccessToken(string refreshToken)
    {
        if (ValidateToken(refreshToken))
        {
            var jwtToken = _handler.ReadToken(refreshToken) as JwtSecurityToken;
            if (jwtToken == null)
            {
                throw new NoTokenProvidedException("No refresh token provided");
            }
            
            var claims = jwtToken.Claims;
            var accessTokenDescriptor = CreateToken(true, new ClaimsIdentity(claims));
            var newAccessToken = _handler.CreateToken(accessTokenDescriptor);
            
            return new Dictionary<string, string>{{"access", _handler.WriteToken(newAccessToken)}};
        }

        throw new TokenNotValidException("Provided token is no longer valid");
    }
    
    public bool ValidateToken(string jwt)
    {
        if (string.IsNullOrWhiteSpace(jwt))
        {
            throw new NoTokenProvidedException("No refresh token provided");
        }
        
        if (jwt.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            jwt = jwt.Substring("Bearer ".Length).Trim();
        }
        
        var key = Encoding.ASCII.GetBytes(_privateKey);
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        try
        {
            _handler.ValidateToken(jwt, validationParameters, out SecurityToken validatedToken);
            return true;
        }
        catch (Exception ex)
        {
            throw new TokenNotValidException("Provided token couldn't be validated");
        }
    }

    private static ClaimsIdentity GenerateClaims(User user)
    {
        var ci = new ClaimsIdentity();
        
        ci.AddClaim(new Claim("id", user.UserId.ToString()));
        ci.AddClaim(new Claim(ClaimTypes.Email, user.Email));
        ci.AddClaim(new Claim(ClaimTypes.Role, user.isAdmin ? "Admin" : "User"));

        return ci;
    }
    
    private SecurityTokenDescriptor CreateToken(bool isAccessToken, ClaimsIdentity claims)
    {
        var encodedKey = Encoding.UTF8.GetBytes(_privateKey);
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(encodedKey),
            SecurityAlgorithms.HmacSha256);
        
        return new SecurityTokenDescriptor
        {
            SigningCredentials = credentials,
            Expires = isAccessToken ? DateTime.Now.AddMinutes(5) : DateTime.Now.AddHours(1),
            Subject = claims
        };
    }
}