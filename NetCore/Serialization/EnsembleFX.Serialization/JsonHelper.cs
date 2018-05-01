using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Text;


namespace EnsembleFX.Utility
{
    public class JsonHelper
    {

        private const string INDENT_STRING = "    ";

        public static string FormatJson(string str)
        {
            var indent = 0;
            var quoted = false;
            var sb = new StringBuilder();
            for (var i = 0; i < str.Length; i++)
            {
                var ch = str[i];
                switch (ch)
                {
                    case '{':
                    case '[':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, ++indent).ForEach(item => sb.Append(INDENT_STRING));
                        }
                        break;
                    case '}':
                    case ']':
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, --indent).ForEach(item => sb.Append(INDENT_STRING));
                        }
                        sb.Append(ch);
                        break;
                    case '"':
                        sb.Append(ch);
                        bool escaped = false;
                        var index = i;
                        while (index > 0 && str[--index] == '\\')
                            escaped = !escaped;
                        if (!escaped)
                            quoted = !quoted;
                        break;
                    case ',':
                        sb.Append(ch);
                        if (!quoted)
                        {
                            sb.AppendLine();
                            Enumerable.Range(0, indent).ForEach(item => sb.Append(INDENT_STRING));
                        }
                        break;
                    case ':':
                        sb.Append(ch);
                        if (!quoted)
                            sb.Append(" ");
                        break;
                    default:
                        sb.Append(ch);
                        break;
                }
            }
            return sb.ToString();
        }

        public static bool IsValidJson(string stringValue)
        {
            if (string.IsNullOrWhiteSpace(stringValue) == false)
            {
                var value = stringValue.Trim();
                if ((value.StartsWith("{") && value.EndsWith("}")) || //For object
                    (value.StartsWith("[") && value.EndsWith("]"))) //For array
                {
                    try
                    {
                        var obj = Newtonsoft.Json.Linq.JToken.Parse(value);
                        return true;
                    }
                    catch (Newtonsoft.Json.JsonReaderException)
                    {
                        return false;
                    }
                }
            }

            return false;
        }

        public static string Serialize(object instance)
        {
            return JsonSerializer.SerializeToString(instance, instance.GetType());
        }


        //public static string SerializeMessage(IMessage instance)
        //{
        //    return JsonSerializer.SerializeToString(instance, instance.GetType());
        //}

        public static string SerializeMessage<T>(T instance)
        {
            return new JsonSerializer<T>().SerializeToString(instance);
        }


        public static object Deserialize(string serializedInstance, string messageTypeName)
        {
            Type messageType = Type.GetType(messageTypeName, true);
            object deserializedMessage = JsonSerializer.DeserializeFromString(serializedInstance, messageType);
            return deserializedMessage;
        }

        public static T Deserialize<T>(Uri locationUrl)
        {
            MemoryStream stream = null;
            if (locationUrl.Scheme.ToLowerInvariant() == "file")
            {
                StreamReader streamReader = File.OpenText(locationUrl.LocalPath);
                stream = new MemoryStream(System.Text.UnicodeEncoding.Unicode.GetBytes(streamReader.ReadToEnd()));

            }
            if (locationUrl.Scheme.ToLowerInvariant() == "http")
            {
                var httpClient = new System.Net.WebClient();
                stream = new MemoryStream(httpClient.DownloadData(locationUrl));

            }
            if (typeof(T).Equals(typeof(string)))
            {
                return (T)Convert.ChangeType(System.Text.UnicodeEncoding.ASCII.GetString(stream.ToArray()), typeof(T), CultureInfo.CurrentCulture);
            }
            else
            {
                return JsonSerializer.DeserializeFromStream<T>(stream);
            }
        }

        public static T Deserialize<T>(string serializedObject)
        {
            return JsonSerializer.DeserializeFromString<T>(serializedObject);
        }

        public static T Deserialize<T>(Stream serializedObject)
        {
            return JsonSerializer.DeserializeFromStream<T>(serializedObject);
        }
    }

    static class Extensions
    {
        public static void ForEach<T>(this IEnumerable<T> ie, Action<T> action)
        {
            foreach (var i in ie)
            {
                action(i);
            }
        }
    }
}
