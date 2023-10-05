using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Aes = System.Security.Cryptography.Aes;


namespace Walruslogics.Advertisement.Framework
{
  public class UrlEncryptionUtility
  {
    static readonly char[] padding = { '=' };
    public static string Encrypt(string clearText)
    {
      try
      {
        string EncryptionKey = "EDM";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
          Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
          encryptor.Key = pdb.GetBytes(32);
          encryptor.IV = pdb.GetBytes(16);
          using (MemoryStream ms = new MemoryStream())
          {
            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
            {
              cs.Write(clearBytes, 0, clearBytes.Length);
              cs.Close();
            }
            clearText = Convert.ToBase64String(ms.ToArray()).TrimEnd(padding).Replace('+', '-').Replace('/', '_');
          }
        }
        return clearText;
      }
      catch
      {

      }
      return "";
    }

    public static string Decrypt(string cipherText)
    {
      string incoming = cipherText.Replace('_', '/').Replace('-', '+');
      switch (cipherText.Length % 4)
      {
        case 2: incoming += "=="; break;
        case 3: incoming += "="; break;
      }
      try
      {
        string EncryptionKey = "EDM";
        byte[] cipherBytes = Convert.FromBase64String(incoming);
        using (Aes encryptor = Aes.Create())
        {
          Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
          encryptor.Key = pdb.GetBytes(32);
          encryptor.IV = pdb.GetBytes(16);
          using (MemoryStream ms = new MemoryStream())
          {
            using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
            {
              cs.Write(cipherBytes, 0, cipherBytes.Length);
              cs.Close();
            }
            cipherText = Encoding.Unicode.GetString(ms.ToArray());
          }
        }
        return cipherText;
      }
      catch
      {

      }
      return "";
    }
  }
}
