using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;

namespace PINChat.Api.Library.Services;

public class EncryptionService : IEncryptionService
{
    private readonly byte[] _key;
    
    public EncryptionService(IConfiguration config)
    {
        _key = Convert.FromBase64String(config["Secrets:EncryptionKey"]!);
    }
    
    public string Encrypt(string plainText)
    {
        using var aesAlg = Aes.Create();

        aesAlg.Key = _key;
        aesAlg.GenerateIV();
        var iv = aesAlg.IV;
    
        using var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
    
        using var msEncrypt = new MemoryStream();
        using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
        using var swEncrypt = new StreamWriter(csEncrypt);
        swEncrypt.Write(plainText);
        swEncrypt.Flush();
        csEncrypt.FlushFinalBlock();
        
        // Combine IV and ciphertext into a single byte array
        var encryptedBytes = iv.Concat(msEncrypt.ToArray()).ToArray();

        // Encode the combined byte array as Base64
        return Convert.ToBase64String(encryptedBytes);
        }
    
    public string Decrypt(string cipherText)
    {
        // Decode the Base64-encoded ciphertext back to its byte array representation
        var encryptedBytes = Convert.FromBase64String(cipherText);

        // Extract the IV from the first 16 bytes of the encrypted data
        var iv = encryptedBytes.Take(16).ToArray();
    
        using var aesAlg = Aes.Create();
        aesAlg.Key = _key;
        aesAlg.IV = iv;

        using var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

        // Decrypt the rest of the encrypted data (excluding the IV)
        var encryptedData = encryptedBytes.Skip(16).ToArray();
        using var msDecrypt = new MemoryStream(encryptedData);
        using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
        using var srDecrypt = new StreamReader(csDecrypt);
    
        // Read and return the decrypted plaintext
        return srDecrypt.ReadToEnd();
    }
}