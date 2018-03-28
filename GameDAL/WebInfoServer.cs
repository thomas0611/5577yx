using Game.Model;
using System;
using System.Data.SqlClient;

namespace Game.DAL
{
    public class WebInfoServer
    {
        DBHelper db = new DBHelper();

        /// <summary>
        /// 获取网站信息
        /// </summary>
        /// <param name="WebInfoId">网站信息ID</param>
        /// <returns>返回网站信息</returns>
        public sys_onepage GetWebInfo(int WebInfoId)
        {
            sys_onepage wi = new sys_onepage();
            try
            {
                string sql = "select * from sys_onepage where id=@WebInfoId";
                SqlParameter[] sp = new SqlParameter[]
               {
                   new SqlParameter("@WebInfoId",WebInfoId)
               };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        wi.id = (int)reder["id"];
                        wi.modelname = reder["modelname"].ToString();
                        wi.title = reder["title"].ToString();
                        wi.contents = reder["contents"].ToString();
                        wi.sort_id = (int)reder["sort_id"];
                        wi.img_url = reder["img_url"].ToString();
                        wi.seo_title = reder["seo_title"].ToString();
                        wi.seo_keyword = reder["seo_keyword"].ToString();
                        wi.seo_desc = reder["seo_desc"].ToString();
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
            return wi;
        }

        /// <summary>
        /// 更新一条网站信息
        /// </summary>
        /// <param name="wi">网站信息</param>
        /// <returns>返回是否更新成功</returns>
        public Boolean UpdateWebInfo(sys_onepage wi)
        {
            try
            {
                string sql = "update sys_onepage set modelname=@modelname,title=@title,contents=@contents,sort_id=@sort_id,seo_title=@seo_title,"
                           + "seo_keyword=@seo_keyword,seo_desc=@seo_desc,img_url=@img_url where id=@id";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@modelname",wi.modelname),
                    new SqlParameter("@title",wi.title),
                    new SqlParameter("@contents",wi.contents), 
                    new SqlParameter("@sort_id", wi.sort_id),
                    new SqlParameter("@seo_title",wi.seo_title),
                    new SqlParameter("@seo_keyword",wi.seo_keyword),
                    new SqlParameter("@seo_desc",wi.seo_desc),
                    new SqlParameter("@img_url",wi.img_url),
                    new SqlParameter("@id",wi.id)
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
        /// 添加一条网站信息
        /// </summary>
        /// <param name="wi">网站信息</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddWebInfo(sys_onepage wi)
        {
            try
            {
                string sql = "insert into sys_onepage(modelname,title,contents,sort_id,seo_title,seo_keyword,seo_desc,img_url)"
                           + "values (@modelname,@title,@contents,@sort_id,@seo_title,@seo_keyword,@seo_desc,@img_url)";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@modelname",wi.modelname),
                    new SqlParameter("@title",wi.title),
                    new SqlParameter("@contents",wi.contents), 
                    new SqlParameter("@sort_id", wi.sort_id),
                    new SqlParameter("@seo_title",wi.seo_title),
                    new SqlParameter("@seo_keyword",wi.seo_keyword),
                    new SqlParameter("@seo_desc",wi.seo_desc),
                    new SqlParameter("@img_url",wi.img_url)
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
