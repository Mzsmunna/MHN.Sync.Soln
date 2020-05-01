using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Linq;

namespace MHN.Sync.Helper
{
    #region newCryptography
    public partial class Cryptography
    {
        private static string EncryptionKey = "@sde@@";
        private static byte[] Key;
        private static byte[] iv;
        public static string Encrypt(string cipherText)
        {
            PreConfig();

            byte[] encrypted = EncryptStringToBytes_Aes(cipherText, Key, iv);
            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt(string cipherText)
        {
            PreConfig();
            byte[] roundtrip = System.Convert.FromBase64String(cipherText);
            return DecryptStringFromBytes_Aes(roundtrip, Key, iv);
        }

        internal static void PreConfig()
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            Key = pdb.GetBytes(32);
            iv = pdb.GetBytes(16);

        }
        internal static byte[] EncryptStringToBytes_Aes(string plainText, byte[] Key, byte[] iv)
        {
            byte[] encrypted;
            byte[] IV;

            using (Aes aesAlg = Aes.Create())
            {

                aesAlg.Key = Key;
                aesAlg.IV = iv;
                IV = aesAlg.IV;
                aesAlg.Mode = CipherMode.CBC;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption. 
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            var combinedIvCt = new byte[IV.Length + encrypted.Length];
            Array.Copy(IV, 0, combinedIvCt, 0, IV.Length);
            Array.Copy(encrypted, 0, combinedIvCt, IV.Length, encrypted.Length);

            // Return the encrypted bytes from the memory stream. 
            return combinedIvCt;

        }

        internal static string DecryptStringFromBytes_Aes(byte[] cipherTextCombined, byte[] Key, byte[] iv)
        {

            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an Aes object 
            // with the specified key and IV. 
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;

                byte[] IV = iv;
                byte[] cipherText = new byte[cipherTextCombined.Length - IV.Length];

                Array.Copy(cipherTextCombined, IV, IV.Length);
                Array.Copy(cipherTextCombined, IV.Length, cipherText, 0, cipherText.Length);

                aesAlg.IV = IV;

                aesAlg.Mode = CipherMode.CBC;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption. 
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }
    }
    #endregion newCryptography

    #region oldCryptography
    //public static class Cryptography
    //{
    //    public static string plainText;
    //    public static string passPhrase = "Pas5pr@se";
    //    public static string saltValue = "s@1tValue";
    //    public static string hashAlgorithm = "MD5";
    //    public static int passwordIterations = 2;
    //    public static string initVector = "@1B2c3D4e5F6g7H8";
    //    public static int keySize = 256;

    //    public static string Encrypt(string plainText)
    //    {
    //        if (!string.IsNullOrEmpty(plainText))
    //        {
    //            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
    //            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
    //            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
    //            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);
    //            byte[] keyBytes = password.GetBytes(keySize / 8);
    //            RijndaelManaged symmetricKey = new RijndaelManaged();
    //            symmetricKey.Mode = CipherMode.CBC;
    //            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes);
    //            MemoryStream memoryStream = new MemoryStream();
    //            CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
    //            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
    //            cryptoStream.FlushFinalBlock();
    //            byte[] cipherTextBytes = memoryStream.ToArray();
    //            //cryptoStream.Close();
    //            memoryStream.Close();
    //            string cipherText = Convert.ToBase64String(cipherTextBytes);
    //            return EncryptionUrlFriendlyString(cipherText);
    //        }
    //        return null;
    //    }

    //    public static string Decrypt(string cipherText)
    //    {
    //        if (!string.IsNullOrEmpty(cipherText))
    //        {
    //            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
    //            byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);
    //            byte[] cipherTextBytes = Convert.FromBase64String(DecryptionUrlFriendlyString(cipherText));
    //            PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm,
    //                                                                   passwordIterations);
    //            byte[] keyBytes = password.GetBytes(keySize / 8);
    //            RijndaelManaged symmetricKey = new RijndaelManaged();
    //            symmetricKey.Mode = CipherMode.CBC;
    //            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);
    //            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
    //            CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
    //            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
    //            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
    //            //cryptoStream.Close();
    //            memoryStream.Close();
    //            string plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
    //            return plainText;
    //        }
    //        return null;
    //    }

    //    private static string DecryptionUrlFriendlyString(string stringToDecrypt)
    //    {
    //        if (!string.IsNullOrEmpty(stringToDecrypt))
    //        {
    //            stringToDecrypt = stringToDecrypt.Replace("-2F-", "/");
    //            stringToDecrypt = stringToDecrypt.Replace("-21-", "!");
    //            stringToDecrypt = stringToDecrypt.Replace("-23-", "#");
    //            stringToDecrypt = stringToDecrypt.Replace("-24-", "$");
    //            stringToDecrypt = stringToDecrypt.Replace("-26-", "&");
    //            stringToDecrypt = stringToDecrypt.Replace("-27-", "'");
    //            stringToDecrypt = stringToDecrypt.Replace("-28-", "(");
    //            stringToDecrypt = stringToDecrypt.Replace("-29-", ")");
    //            stringToDecrypt = stringToDecrypt.Replace("-2A-", "*");
    //            stringToDecrypt = stringToDecrypt.Replace("-2B-", "+");
    //            stringToDecrypt = stringToDecrypt.Replace("-2C-", ",");
    //            stringToDecrypt = stringToDecrypt.Replace("-3A-", ":");
    //            stringToDecrypt = stringToDecrypt.Replace("-3B-", ";");
    //            stringToDecrypt = stringToDecrypt.Replace("-3D-", "=");
    //            stringToDecrypt = stringToDecrypt.Replace("-3F-", "?");
    //            stringToDecrypt = stringToDecrypt.Replace("-40-", "@");
    //            stringToDecrypt = stringToDecrypt.Replace("-5B-", "[");
    //            stringToDecrypt = stringToDecrypt.Replace("-5D-", "]");

    //            return stringToDecrypt;
    //        }
    //        return null;
    //    }

    //    private static string EncryptionUrlFriendlyString(string returnstring)
    //    {
    //        if (!string.IsNullOrEmpty(returnstring))
    //        {
    //            returnstring = returnstring.Replace("/", "-2F-");
    //            returnstring = returnstring.Replace("!", "-21-");
    //            returnstring = returnstring.Replace("#", "-23-");
    //            returnstring = returnstring.Replace("$", "-24-");
    //            returnstring = returnstring.Replace("&", "-26-");
    //            returnstring = returnstring.Replace("'", "-27-");
    //            returnstring = returnstring.Replace("(", "-28-");
    //            returnstring = returnstring.Replace(")", "-29-");
    //            returnstring = returnstring.Replace("*", "-2A-");
    //            returnstring = returnstring.Replace("+", "-2B-");
    //            returnstring = returnstring.Replace(",", "-2C-");
    //            returnstring = returnstring.Replace(":", "-3A-");
    //            returnstring = returnstring.Replace(";", "-3B-");
    //            returnstring = returnstring.Replace("=", "-3D-");
    //            returnstring = returnstring.Replace("?", "-3F-");
    //            returnstring = returnstring.Replace("@", "-40-");
    //            returnstring = returnstring.Replace("[", "-5B-");
    //            returnstring = returnstring.Replace("]", "-5D-");
    //            return returnstring;
    //        }
    //        return null;
    //    }
    //}
    #endregion oldCryptograph
}
