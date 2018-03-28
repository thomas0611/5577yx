using Game.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Game.DAL
{
    public class RoleCompetenceServer
    {
        DBHelper db = new DBHelper();


        /// <summary>
        /// 检测是否具有权限
        /// </summary>
        /// <param name="RoleId">用户组Id</param>
        /// <param name="CompetenceId">权限Id</param>
        /// <returns>返回是否具有权限</returns>
        public Boolean GetRoleCompetence(int RoleId, int CompetenceId)
        {
            try
            {
                string sql = "select count(*) from RoleCompetence where RoleId=@RoleId and CompetenceId=@CompetenceId";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@RoleId", RoleId),
                    new SqlParameter("@CompetenceId",CompetenceId)
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
        /// 获取所有权限
        /// </summary>
        /// <returns>返回权限集合</returns>
        public List<Competence> GetAllCompetence()
        {
            List<Competence> list = new List<Competence>();
            try
            {
                string sql = "select * from Competence";
                using (SqlDataReader reder = db.GetReader(sql))
                {
                    while (reder.Read())
                    {
                        Competence c = new Competence((int)reder["CompetenceId"], (int)reder["ParentCompetenceId"], reder["CompetenceName"].ToString(), (int)reder["CompetenceMenu"]);
                        list.Add(c);
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
        /// 获取权限组的所有权限
        /// </summary>
        /// <param name="RoleId">权限组Id</param>
        /// <returns>返回权限集合</returns>
        public List<Competence> GetAllCompetence(int RoleId)
        {
            List<Competence> list = new List<Competence>();
            try
            {
                string sql = "select * from vw_RoleCompetence where RoleId=@RoleId";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@RoleId", RoleId)
                };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        Competence c = new Competence((int)reder["CompetenceId"], (int)reder["ParentCompetenceId"], reder["CompetenceName"].ToString(), (int)reder["MenuId"]);
                        list.Add(c);
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
        /// 获取权限组的所有权限
        /// </summary>
        /// <param name="CompetenceId">权限组Id</param>
        /// <param name="RoleId">权限组</param>
        /// <returns>返回权限集合</returns>
        public List<Competence> GetChildrenCompetence(int CompetenceId, int RoleId)
        {
            List<Competence> list = new List<Competence>();
            try
            {
                string sql = "select * from vw_RoleCompetence where ParentCompetenceId=@CompetenceId and RoleId=@RoleId";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@CompetenceId", CompetenceId),
                     new SqlParameter("@RoleId", RoleId)
                };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        Competence c = new Competence((int)reder["CompetenceId"], (int)reder["ParentCompetenceId"], reder["CompetenceName"].ToString(), (int)reder["MenuId"]);
                        list.Add(c);
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
        /// 获取菜单
        /// </summary>
        /// <param name="RoleId">权限组Id</param>
        /// <returns>返回菜单集合</returns>
        public List<Menu> GetAllMenu(int RoleId)
        {
            List<Menu> list = new List<Menu>();
            try
            {
                string sql = "select * from vw_RoleCompetence where MenuId>0 and RoleId =" + RoleId;
                using (SqlDataReader reder = db.GetReader(sql))
                {
                    while (reder.Read())
                    {
                        Menu c = new Menu((int)reder["MenuId"], (int)reder["ParentMenuId"], reder["MenuName"].ToString(), reder["MenuURL"].ToString());
                        list.Add(c);
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
        /// 删除权限组的所有权限
        /// </summary>
        /// <param name="RoleId">权限组Id</param>
        /// <returns>返回是否删除成功</returns>
        public Boolean DelAllCom(int RoleId)
        {
            try
            {
                string sql = "delete from RoleCompetence where RoleId=@RoleId";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@RoleId", RoleId)
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
        /// 给权限组增加权限
        /// </summary>
        /// <param name="RoleId">权限组Id</param>
        /// <param name="CompetenceId">权限Id</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddCompentence(int RoleId, int CompetenceId)
        {
            try
            {
                string sql = "insert into RoleCompetence (RoleId,CompetenceId)values(@RoleId,@CompetenceId)";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@RoleId", RoleId),
                    new SqlParameter("@CompetenceId", CompetenceId)
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
