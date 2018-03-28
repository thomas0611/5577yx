using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.DAL;
using Game.Model;

namespace Game.Manager
{
    public class MasterLogManager
    {
        MasterLogServer mls = new MasterLogServer();
        /// <summary>
        /// 获取管理员日志数据总数
        /// </summary>
        /// <param name="WhereStr">条件</param>
        /// <returns>返回数据总数</returns>
        public Double GetMasterLogCount(string WhereStr)
        {
            return mls.GetMasterLogCount(WhereStr);
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
            return mls.GetAllMasterLog(PageSize, PageNum, WhereStr, OrderBy);
        }

        /// <summary>
        /// 添加管理员日志
        /// </summary>
        /// <param name="ml">管理员日志</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddMasterLog(manager_log ml)
        {
            return mls.AddMasterLog(ml);
        }
    }
}
