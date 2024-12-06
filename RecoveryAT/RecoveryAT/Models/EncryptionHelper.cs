/**
    Date: 12/05/24
    Description: Class that allows for values to be secure by encrypting and decrypting using keys and initialization values to ensure
    that user information is protected.
    Bugs: None that we know of.
    Reflection: This class didn't take very long after we figured out what I had to do to implement AES.
**/

using System.Security.Cryptography;
using System.Text;
using System.IO;

public static class EncryptionHelper
{
    /// <summary>
    /// Generates a key and initialization vector that will be used to encrypt and decrypt values.
    /// </summary>
    /// <returns>The key and iv as strings.</returns>
    public static (string key, string iv) GenerateKeyAndIV()
    {
        using (var aesAlg = Aes.Create()) // create AES algorithm instance
        {
            aesAlg.GenerateKey(); // generate random cryptographic key
            aesAlg.GenerateIV();  // generate random initialization vector (iv)

            // convert the generated key and iv into base 64 encoded strings
            string key = Convert.ToBase64String(aesAlg.Key); 
            string iv = Convert.ToBase64String(aesAlg.IV);  

            return (key, iv); // return the key and IV as strings
        }
    }

    /// <summary>
    /// Secures sensitive information by encrypting the plaintext given using a key and initialization vector.
    /// </summary>
    /// <param name="plainText">The string to encrypt.</param>
    /// <param name="key">The key to use for encryption.</param>
    /// <param name="iv">The initialization vector for encryption.</param>
    /// <returns>The encrypted text as a string.</returns>
    public static string Encrypt(string plainText, string key, string iv)
    {
        using (var aesAlg = Aes.Create()) // create AES algorithm instance
        {
            // set the AES key and iv from the provided strings
            aesAlg.Key = Convert.FromBase64String(key); 
            aesAlg.IV = Convert.FromBase64String(iv);   

            // create an encryptor to perform the AES encryption
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV); 

            using (var memoryStream = new MemoryStream()) // use a memory stream to hold the encrypted data
            {
                // create a crypto stream to perform encryption
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (var writer = new StreamWriter(cryptoStream)) // write the plaintext to the crypto stream
                    {
                        writer.Write(plainText);
                    }
                }
                // convert the encrypted byte array to a base 64 encoded string and return it
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }
    }

    /// <summary>
    /// Displays sensitive information by decrypting the cipher text given using a key and initialization vector.
    /// </summary>
    /// <param name="cipherText">The string to decrypt.</param>
    /// <param name="key">The key to use for decryption.</param>
    /// <param name="iv">The initialization vector for decryption.</param>
    /// <returns>The decrypted text as a string.</returns>
    public static string Decrypt(string cipherText, string key, string iv)
    {
        using (var aesAlg = Aes.Create()) // create the instance of an AES algorithm
        {
            // set the AES key and iv from the provided strings
            aesAlg.Key = Convert.FromBase64String(key);
            aesAlg.IV = Convert.FromBase64String(iv); 

            // create the decryptor to perform the AES decryption
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            // use a memory stream to hold the encrypted data
            using (var memoryStream = new MemoryStream(Convert.FromBase64String(cipherText)))
            {
                // create a crypto stream to perform decryption
                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (var reader = new StreamReader(cryptoStream))
                    {
                        return reader.ReadToEnd(); // return the decrypted plaintext as a string
                    }
                }
            }
        }
    }
}
