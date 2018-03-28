using Game.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Game.DAL
{
    public class MasterServer
    {
        DBHelper db = new DBHelper();
        CommonServer cs = new CommonServer();

        /// <summary>
        /// 验证管理员登录
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <returns>返回是否存在</returns>
        public Boolean IsMaster(string UserName)
        {
            try
            {
                string sql = "select COUNT(*) from manager where user_name=@UserName";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@UserName", UserName)
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
        /// 根据用户名和密码获取用户
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="PassWord"></param>
        /// <returns>返回用户信息</returns>
        public Master GetMaster(string UserName, string PassWord)
        {
            Master master = null;
            try
            {
                string sql = "select * from manager where user_name=@UserName and user_pwd=@PassWord";
                SqlParameter[] sp = new SqlParameter[]
               {
                   new SqlParameter("@UserName",UserName),
                       new SqlParameter("@PassWord",PassWord)
               };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        master = new Master((int)reder["id"], (int)reder["role_id"], reder["role_type"].ToString(), reder["user_name"].ToString(),
                            reder["user_pwd"].ToString(), reder["user_pwd1"].ToString(), reder["real_name"].ToString(), reder["telephone"].ToString(),
                            reder["email"].ToString(), (int)reder["is_lock"], (DateTime)reder["add_time"]);
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
            return master;
        }

        /// <summary>
        /// 根据用户名和密码获取用户
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="CzPassWord"></param>
        /// <returns>返回用户信息</returns>
        public Master GetMasterByCz(string UserName, string CzPassWord)
        {
            Master master = null;
            try
            {
                string sql = "select * from manager where user_name=@UserName and user_pwd1=@CzPassWord";
                SqlParameter[] sp = new SqlParameter[]
               {
                   new SqlParameter("@UserName",UserName),
                       new SqlParameter("@CzPassWord",CzPassWord)
               };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        master = new Master((int)reder["id"], (int)reder["role_id"], reder["role_type"].ToString(), reder["user_name"].ToString(),
                            reder["user_pwd"].ToString(), reder["user_pwd1"].ToString(), reder["real_name"].ToString(), reder["telephone"].ToString(),
                            reder["email"].ToString(), (int)reder["is_lock"], (DateTime)reder["add_time"]);
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
            return master;
        }

        /// <summary>
        /// 获取管理员信息
        /// </summary>
        /// <param name="MasterId">管理员Id</param>
        /// <returns>返回管理员</returns>
        public Master GetMaster(int MasterId)
        {
            Master master = null;
            try
            {
                string sql = "select * from manager where id=@MasterId";
                SqlParameter[] sp = new SqlParameter[]
               {
                   new SqlParameter("@MasterId",MasterId)
               };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        master = new Master((int)reder["id"], (int)reder["role_id"], reder["role_type"].ToString(), reder["user_name"].ToString(),
                            reder["user_pwd"].ToString(), reder["user_pwd1"].ToString(), reder["real_name"].ToString(), reder["telephone"].ToString(),
                            reder["email"].ToString(), (int)reder["is_lock"], (DateTime)reder["add_time"]);
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
            return master;
        }

        /// <summary>
        /// 获取管理员数量
        /// </summary>
        /// <param name="WhereStr">条件</param>
        /// <returns>返回管理员数量</returns>
        public Double GetMasterCount(string WhereStr)
        {
            return cs.GetDataCount(WhereStr, "manager");
        }

        /// <summary>
        /// 通过分页获取管理员
        /// </summary>
        /// <param name="PageSize">页大小</param>
        /// <param name="PageNum">页码</param>
        /// <param name="WhereStr">条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns></returns>
        public List<Master> GetAllMaster(int PageSize, int PageNum, string WhereStr, string OrderBy)
        {
            List<Master> list = new List<Master>();
            try
            {
                SqlParameter[] sp = new SqlParameter[] { 
                new SqlParameter("@PageSize",PageSize),
                new SqlParameter("@PageNum",PageNum),
                new SqlParameter("@TableName","manager"),
                new SqlParameter("@WhereStr",WhereStr),
                new SqlParameter("@OrderBy",OrderBy)
                };
                using (SqlDataReader reder = db.GetReaderByProc("Proc_Page", sp))
                {
                    while (reder.Read())
                    {
                        Master m = new Master((int)reder["id"], (int)reder["role_id"], reder["role_type"].ToString(), reder["user_name"].ToString(),
                           reder["user_pwd"].ToString(), reder["user_pwd1"].ToString(), reder["real_name"].ToString(), reder["telephone"].ToString(),
                           reder["email"].ToString(), (int)reder["is_lock"], (DateTime)reder["add_time"]);
                        list.Add(m);
                    }
                }
                return list;
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

        /// <summary>
        /// 获取系统权限组总数
        /// </summary>
        /// <param name="WhereStr"></param>
        /// <returns></returns>
        public Double GetMasterRoleCount(string WhereStr)
        {
            return cs.GetDataCount(WhereStr, "manager_role");
        }

        /// <summary>
        /// 通过分页获取权限组数据
        /// </summary>
        /// <param name="PageSize">页大小</param>
        /// <param name="PageNum">页码</param>
        /// <param name="WhereStr">条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns>返回管理员日志数据集</returns>
        public List<MasterRole> GetAllMasterRole(int PageSize, int PageNum, string WhereStr, string OrderBy)
        {
            List<MasterRole> list = new List<MasterRole>();
            try
            {
                SqlParameter[] sp = new SqlParameter[] { 
                new SqlParameter("@PageSize",PageSize),
                new SqlParameter("@PageNum",PageNum),
                new SqlParameter("@TableName","manager_role"),
                new SqlParameter("@WhereStr",WhereStr),
                new SqlParameter("@OrderBy",OrderBy)
                };
                using (SqlDataReader reder = db.GetReaderByProc("Proc_Page", sp))
                {
                    while (reder.Read())
                    {
                        MasterRole ml = new MasterRole();
                        ml.RoleId = (int)reder["id"];
                        ml.RoleName = reder["role_name"].ToString();
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
        /// 获取权限组数据
        /// </summary>
        /// <returns>返回管理员日志数据集</returns>
        public List<MasterRole> GetAllMasterRole()
        {
            List<MasterRole> list = new List<MasterRole>();
            try
            {
                string sql = "select * from manager_role";
                using (SqlDataReader reder = db.GetReader(sql))
                {
                    while (reder.Read())
                    {
                        MasterRole ml = new MasterRole();
                        ml.RoleId = (int)reder["id"];
                        ml.RoleName = reder["role_name"].ToString();
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
        /// 获取角色权限组
        /// </summary>
        /// <param name="RoleId">角色Id</param>
        /// <returns>返回角色权限组</returns>
        public MasterRole GetMasterRole(int RoleId)
        {
            MasterRole mr = null;
            try
            {
                string sql = "select * from manager_role where id=@RoleId";
                SqlParameter[] sp = new SqlParameter[]
               {
                   new SqlParameter("@RoleId",RoleId)
               };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        mr = new MasterRole((int)reder["id"], reder["role_name"].ToString());
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
            return mr;
        }

        /// <summary>
        /// 添加一个管理员
        /// </summary>
        /// <param name="m">管理员</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddMaster(Master m)
        {
            try
            {
                string sql = "insert into manager(role_id,role_type,user_name,user_pwd,user_pwd1,real_name,telephone,email)"
                           + "values (@RoleId,@RoleType,@UserName,@UserPWD,@UserCzPWD,@RealName,@Phone,@Email)";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@RoleId",m.RoleId),
                    new SqlParameter("@RoleType",m.RoleType),
                    new SqlParameter("@UserName",m.UserName), 
                    new SqlParameter("@UserPWD", m.UserPWD),
                    new SqlParameter("@UserCzPWD",m.UserCzPWD),
                    new SqlParameter("@RealName",m.RealName),
                    new SqlParameter("@Phone", string.IsNullOrEmpty(m.Phone)?"":m.Phone),
                    new SqlParameter("@Email",string.IsNullOrEmpty(m.Email)?"":m.Email),
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
        /// 更新管理员信息
        /// </summary>
        /// <param name="m">管理员</param>
        /// <returns>返回是否更新成功</returns>
        public Boolean UpdateMaster(Master m)
        {
            try
            {
                string sql = "update  manager set role_id=@RoleId,role_type=@RoleType,user_name=@UserName,user_pwd=@UserPWD,user_pwd1=@UserCzPWD,real_name=@RealName,telephone=@Phone,email=@Email,is_lock=@State where id=@Id";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@RoleId",m.RoleId),
                    new SqlParameter("@RoleType",m.RoleType),
                    new SqlParameter("@UserName",m.UserName), 
                    new SqlParameter("@UserPWD", m.UserPWD),
                    new SqlParameter("@UserCzPWD",m.UserCzPWD),
                    new SqlParameter("@RealName",m.RealName),
                    new SqlParameter("@Phone", m.Phone),
                    new SqlParameter("@Email",m.Email),
                    new SqlParameter("@State", m.State),
                    new SqlParameter("@Id",m.Id),
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
        /// 删除管理员
        /// </summary>
        /// <param name="MId">管理员Id</param>
        /// <returns>返回是否删除成功</returns>
        public Boolean DelMaster(int MId)
        {
            try
            {
                string sql = "delete from manager where id=@Id";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@Id",MId),
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
        /// 添加权限组
        /// </summary>
        /// <param name="RoleName">权限组名称</param>
        /// <returns></returns>
        public Boolean AddMasterRole(string RoleName)
        {
            try
            {
                string sql = "insert into manager_role(role_name)"
                           + "values (@RoleName)";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@RoleName",RoleName)
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
        /// 获取权限组
        /// </summary>
        /// <param name="RoleName">权限组名称</param>
        /// <returns>返回权限组</returns>
        public MasterRole GetMasterRole(string RoleName)
        {
            MasterRole mr = null;
            try
            {
                string sql = "select * from manager_role where role_name=@RoleName";
                SqlParameter[] sp = new SqlParameter[]
               {
                   new SqlParameter("@RoleName",RoleName)
               };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        mr = new MasterRole((int)reder["id"], reder["role_name"].ToString());
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
            return mr;
        }
    }
}
