using Game.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Game.DAL
{
    public class RoleServer
    {
        DBHelper db = new DBHelper();

        /// <summary>
        /// 获取用户组
        /// </summary>
        /// <param name="RoleId">用户组Id</param>
        /// <returns>返回用户组</returns>
        public MasterRole GetRole(int RoleId)
        {
            MasterRole role = null;
            try
            {
                string sql = "select * from manager_role where id=@RoleId";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@RoleId", RoleId)
                };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        role = new MasterRole((int)reder["id"], reder["role_name"].ToString());
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
            return role;
        }
    }
}
