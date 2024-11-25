namespace Backend.Services.Interfaces;

public interface ITokenService
{
    bool Validate(string token);
    Dictionary<string, string> RefreshToken(string token);
}