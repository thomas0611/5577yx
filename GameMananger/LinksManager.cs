using Game.DAL;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game.Manager
{
    public class LinksManager
    {
        LinksServer ls = new LinksServer();

        /// <summary>
        /// 获取友情链接数据总数
        /// </summary>
        /// <param name="WhereStr">条件</param>
        /// <returns>返回数据总数</returns>
        public Double GetLinksCount(string WhereStr)
        {
            return ls.GetLinksCount(WhereStr);
        }

        /// <summary>
        /// 通过分页获取友情链接数据
        /// </summary>
        /// <param name="PageSize">页大小</param>
        /// <param name="PageNum">页码</param>
        /// <param name="WhereStr">条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns>返回友情链接数据集</returns>
        public List<link> GetAllLinks(int PageSize, int PageNum, string WhereStr, string OrderBy)
        {
            return ls.GetAllLinks(PageSize, PageNum, WhereStr, OrderBy);
        }
        /// <summary>
        /// 获取所有友情链接
        /// </summary>
        /// <returns>返回所有友情链接</returns>
        public List<link> GetAllLinks()
        {
            return ls.GetAllLinks();
        }

        /// <summary>
        /// 获取所有友情链接
        /// </summary>
        /// <param name="Top">前几条</param>
        /// <returns>返回所有友情链接</returns>
        public List<link> GetAllLinks(int Top)
        {
            return ls.GetAllLinks(Top);
        }


        /// <summary>
        /// 获取友情链接
        /// </summary>
        /// <param name="Id">链接Id</param>
        /// <returns>返回友情链接</returns>
        public link GetLink(int Id)
        {
            return ls.GetLink(Id);
        }
        /// <summary>
        /// 更新友情链接
        /// </summary>
        /// <param name="l">友情链接</param>
        /// <returns>返回是否更新成功</returns>
        public Boolean UpdateLink(link l)
        {
            return ls.UpdateLink(l);
        }
        /// <summary>
        /// 添加友情链接
        /// </summary>
        /// <param name="l">友情链接</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddLink(link l)
        {
            return ls.AddLink(l);
        }

        /// <summary>
        /// 删除友情链接
        /// </summary>
        /// <param name="Id">链接Id</param>
        /// <returns>返回是否删除成功</returns>
        public Boolean DelLink(int Id)
        {
            return ls.DelLink(Id);
        }
    }
}
