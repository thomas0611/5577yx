using Game.DAL;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Manager
{
    public class MenuManager
    {
        MenuServer ms = new MenuServer();
        /// <summary>
        /// 获取当前菜单的根菜单
        /// </summary>
        /// <returns>返回根菜单集合</returns>
        public List<Menu> GetRootMenu()
        {
            return ms.GetRootMenu();
        }

        /// <summary>
        /// 获取某菜单的子级菜单
        /// </summary>
        /// <param name="MenuId">菜单Id</param>
        /// <returns>返回菜单集合</returns>
        public List<Menu> GetChildMenu(int MenuId)
        {
            return ms.GetChildMenu(MenuId);
        }

        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <returns>返回菜单集合</returns>
        public List<Menu> GetAllMenu()
        {
            return ms.GetAllMenu();
        }
    }
}
