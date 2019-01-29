using EohiDingDing.Db;
using EohiDingDing.Models;
using EohiDingDing.Services;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using static DingTalk.Api.Response.OapiUserListbypageResponse;

namespace EohiDingDing.Controllers
{
    public class DingDingController : Controller
    {

        #region 发送消息

 


        /// <summary>
        ///  发送消息
        /// </summary>
        /// <param name="agent_id">企业自建应用是微应用agentId，第三方应用是通过获取授权企业的应用信息接口/service/get_agent获取到的agentI</param>
        /// <param name="msg">消息体</param>
        /// <param name="sendType">发送的方式默认1，1：发送工作消息，2发送普通消息,3群消息，4发送第三方应用消息</param>
        /// <param name="msgType">消息类型，默认5： 5：文件消息，6图片消息7......</param>
        /// <param name="isToAllUser">是否发送给全公司员工，如果不是false,默认false,</param>
        /// <param name="useridlist">在false情总下用户id :="0113650931838912,1530560410843347,133769523037797847"</param>
        /// <param name="depidlist">也可以用部门id ="5646568798798,89798798798798,87997987987"</param>
        /// <param name="sender">当为普通消息时sendType=2,发送者id,必须</param>
        /// <param name="cid">当为普通消息时sendType=2,会话id,必须</param>
        /// <param name="text"></param>
        /// <param name="mediaId"></param>
        /// <param name="title"></param>
        /// <param name="messageUrl"></param>
        /// <param name="picUrl"></param>
        /// <param name="markdown"></param>
        /// <param name="singleTitle"></param>
        /// <param name="singleUrl"></param>
        ///  <param name="app_id"></param>
        /// <param name="app_secret"></param>
        /// <param name="link"></param>
        /// <param name="code"></param>
        /// <param name="img"></param>
        /// <returns></returns>
        public JsonResult SendMessage(long agent_id,string msg, int sendType = 1, int msgType = 5, bool isToAllUser = false, string useridlist = null, string depidlist = null,
           string sender = null, string cid = null, string text = null, string mediaId = null, string title = null, string messageUrl = null, string picUrl = null,
            string markdown = null, string singleTitle = null, string singleUrl = null,string chatid=null,
            string app_id = null, string app_secret = null, string link=null,string code=null,string img=null)
        {
            try
            {
                ResponseParameters response = null;
                RequestParameters requestpams = new RequestParameters()
                {
                    Message = msg,
                    Agent_id = agent_id,
                    SendMsgType = (SendMsgType)msgType,
                    To_all_user = isToAllUser,
                    Dept_id_list = depidlist,
                    Userid_list = useridlist, // Userid_list = "0113650931838912,1530560410843347,133769523037797847",
                    Sender = sender,
                    Cid = cid,
                    Text = text,
                    MediaId = mediaId,
                    Title = title,
                    MessageUrl = messageUrl,
                    PicUrl = picUrl,
                    Markdown = markdown,
                    SingleTitle = singleTitle,
                    SingleUrl = singleUrl,
                    Chatid = chatid,
                    App_id = app_id,
                    App_secret = app_secret,
                    Link = link,
                    Img=img,
                    Code=code
                };
                if (sendType == (int)SendMsgType.workingMsg)//发送工作消息（公司给个人发消息）
                {
                    response = DingDingServices.SendWorkNotificationMessage(requestpams);
                }
                else if (sendType == (int)SendMsgType.generalMsg)//发送普通消息
                {
                    response = DingDingServices.SendAnOrdinaryMessage(requestpams);
                }
                else if (sendType == (int)SendMsgType.groupMsg)//群消息
                {
                    response = DingDingServices.SendGroupMessages(requestpams);
                }
                else if (sendType == (int)SendMsgType.ThirdPartyApp) //第三方应用发消息
                {
                    response = DingDingServices.SendThirdPartyApplicationMessage(requestpams);
                }
                else
                {
                }

                return Json(JsonConvert.SerializeObject(response), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new ResponseParameters()
                {
                    ErroCode = -1,
                    ErroMsg = ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 查询工作通知消息的发送进度
        /// </summary>
        /// <param name="agent_id">	发送消息时使用的微应用的id</param>
        /// <param name="task_id">发送消息时钉钉返回的任务id</param>
        /// <returns></returns>
        /// <returns></returns>
        public JsonResult SendMessageSchedule(long? agent_id, long? task_id)
        {
            ResponseParameters response = DingDingServices.ProgressInSendingAQueryWorkNotificationMessage(agent_id, task_id);
            return Json(JsonConvert.SerializeObject(response), JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 查询工作通知消息的发送结果
        /// </summary>      
        /// <param name="agent_id">	发送消息时使用的微应用的id</param>
        /// <param name="task_id">发送消息时钉钉返回的任务id</param>
        /// <returns></returns>
        public JsonResult SendMessageResult(long? agent_id, long? task_id)
        {
            ResponseParameters response = DingDingServices.QueryTheSendingResultOfTheWorkNotificationMessage(agent_id, task_id);
            return Json(JsonConvert.SerializeObject(response), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 将用户插入到数据库中
        /// </summary>
        /// <returns></returns>
        public JsonResult SaveUsersToDb()
        {
            var response = DingDingServices.GetAllUserList();
            if (response.ErroCode == 0 && response.Userlist != null && response.Userlist.Count > 0)
            {
                response.Userlist.ForEach(u => { if (u.Department != null) u.Department.ToString(); });
                string sqlStr = @"insert into DDUsers(
userid
,unionid
,[order]
,isAdmin
,isBoss
,isHide
,isLeader
,name
,active
,department
,position
,avatar
,jobnumber
) values(
@userid
,@unionid
,@order
,@isAdmin
,@isBoss
,@isHide
,@isLeader
,@name
,@active
,@department
,@position
,@avatar
,@jobnumber)";

                int m = DbHelper.Insert(sqlStr, response.Userlist);
                if (m > 0)
                {
                    return Json(new
                    {
                        code = 0,
                        msg = "保存数据成功，共" + m + "条",
                        data = m
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new
            {
                code = -1,
                msg = "保存数据失败"
            }, JsonRequestBehavior.AllowGet);

        }


        /// <summary>
        /// 从服务端更新数据库，有修改无插入
        /// </summary>
        /// <returns></returns>
        public JsonResult InsertOrUpdateUsersByUserId()
        {
            int mup = 0, min = 0;
            var response = DingDingServices.GetAllUserList();
            if (response.ErroCode == 0 && response.Userlist != null && response.Userlist.Count > 0)
            {
                response.Userlist.ForEach(u =>
                {
                    var user = DbHelper.QueryByUserId(u);
                    if (user != null)
                    {
                        mup += DbHelper.UpdateByUserId(u);
                    }
                    else
                    {
                        min += DbHelper.Insert(@"insert into DDUsers(
userid
,unionid
,[order]
,isAdmin
,isBoss
,isHide
,isLeader
,name
,active
,department
,position
,avatar
,jobnumber
) values(
@userid
,@unionid
,@order
,@isAdmin
,@isBoss
,@isHide
,@isLeader
,@name
,@active
,@department
,@position
,@avatar
,@jobnumber)", u);
                    }
                });
                if (mup > 0)
                {
                    return Json(new
                    {
                        code = 0,
                        msg = "操作成功，共修改" + mup + "条,插入" + min + "条,更新完成",
                        data = mup + min
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new
            {
                code = -1,
                msg = "更新失败"
            }, JsonRequestBehavior.AllowGet);

        }


        public JsonResult GetUserByUserId(string userid)
        {
            return Json(JsonConvert.SerializeObject(DbHelper.QueryByUserId(new UserlistDomain() { Userid = userid })), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUsersByUserIds(List<string> userids)
        {
            return Json(JsonConvert.SerializeObject(DbHelper.QueryInByUserids(userids)), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserByName(string name)
        {
            return Json(JsonConvert.SerializeObject(DbHelper.QueryInByNames(new List<string>() { name })), JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetUsersByNames(List<string> names)
        {
            return Json(JsonConvert.SerializeObject(DbHelper.QueryInByNames(names)), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserByUnionId(string unionId)
        {
            return Json(JsonConvert.SerializeObject(DbHelper.QueryInByUnionids(new List<string>() { unionId })), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserByUnionId(List<string> unionIds)
        {
            return Json(JsonConvert.SerializeObject(DbHelper.QueryInByUnionids(unionIds)), JsonRequestBehavior.AllowGet);
        }
        #endregion


        public ActionResult DDUserList()
        {
            ViewBag.AppKey = RequestParameters.AppKey;
            return View();
        }


    }
}