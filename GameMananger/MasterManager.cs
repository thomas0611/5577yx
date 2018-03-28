using Game.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Model;

namespace Game.Manager
{
    public class MasterManager
    {
        MasterServer ms = new MasterServer();

        /// <summary>
        /// 验证是否管理员
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="PassWord">密码</param>
        /// <returns>返回是否存在</returns>
        public Boolean IsManager(string UserName)
        {
            return ms.IsMaster(UserName);
        }

        /// <summary>
        /// 根据用户名和密码获取用户
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="PassWord"></param>
        /// <returns>返回用户信息</returns>
        public Master GetManager(string UserName, string PassWord)
        {
            return ms.GetMaster(UserName, PassWord);
        }

        /// <summary>
        /// 获取管理员信息
        /// </summary>
        /// <param name="MasterId">管理员Id</param>
        /// <returns>返回管理员</returns>
        public Master GetMaster(int MasterId)
        {
            return ms.GetMaster(MasterId);
        }

        /// <summary>
        /// 获取管理员数量
        /// </summary>
        /// <param name="WhereStr">条件</param>
        /// <returns>返回管理员数量</returns>
        public Double GetMasterCount(string WhereStr)
        {
            return ms.GetMasterCount(WhereStr);
        }

        /// <summary>
        /// 获取所有系统管理员
        /// </summary>
        /// <returns>返回所有管理员集合</returns>
        public List<Master> GetAllMaster(int PageSize, int PageNum, string WhereStr, string OrderBy)
        {
            return ms.GetAllMaster(PageSize, PageNum, WhereStr, OrderBy);
        }

        public Double GetMasterLogCount(string WhereStr)
        {
            return ms.GetMasterLogCount(WhereStr);
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
            return ms.GetAllMasterLog(PageSize, PageNum, WhereStr, OrderBy);
        }

        /// <summary>
        /// 添加管理员日志
        /// </summary>
        /// <param name="ml">管理员日志</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddMasterLog(manager_log ml)
        {
            return ms.AddMasterLog(ml);
        }

        /// <summary>
        /// 获取系统权限组总数
        /// </summary>
        /// <param name="WhereStr"></param>
        /// <returns></returns>
        public Double GetMasterRoleCount(string WhereStr)
        {
            return ms.GetMasterRoleCount(WhereStr);
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
            return ms.GetAllMasterRole(PageSize, PageNum, WhereStr, OrderBy);
        }

        /// <summary>
        /// 获取权限组数据
        /// </summary>
        /// <returns>返回管理员日志数据集</returns>
        public List<MasterRole> GetAllMasterRole()
        {
            return ms.GetAllMasterRole();
        }

        /// <summary>
        /// 获取角色权限组
        /// </summary>
        /// <param name="RoleId">角色Id</param>
        /// <returns>返回角色权限组</returns>
        public MasterRole GetMasterRole(int RoleId)
        {
            return ms.GetMasterRole(RoleId);
        }

        /// <summary>
        /// 添加一个管理员
        /// </summary>
        /// <param name="m">管理员</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddMaster(Master m)
        {
            return ms.AddMaster(m);
        }

        /// <summary>
        /// 更新管理员信息
        /// </summary>
        /// <param name="m">管理员</param>
        /// <returns>返回是否更新成功</returns>
        public Boolean UpdateMaster(Master m)
        {
            return ms.UpdateMaster(m);
        }

        /// <summary>
        /// 删除管理员
        /// </summary>
        /// <param name="MId">管理员Id</param>
        /// <returns>返回是否删除成功</returns>
        public Boolean DelMaster(int MId)
        {
            return ms.DelMaster(MId);
        }

        /// <summary>
        /// 根据用户名和密码获取用户
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="CzPassWord"></param>
        /// <returns>返回用户信息</returns>
        public Master GetMasterByCz(string UserName, string CzPassWord)
        {
            return ms.GetMasterByCz(UserName, CzPassWord);
        }

        /// <summary>
        /// 添加权限组
        /// </summary>
        /// <param name="RoleName">权限组名称</param>
        /// <returns></returns>
        public Boolean AddMasterRole(string RoleName)
        {
            return ms.AddMasterRole(RoleName);
        }

        /// <summary>
        /// 获取权限组
        /// </summary>
        /// <param name="RoleName">权限组名称</param>
        /// <returns>返回权限组</returns>
        public MasterRole GetMasterRole(string RoleName)
        {
            return ms.GetMasterRole(RoleName);
        }
    }
}
