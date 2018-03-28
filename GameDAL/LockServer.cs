using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.DAL
{
    public class LockServer
    {
        DBHelper db = new DBHelper();
        CommonServer cs = new CommonServer();

        /// <summary>
        /// 检测是否被锁定
        /// </summary>
        /// <param name="Lock">锁定参数</param>
        /// <returns>返回是否锁定</returns>
        public Boolean IsLock(string Lock)
        {
            try
            {
                string sql = "select COUNT(*) from Lock where Lock=@Lock";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@Lock", Lock)
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
        /// 添加一个锁定
        /// </summary>
        /// <param name="Lock">推广参数</param>
        /// <param name="Operator">操作者</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddLock(string Lock, string Operator, string LockInfo)
        {
            try
            {
                string sql = "insert into Lock (Lock,Operator,LockInfo)values(@Lock,@Operator,@LockInfo)";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@Lock", Lock),
                     new SqlParameter("@Operator", Operator),
                     new SqlParameter("@LockInfo", LockInfo)
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
        /// 删除锁定
        /// </summary>
        /// <param name="Lock">推广参数</param>
        /// <returns>返回是否删除成功</returns>
        public Boolean DelLock(string Lock)
        {
            try
            {
                string sql = "delete from Lock where Lock=@Lock";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@Lock", Lock)
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
        /// 获取锁定信息条数
        /// </summary>
        /// <returns></returns>
        public Double GetAllLockCount(string WhereStr)
        {
            return cs.GetDataCount(WhereStr, "Lock");
        }

        /// <summary>
        /// 通过分页获取锁定信息
        /// </summary>
        /// <param name="PageSize">页大小</param>
        /// <param name="PageNum">页码</param>
        /// <param name="WhereStr">条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns>返回数据集</returns>
        public DataTable GetAllLock(int PageSize, int PageNum, string WhereStr, string OrderBy)
        {
            return cs.GetAllData(PageSize, PageNum, WhereStr, OrderBy, "Lock");
        }
    }
}
