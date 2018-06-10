using System;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace Ambiel.Utility.StringExtension
{
    public class CompressHelper
    {
        /// <summary>
        /// dec加密
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static byte[] Encrypt(byte[] buffer)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes("ambiel");
                byte[] rgbIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                using (MemoryStream mStream = new MemoryStream())
                {
                    CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                    cStream.Write(buffer, 0, buffer.Length);
                    cStream.FlushFinalBlock();
                    return mStream.ToArray();
                }
            }
            catch
            {
                return buffer;
            }
        }
        public static byte[] UnEncrypt(byte[] buffer)
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes("ambiel");
            byte[] rgbIV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
            DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
            using(MemoryStream reader = new MemoryStream())
            {
                using(MemoryStream mStream = new MemoryStream(buffer))
                {
                    CryptoStream cStream = new CryptoStream(mStream,DCSP.CreateDecryptor(rgbKey,rgbIV),CryptoStreamMode.Read);
                    while(cStream.CanRead)
                    {
                        byte[] cache = new byte[1024];
                        int count = cStream.Read(cache,0,cache.Length);
                        if(count<1)
                            break;
                        reader.Write(cache,0,count);
                    }
                }
                return reader.ToArray();
            } 
        }
        public static string GZipCompress(byte[] buffer)
        {
            try
            {
                using(MemoryStream stream = new MemoryStream())
                {
                    using(GZipStream gZipStream = new GZipStream(stream,CompressionMode.Compress))
                    {
                        gZipStream.Write(buffer,0,buffer.Length);
                        gZipStream.Close();
                    }
                    return System.Convert.ToBase64String(stream.ToArray());
                }
            }
            catch
            {
                
                return System.Convert.ToBase64String(buffer);
            }
        }
    }
}