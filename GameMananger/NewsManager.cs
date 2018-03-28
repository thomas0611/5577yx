using Game.DAL;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Game.Manager
{
    public class NewsManager
    {
        NewsServer ns = new NewsServer();

        /// <summary>
        /// 获取新闻总条数
        /// </summary>
        /// <param name="WhereStr">条件</param>
        /// <returns>返回新闻条数</returns>
        public Double GetNewsCount(string WhereStr)
        {
            return ns.GetNewsCount(WhereStr);
        }


        /// <summary>
        /// 获取分页新闻数据
        /// </summary>
        /// <param name="PageSize">页面大小</param>
        /// <param name="PageNum">第几页</param>
        /// <param name="WhereStr">条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns>返回新闻数据集合</returns>
        public DataTable GetNews(int PageSize, int PageNum, string WhereStr, string OrderBy)
        {
            return ns.GetNews(PageSize, PageNum, WhereStr, OrderBy);
        }

        /// <summary>
        /// 获取一条新闻信息
        /// </summary>
        /// <param name="NewsId">新闻Id</param>
        /// <returns>返回新闻信息</returns>
        public News GetNews(int NewsId)
        {
            return ns.GetNews(NewsId);
        }

        /// <summary>
        /// 更新一条新闻信息
        /// </summary>
        /// <param name="n">新闻信息</param>
        /// <returns>返回是否更新成功</returns>
        public Boolean UpdateNews(News n)
        {
            return ns.UpdateNews(n);
        }

        /// <summary>
        /// 添加一新闻信息
        /// </summary>
        /// <param name="n">网站信息</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddNews(News n)
        {
            return ns.AddNews(n);
        }

        /// <summary>
        /// 删除一条新闻
        /// </summary>
        /// <param name="NewsId">新闻Id</param>
        /// <returns>返回是否删除成功</returns>
        public Boolean DelNews(int NewsId)
        {
            return ns.DelNews(NewsId);
        }

        /// <summary>
        /// 获取最新几条新闻
        /// </summary>
        /// <param name="Top">前几条</param>
        /// <param name="Type">类型</param>
        /// <returns>返回新闻集合</returns>
        public List<News> GetNews(int Top, int Type)
        {
            return ns.GetNews(Top, Type);
        }

        /// <summary>
        /// 获取最新几条新闻
        /// </summary>
        /// <param name="Top">前几条</param>
        /// <param name="Type">类型</param>
        /// <param name="GameId">游戏Id</param>
        /// <returns>返回新闻集合</returns>
        public List<News> GetNews(int Top, int Type, int GameId)
        {
            return ns.GetNews(Top, Type, GameId);
        }
    }
}
