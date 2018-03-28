using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Game.Model;

namespace Game.DAL
{
    public class OnlineLogServers
    {
        DBHelper db = new DBHelper();

        /// <summary>
        /// 获取玩家是否在某游戏的某服务器登录过
        /// </summary>
        /// <param name="GameId">游戏Id</param>
        /// <param name="UserId">用户Id</param>
        /// <param name="ServerId">服务器Id</param>
        /// <returns>返回是否</returns>
        public Boolean IsLogin(int GameId, int UserId, int ServerId)
        {
            try
            {
                string sql = "select count(*) from onlinelog where gameid=@GameId and userid=@UserId and serverid=@ServerId";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@GameId", GameId),
                    new SqlParameter("@UserId",UserId),
                    new SqlParameter("@ServerId",ServerId)
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
        /// 获取某玩家在某游戏登陆过的服务器集合
        /// </summary>
        /// <param name="gameid">游戏Id</param>
        /// <param name="UserId">用户Id</param>
        /// <returns>服务器集合</returns>
        public List<string> GetServerList(int GameId, string UserId)
        {
            List<string> list = new List<string>();
            try
            {
                string sql = "select Distinct serverid from onlinelog where gameid=@GameId and userid=@UserId";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@GameId", GameId),
                    new SqlParameter("@UserId",UserId)
                };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        list.Add(reder["serverid"].ToString());
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
        /// 查询玩家在游戏最后一次登录的服务器
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="GameId">游戏Id</param>
        /// <returns>返回登陆记录</returns>
        public OnlineLog GetLastLogin(int UserId, int GameId)
        {
            OnlineLog ol = null;
            try
            {
                string sql = "select top 1 * from onlinelog where userid=@UserId and gameid=@GameId order by logtime desc";
                SqlParameter[] sp = new SqlParameter[]
               {
                   new SqlParameter("@UserId",UserId),
                   new SqlParameter("@GameId",GameId)
               };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        ol = new OnlineLog((int)reder["id"], (int)reder["userid"], (int)reder["gameid"], (int)reder["serverid"], (DateTime)reder["logtime"], (int)reder["state"], (int)reder["astate"]);
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
            return ol;
        }

        /// <summary>
        /// 获取玩家登录记录
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="Top">几条</param>
        /// <returns>返回玩家登录记录</returns>
        public List<OnlineLog> GetOnlineLog(int UserId, int Top)
        {
            List<OnlineLog> list = new List<OnlineLog>();
            try
            {
                string sql = "SELECT top " + Top + " * FROM onlinelog WHERE id IN (select MAX(id) from onlinelog where userid=@UserId GROUP BY serverid ) order by logtime desc";
                SqlParameter[] sp = new SqlParameter[]
               {
                   new SqlParameter("@UserId",UserId),
                   new SqlParameter("@Top",Top)
               };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        OnlineLog ol = new OnlineLog((int)reder["id"], (int)reder["userid"], (int)reder["gameid"], (int)reder["serverid"], (DateTime)reder["logtime"], (int)reder["state"], (int)reder["astate"]);
                        list.Add(ol);
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
        /// 添加一次登录日志
        /// </summary>
        /// <param name="ol">登录日志</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddOnlineLog(OnlineLog ol)
        {
            try
            {
                string sql = "insert into onlinelog(userid,gameid, serverid,logtime)values (@UserId,@GameId,@ServerId,@LogTime)";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@UserId", ol.UserId),
                    new SqlParameter("@GameId", ol.GameId),
                    new SqlParameter("@ServerId",ol.ServerId),
                    new SqlParameter("@LogTime",ol.LogTime)
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

    }

}
