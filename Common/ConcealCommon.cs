using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    /// <summary>
    /// 加密解密
    /// </summary>
    public static class ConcealCommon
    {
        /// <summary>
        /// des秘钥
        /// </summary>
        private static string DES_KEY = "automaticdev#key";

        /// <summary>
        /// 加密服务器密码
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string EncServerPassword(int id, string password, string salt)
        {
            return MakeCommon.MakeMD5(salt + MakeCommon.MakeMD5(id + password + salt) + salt);
        }

        //默认密钥向量 
        private static byte[] Keys = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };

        /// <summary> 
        /// DES加密字符串 
        /// </summary> 
        /// <param name="encryptString">待加密的字符串</param> 
        /// <param name="encryptKey">加密密钥,要求为16位</param> 
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns> 

        public static string EncryptDES(string encryptString)
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(DES_KEY.Substring(0, 16));
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            var DCSP = Aes.Create();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }

        /// <summary> 
        /// DES解密字符串 
        /// </summary> 
        /// <param name="decryptString">待解密的字符串</param> 
        /// <returns>解密成功返回解密后的字符串</returns> 

        public static string DecryptDES(string decryptString)
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(DES_KEY.Substring(0, 16));
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            var DCSP = Aes.Create();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }
    }
}
