using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EohiDingDing.Models
{
    public enum SendMsgType
    {
        /// <summary>
        /// 工作消息
        /// </summary>
        workingMsg = 1,
        /// <summary>
        /// 普通消息
        /// </summary>
        generalMsg = 2,
        /// <summary>
        /// 群消息
        /// </summary>
        groupMsg=3,
        /// <summary>
        /// 第三方应用
        /// </summary>
        ThirdPartyApp=4,
        /// <summary>
        /// 文本消息
        /// </summary>
        text = 5,
        /// <summary>
        /// 图片
        /// </summary>
        image = 6,
        /// <summary>
        /// 文件
        /// </summary>
        file = 7,
        /// <summary>
        /// 这是支持markdown的文本 \\n## 标题2  \\n* 列表
        /// 首屏会话透出的展示内容
        /// </summary>
        markdown = 8,
        /// <summary>
        /// 是透出到会话列表和通知的文案
        /// 持markdown格式的正文内
        /// </summary>
        action_card = 9,
        /// <summary>
        /// link消息
        /// </summary>
        link = 10,
        /// <summary>
        /// OA
        /// </summary>
        oa=11
       
    }
}