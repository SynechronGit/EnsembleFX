using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace EnsembleFX.Core.Helpers
{
    public static class ExtensionHelper
    {
        public static T Deserialize<T>(this string jsonObject)
        {
            return (T)JsonConvert.DeserializeObject(jsonObject, typeof(T));
        }
        public static string TrimNewLine(this string inputData)
        {
            if (inputData.Length > 4)
            {
                //string result = string.Empty;
                string isNewLine = inputData.Substring(inputData.Length - 4);

                if (isNewLine == "\\r\\n")
                {
                    inputData = inputData.Remove(inputData.Length - 4);
                }
                else if (isNewLine == "r\\n\"")
                {
                    inputData = inputData.Remove(inputData.Length - 5);
                }
            }
            return inputData;
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

        public static string RemoveSpecialCharactersWithoutSpace(this string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_]+", "", RegexOptions.Compiled);
        }

        public static string RemoveSpecialCharactersWithSpace(this string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_]+", " ", RegexOptions.Compiled);
        }

        public static string ReplaceSpecialCharactersWithEmptyString(this string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9_]+", "", RegexOptions.Compiled);
        }

        public static string ReplaceSpecialCharactersWithSingleSpace(this string str)
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

        public static bool ContainsAny(this string fullString, List<string> phraseList)
        {
            if (phraseList != null && phraseList.Count > 0 && !string.IsNullOrWhiteSpace(fullString))
                return phraseList.Any(d => fullString.ToLower().Contains(Convert.ToString(d).ToLower()));

            return false;
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

        public static double ConvertMetersToMiles(this double meters)
        {
            return meters * 0.00062137;
        }
        public static double ConvertMilesToKM(this double meters)
        {
            return meters / 1000;
        }
        public static T ToEnum<T>(this string inputString)
        {
            if (string.IsNullOrWhiteSpace(inputString))
                return default(T);
            return (T)Enum.Parse(typeof(T), Convert.ToString(inputString));
        }
        public static string GetDescription<TEnum>(this TEnum value) where TEnum : struct
        {
            FieldInfo field = value.GetType().GetField(value.ToString());

            DescriptionAttribute attribute
                    = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute))
                        as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }


        public static string StringConcat(string fullcomprehension, string comprehension)
        {
            if (string.IsNullOrEmpty(fullcomprehension))
            {
                fullcomprehension = comprehension.Replace('"', ' ').Trim();
            }
            else
            {
                fullcomprehension = string.Concat(fullcomprehension.Trim(), @"\r\n", comprehension.Replace('"', ' ').Trim());
            }
            return fullcomprehension;
        }

        public static string GetCreateRequestBodyString(string lineSeparator, string tabSepartor, string botName, string fullWidgetText)
        {

            string jsonString = "{ \"name\" : \"" + botName + "\", \"qnaPairs\": [{ \"question\":\"";

            jsonString = jsonString + fullWidgetText.Replace(lineSeparator, "\"}, { \"question\":\"");

            jsonString = jsonString.Replace(tabSepartor, "\",\"answer\":\"");

            jsonString = jsonString + "\"}]}";

            return jsonString;

        }
    }
}
