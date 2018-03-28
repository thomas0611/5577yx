using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Game.Model;

namespace Game.DAL
{
    public class MasterLogServer
    {
        DBHelper db = new DBHelper();
        CommonServer cs = new CommonServer();

        /// <summary>
        /// 获取管理员日志数据总数
        /// </summary>
        /// <param name="WhereStr">条件</param>
        /// <returns>返回数据总数</returns>
        public Double GetMasterLogCount(string WhereStr)
        {
            return cs.GetDataCount(WhereStr, "manager_log");
        }

        /// <summary>
        /// 通过分页获取管理员日志数据
        /// </summary>
        /// <param name="PageSize">页大小</param>
        /// <param name="PageNum">页码</param>
        /// <param name="WhereStr">条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns>返回管理员日志数据集</returns>
        public List<manager_log> GetAllMasterLog(int PageSize, int PageNum, string WhereStr, string OrderBy)
        {
            List<manager_log> list = new List<manager_log>();
            try
            {
                SqlParameter[] sp = new SqlParameter[] { 
                new SqlParameter("@PageSize",PageSize),
                new SqlParameter("@PageNum",PageNum),
                new SqlParameter("@TableName","manager_log"),
                new SqlParameter("@WhereStr",WhereStr),
                new SqlParameter("@OrderBy",OrderBy)
                };
                using (SqlDataReader reder = db.GetReaderByProc("Proc_Page", sp))
                {
                    while (reder.Read())
                    {
                        manager_log ml = new manager_log();
                        ml.id = (int)reder["id"];
                        ml.user_name = reder["user_name"].ToString();
                        ml.action_type = reder["action_type"].ToString();
                        ml.note = reder["note"].ToString();
                        ml.login_ip = reder["login_ip"].ToString();
                        ml.login_time = (DateTime)reder["login_time"];
                        list.Add(ml);
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
        /// 添加管理员日志
        /// </summary>
        /// <param name="ml">管理员日志</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddMasterLog(manager_log ml)
        {
            try
            {
                string sql = "insert into manager_log(user_id,user_name,action_type,note,login_ip,login_time)"
                           + "values (@UserID,@user_name,@action_type,@note,@login_ip,@login_time)";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@UserID",ml.user_id),
                    new SqlParameter("@user_name",ml.user_name),
                    new SqlParameter("@action_type",ml.action_type), 
                    new SqlParameter("@note", ml.note),
                    new SqlParameter("@login_ip",ml.login_ip),
                    new SqlParameter("@login_time",ml.login_time)
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
