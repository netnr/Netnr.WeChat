using System;
using System.Xml;

namespace Netnr.WeChat
{
    public enum WeChatMessageType
    {
        Text, //文本
        Location, //地理位置
        Image, //图片
        Voice, //语音
        Video, //视频
        Link, //连接信息
        Event, //事件推送
    }

    public class WeChatMessage
    {
        public virtual WeChatMessageType Type { set; get; }
        public virtual XmlDocument Body { set; get; }
        
        /// <summary>
        /// 解析微信服务器推送的消息
        /// http://mp.weixin.qq.com/wiki/index.php?title=%E6%8E%A5%E6%94%B6%E6%99%AE%E9%80%9A%E6%B6%88%E6%81%AF
        /// http://mp.weixin.qq.com/wiki/index.php?title=%E6%8E%A5%E6%94%B6%E4%BA%8B%E4%BB%B6%E6%8E%A8%E9%80%81
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static WeChatMessage Parse(string message)
        {
            var msg = new WeChatMessage();
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(message);
            msg.Body = xmlDoc;
            string msgType = msg.Body.SelectSingleNode("//MsgType").InnerText.ToLower();
            if (Enum.TryParse(msgType, true, out WeChatMessageType mtype))
            {
                msg.Type = mtype;
            }
            else
            {
                throw new Exception("does not support this message type:" + msgType);
            }
            return msg;
        }
    }
}