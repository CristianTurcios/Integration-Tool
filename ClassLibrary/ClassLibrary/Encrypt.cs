using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Encrypt
    {
        // string key = "Laureate123*LNO321";
        string key ;

        public Encrypt()
        {
            SqlServerDatabase db = new SqlServerDatabase("172.20.33.13", "", "IntegrationTool", "", "SISUser", "test2016!", null);
            db.openConnection();
            key = db.executeQuery("Select KeyValue From Keys").TrimEnd('%');
            db.closeConnection();
        }

        public string encryptData(string data){
            byte[] arrayKey;

            byte[] arrayData = UTF8Encoding.UTF8.GetBytes(data);

            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

            arrayKey = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            md5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = arrayKey;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform encryptor = tdes.CreateEncryptor();

            byte[] encryptArray = encryptor.TransformFinalBlock(arrayData, 0, arrayData.Length);
            tdes.Clear();

            return Convert.ToBase64String(encryptArray, 0, encryptArray.Length);
        }

        public string decryptData(string data)
        {
            byte[] arrayKey;

             byte[] arrayData = Convert.FromBase64String(data);

             MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
             arrayKey = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
             md5.Clear();

             TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
             tdes.Key = arrayKey;
             tdes.Mode = CipherMode.ECB;
             tdes.Padding = PaddingMode.PKCS7;

             ICryptoTransform decryptor = tdes.CreateDecryptor();
             byte[] decryptArray = decryptor.TransformFinalBlock(arrayData, 0, arrayData.Length);
             tdes.Clear();

             return UTF8Encoding.UTF8.GetString(decryptArray);
        }
    }

}
