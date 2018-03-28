using Common;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace Game.DAL
{
    public class SiteConfigServer
    {
        private static object lockHelper = new object();

        /// <summary>
        ///  读取站点配置文件
        /// </summary>
        public SiteConfig loadConfig(string configFilePath)
        {
            return (SiteConfig)SerializationHelper.Load(typeof(SiteConfig), configFilePath);
        }

        /// <summary>
        /// 写入站点配置文件
        /// </summary>
        public SiteConfig saveConifg(SiteConfig mode, string configFilePath)
        {
            lock (lockHelper)
            {
                SerializationHelper.Save(mode, configFilePath);
            }
            return mode;
        }
    }
}
