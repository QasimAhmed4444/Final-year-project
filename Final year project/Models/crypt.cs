using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Final_year_project.Models
{
    public class crypt
    {
        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray= UTF32Encoding.UTF8.GetBytes(toEncrypt);
            System.Configuration.AppSettingsReader settingsReader = new System.Configuration.AppSettingsReader();

            //Get the key from Config file 

            string key = (string)settingsReader.GetValue("Securitykey", typeof(string));

            if (useHashing)
            { 

             MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider(); 
                keyArray = hashmd5.ComputeHash((UTF8Encoding.UTF8.GetBytes(key)));
                hashmd5.Clear();
            }
            else

            keyArray= UTF8Encoding.UTF8.GetBytes(toEncrypt);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.CBC;
            tdes.Padding = PaddingMode.None;

            ICryptoTransform CTransform  = tdes.CreateEncryptor();
            byte[] resultArray= CTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);

        }
    }
}