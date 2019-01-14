using System;
using System.Security.Cryptography;
using System.Text;

namespace BaseFramework
{
    public class RijndaelCryptUtil
    {
        private const string M_KEY = "meevii.game.file.key.tryyourbest";

        public static string Encrypt(string pString, string pKey = "")
        {
            if (string.IsNullOrEmpty(pKey))
            {
                pKey = M_KEY;
            }
            byte[] keyArray = Encoding.UTF8.GetBytes(pKey);
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(pString);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string pString, string pKey = "")
        {
            if (string.IsNullOrEmpty(pKey))
            {
                pKey = M_KEY;
            }
            byte[] keyArray = Encoding.UTF8.GetBytes(pKey);
            byte[] toEncryptArray = Convert.FromBase64String(pString);
            RijndaelManaged rDel = new RijndaelManaged();
            rDel.Key = keyArray;
            rDel.Mode = CipherMode.ECB;
            rDel.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = rDel.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            return Encoding.UTF8.GetString(resultArray);
        }
    }
}
