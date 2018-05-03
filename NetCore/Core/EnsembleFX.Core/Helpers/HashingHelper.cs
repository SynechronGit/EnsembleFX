using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EnsembleFX.Utility
{
    public static class HashingHelper
    {
        public static string HashSHA512(string password)
        {
            SHA512 shaM = new SHA512Managed();
            byte[] hash =
             shaM.ComputeHash(Encoding.ASCII.GetBytes(password));

            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }

        public static bool Validate(string enteredValue, string hashedValue)
        {
            if (HashSHA512(enteredValue) == hashedValue) return true;

            return false;
        }

        public static string GetHash256(string input)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();

            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(input);

            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }

        public static string GetHash1(string input)
        {
            byte[] byteValue= UnicodeEncoding.Unicode.GetBytes(input);
            SHA1Managed hashString = new SHA1Managed();
            string hex = "";
            byte[] hashValue;
            hashValue = hashString.ComputeHash(byteValue);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }
    }
}
