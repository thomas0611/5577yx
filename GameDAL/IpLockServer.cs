using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Game.DAL
{
    public class IpLockServer
    {
        DBHelper db = new DBHelper();

        /// <summary>
        /// 查询Ip是否被封
        /// </summary>
        /// <param name="Ip">IP地址</param>
        /// <returns>返回是否被封</returns>
        public Boolean IsLock(string Ip)
        {
            try
            {
                string sql = "select count(*) from ip_locking where ip=@Ip";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@Ip",Ip)
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
    }
}
