using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.DAL
{
    public class ActionLockServer
    {
        DBHelper db = new DBHelper();

        /// <summary>
        /// 检测Action是否被锁定
        /// </summary>
        /// <param name="Action">推广参数</param>
        /// <returns>返回是否锁定</returns>
        public Boolean IsLock(string Action)
        {
            try
            {
                string sql = "select COUNT(*) from ActionLock where Action=@Action";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@Action", Action)
                };
                return db.ExecuteScalar(sql, sp) > 0;
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
        /// 添加一个锁定的Action
        /// </summary>
        /// <param name="Ip">推广参数</param>
        /// <param name="Operator">操作者</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddAction(string Action, string Operator)
        {
            try
            {
                string sql = "insert into ActionLock (Action,Operator)values(@Action,@Operator)";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@Action", Action),
                     new SqlParameter("@Operator", Operator)
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
        /// 删除锁定的Action
        /// </summary>
        /// <param name="Ip">推广参数</param>
        /// <returns>返回是否删除成功</returns>
        public Boolean DelAction(string Action)
        {
            try
            {
                string sql = "delete from ActionLock where Action=@Action";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@Action", Action)
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
        /// 添加一个锁定的Action
        /// </summary>
        /// <param name="Ip">Ip</param>
        /// <param name="Operator">操作者</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddIp(string Ip, string Operator)
        {
            try
            {
                string sql = "insert into ip_locking (ip,operator)values(@Ip,@Operator)";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@Ip", Ip),
                     new SqlParameter("@Operator", Operator)
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
        /// 删除锁定的Ip
        /// </summary>
        /// <param name="Ip">Ip</param>
        /// <returns>返回是否删除成功</returns>
        public Boolean DelIp(string Ip)
        {
            try
            {
                string sql = "delete from ip_locking where ip=@Ip";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@Ip", Ip)
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
