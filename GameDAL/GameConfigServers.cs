using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Model;
using System.Data;
using System.Data.SqlClient;

namespace Game.DAL
{
    public class GameConfigServers
    {
        DBHelper db = new DBHelper();

        /// <summary>
        /// 获取GameId的配置参数
        /// </summary>
        /// <param name="GameId">游戏Id</param>
        /// <returns></returns>
        public GameConfig GetGameConfig(int GameId)
        {
            GameConfig gc = null;
            try
            {
                string sql = "select * from gameconfig where gameid=@GameId";
                SqlParameter[] sp = new SqlParameter[]
               {
                   new SqlParameter("@GameId",GameId)
               };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        gc = new GameConfig((int)reder["id"], (int)reder["gameid"], reder["logincom"].ToString(), reder["paycom"].ToString(),
                            reder["existcom"].ToString(), reder["loginticket"].ToString(), reder["payticket"].ToString(),
                            reder["selectticket"].ToString(), reder["fcmticket"].ToString(), reder["agentid"].ToString());
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
            return gc;
        }

        /// <summary>
        /// 更新游戏配置
        /// </summary>
        /// <param name="GameId">游戏Id</param>
        /// <returns>返回是否更新成功</returns>
        public Boolean UpdateGameConfig(GameConfig gc)
        {
            try
            {
                string sql = "update GameConfig set logincom=@LoginCom,paycom=@PayCom,existcom=@ExistCom,loginticket=@LoginTicket," +
                            "payticket=@PayTicket,selectticket=@SelectTicket,fcmticket=@FcmTicket,agentid=@AgentId"
                             + " where gameid=@GameId ";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@LoginCom",gc.LoginCom),
                    new SqlParameter("@PayCom",gc.PayCom),
                    new SqlParameter("@ExistCom",gc.ExistCom), 
                    new SqlParameter("@LoginTicket",gc.LoginTicket),
                    new SqlParameter("@PayTicket",gc.PayTicket),
                    new SqlParameter("@SelectTicket",gc.SelectTicket),
                    new SqlParameter("@FcmTicket",gc.FcmTicket),
                    new SqlParameter("@AgentId",gc.AgentId),
                    new SqlParameter("@GameId",gc.GameId)
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
        /// 添加游戏配置
        /// </summary>
        /// <param name="GameId">游戏Id</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddGameConfig(GameConfig gc)
        {
            try
            {
                string sql = "insert into GameConfig (logincom,paycom,existcom,loginticket,payticket,selectticket,fcmticket,agentid,gameid) values(@LoginCom,@PayCom,@ExistCom,@LoginTicket,@PayTicket,@SelectTicket,@FcmTicket,@AgentId,@GameId)";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@LoginCom",gc.LoginCom),
                    new SqlParameter("@PayCom",gc.PayCom),
                    new SqlParameter("@ExistCom",gc.ExistCom), 
                    new SqlParameter("@LoginTicket",gc.LoginTicket),
                    new SqlParameter("@PayTicket",gc.PayTicket),
                    new SqlParameter("@SelectTicket",gc.SelectTicket),
                    new SqlParameter("@FcmTicket",gc.FcmTicket),
                    new SqlParameter("@AgentId",gc.AgentId),
                    new SqlParameter("@GameId",gc.GameId)
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
