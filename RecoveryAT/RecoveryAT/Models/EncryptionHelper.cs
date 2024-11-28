using System.Security.Cryptography;
using System.Text;
using System.IO;

public static class EncryptionHelper
{
    // Method to generate a random AES key and IV
    public static (string key, string iv) GenerateKeyAndIV()
    {
        using (var aesAlg = Aes.Create())
        {
            aesAlg.GenerateKey(); // Generate random key
            aesAlg.GenerateIV();  // Generate random IV

            string key = Convert.ToBase64String(aesAlg.Key); // Base64 encode the key
            string iv = Convert.ToBase64String(aesAlg.IV);   // Base64 encode the IV

            return (key, iv); // Return the key and IV as strings
        }
    }

    // Encrypt method
    public static string Encrypt(string plainText, string key, string iv)
    {
        using (var aesAlg = Aes.Create())
        {
            aesAlg.Key = Convert.FromBase64String(key); // Use the provided key
            aesAlg.IV = Convert.FromBase64String(iv);   // Use the provided IV

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (var writer = new StreamWriter(cryptoStream))
                    {
                        writer.Write(plainText);
                    }
                }
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }
    }

    // Decrypt method
    public static string Decrypt(string cipherText, string key, string iv)
    {
        using (var aesAlg = Aes.Create())
        {
            aesAlg.Key = Convert.FromBase64String(key); // Use the provided key
            aesAlg.IV = Convert.FromBase64String(iv);   // Use the provided IV

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            using (var memoryStream = new MemoryStream(Convert.FromBase64String(cipherText)))
            {
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (var reader = new StreamReader(cryptoStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }
    }
}
