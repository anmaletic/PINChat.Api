using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;

namespace PINChat.Api.Library.Services;

public class EncryptionService : IEncryptionService
{
    private readonly byte[] _key;
    
    public EncryptionService(IConfiguration config)
    {
        _key = Convert.FromBase64String(config["EncryptionKey"]!);
    }
    
    public string Encrypt(string plainText)
    {
        using var aesAlg = Aes.Create();
        var iv = new byte[16];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(iv);

        aesAlg.Key = _key;
        aesAlg.IV = iv;
    
        using var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
    
        using var msEncrypt = new MemoryStream();
        using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
        using var swEncrypt = new StreamWriter(csEncrypt);
        swEncrypt.Write(plainText);
    
        return $"{Convert.ToBase64String(iv)}.{msEncrypt.ToArray()}";
    }
    
    public string Decrypt(string cipherText)
    {
        using var aesAlg = Aes.Create();
        var cipherParts = cipherText.Split(".");
            
        aesAlg.Key = _key;
        aesAlg.IV = Convert.FromBase64String(cipherParts[0]);
    
        using var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
    
        using var msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText));
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var srDecrypt = new StreamReader(csDecrypt);
        return srDecrypt.ReadToEnd();
    }
}