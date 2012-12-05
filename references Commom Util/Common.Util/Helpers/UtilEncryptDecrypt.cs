using System;
using System.IO;
using System.Security.Cryptography;

namespace Common.Util.Helpers
{
    public class UtilEncryptDecrypt
    {
        string Password;

        public UtilEncryptDecrypt(string key)
        {
            Password = key;
        }

        public string Encrypt(string clearText)
        {
            return Encrypt(clearText, Password, true);
        }
        public string Encrypt(string clearText, string password, bool urlEncode)
        {
            if (string.IsNullOrEmpty(clearText))
                return clearText;

            byte[] clearBytes =
              System.Text.Encoding.Unicode.GetBytes(clearText);

            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password,
                new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});


            byte[] encryptedData = Encrypt(clearBytes,
                     pdb.GetBytes(32), pdb.GetBytes(16));

            if (System.Web.HttpContext.Current != null && urlEncode)
                return System.Web.HttpContext.Current.Server.UrlEncode(Convert.ToBase64String(encryptedData));
            else
                return Convert.ToBase64String(encryptedData);

        }
        byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        {
            MemoryStream ms = new MemoryStream();

            Rijndael alg = Rijndael.Create();

            alg.Key = Key;
            alg.IV = IV;

            CryptoStream cs = new CryptoStream(ms,
               alg.CreateEncryptor(), CryptoStreamMode.Write);


            cs.Write(clearData, 0, clearData.Length);


            cs.Close();

            byte[] encryptedData = ms.ToArray();

            return encryptedData;
        }

        public string Decrypt(string cipherText)
        {
            return Decrypt(cipherText, Password);
        }
        public string Decrypt(string cipherText, string password)
        {
            if (string.IsNullOrEmpty(cipherText))
                return cipherText;
            try
            {
                if (System.Web.HttpContext.Current != null)
                    cipherText = System.Web.HttpContext.Current.Server.UrlDecode(cipherText);

                cipherText = cipherText.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(cipherText);


                PasswordDeriveBytes pdb = new PasswordDeriveBytes(password,
                    new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 
            0x64, 0x76, 0x65, 0x64, 0x65, 0x76});

                byte[] decryptedData = Decrypt(cipherBytes,
                    pdb.GetBytes(32), pdb.GetBytes(16));

                return System.Text.Encoding.Unicode.GetString(decryptedData);
            }
            catch
            {
                return string.Empty;
            }
        }
        byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        {

            MemoryStream ms = new MemoryStream();


            Rijndael alg = Rijndael.Create();

            alg.Key = Key;
            alg.IV = IV;

            CryptoStream cs = new CryptoStream(ms,
                alg.CreateDecryptor(), CryptoStreamMode.Write);


            cs.Write(cipherData, 0, cipherData.Length);

            cs.Close();


            byte[] decryptedData = ms.ToArray();

            return decryptedData;
        }

       
    }
}
