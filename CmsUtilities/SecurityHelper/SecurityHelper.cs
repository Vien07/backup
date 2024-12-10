using NETCore.Encrypt;
using NETCore.Encrypt.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CmsUtilities.SecurityHelper
{
    ///<summary>Mã hóa password thì dùng hàm không thể giải mã như MD5, SHA</summary>
    public static class SecurityHelper
    {
        private static readonly string keyEncrypt = "b15da5898a4e4133bbce2ea2315a1916";
        public static string EncryptString(string plainText)
        {
            try
            {
                byte[] iv = new byte[16];
                byte[] array;

                using (System.Security.Cryptography.Aes aes = System.Security.Cryptography.Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(keyEncrypt);
                    aes.IV = iv;

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                            {
                                streamWriter.Write(plainText);
                            }

                            array = memoryStream.ToArray();
                        }
                    }
                }

                return Convert.ToBase64String(array);
            }
            catch
            {
                return "";
            }         
        }
        public static string DecryptString(string cipherText)
        {
            try
            {
                byte[] iv = new byte[16];
                byte[] buffer = Convert.FromBase64String(cipherText);

                using (System.Security.Cryptography.Aes aes = System.Security.Cryptography.Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(keyEncrypt);
                    aes.IV = iv;
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                            {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch
            {
                return "";
            }        
        }

        #region AES
        private static AESKey aesKey = EncryptProvider.CreateAesKey();
        private static string keyAES = aesKey.Key;
        private static string ivAES = aesKey.IV;
        public static string AESEncrypt(string plaintext)
        {
            return EncryptProvider.AESEncrypt(plaintext, keyAES, ivAES);
        }
        public static string AESDecrypt(string encrypted)
        {
            return EncryptProvider.AESDecrypt(encrypted, keyAES, ivAES);
        }
        #endregion

        #region DES
        private static string desKey = EncryptProvider.CreateDesKey();
        public static string DESEncrypt(string plaintext)
        {
            return EncryptProvider.DESEncrypt(plaintext, desKey);
        }
        public static string DESDecrypt(string encrypted)
        {
            return EncryptProvider.DESDecrypt(encrypted, desKey);
        }
        #endregion

        #region MD5 not decrypt------- 
        ///<summary>length 16 or 32</summary>
        public static string Md5(string plaintext, int length = 16)
        {
            if (length == 16 || length == 32)
                return EncryptProvider.Md5(plaintext, length == 16 ? MD5Length.L16 : MD5Length.L32);
            return plaintext;
        }
        #endregion

        #region SHA not decrypt------- 
        public static string Sha1(string plaintext)
        {
            return EncryptProvider.Sha1(plaintext);
        }
        public static string Sha256(string plaintext)
        {
            return EncryptProvider.Sha256(plaintext);
        }
        public static string Sha384(string plaintext)
        {
            return EncryptProvider.Sha384(plaintext);
        }
        public static string Sha512(string plaintext)
        {
            return EncryptProvider.Sha512(plaintext);
        }
        #endregion

        #region RSA
        private static RSAKey rsaKey = EncryptProvider.CreateRsaKey();    //default is 2048
        //private static RSAKey rsaKey = EncryptProvider.CreateRsaKey(RsaSize.R3072);
        private static string publicKey = rsaKey.PublicKey;
        private static string privateKey = rsaKey.PrivateKey;
        public static string RSAEncrypt(string plaintext)
        {
            return EncryptProvider.RSAEncrypt(publicKey, plaintext);
        }
        public static string RSADecrypt(string encrypted)
        {
            return EncryptProvider.RSADecrypt(privateKey, encrypted);
        }
        #endregion

        #region Base64
        public static string Base64Encrypt(string plaintext)
        {
            return EncryptProvider.Base64Encrypt(plaintext);
        }
        public static string Base64Decrypt(string encrypted)
        {
            return EncryptProvider.Base64Decrypt(encrypted);
        }
        #endregion
    }
}
