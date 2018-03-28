using Game.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Game.DAL
{
    public class LinksServer
    {
        DBHelper db = new DBHelper();
        CommonServer cs = new CommonServer();

        /// <summary>
        /// 获取友情链接数据总数
        /// </summary>
        /// <param name="WhereStr">条件</param>
        /// <returns>返回数据总数</returns>
        public Double GetLinksCount(string WhereStr)
        {
            return cs.GetDataCount(WhereStr, "link");
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
            List<link> list = new List<link>();
            try
            {
                SqlParameter[] sp = new SqlParameter[] { 
                new SqlParameter("@PageSize",PageSize),
                new SqlParameter("@PageNum",PageNum),
                new SqlParameter("@TableName","link"),
                new SqlParameter("@WhereStr",WhereStr),
                new SqlParameter("@OrderBy",OrderBy)
                };
                using (SqlDataReader reder = db.GetReaderByProc("Proc_Page", sp))
                {
                    while (reder.Read())
                    {
                        link l = new link();
                        l.Id = (int)reder["id"];
                        l.Title = reder["title"].ToString();
                        l.User_name = reder["user_name"].ToString();
                        l.User_tel = reder["user_tel"].ToString();
                        l.Email = reder["email"].ToString();
                        l.Site_url = reder["site_url"].ToString();
                        l.Img_url = reder["img_url"].ToString();
                        l.Is_image = (int)reder["is_image"];
                        l.Sort_id = (int)reder["sort_id"];
                        l.Is_red = int.Parse(reder["is_red"].ToString().Trim());
                        l.Is_lock = int.Parse(reder["is_lock"].ToString().Trim());
                        l.Add_time = (DateTime)reder["add_time"];
                        list.Add(l);
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
        /// 获取所有友情链接
        /// </summary>
        /// <returns>返回所有友情链接</returns>
        public List<link> GetAllLinks()
        {
            List<link> list = new List<link>();
            try
            {
                using (SqlDataReader reder = db.GetReader("Select * from link"))
                {
                    while (reder.Read())
                    {
                        link l = new link();
                        l.Id = (int)reder["id"];
                        l.Title = reder["title"].ToString();
                        l.User_name = reder["user_name"].ToString();
                        l.User_tel = reder["user_tel"].ToString();
                        l.Email = reder["email"].ToString();
                        l.Site_url = reder["site_url"].ToString();
                        l.Img_url = reder["img_url"].ToString();
                        l.Is_image = (int)reder["is_image"];
                        l.Sort_id = (int)reder["sort_id"];
                        l.Is_red = int.Parse(reder["is_red"].ToString().Trim());
                        l.Is_lock = int.Parse(reder["is_lock"].ToString().Trim());
                        l.Add_time = (DateTime)reder["add_time"];
                        list.Add(l);
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
        /// 获取所有友情链接
        /// </summary>
        /// <param name="Top">前几条</param>
        /// <returns>返回所有友情链接</returns>
        public List<link> GetAllLinks(int Top)
        {
            List<link> list = new List<link>();
            try
            {
                using (SqlDataReader reder = db.GetReader("Select top " + Top + " * from link"))
                {
                    while (reder.Read())
                    {
                        link l = new link();
                        l.Id = (int)reder["id"];
                        l.Title = reder["title"].ToString();
                        l.User_name = reder["user_name"].ToString();
                        l.User_tel = reder["user_tel"].ToString();
                        l.Email = reder["email"].ToString();
                        l.Site_url = reder["site_url"].ToString();
                        l.Img_url = reder["img_url"].ToString();
                        l.Is_image = (int)reder["is_image"];
                        l.Sort_id = (int)reder["sort_id"];
                        l.Is_red = int.Parse(reder["is_red"].ToString().Trim());
                        l.Is_lock = int.Parse(reder["is_lock"].ToString().Trim());
                        l.Add_time = (DateTime)reder["add_time"];
                        list.Add(l);
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
        /// 获取友情链接
        /// </summary>
        /// <param name="Id">链接Id</param>
        /// <returns>返回友情链接</returns>
        public link GetLink(int Id)
        {
            link l = new link();
            try
            {
                using (SqlDataReader reder = db.GetReader("Select * from link where id=" + Id))
                {
                    while (reder.Read())
                    {
                        l = new link();
                        l.Id = (int)reder["id"];
                        l.Title = reder["title"].ToString();
                        l.User_name = reder["user_name"].ToString();
                        l.User_tel = reder["user_tel"].ToString();
                        l.Email = reder["email"].ToString();
                        l.Site_url = reder["site_url"].ToString();
                        l.Img_url = reder["img_url"].ToString();
                        l.Is_image = (int)reder["is_image"];
                        l.Sort_id = (int)reder["sort_id"];
                        l.Is_red = int.Parse(reder["is_red"].ToString().Trim());
                        l.Is_lock = int.Parse(reder["is_lock"].ToString().Trim());
                        l.Add_time = (DateTime)reder["add_time"];
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
            return l;
        }

        /// <summary>
        /// 更新友情链接
        /// </summary>
        /// <param name="l">友情链接</param>
        /// <returns>返回是否更新成功</returns>
        public Boolean UpdateLink(link l)
        {
            try
            {
                string sql = "update link set title=@Title,user_name=@User_name,user_tel=@User_tel,email=@Email,site_url=@Site_url,img_url=@Img_url,sort_id=@SortId,is_red=@Is_red,is_lock=@Is_lock,is_image=@Is_image where id=@Id ";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@Title",l.Title),
                    new SqlParameter("@User_name",l.User_name),
                    new SqlParameter("@User_tel",l.User_tel), 
                    new SqlParameter("@Email",l.Email),
                    new SqlParameter("@Site_url",l.Site_url),
                    new SqlParameter("@Img_url",l.Img_url),
                    new SqlParameter("@SortId",l.Sort_id),
                    new SqlParameter("@Is_red",l.Is_red),
                    new SqlParameter("@Is_lock",l.Is_lock), 
                    new SqlParameter("@Is_image",l.Is_image), 
                    new SqlParameter("@Id",l.Id)
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
        /// 添加友情链接
        /// </summary>
        /// <param name="l">友情链接</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddLink(link l)
        {
            try
            {
                string sql = "insert into link (title,user_name,user_tel,email,site_url,img_url,sort_id,is_red,is_lock)values(@Title,@User_name,@User_tel,@Email,@Site_url,@Img_url,@SortId,@Is_red,@Is_lock)";
                SqlParameter[] sp = new SqlParameter[] 
                {
                     new SqlParameter("@Title",l.Title),
                    new SqlParameter("@User_name",l.User_name),
                    new SqlParameter("@User_tel",l.User_tel), 
                    new SqlParameter("@Email",l.Email),
                    new SqlParameter("@Site_url",l.Site_url),
                    new SqlParameter("@Img_url",l.Img_url),
                    new SqlParameter("@SortId",l.Sort_id),
                    new SqlParameter("@Is_red",l.Is_red),
                    new SqlParameter("@Is_lock",l.Is_lock), 
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
        /// 删除友情链接
        /// </summary>
        /// <param name="Id">链接Id</param>
        /// <returns>返回是否删除成功</returns>
        public Boolean DelLink(int Id)
        {
            try
            {
                string sql = "Delete from link where id=@Id ";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@Id",Id)
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
    }
}
