using Game.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Game.DAL
{
    public class ValiDateCodeServer
    {
        DBHelper db = new DBHelper();

        /// <summary>
        /// 删除验证码
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="Type">类别</param>
        /// <returns>返回是否删除成功</returns>
        public Boolean DelValiDateCode(int UserId, int Type)
        {
            try
            {
                string sql = "delete from validatecode where userid=@UserId and type=@Type";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@UserId",UserId),
                    new SqlParameter("@Type",Type)
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
        /// 添加验证码
        /// </summary>
        /// <param name="vdc">验证码</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddValiDateCode(validatecode vdc)
        {
            try
            {
                string sql = "insert into validatecode(userid,type,code,sendtime,email,phone)values " +
                    "(@userid,@type,@code,@sendtime,@email,@phone)";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@userid",vdc.userid),
                    new SqlParameter("@type",vdc.type),
                    new SqlParameter("@code",vdc.code),
                    new SqlParameter("@sendtime",vdc.sendtime),
                    new SqlParameter("@email",vdc.email),
                    new SqlParameter("@phone",string.IsNullOrEmpty(vdc.phone)?"":vdc.phone)
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
        /// 获取是否存在验证码
        /// </summary>
        /// <param name="UserId">用Id</param>
        /// <param name="Type">类型</param>
        /// <param name="SendTime">发送时间</param>
        /// <returns>返回是否存在</returns>
        public Boolean ExitValiDateCode(int UserId, int Type, DateTime SendTime)
        {
            try
            {
                string sql = "select count(*) from validatecode where userid=@UserId and type=@Type and sendtime>@SendTime";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@UserId",UserId),
                    new SqlParameter("@Type",Type),
                    new SqlParameter("@SendTime",SendTime)
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
        /// 获取验证码
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="Type">类型</param>
        /// <returns>返回验证码</returns>
        public validatecode GetValiDateCode(int UserId, int Type)
        {
            validatecode vdc = new validatecode();
            try
            {
                string sql = "select top 1 * from validatecode where userid=@UserId and type=@Type";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@UserId",UserId),
                    new SqlParameter("@Type",Type)
                };
                using (SqlDataReader reader = db.GetReader(sql, sp))
                {
                    while (reader.Read())
                    {
                        vdc.id = (int)reader["id"];
                        vdc.type = (int)reader["type"];
                        vdc.userid = (int)reader["userid"];
                        vdc.code = reader["code"].ToString();
                        vdc.sendtime = (DateTime)reader["sendtime"];
                        vdc.email = reader["type"].ToString();
                        vdc.phone = reader["phone"].ToString();
                    }
                };
            }
            catch (SqlException ex)
            {
                throw new Exception("数据库异常！原因：" + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("未知异常！原因：" + ex.Message);
            }
            return vdc;
        }
    }
}
