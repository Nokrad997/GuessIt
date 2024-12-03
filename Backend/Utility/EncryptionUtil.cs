using System.Security.Cryptography;

namespace Backend.Utility;

public class EncryptionUtil
{
    private readonly byte[] _key;
    private readonly byte[] _iv;

    public EncryptionUtil(string key, string iv)
    {
        _key = Convert.FromBase64String(key);
        _iv = Convert.FromBase64String(iv);
    }

    public string Encrypt(string plainText)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = _key;
            aes.IV = _iv;

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            {
                byte[] plainBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                byte[] encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                return Convert.ToBase64String(encryptedBytes);
            }
        }
    }

    public string Decrypt(string cipherText)
    {
        using (var aes = Aes.Create())
        {
            aes.Key = _key;
            aes.IV = _iv;

            using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
            {
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                byte[] plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                return System.Text.Encoding.UTF8.GetString(plainBytes);
            }
        }
    }
}
