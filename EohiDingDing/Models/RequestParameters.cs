using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace EohiDingDing.Models
{
    public class RequestParameters
    {
        #region 主要参数

        public static string AppKey => ConfigurationManager.AppSettings["appKey"];
        public static string AppSecret=> ConfigurationManager.AppSettings["appSecret"];
        /// <summary>
        /// 消息发送者 userId
        /// </summary>
        public string Sender { get; set; }
        /// <summary>
        /// 群消息或者个人聊天会话Id，(通过JSAPI的dd.chooseChatForNormalMsg接口唤起联系人界面选择之后即可拿到会话ID，之后您可以使用获取到的cid调用此接口）
        /// </summary>
        public string Cid { get; set; }
        /// <summary>
        /// 企业自建应用是微应用agentId，第三方应用是通过获取授权企业的应用信息接口/service/get_agent获取到的agentId
        /// 1234
        /// </summary>
        public long Agent_id { get; set; }
        public string Chatid { get; set; }
        private long pDepId = 1L;
        public long PDepId
        {
            get
            {
                return pDepId;
            }
            set
            {
                pDepId = value;
            }
        }


        /// <summary>
        /// 接收者的用户userid列表，最大列表长度：20
        /// zhangsan,lisi
        /// </summary>
        public string Userid_list { get; set; }
        private string _dept_id_list = null;
        /// <summary>
        /// 接收者的部门id列表，最大列表长度：20,  接收者是部门id下(包括子部门下)的所有用户
        /// 
        /// </summary>
        public string Dept_id_list
        {
            get
            {
                return _dept_id_list;
            }
            set
            {
                _dept_id_list = value;
            }
        }
        private bool _to_all_user = false;
        /// <summary>
        /// 是否发送给企业全部用户(ISV不能设置true)
        /// </summary>
        public bool To_all_user
        {
            get
            {
                return _to_all_user;
            }
            set
            {
                _to_all_user = value;
            }
        }
        /// <summary>
        /// 消息内容，具体见“消息类型与数据格式”。最长不超过2048个字节
        /// </summary>
        public string Message { get; set; }

        public SendMsgType SendMsgType { get; set; }

        #endregion

        #region 发送工作消息

        #endregion
        #region 发送普通消息

        #endregion
        #region image file
        /// <summary>
        /// image file
        /// </summary>
        public string MediaId { get; set; }

        #endregion

        #region link markdown action_cord oa
        public string Title { get; set; }
        public string Text { get; set; }
        public string MessageUrl { get; set; }
        public string PicUrl { get; set; }


        #endregion
        #region markdown
        public string Markdown { get; set; }
        public string SingleTitle { get; set; }
        public string SingleUrl { get; set; }
        #endregion


        public string App_id { get; set; }
        public string App_secret { get; set; }
        public string Code { get; set; }
        public string Img { get; set; }
        public string Link { get; set; }

    }


}