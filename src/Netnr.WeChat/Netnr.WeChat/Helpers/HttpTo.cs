using System.IO;
using System.Net;
using System.Text;

namespace Netnr.WeChat
{
    public class HttpTo
    {
        #region 服务器 发送请求

        /// <summary>
        /// 发送请求参数设置
        /// </summary>
        /// <param name="url">地址</param>
        /// <param name="type">请求类型 默认GET </param>
        /// <param name="postData">POST发送内容 默认空</param>
        /// <returns></returns>
        public static HttpWebRequest HWRequest(string url, string type = "GET", string postData = "")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = type;
            request.KeepAlive = true;
            request.AllowAutoRedirect = true;
            request.MaximumAutomaticRedirections = 4;
            request.Timeout = short.MaxValue * 3;//MS
            request.ContentType = "application/x-www-form-urlencoded";

            if (type != "GET")
            {
                //发送内容
                byte[] bytes = Encoding.UTF8.GetBytes(postData);
                request.ContentLength = Encoding.UTF8.GetBytes(postData).Length;
                Stream outputStream = request.GetRequestStream();
                outputStream.Write(bytes, 0, bytes.Length);
                outputStream.Close();
            }
            return request;
        }

        /// <summary>
        /// 发送请求 得到请求结果
        /// </summary>
        /// <param name="request">HttpWebRequest对象 可通过HWRequest方法创建</param>
        /// <param name="e">返回类容编码 默认UTF-8</param>
        /// <returns></returns>
        public static string Url(HttpWebRequest request, Encoding e)
        {
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            if (string.Compare(response.ContentEncoding, "gzip", true) >= 0)
                responseStream = new System.IO.Compression.GZipStream(responseStream, System.IO.Compression.CompressionMode.Decompress);
            StreamReader reader = new StreamReader(responseStream, e);
            string result = "", strData = "";
            //result = reader.ReadToEnd();
            while ((strData = reader.ReadLine()) != null)
            {
                result += strData + "\r\n";
            }
            reader.Close();
            responseStream.Close();
            return result;
        }

        /// <summary>
        /// 发送请求 得到请求结果
        /// </summary>
        /// <param name="request">HttpWebRequest对象 可通过HWRequest方法创建</param>
        /// <returns></returns>
        public static string Url(HttpWebRequest request)
        {
            return Url(request, Encoding.UTF8);
        }

        /// <summary>
        /// 发送 GET 请求 得到请求结果
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns></returns>
        public static string Get(string url)
        {
            return Url(HWRequest(url));
        }

        /// <summary>
        /// 发送 POST 请求
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="postData">发送内容</param>
        /// <returns></returns>
        public static string Post(string url, string postData)
        {
            return Url(HWRequest(url, "POST", postData.ToEncode()));
        }

        #endregion
    }
}