using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Collections.Specialized;

namespace Walruslogics.Advertisement.Framework.Security
{
    public static class Cryptography
    {
        private const int keysize = 256;
        private static readonly string _defalutSalt = "Zi88le46oa73is27";
        private static readonly string _defaultPassPhrase = "^@iKeb#)pcU^$H57*";

        private static readonly string _key = "kOReGT2IIMiieTON6NpEgxwLyrNEjklO";

        public static string GetHashedString(string stringToHash)
        {
            byte[] data = Encoding.ASCII.GetBytes(stringToHash);
            SHA256 shaManaged = new SHA256Managed();
            byte[] result = shaManaged.ComputeHash(data);

            return Convert.ToBase64String(result);
        }

        public static bool CompareHashedString(string stringValue, string hashedStringValue)
        {
            return GetHashedString(stringValue) == hashedStringValue;
        }

        public static string Encrypt(string stringToEncrypt, string passPhrase, string salt16Chars)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(stringToEncrypt);
            using (PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(passPhrase, null))
            {
                byte[] keyBytes = passwordDeriveBytes.GetBytes(keysize / 8);
                using (RijndaelManaged symmetricKey = new RijndaelManaged())
                {
                    byte[] initVectorBytes = Encoding.ASCII.GetBytes(salt16Chars);

                    symmetricKey.Mode = CipherMode.CBC;
                    using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes))
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                byte[] cipherTextBytes = memoryStream.ToArray();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        public static string Encrypt(string stringToEncrypt, string passPhrase)
        {
            return Encrypt(stringToEncrypt, passPhrase, _defalutSalt);
        }

        public static string Encrypt(string stringToEncrypt)
        {
            return Encrypt(stringToEncrypt, _defaultPassPhrase);
        }

        public static string Decrypt(string stringToDecrypt, string passPhrase, string salt16Chars)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(stringToDecrypt);
            using (PasswordDeriveBytes passwordDeriveBytes = new PasswordDeriveBytes(passPhrase, null))
            {
                byte[] keyBytes = passwordDeriveBytes.GetBytes(keysize / 8);
                using (RijndaelManaged symmetricKey = new RijndaelManaged())
                {
                    byte[] initVectorBytes = Encoding.ASCII.GetBytes(salt16Chars);

                    symmetricKey.Mode = CipherMode.CBC;
                    using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes))
                    {
                        using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        public static string Decrypt(string stringToDecrypt, string passPhrase)
        {
            return Decrypt(stringToDecrypt, passPhrase, _defalutSalt);
        }

        public static string Decrypt(string stringToDecrypt)
        {
            return Decrypt(stringToDecrypt, _defaultPassPhrase);
        }

        public static string EncryptQueryString(string plainString)
        {
            // Check arguments.
            if (string.IsNullOrEmpty(plainString))
                throw new ArgumentNullException("plainString");

            byte[] encrypted;
            // Create an AesManaged object
            // with the specified key and IV.
            using (Rijndael algorithm = Rijndael.Create())
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(_key);

                algorithm.Key = keyBytes;

                // Create a decrytor to perform the stream transform.
                var encryptor = algorithm.CreateEncryptor(algorithm.Key, algorithm.IV);

                // Create the streams used for encryption.
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            // Write IV first
                            msEncrypt.Write(algorithm.IV, 0, algorithm.IV.Length);
                            //Write all data to the stream.
                            swEncrypt.Write(plainString);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            string resetCode = Convert.ToBase64String(encrypted);

            return resetCode;
        }

        public static string DecryptQueryString(string cipherString)
        {
            try
            {
                // Check arguments.
                if (string.IsNullOrEmpty(cipherString))
                    throw new ArgumentNullException("cipherString");

                // Declare the string used to hold
                // the decrypted text.
                string plaintext = null;

                // Create an AesManaged object
                // with the specified key and IV.
                using (Rijndael algorithm = Rijndael.Create())
                {
                    byte[] keyBytes = Encoding.UTF8.GetBytes(_key);

                    algorithm.Key = keyBytes;

                    // Get bytes from input string
                    byte[] cipherBytes = Convert.FromBase64String(cipherString);

                    // Create the streams used for decryption.
                    using (MemoryStream msDecrypt = new MemoryStream(cipherBytes))
                    {
                        // Read IV first
                        byte[] IV = new byte[16];
                        msDecrypt.Read(IV, 0, IV.Length);

                        // Assign IV to an algorithm
                        algorithm.IV = IV;

                        // Create a decrytor to perform the stream transform.
                        var decryptor = algorithm.CreateDecryptor(algorithm.Key, algorithm.IV);

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
            catch (Exception)
            {
                return "Invalid Token";
            }
        }

        public static NameValueCollection DescrambleQueryString(string queryString)
        {
            NameValueCollection result = new NameValueCollection();
            char[] splitChar = new char[] { '&' };
            char[] equalChar = new char[] { '=' };

            // Split query string to components
            foreach (string s in queryString.Split(splitChar))
            {
                // split each component to key and value
                string[] keyVal = s.Split(equalChar, 2);
                string key = keyVal[0];
                string val = String.Empty;
                if (keyVal.Length > 1) val = keyVal[1];
                // Add to the hashtable
                result.Add(key, val);
            }

            // return the resulting hashtable
            return result;
        }

        /// <summary>
        /// Encrypt a sting with MD5 Encryption
        /// </summary>
        /// <param name="plainString">Plan String</param>
        /// <returns>Encrypted String</returns>
        public static string EncryptMD5(string plainString)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(plainString));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                stringBuilder.Append(result[i].ToString("x2"));
            }

            return stringBuilder.ToString();
        }
    }
}
