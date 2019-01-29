using DingTalk.Api;
using DingTalk.Api.Request;
using DingTalk.Api.Response;
using EohiDingDing.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web;
using static DingTalk.Api.Response.OapiUserListbypageResponse;

namespace EohiDingDing.Services
{
    public class DingDingServices
    {
        static ILog log = LogManager.GetLogger("程序运行信息");

        /// <summary>
        /// 获取Access_Token
        /// </summary>
        /// <returns></returns>
        public static string Get_Access_Token()
        {
            try
            {
                var Appkey = ConfigurationManager.AppSettings["appKey"];
                var Appsecret = ConfigurationManager.AppSettings["appSecret"];
                log.Info(string.Format("通过Appkey:{0},Appsecret{1}获取Access_Token", Appkey, Appsecret));
                DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/gettoken");
                OapiGettokenRequest request = new OapiGettokenRequest();
                request.Appkey = Appkey;
                request.Appsecret = Appsecret;
                request.SetHttpMethod("GET");
                OapiGettokenResponse response = client.Execute(request);
                if (response.Errcode == 0)
                {
                    log.Info("AccessToken:" + response.AccessToken);
                    return response.AccessToken;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return "请求失败，请确认Appkey，Appsecret是否正确，或查看网络连接是否正常";
        }
        ////
        /**
         同一个微应用相同消息内容同一个用户一天只能接收一次，重复发送会发送成功但用户接收不到。
该接口是异步发送消息，接口返回成功并不表示用户收到消息，需要通过“查询工作通知消息的发送结果”接口查询是否给用户发送成功。
                     */
        /// <summary>
        /// 发送工作消息
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static ResponseParameters SendWorkNotificationMessage(RequestParameters parameters)
        {
            try
            {
                log.Info("发送工作消息");
                DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/message/corpconversation/asyncsend_v2");

                OapiMessageCorpconversationAsyncsendV2Request request = new OapiMessageCorpconversationAsyncsendV2Request();
                log.Info("UseridList:" + parameters.Userid_list + ";AgentId:" + parameters.Agent_id + ";ToAllUser:" + parameters.To_all_user);
                request.UseridList = parameters.Userid_list;
                request.DeptIdList = parameters.Dept_id_list;
                request.AgentId = parameters.Agent_id;
                request.ToAllUser = parameters.To_all_user;

                OapiMessageCorpconversationAsyncsendV2Request.MsgDomain msg = new OapiMessageCorpconversationAsyncsendV2Request.MsgDomain();

                switch (parameters.SendMsgType)
                {
                    case SendMsgType.text:
                        OapiMessageCorpconversationAsyncsendV2Request.TextDomain text = new OapiMessageCorpconversationAsyncsendV2Request.TextDomain();
                        text.Content = parameters.Message;
                        msg.Msgtype = "text";
                        msg.Text = text;
                        request.Msg_ = msg;
                        log.Info("发送Text消息：" + parameters.Message);
                        break;
                    case SendMsgType.image:
                        OapiMessageCorpconversationAsyncsendV2Request.ImageDomain image = new OapiMessageCorpconversationAsyncsendV2Request.ImageDomain();
                        image.MediaId = parameters.MediaId;
                        msg.Msgtype = "image";
                        msg.Image = image;
                        request.Msg_ = msg;
                        log.Info("发送image：" + parameters.MediaId);
                        break;
                    case SendMsgType.file:
                        OapiMessageCorpconversationAsyncsendV2Request.FileDomain file = new OapiMessageCorpconversationAsyncsendV2Request.FileDomain();
                        file.MediaId = parameters.MediaId;
                        msg.Msgtype = "file";
                        msg.File = file;
                        request.Msg_ = msg;
                        log.Info("发送file：" + parameters.MediaId);
                        break;
                    case SendMsgType.link:
                        OapiMessageCorpconversationAsyncsendV2Request.LinkDomain link = new OapiMessageCorpconversationAsyncsendV2Request.LinkDomain();
                        link.Title = parameters.Title;
                        link.Text = parameters.Text;
                        link.MessageUrl = parameters.MessageUrl;
                        link.PicUrl = parameters.PicUrl;
                        msg.Link = link;
                        msg.Msgtype = "link";
                        request.Msg_ = msg;
                        log.Info("发送link：Title" + parameters.Title);
                        break;
                    case SendMsgType.markdown:
                        OapiMessageCorpconversationAsyncsendV2Request.MarkdownDomain markdown = new OapiMessageCorpconversationAsyncsendV2Request.MarkdownDomain();
                        markdown.Text = parameters.Text;
                        markdown.Title = parameters.Title;
                        msg.Msgtype = "markdown";
                        msg.Markdown = markdown;
                        request.Msg_ = msg;
                        break;
                    case SendMsgType.action_card:
                        OapiMessageCorpconversationAsyncsendV2Request.ActionCardDomain actionCard = new OapiMessageCorpconversationAsyncsendV2Request.ActionCardDomain();
                        actionCard.Title = parameters.Title;
                        actionCard.Markdown = parameters.Markdown;
                        actionCard.SingleTitle = parameters.SingleTitle;
                        actionCard.SingleUrl = parameters.SingleUrl;
                        msg.ActionCard = actionCard;
                        msg.Msgtype = "action_card";
                        request.Msg_ = msg;
                        break;
                    case SendMsgType.oa:
                        OapiMessageCorpconversationAsyncsendV2Request.OADomain oA = new OapiMessageCorpconversationAsyncsendV2Request.OADomain();
                        OapiMessageCorpconversationAsyncsendV2Request.HeadDomain head = new OapiMessageCorpconversationAsyncsendV2Request.HeadDomain();
                        OapiMessageCorpconversationAsyncsendV2Request.BodyDomain body = new OapiMessageCorpconversationAsyncsendV2Request.BodyDomain();
                        head.Text = parameters.Text;
                        body.Content = parameters.Message;
                        oA.Head = head;
                        oA.Body = body;
                        msg.Oa = oA;
                        msg.Msgtype = "oa";
                        request.Msg_ = msg;
                        break;
                    default:
                        break;
                }
                OapiMessageCorpconversationAsyncsendV2Response response = client.Execute(request, Get_Access_Token());
                log.Info("请求完成" + response.ToString());
                return new ResponseParameters()
                {
                    ErroCode = response.Errcode,
                    ErroMsg = response.Errmsg,
                    TaskId = response.TaskId
                };
            }
            catch (Exception ex)
            {
                return new ResponseParameters()
                {
                    ErroCode = -1,
                    ErroMsg = ex.Message
                };
            }
        }

        /// <summary>
        /// 查询工作通知消息的发送进度
        /// </summary>
        /// <param name="agent_id">	发送消息时使用的微应用的id</param>
        /// <param name="task_id">发送消息时钉钉返回的任务id</param>
        /// <returns></returns>
        public static ResponseParameters ProgressInSendingAQueryWorkNotificationMessage(long? agent_id,long? task_id)
        {
            try
            {
                DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/message/corpconversation/getsendprogress");
                OapiMessageCorpconversationGetsendprogressRequest request = new OapiMessageCorpconversationGetsendprogressRequest();
                request.AgentId = agent_id; //setAgentId(135717601L);
                request.TaskId = task_id;// setTaskId(9326688016L);
                OapiMessageCorpconversationGetsendprogressResponse response = client.Execute(request, Get_Access_Token());
                return new ResponseParameters()
                {
                    ErroCode = response.Errcode,
                    ErroMsg = response.Errmsg,
                    ProgressInPercent = response.Progress.ProgressInPercent,
                    Status = response.Progress.Status
                };
            }
            catch (Exception ex)
            {
                return new ResponseParameters()
                {
                    ErroCode = -1,
                    ErroMsg = ex.Message
                };
            }
        }
        /// <summary>
        /// 查询工作通知消息的发送结果
        /// </summary>      
        /// <param name="agent_id">	发送消息时使用的微应用的id</param>
        /// <param name="task_id">发送消息时钉钉返回的任务id</param>
        /// <returns></returns>
        public static ResponseParameters QueryTheSendingResultOfTheWorkNotificationMessage(long? agent_id, long? task_id)
        {
            try
            {
                DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/message/corpconversation/getsendresult");
                OapiMessageCorpconversationGetsendresultRequest request = new OapiMessageCorpconversationGetsendresultRequest();
                request.AgentId = agent_id;
                request.TaskId = task_id;
                OapiMessageCorpconversationGetsendresultResponse response = client.Execute(request, Get_Access_Token());
                return new ResponseParameters()
                {
                    ErroCode = response.Errcode,
                    ErroMsg = response.Errmsg,
                    InvalidDeptIdList=response.SendResult.InvalidDeptIdList,
                    InvalidUserIdList=response.SendResult.InvalidUserIdList,
                    FailedUserIdList=response.SendResult.FailedUserIdList,
                    ForbiddenUserIdList=response.SendResult.ForbiddenUserIdList,
                    ReadUserIdList=response.SendResult.ReadUserIdList,
                    UnreadUserIdList=response.SendResult.UnreadUserIdList
                };
            }
            catch (Exception ex)
            {
                return new ResponseParameters()
                {
                    ErroCode = -1,
                    ErroMsg = ex.Message
                };
            }
        }
        /// <summary>
        /// 发送普通消息
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static ResponseParameters SendAnOrdinaryMessage(RequestParameters parameters)
        {
            try
            {
                log.Info("发送普通消息");
                DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/message/send_to_conversation");

                OapiMessageSendToConversationRequest req = new OapiMessageSendToConversationRequest();
                log.Info("Sender:" + parameters.Sender + ";Cid:" + parameters.Cid);
                req.Sender = parameters.Sender;
                req.Cid = parameters.Cid;
                OapiMessageSendToConversationRequest.MsgDomain msg = new OapiMessageSendToConversationRequest.MsgDomain();
                switch (parameters.SendMsgType)
                {
                    case SendMsgType.text:
                        // 文本消息
                        OapiMessageSendToConversationRequest.TextDomain text = new OapiMessageSendToConversationRequest.TextDomain();
                        text.Content = parameters.Message;
                        msg.Text = text;
                        msg.Msgtype = "text";
                        req.Msg_ = msg;
                        log.Info("发送文本消息" + parameters.Message);
                        break;
                    case SendMsgType.image:
                        // 图片
                        OapiMessageSendToConversationRequest.ImageDomain image = new OapiMessageSendToConversationRequest.ImageDomain();
                        image.MediaId = parameters.Message;
                        msg.Image = image;
                        msg.Msgtype = "image";
                        req.Msg_ = msg;
                        break;
                    case SendMsgType.file:
                        // 文件
                        OapiMessageSendToConversationRequest.FileDomain file = new OapiMessageSendToConversationRequest.FileDomain();
                        file.MediaId = parameters.Message;
                        msg.File = file;
                        msg.Msgtype = "file";
                        req.Msg_ = msg;
                        break;
                    case SendMsgType.markdown:
                        OapiMessageSendToConversationRequest.MarkdownDomain markdown = new OapiMessageSendToConversationRequest.MarkdownDomain();
                        markdown.Text = parameters.Text;
                        markdown.Title = parameters.Title;
                        msg.Markdown = markdown;
                        msg.Msgtype = "markdown";
                        req.Msg_ = msg;
                        break;
                    case SendMsgType.action_card:
                        OapiMessageSendToConversationRequest.ActionCardDomain actionCard = new OapiMessageSendToConversationRequest.ActionCardDomain();
                        actionCard.Title = parameters.Title;
                        actionCard.Markdown = parameters.Markdown;
                        actionCard.SingleTitle = parameters.SingleTitle;
                        actionCard.SingleUrl = parameters.SingleUrl;
                        msg.ActionCard = actionCard;
                        msg.Msgtype = "action_card";
                        req.Msg_ = msg;
                        break;
                    case SendMsgType.link:
                        // link消息
                        OapiMessageSendToConversationRequest.LinkDomain link = new OapiMessageSendToConversationRequest.LinkDomain();
                        link.MessageUrl = parameters.MessageUrl;
                        link.PicUrl = parameters.PicUrl;
                        link.Text = parameters.Text;
                        link.Title = parameters.Title;
                        msg.Link = link;
                        msg.Msgtype = "link";
                        req.Msg_ = msg;
                        break;
                    default:
                        break;

                }
                OapiMessageSendToConversationResponse response = client.Execute(req, Get_Access_Token());
                log.Info("请求完成" + response.ToString());
                return new ResponseParameters()
                {
                    ErroCode = response.Errcode,
                    ErroMsg = response.Errmsg,
                    Receiver = response.Receiver
                };
            }
            catch (Exception ex)
            {
                return new ResponseParameters()
                {
                    ErroCode = -1,
                    ErroMsg = ex.Message
                };
            }
        }
        /// <summary>
        /// 发送群消息
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static ResponseParameters SendGroupMessages(RequestParameters parameters)
        {
            try
            {
                DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/chat/send");
                OapiChatSendRequest request = new OapiChatSendRequest();
                request.Chatid = parameters.Chatid;

                OapiChatSendRequest.MsgDomain msg = new OapiChatSendRequest.MsgDomain();
                msg.Msgtype = "text";
                OapiChatSendRequest.TextDomain text = new OapiChatSendRequest.TextDomain();
                text.Content = parameters.Message;
                msg.Text = text;

                request.Msg_ = msg;
                OapiChatSendResponse response = client.Execute(request, Get_Access_Token());
                return new ResponseParameters()
                {
                    ErroCode = response.Errcode,
                    ErroMsg = response.Errmsg,
                    MessageId = response.MessageId
                };
            }
            catch (Exception ex)
            {
                return new ResponseParameters()
                {
                    ErroCode = -1,
                    ErroMsg = ex.Message
                };
            }
        }


        /// <summary>
        /// 第三方应用消息
        /// </summary>
        /// <returns></returns>
        public static ResponseParameters SendThirdPartyApplicationMessage(RequestParameters parameters)
        {
            try
            {
                DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/sns/send_msg");
                OapiSnsSendMsgRequest req = new OapiSnsSendMsgRequest();
                req.Code = parameters.Code;
                OapiSnsSendMsgRequest.EappDomain eapp = new OapiSnsSendMsgRequest.EappDomain();
                eapp.Img = parameters.Img;
                eapp.Content = parameters.Message;
                eapp.Title = parameters.Title;
                eapp.Link = parameters.Link;
                OapiSnsSendMsgRequest.MsgDomain msg = new OapiSnsSendMsgRequest.MsgDomain();
                msg.Msgtype = "eapp";
                msg.Eapp = eapp;
                req.Msg_ = msg;
                OapiSnsSendMsgResponse response = client.Execute(req, parameters.App_id, parameters.App_secret);
                return new ResponseParameters()
                {
                    ErroCode = response.Errcode,
                    ErroMsg = response.Errmsg
                };
            }
            catch (Exception ex)
            {
                return new ResponseParameters()
                {
                    ErroCode = -1,
                    ErroMsg = ex.Message
                };
            }
        }

        /// <summary>
        /// 获取通讯录权限范围
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static ResponseParameters GetTheRangeOfAddressBookPermissions()
        {
            try
            {
                DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/auth/scopes");
                OapiAuthScopesRequest request = new OapiAuthScopesRequest();
                request.SetHttpMethod("GET");
                OapiAuthScopesResponse response = client.Execute(request, Get_Access_Token());

                return new ResponseParameters()
                {
                    ErroCode = response.Errcode,
                    ErroMsg = response.Errmsg,
                    AuthUserField = response.AuthUserField,
                    ConditionField = response.ConditionField,
                    AuthedDept = response.AuthOrgScopes.AuthedDept,
                    AuthedUser = response.AuthOrgScopes.AuthedUser
                };
            }
            catch (Exception ex)
            {
                return new ResponseParameters()
                {
                    ErroCode = -1,
                    ErroMsg = ex.Message
                };
            }

        }


      
        /// <summary>
        /// 通过父部门找到所有子部门
        /// </summary>
        /// <param name="PdepId"></param>
        /// <returns></returns>
        public static ResponseParameters GetDepListByPDepId(long pdepId)
        {
            try
            {
                DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/department/list");
                OapiDepartmentListRequest request = new OapiDepartmentListRequest();
                request.Id = pdepId.ToString();
                request.SetHttpMethod("GET");
                OapiDepartmentListResponse response = client.Execute(request, Get_Access_Token());
                response.Department.Add(new OapiDepartmentListResponse.DepartmentDomain()
                {
                    Id = pdepId
                });
                return new ResponseParameters()
                {
                    ErroCode = response.Errcode,
                    ErroMsg = response.Errmsg,
                    DepartmentList = response.Department
                };
            }
            catch (Exception ex)
            {
                return new ResponseParameters()
                {
                    ErroCode = -1,
                    ErroMsg = ex.Message
                };
            }
        }
        /// <summary>
        /// 通过部门Id找到用户列表
        /// </summary>
        /// <param name="depId"></param>
        /// <returns></returns>
        /// 
        private static List<UserlistDomain> userlistDomains = new List<UserlistDomain>();
        public static void GetUsersByDepId(long depId,long offSet,long size)
        {
            try
            {
                DefaultDingTalkClient client = new DefaultDingTalkClient("https://oapi.dingtalk.com/user/listbypage");
                OapiUserListbypageRequest request = new OapiUserListbypageRequest();
                request.DepartmentId = depId;
                request.Offset = offSet;
                request.Size = size;
                request.Order = "entry_desc";
                request.SetHttpMethod("GET");
                OapiUserListbypageResponse execute = client.Execute(request, Get_Access_Token());
                if (execute != null && execute.Errcode == 0 && execute.Userlist != null && execute.Userlist.Count > 0)
                {
                    userlistDomains = userlistDomains.Union(execute.Userlist).ToList();
               
                    if (execute.HasMore)
                    {
                        GetUsersByDepId(depId, ++offSet*size, size);
                    }
                }
                //根据userid去重
                userlistDomains.Where((x, y) => userlistDomains.FindIndex(z => z.Userid == x.Userid) == y).ToList();

            }
            catch (Exception ex)
            {
                log.Info("部门下所有用户" + ex.Message);
            }
        }
        /// <summary>
        /// 找到部门下的所有用户列表
        /// </summary>
        /// <param name="parameters">PDepId必填</param>
        /// <returns></returns>
        public static ResponseParameters GetAllUserList(long pdepId=1L)
        {
            try
            {
                List<string> userids = new List<string>();
                List<UserlistDomain> userslist = new List<UserlistDomain>();
                userlistDomains.Clear();
                var resparms = GetDepListByPDepId(pdepId);
                if (resparms.ErroCode==0&&resparms.DepartmentList.Count > 0)
                {
                    resparms.DepartmentList.ForEach(u =>
                    {
                       GetUsersByDepId(u.Id,0L,100L);
                    });
                }
                if (userlistDomains.Count>0)
                {
                    userlistDomains.ForEach(u =>
                    {
                        if (!userids.Contains(u.Userid))
                        {
                            userslist.Add(u);
                            userids.Add(u.Userid);
                        }
                    });
                    return new ResponseParameters()
                    {
                        ErroCode = 0,
                        ErroMsg = "ok",
                        Userlist = userslist
                    };
                }
                else
                {
                    return new ResponseParameters()
                    {
                        ErroCode = -1,
                        ErroMsg = "无数据"
                    };
                }
               
            }
            catch (Exception ex)
            {
                return new ResponseParameters()
                {
                    ErroCode = -1,
                    ErroMsg = ex.Message
                };
            }

        }
    }
}