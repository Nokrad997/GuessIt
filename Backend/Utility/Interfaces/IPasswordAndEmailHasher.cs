namespace Backend.Utility.Interfaces;

public interface IPasswordAndEmailHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
    string HashEmail(string email);
    bool VerifyEmail(string email, string hashedEmail);
}