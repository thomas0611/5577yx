using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Model;
using System.Data.SqlClient;
using System.Data;

namespace Game.DAL
{
    public class GameServerServers
    {
        DBHelper db = new DBHelper();

        /// <summary>
        /// 获取服务器信息
        /// </summary>
        /// <param name="ServerId">服务器Id</param>
        /// <returns>返回服务器</returns>
        public GameServer GetGameServer(int ServerId)
        {
            GameServer gs = null;
            try
            {
                string sql = "select * from gameserver where id =@ServerId";
                SqlParameter[] sp = new SqlParameter[]
               {
                   new SqlParameter("@ServerId",ServerId)
               };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        gs = new GameServer((int)reder["id"], (int)reder["gameid"], reder["no"].ToString(), reder["qufu"].ToString(), reder["name"].ToString(), reder["img"].ToString(), reder["serverdesc"].ToString(), reder["line"].ToString(), (int)reder["state"], (DateTime)reder["starttime"], (int)reder["sort_id"]);
                    }
                }
                return gs;
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
        /// 获取GameId下的所有服务器
        /// </summary>
        /// <param name="GameId">游戏Id</param>
        /// <returns>返回服务器集合</returns>
        public List<GameServer> GetServersByGame(int GameId)
        {
            List<GameServer> list = new List<GameServer>();
            try
            {
                string sql = "select * from gameserver where gameid =@GameId order by sort_id,starttime desc ";
                SqlParameter[] sp = new SqlParameter[]
               {
                   new SqlParameter("@GameId",GameId)
               };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        GameServer gs = new GameServer((int)reder["id"], (int)reder["gameid"], reder["no"].ToString(), reder["qufu"].ToString(), reder["name"].ToString(), reder["img"].ToString(), reder["serverdesc"].ToString(), reder["line"].ToString(), (int)reder["state"], (DateTime)reder["starttime"], (int)reder["sort_id"]);
                        list.Add(gs);
                    }
                }
                return list;
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
        /// 获取GameId下的所有服务器
        /// </summary>
        /// <param name="GameId">游戏Id</param>
        /// <param name="Top">前几条数据</param>
        /// <returns>返回服务器集合</returns>
        public List<GameServer> GetServersByGame(int GameId, int Top)
        {
            List<GameServer> list = new List<GameServer>();
            try
            {
                string sql = "select top " + Top + " * from gameserver where gameid =@GameId order by sort_id,starttime desc ";
                SqlParameter[] sp = new SqlParameter[]
               {
                   new SqlParameter("@GameId",GameId)
               };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        GameServer gs = new GameServer((int)reder["id"], (int)reder["gameid"], reder["no"].ToString(), reder["qufu"].ToString(), reder["name"].ToString(), reder["img"].ToString(), reder["serverdesc"].ToString(), reder["line"].ToString(), (int)reder["state"], (DateTime)reder["starttime"], (int)reder["sort_id"]);
                        list.Add(gs);
                    }
                }
                return list;
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
        /// 获取服务器信息
        /// </summary>
        /// <param name="GameId">游戏Id</param>
        /// <param name="Qf">区服</param>
        /// <returns>返回服务器</returns>
        public GameServer GetGameServer(int GameId, int Qf)
        {
            GameServer gs = null;
            try
            {
                string sql = "select * from gameserver where gameid = @GameId and qufu = @Qf";
                SqlParameter[] sp = new SqlParameter[]
               {
                   new SqlParameter("@GameId",GameId),
                   new SqlParameter("@Qf",Qf)
               };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        gs = new GameServer((int)reder["id"], (int)reder["gameid"], reder["no"].ToString(), reder["qufu"].ToString(), reder["name"].ToString(), reder["img"].ToString(), reder["serverdesc"].ToString(), reder["line"].ToString(), (int)reder["state"], (DateTime)reder["starttime"], (int)reder["sort_id"]);
                    }
                }
                return gs;
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
        /// 获取服务器数据总条数
        /// </summary>
        /// <param name="WhereStr">条件</param>
        /// <returns>返回条数</returns>
        public Double GetServerCount(string WhereStr)
        {
            try
            {
                string sql = "select count(*) from vw_Servers " + WhereStr;
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
        /// 获取服务器分页数据
        /// </summary>
        /// <param name="PageSize">页大小</param>
        /// <param name="PageNum">页码</param>
        /// <param name="WhereStr">条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns>返回服务器集合</returns>
        public DataTable GetAllServer(int PageSize, int PageNum, string WhereStr, string OrderBy)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] sp = new SqlParameter[] { 
                new SqlParameter("@PageSize",PageSize),
                new SqlParameter("@PageNum",PageNum),
                new SqlParameter("@TableName","vw_Servers"),
                new SqlParameter("@WhereStr",WhereStr),
                new SqlParameter("@OrderBy",OrderBy)
                };
                using (dt = db.GetTableByProc("Proc_Page", sp))
                {
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
            return dt;
        }

        /// <summary>
        /// 获取汇总分页数据
        /// </summary>
        /// <param name="PageSize">页大小</param>
        /// <param name="PageNum">页码</param>
        /// <param name="WhereStr">条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns>返回汇总数据集合</returns>
        public DataTable GetAllCollect(int PageSize, int PageNum, string WhereStr, string OrderBy)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlParameter[] sp = new SqlParameter[] { 
                new SqlParameter("@PageSize",PageSize),
                new SqlParameter("@PageNum",PageNum),
                new SqlParameter("@TableName","vw_Collect"),
                new SqlParameter("@WhereStr",WhereStr),
                new SqlParameter("@OrderBy",OrderBy)
                };
                using (dt = db.GetTableByProc("Proc_Page", sp))
                {
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
            return dt;
        }

        /// <summary>
        /// 更新服务器信息
        /// </summary>
        /// <param name="gs">服务器</param>
        /// <returns>返回是否更新成功</returns>
        public Boolean UpdateServer(GameServer gs)
        {
            try
            {
                string sql = "update gameserver set gameid=@GameId,no=@ServerNo,qufu=@QuFu,name=@Name," +
               "img=@Img,serverdesc=@ServerDesc,line=@Line,state=@State,sort_id=@Sort_Id,starttime=@StartTime" +
                 " where id=@Id ";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@GameId",gs.GameId),
                    new SqlParameter("@ServerNo",gs.ServerNo),
                    new SqlParameter("@QuFu",gs.QuFu), 
                    new SqlParameter("@Name",gs.Name),
                    new SqlParameter("@Img",string.IsNullOrEmpty( gs.Img)?"":gs.Img),
                    new SqlParameter("@ServerDesc",string.IsNullOrEmpty(gs.ServerDesc)?"":gs.ServerDesc),
                    new SqlParameter("@Line",gs.Line),
                    new SqlParameter("@State",gs.State),
                    new SqlParameter("@Sort_Id",gs.Sort_Id),
                    new SqlParameter("@StartTime",gs.StartTime),
                    new SqlParameter("@Id",gs.Id)
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
        /// 添加服务器信息
        /// </summary>
        /// <param name="gs">服务器</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddServer(GameServer gs)
        {
            try
            {
                string sql = "insert into gameserver(gameid,no,qufu,name,img,serverdesc,line,state,sort_id,starttime)"
                        + "values (@GameId,@ServerNo,@QuFu,@Name,@Img,@ServerDesc,@Line,@State,@Sort_Id,@StartTime)";
                SqlParameter[] sp = new SqlParameter[] 
                {
                     new SqlParameter("@GameId",gs.GameId),
                    new SqlParameter("@ServerNo",gs.ServerNo),
                    new SqlParameter("@QuFu",gs.QuFu), 
                    new SqlParameter("@Name",gs.Name),
                    new SqlParameter("@Img",string.IsNullOrEmpty( gs.Img)?"":gs.Img),
                    new SqlParameter("@ServerDesc",string.IsNullOrEmpty(gs.ServerDesc)?"":gs.ServerDesc),
                    new SqlParameter("@Line",gs.Line),
                    new SqlParameter("@State",gs.State),
                    new SqlParameter("@Sort_Id",gs.Sort_Id),
                    new SqlParameter("@StartTime",gs.StartTime)
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
        /// 删除服务器
        /// </summary>
        /// <param name="ServerId">服务器Id</param>
        /// <returns>返回是否删除成功</returns>
        public Boolean DelServer(int ServerId)
        {
            try
            {
                string sql = "delete from gameserver where id=@Id";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@Id",ServerId)
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
        /// 获取最新开服
        /// </summary>
        /// <param name="Top">前几条</param>
        /// <returns>返回最新开服信息</returns>
        public List<GameServer> GetNewsServer(int Top)
        {
            List<GameServer> list = new List<GameServer>();
            try
            {
                string sql = "select top " + Top + " * from gameserver order by starttime desc";
                using (SqlDataReader reder = db.GetReader(sql))
                {
                    while (reder.Read())
                    {
                        GameServer gs = new GameServer((int)reder["id"], (int)reder["gameid"], reder["no"].ToString(), reder["qufu"].ToString(), reder["name"].ToString(), reder["img"].ToString(), reder["serverdesc"].ToString(), reder["line"].ToString(), (int)reder["state"], (DateTime)reder["starttime"], (int)reder["sort_id"]);
                        list.Add(gs);
                    }
                }
                return list;
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
