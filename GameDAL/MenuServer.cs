using Game.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Game.DAL
{
    public class MenuServer
    {
        DBHelper db = new DBHelper();

        /// <summary>
        /// 获取当前菜单的根菜单
        /// </summary>
        /// <returns>返回根菜单集合</returns>
        public List<Menu> GetRootMenu()
        {
            List<Menu> list = new List<Menu>();
            try
            {
                string sql = "select * from Menu where ParentMenuId=0";
                using (SqlDataReader reder = db.GetReader(sql))
                {
                    while (reder.Read())
                    {
                        Menu me = new Menu((int)reder["MenuId"], (int)reder["ParentMenuId"], reder["MenuName"].ToString(), reder["MenuURL"].ToString());
                        list.Add(me);
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
        /// 获取某菜单的子级菜单
        /// </summary>
        /// <param name="MenuId">菜单Id</param>
        /// <returns>返回菜单集合</returns>
        public List<Menu> GetChildMenu(int MenuId)
        {
            List<Menu> list = new List<Menu>();
            try
            {
                string sql = "select * from Menu where ParentMenuId =@MenuId";
                SqlParameter[] sp = new SqlParameter[] {
                new SqlParameter("@MenuId",MenuId)
                };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        Menu me = new Menu((int)reder["MenuId"], (int)reder["ParentMenuId"], reder["MenuName"].ToString(), reder["MenuURL"].ToString());
                        list.Add(me);
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
        /// 获取所有菜单
        /// </summary>
        /// <returns>返回菜单集合</returns>
        public List<Menu> GetAllMenu()
        {
            List<Menu> list = new List<Menu>();
            try
            {
                string sql = "select * from Menu";
                using (SqlDataReader reder = db.GetReader(sql))
                {
                    while (reder.Read())
                    {
                        Menu me = new Menu((int)reder["MenuId"], (int)reder["ParentMenuId"], reder["MenuName"].ToString(), reder["MenuURL"].ToString());
                        list.Add(me);
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
    }
}
