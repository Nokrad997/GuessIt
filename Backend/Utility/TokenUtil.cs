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
        if (!ValidateToken(refreshToken))
        {
            throw new TokenNotValidException("Provided token is no longer valid");
        }
        
        var jwtToken = (JwtSecurityToken) _handler.ReadToken(refreshToken);
        if (jwtToken == null)
        {
            throw new NoTokenProvidedException("No refresh token provided");
        }
            
        var claims = jwtToken.Claims;
        var accessTokenDescriptor = CreateToken(true, new ClaimsIdentity(claims));
        var newAccessToken = _handler.CreateToken(accessTokenDescriptor);
            
        return new Dictionary<string, string>{{"access", _handler.WriteToken(newAccessToken)}};
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
            return false;
        }
    }
    public int GetIdFromToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Token is empty");

        if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            token = token.Substring("Bearer ".Length).Trim();

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
            var principal = _handler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            var idClaim = principal.Claims.FirstOrDefault(c => c.Type == "id");
            return int.Parse(idClaim?.Value);

        }
        catch
        {
            throw new SecurityTokenValidationException("Invalid token");
        }
    }

    public string GetRoleFromToken(string token)
    {   
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException("Token is empty");

        if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            token = token.Substring("Bearer ".Length).Trim();

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
            var principal = _handler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            var roleClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            return roleClaim?.Value;
        }
        catch
        {
            throw new SecurityTokenValidationException("Invalid token");
        }
    }

    private static ClaimsIdentity GenerateClaims(User user)
    {
        var ci = new ClaimsIdentity();
        
        ci.AddClaim(new Claim("id", user.UserId.ToString()));
        ci.AddClaim(new Claim(ClaimTypes.Email, user.Email));
        ci.AddClaim(new Claim(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User"));

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
            Expires = isAccessToken ? DateTime.Now.AddHours(2) : DateTime.Now.AddHours(3),
            Subject = claims
        };
    }
}