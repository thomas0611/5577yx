using Game.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Model;

namespace Game.Manager
{
    public class WebInfoManager
    {
        WebInfoServer wis = new WebInfoServer();

        /// <summary>
        /// 获取网站信息
        /// </summary>
        /// <param name="WebInfoId">网站信息ID</param>
        /// <returns>返回网站信息</returns>
        public sys_onepage GetWebInfo(int WebInfoId)
        {
            return wis.GetWebInfo(WebInfoId);
        }

        /// <summary>
        /// 更新一条网站信息
        /// </summary>
        /// <param name="wi">网站信息</param>
        /// <returns>返回是否更新成功</returns>
        public Boolean UpdateWebInfo(sys_onepage wi)
        {
            return wis.UpdateWebInfo(wi);
        }
    }
}
