using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Lpp.Dns.DataMart.Client.Utils
{
    //NOTE: this is a clone of core functions of the crypto class in Lpp.Utilities since due to different frameworks that assembly cannot currently be referenced by the DMC.

    internal static class Crypto
    {
        private readonly static byte[] keyb = {	 2,253,5,54,52,91,193,
										 133,193,121,221,164,57,128,
										 91,91,19,39,111,193,125,98,
										 89,48,97,154,83,187,222,167,171,74};
        private readonly static byte[] ivb = { 	12,61,235,121,122,120,80,248,
										13,182,196,212,176,46,23,85};

        ///// <summary>
        ///// Takes a Guid and provides a relatively unique hashed value.
        ///// </summary>
        ///// <param name="UniqueID">The Guid to hash</param>
        ///// <returns>A hashed String of the Guid.</returns>
        //public static string Hash(this Guid UniqueID)
        //{
        //    using (System.Security.Cryptography.MACTripleDES des = new MACTripleDES())
        //    {
        //        return Base36.NumberToBase36(Convert.ToInt64(System.BitConverter.ToString(des.ComputeHash(UniqueID.ToByteArray())).Replace("-", ""), 16)).Replace("-", "0");
        //    }
        //}

        /// <summary>
        /// Encrypts a string using the cryptography class.
        /// </summary>
        /// <param name="src">The source string.</param>
        /// <returns>The encrypted string.</returns>
        public static string EncryptString(this string src)
        {
            if (src.IsEmpty())
                return string.Empty;

            byte[] p = Encoding.ASCII.GetBytes(src.ToCharArray());
            byte[] encodedBytes = { };

            using (RijndaelManaged rv = new RijndaelManaged())
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, rv.CreateEncryptor(keyb, ivb), CryptoStreamMode.Write))
                {
                    cs.Write(p, 0, p.Length);
                    cs.FlushFinalBlock();
                    encodedBytes = ms.ToArray();
                }
            }

            return Convert.ToBase64String(encodedBytes);
        }

        ///// <summary>
        ///// Decrypts a string created by the EncryptString function.
        ///// </summary>
        ///// <param name="src">The string to descrypt.</param>
        ///// <returns>The decrypted string.</returns>
        //public static string DecryptString(this string src)
        //{
        //    if (src.IsEmpty())
        //        return string.Empty;

        //    src = src.Trim("~".ToCharArray());

        //    byte[] p = Convert.FromBase64String(src);
        //    byte[] initialText = new Byte[p.Length];

        //    using (RijndaelManaged rv = new RijndaelManaged())
        //    using (MemoryStream ms = new MemoryStream(p))
        //    {
        //        using (CryptoStream cs = new CryptoStream(ms, rv.CreateDecryptor(keyb, ivb), CryptoStreamMode.Read))
        //        {
        //            cs.Read(initialText, 0, initialText.Length);
        //        }
        //    }

        //    StringBuilder sb = new StringBuilder();
        //    for (int i = 0; i < initialText.Length; ++i)
        //    {
        //        sb.Append((char)initialText[i]);
        //    }

        //    return sb.ToString().Replace("\0", ""); //Why does it put "/0" on the end?
        //}


        ///// <summary>
        ///// Encrypt the given string using AES.  The string can be decrypted using
        ///// DecryptStringAES().  The sharedSecret parameters must match.
        ///// </summary>
        ///// <param name="plainText">The text to encrypt.</param>
        ///// <param name="sharedSecret">A password used to generate a key for encryption.</param>
        //public static string EncryptStringAES(string plainText, string sharedSecret, string salt)
        //{
        //    if (string.IsNullOrEmpty(plainText))
        //        throw new ArgumentNullException("plainText");
        //    if (string.IsNullOrEmpty(sharedSecret))
        //        throw new ArgumentNullException("sharedSecret");
        //    if (string.IsNullOrEmpty(salt))
        //        throw new ArgumentNullException("salt");

        //    byte[] _salt = Encoding.ASCII.GetBytes(salt);

        //    string outStr = null;                 // Encrypted string to return
        //    RijndaelManaged aesAlg = null;        // RijndaelManaged object used to encrypt the data.

        //    try
        //    {
        //        // generate the key from the shared secret and the salt
        //        Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(sharedSecret, _salt);

        //        // Create a RijndaelManaged object
        //        aesAlg = new RijndaelManaged();
        //        aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);

        //        // Create a decryptor to perform the stream transform.
        //        ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

        //        // Create the streams used for encryption.
        //        using (MemoryStream msEncrypt = new MemoryStream())
        //        {
        //            // prepend the IV
        //            msEncrypt.Write(BitConverter.GetBytes(aesAlg.IV.Length), 0, sizeof(int));
        //            msEncrypt.Write(aesAlg.IV, 0, aesAlg.IV.Length);
        //            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
        //            {
        //                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
        //                {
        //                    //Write all data to the stream.
        //                    swEncrypt.Write(plainText);
        //                }
        //            }
        //            outStr = Convert.ToBase64String(msEncrypt.ToArray());
        //        }
        //    }
        //    catch
        //    {
        //        throw new SecurityException("Enter Proper Key value.");
        //    }
        //    finally
        //    {
        //        // Clear the RijndaelManaged object.
        //        if (aesAlg != null)
        //            aesAlg.Clear();
        //    }

        //    // Return the encrypted bytes from the memory stream.
        //    return outStr;
        //}

        ///// <summary>
        ///// Decrypt the given string.  Assumes the string was encrypted using
        ///// EncryptStringAES(), using an identical sharedSecret.
        ///// </summary>
        ///// <param name="cipherText">The text to decrypt.</param>
        ///// <param name="sharedSecret">A password used to generate a key for decryption.</param>
        //public static string DecryptStringAES(string cipherText, string sharedSecret, string salt)
        //{
        //    if (string.IsNullOrEmpty(cipherText))
        //        throw new ArgumentNullException("cipherText");
        //    if (string.IsNullOrEmpty(sharedSecret))
        //        throw new ArgumentNullException("sharedSecret");
        //    if (string.IsNullOrEmpty(salt))
        //        throw new ArgumentNullException("salt");

        //    byte[] _salt = Encoding.ASCII.GetBytes(salt);


        //    // Declare the RijndaelManaged object
        //    // used to decrypt the data.
        //    RijndaelManaged aesAlg = null;

        //    // Declare the string used to hold
        //    // the decrypted text.
        //    string plaintext = null;
        //    Rfc2898DeriveBytes key = null;
        //    MemoryStream msDecrypt = null;
        //    try
        //    {
        //        // generate the key from the shared secret and the salt
        //        key = new Rfc2898DeriveBytes(sharedSecret, _salt);

        //        // Create the streams used for decryption.
        //        byte[] bytes = Convert.FromBase64String(cipherText);
        //        msDecrypt = new MemoryStream(bytes);
        //        // Create a RijndaelManaged object
        //        // with the specified key and IV.
        //        aesAlg = new RijndaelManaged();
        //        aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
        //        // Get the initialization vector from the encrypted stream
        //        aesAlg.IV = ReadByteArray(msDecrypt);
        //        // Create a decrytor to perform the stream transform.
        //        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
        //        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
        //        {
        //            using (StreamReader srDecrypt = new StreamReader(csDecrypt))

        //                // Read the decrypted bytes from the decrypting stream
        //                // and place them in a string.
        //                plaintext = srDecrypt.ReadToEnd();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new SecurityException("Enter Proper Key value: " + ex.Message);
        //    }
        //    finally
        //    {
        //        // Clear the RijndaelManaged object.
        //        if (aesAlg != null)
        //            aesAlg.Dispose();

        //        if (key != null)
        //            key.Dispose();

        //        if (msDecrypt != null)
        //            msDecrypt.Dispose();
        //    }

        //    return plaintext;
        //}

        private static byte[] ReadByteArray(Stream s)
        {
            byte[] rawLength = new byte[sizeof(int)];
            if (s.Read(rawLength, 0, rawLength.Length) != rawLength.Length)
            {
                throw new SecurityException("Stream did not contain properly formatted byte array");
            }

            byte[] buffer = new byte[BitConverter.ToInt32(rawLength, 0)];
            if (s.Read(buffer, 0, buffer.Length) != buffer.Length)
            {
                throw new InvalidOperationException("Did not read byte array properly");
            }

            return buffer;
        }
    }
}
