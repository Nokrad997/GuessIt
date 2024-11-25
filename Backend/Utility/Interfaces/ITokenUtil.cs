using Backend.Entities;

namespace Backend.Utility.Interfaces;

public interface ITokenUtil
{
    Dictionary<string, string> CreateTokenPair(User user);
    Dictionary<string, string> RefreshAccessToken(string refreshToken);
    bool ValidateToken(string jwt);
    int GetIdFromToken(string token);
    string GetRoleFromToken(string token);
    
}