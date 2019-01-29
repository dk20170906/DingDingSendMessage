using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static DingTalk.Api.Response.OapiDepartmentListResponse;
using static DingTalk.Api.Response.OapiUserListbypageResponse;

namespace EohiDingDing.Models
{
    public class ResponseParameters
    {
        /// <summary>
        /// 返回码
        /// </summary>
        public long ErroCode { get; set; }
        /// <summary>
        /// 对返回码的文本描述内容
        /// </summary>
        public string ErroMsg { get; set; }
        /// <summary>
        /// 创建的发送任务id
        /// </summary>
        public long TaskId { get; set; }
        /// <summary>
        /// 如果是钉钉群，返回与发送者同一企业的一组userid。
        /// 如果是个人聊天，返回与发送者同一企业的一个userid。
        ///不在同一企业，发送会失败。
        /// </summary>
        public string Receiver { get; set; }

        public string AccessToken { get; set; }
        public long ExpiresIn { get; set; }

        public List<string> AuthUserField { get; set; }
        public List<string> ConditionField { get; set; }
        /// <summary>
        /// 授权部门列表
        /// </summary>
        public List<long> AuthedDept { get; set; }
        /// <summary>
        /// 授权用户列表
        /// </summary>
        public List<string> AuthedUser { get; set; }

        public List<UserlistDomain> Userlist { get; set; }
        public List<DepartmentDomain> DepartmentList { get; set; }

        public string MessageId { get; set; }
        public long ProgressInPercent { get; set; }
        public long Status { get; set; }

        public List<string> FailedUserIdList { get; set; }
        public List<string> ForbiddenUserIdList { get; set; }
        public List<long> InvalidDeptIdList { get; set; }
        public List<string> InvalidUserIdList { get; set; }
        public List<string> ReadUserIdList { get; set; }
        public List<string> UnreadUserIdList { get; set; }
    }
}