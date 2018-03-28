using Game.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Game.DAL
{
    public class CardServer
    {
        DBHelper db = new DBHelper();
        CommonServer cs = new CommonServer();

        /// <summary>
        /// 获取新闻总条数
        /// </summary>
        /// <param name="WhereStr">条件</param>
        /// <returns>返回新闻条数</returns>
        public Double GetCardCount(string WhereStr)
        {
            return cs.GetDataCount(WhereStr, "vw_Cards");
        }

        /// <summary>
        /// 获取分页新手卡数据
        /// </summary>
        /// <param name="PageSize">页面大小</param>
        /// <param name="PageNum">第几页</param>
        /// <param name="WhereStr">条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns>返回新闻数据集合</returns>
        public DataTable GetCards(int PageSize, int PageNum, string WhereStr, string OrderBy)
        {
            return cs.GetAllData(PageSize, PageNum, WhereStr, OrderBy, "vw_Cards");
        }

        /// <summary>
        /// 获取新手卡
        /// </summary>
        /// <param name="CardId">卡Id</param>
        /// <returns>返回新手卡</returns>
        public cardsname GetCard(int CardId)
        {
            cardsname cn = new cardsname();
            try
            {
                string sql = "select * from cardsname where id=@CardId";
                SqlParameter[] sp = new SqlParameter[]
               {
                   new SqlParameter("@CardId",CardId)
               };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        cn.id = (int)reder["id"];
                        cn.gameid = (int)reder["gameid"];
                        cn.serverid = (int)reder["serverid"];
                        cn.cardname = reder["cardname"].ToString();
                        cn.carddesc = reder["carddesc"].ToString();
                        cn.urls = reder["urls"].ToString();
                        cn.islock = (int)reder["islock"];
                        cn.gamename = reder["gamename"].ToString();
                        cn.servername = reder["servername"].ToString();
                        cn.img = reder["img"].ToString();
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
            return cn;
        }

        /// <summary>
        /// 更新新手卡信息
        /// </summary>
        /// <param name="cn">新手卡</param>
        /// <returns>返回是否更新成功</returns>
        public Boolean UpdateCard(cardsname cn)
        {
            try
            {
                string sql = "update cardsname set gameid=@GameId,serverid=@serverid,cardname=@cardname,carddesc=@carddesc," +
               "img=@Img,urls=@urls,islock=@islock,gamename=@gamename,servername=@servername" +
                 " where id=@Id ";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@GameId",cn.gameid),
                    new SqlParameter("@serverid",cn.serverid),
                    new SqlParameter("@cardname",cn.cardname), 
                    new SqlParameter("@carddesc",string.IsNullOrEmpty(cn.carddesc)?"":cn.carddesc),
                    new SqlParameter("@Img",string.IsNullOrEmpty( cn.img)?"":cn.img),
                    new SqlParameter("@urls",string.IsNullOrEmpty(cn.urls)?"":cn.urls),
                    new SqlParameter("@islock",cn.islock),
                    new SqlParameter("@gamename",cn.gamename),
                    new SqlParameter("@servername",cn.servername),
                    new SqlParameter("@Id",cn.id)
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
        /// 添加新手卡
        /// </summary>
        /// <param name="cn">新手卡</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddCard(cardsname cn)
        {
            try
            {
                string sql = "insert into cardsname(gameid,serverid,cardname,carddesc,img,urls,islock,gamename,servername)"
                                 + "values (@GameId,@serverid,@cardname,@carddesc,@Img,@urls,@islock,@gamename,@servername)";
                SqlParameter[] sp = new SqlParameter[] 
                {
                   new SqlParameter("@GameId",cn.gameid),
                    new SqlParameter("@serverid",cn.serverid),
                    new SqlParameter("@cardname",cn.cardname), 
                   new SqlParameter("@carddesc",string.IsNullOrEmpty(cn.carddesc)?"":cn.carddesc),
                    new SqlParameter("@Img",string.IsNullOrEmpty( cn.img)?"":cn.img),
                    new SqlParameter("@urls",string.IsNullOrEmpty(cn.urls)?"":cn.urls),
                    new SqlParameter("@islock",cn.islock),
                    new SqlParameter("@gamename",cn.gamename),
                    new SqlParameter("@servername",cn.servername)
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
        /// 删除新手卡
        /// </summary>
        /// <param name="CardId">卡Id</param>
        /// <returns>返回是否删除成功</returns>
        public Boolean DelCard(int CardId)
        {
            try
            {
                string sql = "delete from cardsname where id=@Id";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@Id",CardId)
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
        /// 添加新手卡号
        /// </summary>
        /// <param name="c">卡号</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddCard(cards c)
        {
            try
            {
                string sql = "insert into cards(cardnum,cardnameid)values (@cardnum,@cardnameid)";
                SqlParameter[] sp = new SqlParameter[] 
                {
                   new SqlParameter("@cardnum",c.cardnum),
                    new SqlParameter("@cardnameid",c.cardnameid)
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
        /// 检查是否存在该卡号
        /// </summary>
        /// <param name="CardId">卡Id</param>
        /// <param name="CardNum">卡号</param>
        /// <returns>返回是否存在</returns>
        public Boolean ExitCard(int CardId, string CardNum)
        {
            try
            {
                string sql = "select COUNT(*) from cards where cardnameid=@cardnameid and cardnum=@cardnum";
                SqlParameter[] sp = new SqlParameter[] 
                {
                   new SqlParameter("@cardnum",CardNum),
                    new SqlParameter("@cardnameid",CardId)
                };
                return db.ExecuteScalar(sql, sp) > 0;
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
        /// 获取新手卡领取记录数量
        /// </summary>
        /// <param name="WhereStr">条件</param>
        /// <returns>返回记录数量</returns>
        public Double GetCardLogCount(string WhereStr)
        {
            return cs.GetDataCount(WhereStr, "vw_CardLog");
        }

        /// <summary>
        /// 通过分页获取所有新手卡领取记录
        /// </summary>
        /// <param name="PageSize">页大小</param>
        /// <param name="PageNum">页码</param>
        /// <param name="WhereStr">条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns>返回数据集合</returns>
        public DataTable GetAllCardLog(int PageSize, int PageNum, string WhereStr, string OrderBy)
        {
            return cs.GetAllData(PageSize, PageNum, WhereStr, OrderBy, "vw_CardLog");
        }

        /// <summary>
        /// 获取新手卡剩余数量
        /// </summary>
        /// <param name="CardId">卡Id</param>
        /// <returns>返回新手卡数量</returns>
        public Double GetCardCount(int CardId)
        {
            try
            {
                string sql = "select count(*) from cards where cardnameid=" + CardId + " and state=0";
                return db.ExecuteScalar(sql);
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
        /// 检测玩家是否已经领取新手卡
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="CardId">卡Id</param>
        /// <returns>返回是否领取</returns>
        public Boolean ExitCardLog(int UserId, int CardId)
        {
            try
            {
                string sql = "select COUNT(*) from cardslog where userid=@UserId and cardid=CardId";
                SqlParameter[] sp = new SqlParameter[] 
                {
                   new SqlParameter("@UserId",UserId),
                    new SqlParameter("@CardId",CardId)
                };
                return db.ExecuteScalar(sql, sp) > 0;
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
        /// 获取一张未领取的新手卡
        /// </summary>
        /// <param name="CardId">卡Id</param>
        /// <returns>返回新手卡</returns>
        public cards GetCards(int CardId)
        {
            cards c = new cards();
            try
            {
                string sql = "select top 1 * from cards where state =0 and cardnameid=@CardId";
                SqlParameter[] sp = new SqlParameter[]
               {
                   new SqlParameter("@CardId",CardId)
               };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        c.id = (int)reder["id"];
                        c.cardnum = reder["cardnum"].ToString();
                        c.cardnameid = (int)reder["cardnameid"];
                        c.state = (int)reder["state"];
                        c.addtime = (DateTime)reder["addtime"];
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
            return c;
        }

        /// <summary>
        /// 添加一天领卡记录
        /// </summary>
        /// <param name="cl">领卡记录</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddCardLog(cardslog cl)
        {
            try
            {
                string sql = "insert into cardslog(userid,cardid,cardsid)values (@UserId,@CardId,@CardsId)";
                SqlParameter[] sp = new SqlParameter[] 
                {
                   new SqlParameter("@UserId",cl.userid),
                    new SqlParameter("@CardId",cl.cardid),
                     new SqlParameter("@CardsId",cl.cardsid)
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
        /// 更新新手卡状态
        /// </summary>
        /// <param name="State">状态</param>
        /// <param name="CardsId">卡Id</param>
        /// <returns>返回是否更新成功</returns>
        public Boolean UpdateCard(int State, int CardsId)
        {
            try
            {
                string sql = "update cards set state=@State where id=@Id ";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@State",State),
                    new SqlParameter("@Id",CardsId),
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
