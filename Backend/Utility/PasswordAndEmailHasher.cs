using System.Security.Cryptography;
using System.Text;
using Backend.Utility.Interfaces;
using BCrypt.Net;

namespace Backend.Utility;

public class PasswordAndEmailHasher : IPasswordAndEmailHasher
{
    public string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
    
    public string HashEmail(string email)
    {
        using (var sha256 = SHA256.Create())
        {
            var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(email));
            return Convert.ToBase64String(hash);
        }
    }

    public bool VerifyEmail(string email, string hashedEmail)
    {
        return HashEmail(email) == hashedEmail;
    }
}