using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Model;
using System.Data.SqlClient;
using System.Data;

namespace Game.DAL
{
    public class OrdersServers
    {
        DBHelper db = new DBHelper();
        CommonServer cs = new CommonServer();


        /// <summary>
        /// 根据订单号获取订单
        /// </summary>
        /// <param name="OrderNo">订单号</param>
        /// <returns>返回订单信息</returns>
        public Orders GetOrder(string OrderNo)
        {
            Orders order = null; ;
            try
            {
                string sql = "select * from orders where orderno=@OrderNo";
                SqlParameter[] sp = new SqlParameter[]
               {
                   new SqlParameter("@OrderNo",OrderNo)
               };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        order = new Orders((int)reder["id"], reder["orderno"].ToString(), (int)reder["type"], (int)reder["paytypeid"], (int)reder["state"],
                            reder["username"].ToString(), float.Parse(reder["pmoney"].ToString()), float.Parse(reder["paymoney"].ToString()), (DateTime)reder["paytime"],
                            reder["ip"].ToString(), (int)reder["gameid"], (int)reder["serverid"], reder["gamename"].ToString(), reder["servername"].ToString(),
                            reder["phone"].ToString(), float.Parse(reder["rebatenum"].ToString()), (int)reder["rebateid"], (int)reder["convertid"], reder["AdminUserName"].ToString());
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
            return order;
        }

        public Orders GetOrder(int id)
        {
            Orders order = null; ;
            try
            {
                string sql = "select * from orders where id=@id";
                SqlParameter[] sp = new SqlParameter[]
               {
                   new SqlParameter("@id",id)
               };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        order = new Orders((int)reder["id"], reder["orderno"].ToString(), (int)reder["type"], (int)reder["paytypeid"], (int)reder["state"],
                            reder["username"].ToString(), float.Parse(reder["pmoney"].ToString()), float.Parse(reder["paymoney"].ToString()), (DateTime)reder["paytime"],
                            reder["ip"].ToString(), (int)reder["gameid"], (int)reder["serverid"], reder["gamename"].ToString(), reder["servername"].ToString(),
                            reder["phone"].ToString(), float.Parse(reder["rebatenum"].ToString()), (int)reder["rebateid"], (int)reder["convertid"], reder["AdminUserName"].ToString());
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
            return order;
        }

        /// <summary>
        /// 手动修改订单状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public Boolean ChangeOrderState(int id, int state)
        {
            try
            {
                string sql = "update orders set state=@State where id=@Id ";
                SqlParameter[] sp = new SqlParameter[]
                {
                    new SqlParameter("@State",state),
                    new SqlParameter("@Id",id)
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

        public DataTable GetComOrder(string OrderNo)
        {
            DataTable dt = new DataTable();
            try
            {
                string sql = "select * from vw_Orders where orderno=@OrderNo";
                SqlParameter[] sp = new SqlParameter[]
               {
                   new SqlParameter("@OrderNo",OrderNo)
               };
               dt = db.GetTable(sql,sp);
            }
            catch (SqlException ex)
            {
                throw new Exception("数据库异常！原因：" + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("未知异常！原因：" + ex.Message);
            }
            return dt;
        }

        /// <summary>
        /// 获取用户首次充值
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <returns>返回订单信息</returns>
        public Orders GetFOrder(string UserName)
        {
            Orders order = new Orders(); ;
            try
            {
                string sql = "select top 1 * from orders where username=@UserName and state=2 order by paytime asc";
                SqlParameter[] sp = new SqlParameter[]
               {
                   new SqlParameter("@UserName",UserName)
               };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        order = new Orders((int)reder["id"], reder["orderno"].ToString(), (int)reder["type"], (int)reder["paytypeid"], (int)reder["state"],
                            reder["username"].ToString(), float.Parse(reder["pmoney"].ToString()), float.Parse(reder["paymoney"].ToString()), (DateTime)reder["paytime"],
                            reder["ip"].ToString(), (int)reder["gameid"], (int)reder["serverid"], reder["gamename"].ToString(), reder["servername"].ToString(),
                            reder["phone"].ToString(), float.Parse(reder["rebatenum"].ToString()), (int)reder["rebateid"], (int)reder["convertid"], reder["AdminUserName"].ToString());
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
            return order;
        }

        /// <summary>
        /// 根据汇付宝返回的支付类型更改订单支付类型
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <param name="PayTypeId"></param>
        /// <returns></returns>
        public Boolean EditOrder(string OrderNo, int PayTypeId)
        {
            try
            {
                Orders order = GetOrder(OrderNo);
                string sql = "update orders set paytypeid=@PayTypeId where orderno=@OrderNo";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@PayTypeId",PayTypeId),
                    new SqlParameter("@OrderNo",OrderNo)
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
        /// 支付成功之后更新订单
        /// </summary>
        /// <param name="OrderNo">订单号</param>
        /// <returns>返回是否成功</returns>
        public Boolean UpdateOrder(string OrderNo)
        {
            try
            {
                Orders order = GetOrder(OrderNo);
                if (order.State == 0)
                {
                    if (order.PayTypeId == 5 || order.PayTypeId == 9 || order.PayTypeId == 12 || order.PayTypeId == 13 || order.PayTypeId == 14)
                    {
                        order.PayMoney = order.PayMoney * 0.8f; //骏网卡，Q币卡，盛大卡，网易一卡通，完美一卡通支付比例为10：8
                    }
                    else if (order.PayTypeId == 2 || order.PayTypeId == 3 || order.PayTypeId == 4)
                    {
                        order.PayMoney = order.PayMoney * 0.9f;                                     //三种手机卡支付比例为10：9
                    }
                    order.State = 1;
                }
                else if (order.State == 1)
                {
                    order.State = 2;
                }
                string sql = "update orders set paymoney=@PayMoney,state=@State where orderno=@OrderNo";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@PayMoney", order.PayMoney),
                    new SqlParameter("@OrderNo",OrderNo),
                    new SqlParameter("@State",order.State)
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
        /// 添加一笔订单
        /// </summary>
        /// <param name="o">订单</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddOrder(Orders o)
        {
            try
            {
                string sql = "insert into orders(orderno,type,paytypeid,state,username,pmoney,paymoney,paytime,ip,gameid,serverid,gamename,servername,AdminUserName)"
                            + "values(@OrderNo,@Type,@PayTypeId,@State,@UserName,@PMoney,@PayMoney,@PayTime,@Ip,@GameId,@ServerId,@GameName,@ServerName,@AdminUserName)";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@OrderNo", o.OrderNo),
                    new SqlParameter("@Type",o.Type),
                    new SqlParameter("@PayTypeId",o.PayTypeId), 
                    new SqlParameter("@State", o.State),
                    new SqlParameter("@UserName",o.UserName),
                    new SqlParameter("@PMoney",o.PMoney),
                    new SqlParameter("@PayMoney", o.PayMoney),
                    new SqlParameter("@PayTime",o.PayTime),
                    new SqlParameter("@Ip",o.Ip), 
                    new SqlParameter("@GameId", o.GameId),
                    new SqlParameter("@ServerId",o.ServerId),
                    new SqlParameter("@GameName",o.GameName),
                    new SqlParameter("@ServerName",o.ServerName), 
                    new SqlParameter("@AdminUserName", o.AdminUserName)
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
        /// 获取玩家的消费总金额
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <returns>返回消费总金额</returns>
        public string GetOrderInfo(string UserName)
        {
            try
            {
                string sql = "select isnull(sum(paymoney),0) from orders where username=@UserName and state=2 and type=1";
                SqlParameter[] sp = new SqlParameter[]
               {
                   new SqlParameter("@UserName",UserName)
               };
                return db.ExecuteScalar(sql, sp).ToString();
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
        /// 获取指定游戏服的充值排名
        /// </summary>
        /// <param name="gameId"></param>
        /// <param name="serverId"></param>
        /// <returns></returns>
        public DataTable GetSumMoneyOrder(int gameId,int serverId)
        {
            //string sql = @"SELECT TOP 100 sum(paymoney) as sumMoney,username FROM orders where orderno like 'G%' and state <> 0 and  gameid= @gameId and serverid = @serverId group by username order by sumMoney desc";
            string sql = @"SELECT r.username ,r.sumMoney ,u.source,u.sourcename,u.userid    
  FROM (SELECT TOP 100  sum(paymoney) as sumMoney,username FROM orders where state <> 0 and orderno like 'G%' and gameid= @gameId and serverid = @serverId group by username order by sumMoney desc) r left join (select a.username,a.id as userid, a.source, b.username as sourcename from game_users a left join game_users b on a.source = b.id )u on r.username = u.username";
            try
            {
                SqlParameter[] sp = new SqlParameter[]
                {
                    new SqlParameter("@gameId",gameId),
                    new SqlParameter("@serverId",serverId)
                };
                DataTable dt = db.GetTable(sql,sp);
                return dt;
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
        /// 获取订单条数
        /// </summary>
        /// <param name="WhereStr">条件</param>
        /// <returns>返回条数</returns>
        public Double GetOrderCount(string WhereStr)
        {
            return cs.GetDataCount(WhereStr, "vw_Orders");
        }

        /// <summary>
        /// 获取订单分页数据
        /// </summary>
        /// <param name="PageSize">页大小</param>
        /// <param name="PageNum">页码</param>
        /// <param name="WhereStr">条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns>返回订单数据集合</returns>
        public DataTable GetAllOrders(int PageSize, int PageNum, string WhereStr, string OrderBy)
        {
            return cs.GetAllData(PageSize, PageNum, WhereStr, OrderBy, "vw_Orders");
        }

        /// <summary>
        /// 获取推广员的推广总额
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <returns>返回总金额</returns>
        public Double GetSumMoney(int UserId, string WhereStr)
        {
            GameUserServers gus = new GameUserServers();
            GameUser gu = gus.GetGameUser(UserId);
            try
            {
                if (gu.IsSpreader == 1)
                {
                    string sql = "select ISNULL(sum(o.paymoney),0) from orders as o, game_users as gu where o.username in" +
                                    "(select username from game_users where source=@UserId)and o.state=2 and o.orderno like 'G%' and o.username=gu.username and o.gameid = gu.reggame and gu.isspreader=0 " + WhereStr;
                    SqlParameter[] sp = new SqlParameter[]{
                           new SqlParameter("@UserId",UserId)
                       };
                    return db.ExecuteScalar(sql, sp);
                }
                else if (gu.IsSpreader == 2)
                {
                    Double SpreadMoney = 0;
                    List<GameUser> list = new List<GameUser>();
                    list = gus.GetSpreadUser(UserId);
                    foreach (GameUser gameuser in list)
                    {
                        SpreadMoney += GetSumMoney(gameuser.Id, WhereStr);
                    }
                    return SpreadMoney;
                }
                else
                {
                    string sql = "select ISNULL(sum(o.paymoney),0) from orders as o, game_users as gu where o.username=@UserName and o.state=2 and o.orderno like 'G%' and o.username=gu.username and o.gameid = gu.reggame and gu.isspreader=0 " + WhereStr;
                    SqlParameter[] sp = new SqlParameter[]{
                           new SqlParameter("@UserName",gu.UserName)
                       };
                    return db.ExecuteScalar(sql, sp);
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
        }

        /// <summary>
        /// 获取订单总额
        /// </summary>
        /// <param name="WhereStr">条件</param>
        /// <returns>返回订单总额</returns>
        public Double GetSumMoney(string WhereStr)
        {
            try
            {
                string sql = "select ISNULL(SUM(PayMoney),0) from vw_Orders " + WhereStr;

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
        /// 获取推广分析数据条数
        /// </summary>
        /// <param name="WhereStr">条件</param>
        /// <returns>返回数据条数</returns>
        public Double GetPromoCount(string WhereStr)
        {
            return cs.GetDataCount(WhereStr, "vw_Promo");
        }

        /// <summary>
        /// 获取推广分析分页数据
        /// </summary>
        /// <param name="PageSize">页大小</param>
        /// <param name="PageNum">页码</param>
        /// <param name="WhereStr">条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns>返回数据集</returns>
        public DataTable GetAllPromo(int PageSize, int PageNum, string WhereStr, string OrderBy)
        {
            return cs.GetAllData(PageSize, PageNum, WhereStr, OrderBy, "vw_Promo");
        }

        /// <summary>
        /// 获取推广人数
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <returns>返回推广人数</returns>
        public Double GetSpreadCount(int UserId, Boolean IsUnder)
        {
            try
            {
                string sql = "";
                if (IsUnder)
                {
                    sql = "select count(*) from game_users where source=@UserId";
                }
                else
                {
                    sql = "select count(*) from game_users where source=@UserId";
                }
                SqlParameter[] sp = new SqlParameter[] {
                 new SqlParameter("@UserId",UserId)
                };
                return db.ExecuteScalar(sql, sp);
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
        /// 获取全部推广人数
        /// </summary>
        /// <param name="UserId">用Id</param>
        /// <returns>返回推广人数</returns>
        public Double GetAllSpreadCount(int UserId)
        {
            GameUserServers gus = new GameUserServers();
            GameUser gu = gus.GetGameUser(UserId);
            try
            {
                if (gu.IsSpreader == 1)
                {
                    string sql = "select count(*) from game_users where source=@UserId";
                    SqlParameter[] sp = new SqlParameter[] {
                     new SqlParameter("@UserId",UserId)
                    };
                    return db.ExecuteScalar(sql, sp);
                }
                else if (gu.IsSpreader == 2)
                {
                    Double SpreadCount = 0;
                    List<GameUser> list = new List<GameUser>();
                    list = gus.GetSpreadUser(UserId);
                    list.Add(gu);
                    foreach (GameUser gameuser in list)
                    {
                        string sql = "select count(*) from game_users where source=@UserId";
                        SqlParameter[] sp = new SqlParameter[] {
                         new SqlParameter("@UserId",gameuser.Id)
                        };
                        SpreadCount += db.ExecuteScalar(sql, sp);
                    }
                    return SpreadCount;
                }
                else
                {
                    return 0;
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
        }

        /// <summary>
        /// 获取有效推广人数
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public double GetAllSpreadCountS(int UserId)
        {
            GameUserServers gus = new GameUserServers();
            GameUser gu = gus.GetGameUser(UserId);
            try
            {
                if (gu.IsSpreader == 1)
                {
                    string sql = "select count(distinct ip) from game_users where source=@UserId";
                    SqlParameter[] sp = new SqlParameter[] {
                     new SqlParameter("@UserId",UserId)
                    };
                    return db.ExecuteScalar(sql, sp);
                }
                else if (gu.IsSpreader == 2)
                {
                    Double SpreadCount = 0;
                    List<GameUser> list = new List<GameUser>();
                    list = gus.GetSpreadUser(UserId);
                    list.Add(gu);
                    foreach (GameUser gameuser in list)
                    {
                        string sql = "select count(distinct ip) from game_users where source=@UserId";
                        SqlParameter[] sp = new SqlParameter[] {
                         new SqlParameter("@UserId",gameuser.Id)
                        };
                        SpreadCount += db.ExecuteScalar(sql, sp);
                    }
                    return SpreadCount;
                }
                else
                {
                    return 0;
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
        }

        /// <summary>
        /// 获取推广用户的充值详情总条数
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <returns>返回总条数</returns>
        public Double GetSpreadPayCount(int UserId, string GameId, string StartTime, string EndTime)
        {
            try
            {
                SqlParameter[] sp = new SqlParameter[] { 
                    new SqlParameter("@UserId",UserId), 
                    new SqlParameter("@StartTime",StartTime),
                     new SqlParameter("@EndTime",EndTime),
                     new SqlParameter("@GameId",GameId)
                };
                return db.ExecuteScalarByProc("Proc_Count_SpreadPay", sp);
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
        /// 通过分页获取推广付费详情
        /// </summary>
        /// <param name="PageSize">页大小</param>
        /// <param name="PageNum">页码</param>
        /// <param name="UserId">用户Id</param>
        /// <returns>返回订单集合</returns>
        public List<Orders> GetAllSpreadPay(int PageSize, int PageNum, int UserId, string GameId, string StartTime, string EndTime)
        {
            List<Orders> list = new List<Orders>();
            try
            {
                SqlParameter[] sp = new SqlParameter[] { 
                new SqlParameter("@UserId",UserId),
                 new SqlParameter("@PageSize",PageSize),
                new SqlParameter("@PageNum",PageNum),
                 new SqlParameter("@StartTime",StartTime),
                new SqlParameter("@EndTime",EndTime),
                 new SqlParameter("@GameId",GameId)
                };
                using (SqlDataReader reder = db.GetReaderByProc("Proc_Page_SpreadPay", sp))
                {
                    while (reder.Read())
                    {
                        Orders order = new Orders((int)reder["id"], reder["orderno"].ToString(), (int)reder["type"], (int)reder["paytypeid"], (int)reder["state"],
                              reder["username"].ToString(), float.Parse(reder["pmoney"].ToString()), float.Parse(reder["paymoney"].ToString()), (DateTime)reder["paytime"],
                              reder["ip"].ToString(), (int)reder["gameid"], (int)reder["serverid"], reder["gamename"].ToString(), reder["servername"].ToString(),
                              reder["phone"].ToString(), float.Parse(reder["rebatenum"].ToString()), (int)reder["rebateid"], (int)reder["convertid"], reder["AdminUserName"].ToString());
                        list.Add(order);
                    }
                };
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
        /// 获取推广游戏条数
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <returns>返回总条数</returns>
        public Double GetSpreadGameCount(int UserId, string WhereStr)
        {
            try
            {
                string sql = "select count( Distinct o.serverid )from game_users as gu,orders as o where source in(select id from game_users where source=@UserId or id=@UserId)and o.username = gu.username and o.username=gu.username and o.state=2 and o.orderno like 'G%'and o.username=gu.username and o.gameid = gu.reggame and gu.isspreader=0" + WhereStr;
                SqlParameter[] sp = new SqlParameter[] {
                     new SqlParameter("@UserId",UserId)
                    };
                return db.ExecuteScalar(sql, sp);
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
        /// 获取所有推广游戏
        /// </summary>
        /// <param name="PageSize">页大小</param>
        /// <param name="PageNum">页码</param>
        /// <param name="UserId">用户Id</param>
        /// <returns>返回所有推广游戏集合</returns>
        public Dictionary<string, string> GetAllSpreadGame(int PageSize, int PageNum, int UserId, string GameId, string StartTime, string EndTime)
        {
            Dictionary<string, string> list = new Dictionary<string, string>();
            try
            {
                SqlParameter[] sp = new SqlParameter[] { 
                new SqlParameter("@UserId",UserId),
                 new SqlParameter("@PageSize",PageSize),
                new SqlParameter("@PageNum",PageNum),
                new SqlParameter("@StartTime",StartTime),
                new SqlParameter("@EndTime",EndTime),
                 new SqlParameter("@GameId",GameId)
                };
                using (SqlDataReader reder = db.GetReaderByProc("Proc_Page_SpreadGame", sp))
                {
                    while (reder.Read())
                    {
                        list.Add(reder["ServerId"].ToString(), reder["PayMoney"].ToString());
                    }
                };
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
