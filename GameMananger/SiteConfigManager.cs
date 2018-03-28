using Game.DAL;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Manager
{
    public class SiteConfigManager
    {
        SiteConfigServer scs = new SiteConfigServer();
        /// <summary>
        ///  读取站点配置文件
        /// </summary>
        public SiteConfig loadConfig(string configFilePath)
        {
            return scs.loadConfig(configFilePath);
        }

        /// <summary>
        /// 写入站点配置文件
        /// </summary>
        public SiteConfig saveConifg(SiteConfig mode, string configFilePath)
        {
            return scs.saveConifg(mode, configFilePath);
        }
    }
}
