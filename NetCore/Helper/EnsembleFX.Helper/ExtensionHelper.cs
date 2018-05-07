﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace EnsembleFX.Helper
{
    public static class ExtensionHelper
    {
        public static T Deserialize<T>(this string jsonObject)
        {
            return (T)JsonConvert.DeserializeObject(jsonObject, typeof(T));
        }

        public static DateTime? ToNullableDateTime(this string dateTimeString)
        {
            DateTime resultDateTime = new DateTime();

            if (DateTime.TryParse(dateTimeString, out resultDateTime))
                return resultDateTime;
            return null;
        }

        public static DateTime ToDateTime(this string dateTimeString)
        {
            DateTime resultDateTime = new DateTime();

            if (DateTime.TryParse(dateTimeString, out resultDateTime))
                return resultDateTime;
            return new DateTime();
        }

        public static decimal ToDecimal(this string decimalString)
        {
            decimal resultDecimal = 0;

            if (Decimal.TryParse(decimalString, out resultDecimal))
                return resultDecimal;
            return 0;
        }

        public static bool IsAny<T>(this List<T> entity)
        {
            if (entity != null && entity.Count > 0)
                return true;
            return false;
        }

        public static bool IsAny<T1, T2>(this Dictionary<T1, T2> entity)
        {
            if (entity != null && entity.Count > 0)
                return true;
            return false;
        }

        public static string RemoveHtmlString(this string inputText)
        {
            if (string.IsNullOrWhiteSpace(inputText))
                return string.Empty;

            return Regex.Replace(inputText, @"<[^>]+>|&nbsp;|\n|\t|\r", " ").Trim();
        }

        public static string RemoveNonAlphaNumericOnly(this string inputText)
        {
            if (string.IsNullOrWhiteSpace(inputText))
                return string.Empty;

            return Regex.Replace(inputText, "[^a-zA-Z0-9_ ]+", "").Trim();
        }

        public static string RemoveMultipleSpaces(this string inputText)
        {
            if (string.IsNullOrWhiteSpace(inputText))
                return string.Empty;

            return Regex.Replace(inputText, @"\s{2,}", " ").Trim();
        }

        public static string ConvertToString(this string inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString))
                return string.Empty;

            return Convert.ToString(inputString).Trim();
        }

        public static string RemoveSpecialCharactersWithoutSpace(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_]+", "", RegexOptions.Compiled);
        }

        public static string RemoveSpecialCharactersWithSpace(this string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_]+", " ", RegexOptions.Compiled);
        }

        public static string ConvertToHashKey(this string inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString))
                return string.Empty;

            return GetHashHashSHA1(inputString);
        }

        private static string GetHashSHA512(string hashKeyString)
        {
            HashAlgorithm hashAlgorithm = new SHA256CryptoServiceProvider();

            byte[] byteValue = System.Text.Encoding.UTF8.GetBytes(hashKeyString);

            byte[] byteHash = hashAlgorithm.ComputeHash(byteValue);

            return Convert.ToBase64String(byteHash);
        }

        public static string GetHashHashSHA1(string hashKeyString)
        {
            using (SHA1Managed sha1managed = new SHA1Managed())
            {
                var hash = sha1managed.ComputeHash(Encoding.UTF8.GetBytes(hashKeyString));
                var stringBuilder = new StringBuilder(hash.Length * 2);

                foreach (byte bitem in hash)
                {
                    // can be "x2" if you want lowercase
                    stringBuilder.Append(bitem.ToString("X2"));
                }

                return stringBuilder.ToString();
            }
        }

        public static string ReplaceFirstOccurrence(string source, string find, string replace)
        {
            int foundIndex = source.IndexOf(find);
            string result = source.Remove(foundIndex, find.Length).Insert(foundIndex, replace);
            return result;
        }

        public static byte[] ToByteArray(this Stream input)
        {
            byte[] result = new byte[1024];
            int bytesBuffer = 1024;
            byte[] buffer = new byte[bytesBuffer];
            using (MemoryStream ms = new MemoryStream())
            {
                int readBytes;
                while ((readBytes = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, readBytes);
                }
                result = ms.ToArray();
            }
            return result;
        }

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static string[] GetWords(this string inputString, string splitter)
        {
            return inputString.Trim().Split(new string[] { " " }, StringSplitOptions.None);
        }

        public static DateTime? ConvertToUsDate(this string inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString))
                return null;

            DateTime result = DateTime.MinValue;
            CultureInfo enUS = new CultureInfo("en-US");
            if (DateTime.TryParseExact(inputString, "MM/dd/yyyy", enUS, DateTimeStyles.None, out result))
                return result;
            return null;
        }

        public static string CleanDate(this string inputDateString)
        {
            if (string.IsNullOrWhiteSpace(inputDateString))
                return "";

            inputDateString = inputDateString.Replace("-", "/").Replace(" ", "");

            var regex = new Regex(@"[0-9]{2}\/[0-9]{2}\/[0-9]{4}");

            foreach (Match m in regex.Matches(inputDateString))
            {
                return m.Value;
            }

            return inputDateString;
        }
    }
}
