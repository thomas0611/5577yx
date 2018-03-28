using Game.DAL;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Manager
{
    public class RoleCompetenceManager
    {
        RoleCompetenceServer rcs = new RoleCompetenceServer();
        /// <summary>
        /// 检测是否具有权限
        /// </summary>
        /// <param name="RoleId">用户组Id</param>
        /// <param name="CompetenceId">权限Id</param>
        /// <returns>返回是否具有权限</returns>
        public Boolean GetRoleCompetence(int RoleId, int CompetenceId)
        {
            return rcs.GetRoleCompetence(RoleId, CompetenceId);
        }

        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <returns>返回权限集合</returns>
        public List<Competence> GetAllCompetence()
        {
            return rcs.GetAllCompetence();
        }

        /// <summary>
        /// 获取权限组的所有权限
        /// </summary>
        /// <param name="RoleId">权限组Id</param>
        /// <returns>返回权限集合</returns>
        public List<Competence> GetAllCompetence(int RoleId)
        {
            return rcs.GetAllCompetence(RoleId);
        }
        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="RoleId">权限组Id</param>
        /// <returns>返回菜单集合</returns>
        public List<Menu> GetAllMenu(int RoleId)
        {
            return rcs.GetAllMenu(RoleId);
        }

        /// <summary>
        /// 获取权限组的所有权限
        /// </summary>
        /// <param name="CompetenceId">权限组Id</param>
        /// <returns>返回权限集合</returns>
        public List<Competence> GetChildrenCompetence(int CompetenceId, int RoleId)
        {
            return rcs.GetChildrenCompetence(CompetenceId, RoleId);
        }

        /// <summary>
        /// 删除权限组的所有权限
        /// </summary>
        /// <param name="RoleId">权限组Id</param>
        /// <returns>返回是否删除成功</returns>
        public Boolean DelAllCom(int RoleId)
        {
            return rcs.DelAllCom(RoleId);
        }

        /// <summary>
        /// 给权限组增加权限
        /// </summary>
        /// <param name="RoleId">权限组Id</param>
        /// <param name="CompetenceId">权限Id</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddCompentence(int RoleId, int CompetenceId)
        {
            return rcs.AddCompentence(RoleId, CompetenceId);
        }
    }
}
