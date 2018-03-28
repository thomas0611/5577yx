using Game.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;

namespace Game.DAL
{
    public class NewsServer
    {
        DBHelper db = new DBHelper();
        CommonServer cs = new CommonServer();

        /// <summary>
        /// 获取新闻总条数
        /// </summary>
        /// <param name="WhereStr">条件</param>
        /// <returns>返回新闻条数</returns>
        public Double GetNewsCount(string WhereStr)
        {
            return cs.GetDataCount(WhereStr, "vw_News");
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
            return cs.GetAllData(PageSize, PageNum, WhereStr, OrderBy, "vw_News");
        }

        /// <summary>
        /// 获取一条新闻信息
        /// </summary>
        /// <param name="NewsId">新闻Id</param>
        /// <returns>返回新闻信息</returns>
        public News GetNews(int NewsId)
        {
            News n = new News();
            try
            {
                string sql = "select * from news where id=@NewsId";
                SqlParameter[] sp = new SqlParameter[]
               {
                   new SqlParameter("@NewsId",NewsId)
               };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        n = new News(NewsId, (int)reder["type_id"], (int)reder["gameid"], (int)reder["type"], reder["title"].ToString(),
                            reder["keyword"].ToString(), (DateTime)reder["release_time"], reder["photo"].ToString(), reder["source"].ToString(),
                            reder["news_content"].ToString(), (int)reder["sort_id"], reder["seo_title"].ToString(), reder["seo_keyword"].ToString(),
                            reder["seo_desc"].ToString(), (int)reder["is_top"], (int)reder["is_red"], (int)reder["is_hot"], (int)reder["is_slide"],
                            (int)reder["is_lock"], reder["namecolor"].ToString());
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("数据库异常！原因：" + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("未知异常！原因：" + ex.Message);
            }
            return n;
        }

        /// <summary>
        /// 更新一条新闻信息
        /// </summary>
        /// <param name="n">新闻信息</param>
        /// <returns>返回是否更新成功</returns>
        public Boolean UpdateNews(News n)
        {
            try
            {
                string sql = "update news set type_id=@TypeId,gameid=@GameId,type=@Type,title=@Title,keyword=@KeyWord," +
                            "photo=@Photo,source=@Source,news_content=@NewsContent,sort_id=@SortId,is_top=@IsTop,is_red=@IsRed,"
                             + "is_hot=@IsHot,is_slide=@IsSlide,is_lock=@IsLock,namecolor=@NameColor where id=@Id ";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@TypeId",n.TypeId),
                    new SqlParameter("@GameId",n.GameId),
                    new SqlParameter("@Type",n.Type), 
                    new SqlParameter("@Title", n.Title),
                    new SqlParameter("@KeyWord",n.KeyWord),
                    new SqlParameter("@Photo",n.Photo),
                    new SqlParameter("@Source",n.Source),
                    new SqlParameter("@NewsContent",n.NewsContent),
                    new SqlParameter("@SortId",n.SortId),
                    new SqlParameter("@IsTop",n.IsTop),
                    new SqlParameter("@IsRed",n.IsRed), 
                    new SqlParameter("@IsHot", n.IsHot),
                    new SqlParameter("@IsSlide",n.IsSlide),
                    new SqlParameter("@IsLock",n.IsLock),
                    new SqlParameter("@NameColor",n.NameColor),
                    new SqlParameter("@Id",n.Id)
                };
                return db.ExecuteNonQuery(sql, sp);
            }
            catch (SqlException ex)
            {
                throw new Exception("数据库异常！原因：" + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("未知异常！原因：" + ex.Message);
            }
        }

        /// <summary>
        /// 添加一新闻信息
        /// </summary>
        /// <param name="n">网站信息</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddNews(News n)
        {
            try
            {
                string sql = "insert into news(type_id,gameid,type,title,keyword,photo,source,news_content,sort_id,is_top,is_red,is_hot,is_slide,is_lock,namecolor)"
                           + "values (@TypeId,@GameId,@Type,@Title,@KeyWord,@Photo,@Source,@NewsContent,@SortId,@IsTop,@IsRed,@IsHot,@IsSlide,@IsLock,@NameColor)";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@TypeId",n.TypeId),
                    new SqlParameter("@GameId",n.GameId),
                    new SqlParameter("@Type",n.Type), 
                    new SqlParameter("@Title", n.Title),
                    new SqlParameter("@KeyWord",n.KeyWord),
                    new SqlParameter("@Photo",n.Photo),
                    new SqlParameter("@Source",n.Source),
                    new SqlParameter("@NewsContent",n.NewsContent),
                    new SqlParameter("@SortId",n.SortId),
                    new SqlParameter("@IsTop",n.IsTop), 
                    new SqlParameter("@IsRed", n.IsRed),
                    new SqlParameter("@IsHot",n.IsHot),
                    new SqlParameter("@IsSlide",n.IsSlide),
                    new SqlParameter("@IsLock",n.IsLock),
                    new SqlParameter("@NameColor",n.NameColor)
                };
                return db.ExecuteNonQuery(sql, sp);
            }
            catch (SqlException ex)
            {
                throw new Exception("数据库异常！原因：" + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("未知异常！原因：" + ex.Message);
            }
        }

        /// <summary>
        /// 删除一条新闻
        /// </summary>
        /// <param name="NewsId">新闻Id</param>
        /// <returns>返回是否删除成功</returns>
        public Boolean DelNews(int NewsId)
        {
            try
            {
                string sql = "delete from news where id=@Id";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@Id",NewsId)
                };
                return db.ExecuteNonQuery(sql, sp);
            }
            catch (SqlException ex)
            {
                throw new Exception("数据库异常！原因：" + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("未知异常！原因：" + ex.Message);
            }
        }

        /// <summary>
        /// 获取最新几条新闻
        /// </summary>
        /// <param name="Top">前几条</param>
        /// <param name="Type">类型</param>
        /// <returns>返回新闻集合</returns>
        public List<News> GetNews(int Top, int Type)
        {
            List<News> list = new List<News>();
            try
            {
                string sql = "";
                SqlParameter[] sp;
                if (Type > 0)
                {
                    sql = "select top " + Top + " * from news where type =@Type order by release_time desc";
                    sp = new SqlParameter[] {
                           new SqlParameter("@Type",Type)
                       };
                }
                else
                {
                    sql = "select top " + Top + " * from news order by release_time desc";
                    sp = new SqlParameter[] { };
                }
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        News n = new News((int)reder["id"], (int)reder["type_id"], (int)reder["gameid"], (int)reder["type"], reder["title"].ToString(),
                             reder["keyword"].ToString(), (DateTime)reder["release_time"], reder["photo"].ToString(), reder["source"].ToString(),
                             reder["news_content"].ToString(), (int)reder["sort_id"], reder["seo_title"].ToString(), reder["seo_keyword"].ToString(),
                             reder["seo_desc"].ToString(), (int)reder["is_top"], (int)reder["is_red"], (int)reder["is_hot"], (int)reder["is_slide"],
                             (int)reder["is_lock"], reder["namecolor"].ToString());
                        list.Add(n);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("数据库异常！原因：" + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("未知异常！原因：" + ex.Message);
            }
            return list;
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
            List<News> list = new List<News>();
            try
            {
                string sql = "";
                SqlParameter[] sp;
                if (Type > 0)
                {
                    sql = "select top " + Top + " * from news where type=@Type and gameid=@GameId order by release_time desc";
                    sp = new SqlParameter[]{
                       new SqlParameter("@Type",Type),
                       new SqlParameter("@GameId",GameId)
                    };
                }
                else
                {
                    sql = "select top " + Top + " * from news where gameid=@GameId order by release_time desc";
                    sp = new SqlParameter[]{
                       new SqlParameter("@GameId",GameId)
                        };
                }
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        News n = new News((int)reder["id"], (int)reder["type_id"], (int)reder["gameid"], (int)reder["type"], reder["title"].ToString(),
                             reder["keyword"].ToString(), (DateTime)reder["release_time"], reder["photo"].ToString(), reder["source"].ToString(),
                             reder["news_content"].ToString(), (int)reder["sort_id"], reder["seo_title"].ToString(), reder["seo_keyword"].ToString(),
                             reder["seo_desc"].ToString(), (int)reder["is_top"], (int)reder["is_red"], (int)reder["is_hot"], (int)reder["is_slide"],
                             (int)reder["is_lock"], reder["namecolor"].ToString());
                        list.Add(n);
                    }
                }
            }
            catch (SqlException ex)
            {
                throw new Exception("数据库异常！原因：" + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("未知异常！原因：" + ex.Message);
            }
            return list;
        }
    }
}
