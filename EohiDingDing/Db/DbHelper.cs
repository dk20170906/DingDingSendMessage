using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using Top.Api;
using static DingTalk.Api.Response.OapiUserListbypageResponse;

namespace EohiDingDing.Db
{
    public class DbHelper
    {
        private static  readonly string conStr = ConfigurationManager.ConnectionStrings["connectionStr"].ToString();
        /// <summary>
        /// 插入单个对像
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int Insert(string sqlStr, UserlistDomain  user)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(conStr))
                {
                    return connection.Execute(sqlStr, user);
                }
            }
            catch (Exception )
            {

                throw;
            }
        }
        /// <summary>
        /// 批量插入UserlistDomain数据，返回影响行数
        /// </summary>
        /// <param name="userlists"></param>
        /// <returns>影响行数</returns>
        public static int Insert(string sqlStr, List<UserlistDomain>   userlists)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(conStr))
                {
                    return connection.Execute(sqlStr, userlists);
                }
            }
            catch (Exception )
            {

                throw;
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static int Update(string sqlStr, UserlistDomain  user)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(conStr))
                {
                    return connection.Execute(sqlStr, user);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static int UpdateByUserId(UserlistDomain user)
        {
            using (IDbConnection connection = new SqlConnection(conStr))
            {
                return connection.Execute(@"update DDUsers set
unionid=@unionid
,[order]=@order
,isAdmin=@isBoss
,isBoss=@isBoss
,isHide=@isHide
,isLeader=@isLeader
,name=@name
,active=@active
,department=@department
,position=@position
,avatar=@avatar
,jobnumber=@jobnumber
where userid=@userid", user);
            }
        }
        /// <summary>
        /// 批量修改
        /// </summary>
        /// <param name="sqlStr"></param>
        /// <param name="userlists"></param>
        /// <returns></returns>
        public static int Update(string sqlStr, List<UserlistDomain>  userlists)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(conStr))
                {
                    return connection.Execute(sqlStr, userlists);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="UserlistDomain"></param>
        /// <returns></returns>
        public static int Delete(string sqlStr, UserlistDomain UserlistDomain)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(conStr))
                {
                    return connection.Execute(sqlStr, UserlistDomain);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="UserlistDomains"></param>
        /// <returns></returns>
        public static int Delete(string sqlStr, List<UserlistDomain> UserlistDomains)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(conStr))
                {
                    return connection.Execute(sqlStr, UserlistDomains);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 无参查询所有数据
        /// </summary>
        /// <returns></returns>
        public static List<UserlistDomain> QuerySelectAll()
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(conStr))
                {
                    return connection.Query<UserlistDomain>("select * from UserlistDomain").ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 查询指定数据
        /// </summary>
        /// <param name="UserlistDomain"></param>
        /// <returns></returns>
        public static UserlistDomain Query(string sqlStr, UserlistDomain UserlistDomain)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(conStr))
                {
                    return connection.Query<UserlistDomain>(sqlStr, UserlistDomain).SingleOrDefault();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 能过userid查询
        /// </summary>
        /// <param name="UserlistDomain"></param>
        /// <returns></returns>
        public static UserlistDomain QueryByUserId(UserlistDomain UserlistDomain)
        {
            using (IDbConnection connection = new SqlConnection(conStr))
            {
                return connection.Query<UserlistDomain>("select * from DDUsers where userid=@userid", UserlistDomain).SingleOrDefault();
            }
        }
        /// <summary>
        /// In操作 通过用户id批量查询
        /// </summary>
        public static List<UserlistDomain> QueryInByUserids(List<string> userids)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(conStr))
                {
                    var sql = "select * from UserlistDomain where userid in @userids";
                    //参数类型是Array的时候，dappper会自动将其转化
                    return connection.Query<UserlistDomain>(sql, new { userids }).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static List<UserlistDomain> QueryInByUnionids(List<string> unionids)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(conStr))
                {
                    var sql = "select * from UserlistDomain where unionid in @unionids";
                    //参数类型是Array的时候，dappper会自动将其转化
                    return connection.Query<UserlistDomain>(sql, new { unionids }).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        
        public static List<UserlistDomain> QueryInByNames(List<string> names)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(conStr))
                {
                    var sql = "select * from UserlistDomain where name in @names";
                    //参数类型是Array的时候，dappper会自动将其转化
                    return connection.Query<UserlistDomain>(sql, new { names }).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public static List<UserlistDomain> QueryIn(int[] ids)
        {
            try
            {
                using (IDbConnection connection = new SqlConnection(conStr))
                {
                    var sql = "select * from UserlistDomain where id in @ids";
                    //参数类型是Array的时候，dappper会自动将其转化
                    return connection.Query<UserlistDomain>(sql, new { ids }).ToList();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}