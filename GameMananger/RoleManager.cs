using Game.DAL;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Manager
{
    public class RoleManager
    {
        RoleServer rs = new RoleServer();
        /// <summary>
        /// 获取用户组
        /// </summary>
        /// <param name="RoleId">用户组Id</param>
        /// <returns>返回用户组</returns>
        public MasterRole GetRole(int RoleId)
        {
            return rs.GetRole(RoleId);
        }
    }
}
