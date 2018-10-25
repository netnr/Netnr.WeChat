using System;
using System.Globalization;
using System.Text;
using System.Xml;

namespace Netnr.WeChat
{
    public static class Extend
    {
        #region object ⇋ json

        /// <summary>
        /// object 转 JSON 字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="DateTimeFormat">时间格式化</param>
        /// <returns></returns>
        public static string ToJson(this object obj, string DateTimeFormat = "yyyy-MM-dd HH:mm:ss")
        {
            Newtonsoft.Json.Converters.IsoDateTimeConverter dtFmt = new Newtonsoft.Json.Converters.IsoDateTimeConverter
            {
                DateTimeFormat = DateTimeFormat
            };
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, dtFmt);
        }

        /// <summary>
        /// 解析 JSON字符串 为JObject对象
        /// </summary>
        /// <param name="json">JSON字符串</param>
        /// <returns>JObject对象</returns>
        public static Newtonsoft.Json.Linq.JObject ToJObject(this string json)
        {
            return Newtonsoft.Json.Linq.JObject.Parse(json);
        }

        /// <summary>
        /// 解析 JSON字符串 为JArray对象
        /// </summary>
        /// <param name="json">JSON字符串</param>
        /// <returns>JArray对象</returns>
        public static Newtonsoft.Json.Linq.JArray ToJArray(this string json)
        {
            return Newtonsoft.Json.Linq.JArray.Parse(json);
        }

        #endregion

        #region JSON转义
        /// <summary>
        /// 字符串 JSON转义
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string OfJson(this string s)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                char c = s.ToCharArray()[i];
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\""); break;
                    case '\\':
                        sb.Append("\\\\"); break;
                    case '/':
                        sb.Append("\\/"); break;
                    case '\b':
                        sb.Append("\\b"); break;
                    case '\f':
                        sb.Append("\\f"); break;
                    case '\n':
                        sb.Append("\\n"); break;
                    case '\r':
                        sb.Append("\\r"); break;
                    case '\t':
                        sb.Append("\\t"); break;
                    default:
                        sb.Append(c); break;
                }
            }
            return sb.ToString();
        }

        #endregion

        #region 解析 JToken 里面的键转为字符串 null值返回空字符串

        /// <summary>
        /// 把jArray里面的json对象转为字符串
        /// </summary>
        /// <param name="jt"></param>
        /// <returns></returns>
        public static string ToStringOrEmpty(this Newtonsoft.Json.Linq.JToken jt)
        {
            try
            {
                return jt == null ? "" : jt.ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        #endregion

        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="charset"></param>
        /// <returns></returns>
        public static string ToEncode(this string uri, string charset = "utf-8")
        {
            string URL_ALLOWED_CHARS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_.~";

            if (string.IsNullOrEmpty(uri))
                return string.Empty;

            const string escapeFlag = "%";
            var encodedUri = new StringBuilder(uri.Length * 2);
            var bytes = Encoding.GetEncoding(charset).GetBytes(uri);
            foreach (var b in bytes)
            {
                char ch = (char)b;
                if (URL_ALLOWED_CHARS.IndexOf(ch) != -1)
                    encodedUri.Append(ch);
                else
                {
                    encodedUri.Append(escapeFlag).Append(string.Format(CultureInfo.InstalledUICulture, "{0:X2}", (int)b));
                }
            }
            return encodedUri.ToString();
        }

        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="uriToDecode"></param>
        /// <returns></returns>
        public static string ToDecode(this string uriToDecode)
        {
            if (!string.IsNullOrEmpty(uriToDecode))
            {
                uriToDecode = uriToDecode.Replace("+", " ");
                return Uri.UnescapeDataString(uriToDecode);
            }

            return string.Empty;
        }

        /// <summary>
        /// 将Datetime转换成时间戳
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public static long ToTimestamp(this DateTime datetime)
        {
            return (datetime.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        /// <summary>
        /// 获取XmlDocument的内容
        /// </summary>
        /// <param name="xmlDocument"></param>
        /// <param name="nodeName">节点名称</param>
        /// <returns></returns>
        public static string GetText(this XmlDocument xmlDocument, string nodeName)
        {
            string result = xmlDocument.SelectSingleNode("//" + nodeName).InnerText;
            return result;
        }
    }
}