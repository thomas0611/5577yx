using Game.DAL;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Game.Manager
{
    public class SourceChangeManager
    {
        SourceChangeServer scs = new SourceChangeServer();
        /// <summary>
        /// 添加一条来源变更信息
        /// </summary>
        /// <param name="sc">来源变更</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddSourceChange(SourceChange sc)
        {
            return scs.AddSourceChange(sc);
        }


        /// <summary>
        /// 获取来源变更统计数据条数
        /// </summary>
        /// <param name="WhereStr">条件</param>
        /// <returns>返回数据条数</returns>
        public Double GetSourceChangeCount(string WhereStr)
        {
            return scs.GetSourceChangeCount(WhereStr);
        }

        /// <summary>
        /// 通过分页获取来源变更统计数据
        /// </summary>
        /// <param name="PageSize">页大小</param>
        /// <param name="PageNum">页码</param>
        /// <param name="WhereStr">条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns>返回来源变更统计数据</returns>
        public DataTable GetAllSourceChange(int PageSize, int PageNum, string WhereStr, string OrderBy)
        {
            DataTable dt = new DataTable();
            dt = scs.GetAllSourceChange(PageSize, PageNum, WhereStr, OrderBy);
            foreach (DataRow row in dt.Rows)
            {
                if (string.IsNullOrEmpty(row["SourceName"].ToString()))
                {
                    row["SourceName"] = "用户注册";
                }
            }
            return dt;
        }

        /// <summary>
        /// 删除来源变更统计
        /// </summary>
        /// <param name="SCId">Id</param>
        /// <returns>返回是否删除成功</returns>
        public Boolean DelSourceChange(int SCId)
        {
            return scs.DelSourceChange(SCId);
        }
    }
}
