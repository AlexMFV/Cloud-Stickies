using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WindowsCloudStickies
{
    public static class Encrypt
    {
        //EARLIER GENERATIONS
        //public static string EncryptString(string toEncrypt)
        //{
        //    SHA256 encryptor = SHA256.Create();
        //    byte[] encrypted = encryptor.ComputeHash(ToByteArray(toEncrypt));
        //    string test = Encoding.Default.GetString(encrypted);
        //    return test;
        //    //Convert.ToString(encrypted);
        //}
        //
        //public static byte[] ToByteArray(string toConvert)
        //{
        //    byte[] array = new byte[toConvert.Length];
        //
        //    int idx = 0;
        //    foreach(char c in toConvert)
        //    {
        //        array[idx] = Convert.ToByte(c);
        //        idx++;
        //    }
        //
        //    return array;
        //}

        public static string ComputeHash(string data)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(data));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
