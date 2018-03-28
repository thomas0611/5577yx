using Game.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Game.DAL
{
    public class SysMsgServer
    {
        DBHelper db = new DBHelper();
        CommonServer cs = new CommonServer();

        /// <summary>
        /// 添加一条系统消息
        /// </summary>
        /// <param name="sm">消息</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddSysMsg(sysmsg sm)
        {
            try
            {
                string sql = "insert into sysmsg(type,title,msg,userid,fromid,msgid)"
                           + "values (@type,@title,@msg,@userid,@fromid,@msgid)";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@type",sm.type),
                    new SqlParameter("@title",sm.title),
                    new SqlParameter("@msg",sm.msg), 
                    new SqlParameter("@userid", sm.userid),
                    new SqlParameter("@fromid",sm.fromid),
                    new SqlParameter("@msgid",sm.msgid)
                };
                return db.ExecuteNonQuery(sql, sp);
            }
            catch (SqlException ex)
            {
                throw new Exception("数据库异常！原因：" + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("未知异常！原因：" + ex.Message);
            }
        }

        /// <summary>
        /// 获取系统消息数量
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <returns>返回消息数量</returns>
        public Double GetSysMsgCount(int UserId)
        {
            return cs.GetDataCount("where userid=" + UserId, "sysmsg");
        }

        /// <summary>
        /// 通过分页获取系统消息
        /// </summary>
        /// <param name="PageSize">页大小</param>
        /// <param name="PageNum">页码</param>
        /// <param name="WhereStr">条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns></returns>
        public List<sysmsg> GetAllSysMsg(int PageSize, int PageNum, string WhereStr, string OrderBy)
        {
            List<sysmsg> list = new List<sysmsg>();
            try
            {
                SqlParameter[] sp = new SqlParameter[] { 
                new SqlParameter("@PageSize",PageSize),
                new SqlParameter("@PageNum",PageNum),
                new SqlParameter("@TableName","sysmsg"),
                new SqlParameter("@WhereStr",WhereStr),
                new SqlParameter("@OrderBy",OrderBy)
                };
                using (SqlDataReader reder = db.GetReaderByProc("Proc_Page", sp))
                {
                    while (reder.Read())
                    {
                        sysmsg sm = new sysmsg();
                        sm.id = (int)reder["id"];
                        sm.type = (int)reder["type"];
                        sm.title = reder["title"].ToString();
                        sm.msg = reder["msg"].ToString();
                        sm.msgid = (int)reder["msgid"];
                        sm.state = (int)reder["state"];
                        sm.userid = (int)reder["userid"];
                        sm.addtime = (DateTime)reder["addtime"];
                        list.Add(sm);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("数据库异常！原因：" + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("未知异常！原因：" + ex.Message);
            }
            return list;
        }

        /// <summary>
        /// 删除系统消息
        /// </summary>
        /// <param name="SysMsgId">消息Id</param>
        /// <returns>返回是否删除成功</returns>
        public Boolean DelSysMsg(int SysMsgId)
        {
            try
            {
                string sql = "delete from sysmsg where id=@Id";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@Id",SysMsgId)
                };
                return db.ExecuteNonQuery(sql, sp);
            }
            catch (SqlException ex)
            {
                throw new Exception("数据库异常！原因：" + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("未知异常！原因：" + ex.Message);
            }
        }
    }
}
