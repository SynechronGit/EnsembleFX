using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace EnsembleFX.Encryption
{
    public class Cryptography
    {
        #region Constants
        const string KEY = "abcdefghijklmnop";//TODO: need to revisit as key is not recognized properly
        #endregion



        #region Public Methods
        /// <summary>
        /// Encryption
        /// </summary>
        /// <param name="input">Provide input string</param>
        /// <returns></returns>
        public string Encrypt(string input)
        {
            byte[] inputArray = UTF8Encoding.UTF8.GetBytes(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(KEY);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tripleDES.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        /// <summary>
        /// This method Decrypts Input
        /// </summary>
        /// <param name="input">Provide Input Text</param>
        /// <param name="key">provide Key Text</param>
        /// <returns></returns>
        public string Decrypt(string input)
        {
            byte[] inputArray = Convert.FromBase64String(input);
            TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
            tripleDES.Key = UTF8Encoding.UTF8.GetBytes(KEY);
            tripleDES.Mode = CipherMode.ECB;
            tripleDES.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tripleDES.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
            tripleDES.Clear();
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        #endregion
    }
}
