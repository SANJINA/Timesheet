using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace MVCTutorial.WebAPI
{
    public class Cryptography
    {
        //private static String securityAESKey = "YHeJuDTJGFKItS456H67qFD9GKBZg7ks";
        private static String securityAESKey = "YHeJuDTJGFKItS456H67qFD9GKBZg$#e";

        public static string Encryption(string decryptedstring)
        {
            return AESEncryption(decryptedstring);
        }

        public static string Decryption(string encryptedstring)
        {
            return AESDecryption(encryptedstring);
        }


        private static string DecryptString(string base64StringToDecrypt, string passphrase)
        {
            RijndaelManaged AesEncryption = new RijndaelManaged();
            string plainStr = base64StringToDecrypt; // The text that would be encrypted
            AesEncryption.KeySize = 256; // 192, 256
            AesEncryption.BlockSize = 128;
            AesEncryption.Mode = CipherMode.CBC;
            AesEncryption.Padding = PaddingMode.PKCS7;

            byte[] keyArr = Encoding.UTF8.GetBytes(passphrase);
            byte[] ivArr = new byte[16] { 8, 4, 6, 2, 8, 4, 6, 7, 1, 2, 9, 8, 8, 1, 0, 4 };
            
            AesEncryption.IV = ivArr;
            AesEncryption.Key = keyArr;

            // This array will contain the plain text in bytes
            byte[] cipherText = Convert.FromBase64String(base64StringToDecrypt);

            ICryptoTransform decrypto = AesEncryption.CreateDecryptor();
            // The result of the encrypion and decryption
            byte[] decryptedText = decrypto.TransformFinalBlock(cipherText, 0, cipherText.Length);
            return ASCIIEncoding.UTF8.GetString(decryptedText);
        }

        private static string EncryptString(string plainSourceStringToEncrypt, string passPhrase)
        {
            RijndaelManaged AesEncryption = new RijndaelManaged();
            AesEncryption.KeySize = 256; // 192, 256
            AesEncryption.BlockSize = 128;
            AesEncryption.Mode = CipherMode.CBC;
            AesEncryption.Padding = PaddingMode.PKCS7;

            //AesManaged aes = new AesManaged();
            //aes.GenerateIV();

            byte[] keyArr = Encoding.UTF8.GetBytes(passPhrase);
            byte[] ivArr = new byte[16] { 8, 4, 6, 2, 8, 4, 6, 7, 1, 2, 9, 8, 8, 1, 0, 4 };

            AesEncryption.IV = ivArr;
            AesEncryption.Key = keyArr;

            // This array will contain the plain text in bytes
            byte[] plainText = ASCIIEncoding.UTF8.GetBytes(plainSourceStringToEncrypt);

            // Creates Symmetric encryption and decryption objects   
            ICryptoTransform crypto = AesEncryption.CreateEncryptor();
            ICryptoTransform decrypto = AesEncryption.CreateDecryptor();
            // The result of the encrypion and decryption
            byte[] cipherText = crypto.TransformFinalBlock(plainText, 0, plainText.Length);
            return Convert.ToBase64String(cipherText);
        }

        private static string AESDecryption(string encryptedstring)
        {
            string decryptedkey = string.Empty;
            try
            {
                decryptedkey = DecryptString(encryptedstring.Replace("%2b", "+").ToString(), securityAESKey);
            }
            catch
            {
                return null;
            }
            return decryptedkey;
        }

        private static string AESEncryption(string decryptedstring)
        {
            string encryptedstring = string.Empty;
            try
            {
                encryptedstring = EncryptString(decryptedstring, securityAESKey);
                return encryptedstring;
            }
            catch
            {
                return encryptedstring;
            }
        }

    }
}