using System;
using System.Data.SqlClient;
using System.Web;
using Game.Model;
using System.Collections.Generic;
using System.Data;

namespace Game.DAL
{
    public class GameUserServers
    {
        DBHelper db = new DBHelper();
        CommonServer cs = new CommonServer();

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <returns>返回用户</returns>
        public GameUser GetGameUser(int UserId)
        {
            GameUser gu = null;
            try
            {
                string sql = "select * from game_users where id=@UserId";
                SqlParameter[] sp = new SqlParameter[]
               {
                   new SqlParameter("@UserId",UserId)
               };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        gu = new GameUser((int)reder["id"], reder["username"].ToString(), reder["pwd"].ToString(), reder["nick"].ToString(),
                            reder["sex"].ToString(), reder["phone"].ToString(), reder["tel"].ToString(), reder["realname"].ToString(),
                            reder["email"].ToString(), reder["qq"].ToString(), reder["birthday"].ToString(), reder["cards"].ToString(),
                            reder["photo"].ToString(), (int)reder["source"], reder["userdesc"].ToString(), (int)reder["isvalidate"],
                            (int)reder["isvaliphone"], (int)reder["isvalicard"], (int)reder["islock"], (int)reder["isspreader"], (int)reder["gradeid"],
                            (DateTime)reder["addtime"], (DateTime)reder["lastlogintime"], float.Parse(reder["points"].ToString()),
                            float.Parse(reder["money"].ToString()), float.Parse(reder["gamemoney"].ToString()), float.Parse(reder["rebatemoney"].ToString()),
                            reder["ip"].ToString(), reder["from_url"].ToString(), (int)reder["reggame"], reder["spvalue"].ToString(),
                           reder["annalID"].ToString());
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
            return gu;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <returns>返回用户</returns>
        public GameUser GetGameUser(string UserName)
        {
            GameUser gu = null;
            try
            {
                string sql = "select * from game_users where username=@UserName";
                SqlParameter[] sp = new SqlParameter[]
               {
                   new SqlParameter("@UserName",UserName)
               };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        gu = new GameUser((int)reder["id"], reder["username"].ToString(), reder["pwd"].ToString(), reder["nick"].ToString(),
                            reder["sex"].ToString(), reder["phone"].ToString(), reder["tel"].ToString(), reder["realname"].ToString(),
                            reder["email"].ToString(), reder["qq"].ToString(), reder["birthday"].ToString(), reder["cards"].ToString(),
                            reder["photo"].ToString(), (int)reder["source"], reder["userdesc"].ToString(), (int)reder["isvalidate"],
                            (int)reder["isvaliphone"], (int)reder["isvalicard"], (int)reder["islock"], (int)reder["isspreader"], (int)reder["gradeid"],
                            (DateTime)reder["addtime"], (DateTime)reder["lastlogintime"], float.Parse(reder["points"].ToString()),
                            float.Parse(reder["money"].ToString()), float.Parse(reder["gamemoney"].ToString()), float.Parse(reder["rebatemoney"].ToString()),
                            reder["ip"].ToString(), reder["from_url"].ToString(), (int)reder["reggame"], reder["spvalue"].ToString(),
                            reder["annalID"].ToString());
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
            return gu;
        }

        /// <summary>
        /// 更新最后一次登录时间
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <returns>返回是否跟新成功</returns>
        public Boolean UpdateLastLogin(int UserId)
        {
            try
            {
                string sql = "update game_users set lastlogintime=@LastLoginTime where id=@Id";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@Id", UserId),
                    new SqlParameter("@LastLoginTime",DateTime.Now)
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
        /// 是否存在该用户
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <returns>返回是否存在</returns>
        public Boolean IsGameUser(string UserName)
        {
            try
            {
                string sql = "select COUNT(*) from game_users where username=@UserName";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@UserName", UserName)
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
        /// 更新玩家平台币
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="Money">平台币数量</param>
        /// <returns>返回跟新是否成功</returns>
        public Boolean UpdateUserMoney(string UserName, float Money, string IsAdd)
        {
            try
            {
                string sql = "update game_users set money =money " + IsAdd + " @Money where username=@UserName";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@UserName", UserName),
                    new SqlParameter("@Money",Money)
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
        /// 更新玩家返利币
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="Money"></param>
        /// <param name="IsAdd"></param>
        /// <returns></returns>
        public Boolean UpdateUserFlMoney(string UserName, float Money, string IsAdd)
        {
            try
            {
                string sql = "update game_users set rebatemoney =rebatemoney " + IsAdd + " @Money where username=@UserName";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@UserName", UserName),
                    new SqlParameter("@Money",Money)
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
        /// 更新玩家积分
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="Points">积分数量</param>
        /// <returns>返回跟新是否成功</returns>
        public Boolean UpdateUserPoints(string UserName, float Points, string IsAdd)
        {
            try
            {
                string sql = "update game_users set points=points " + IsAdd + " @Points where username=@UserName";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@UserName", UserName),
                    new SqlParameter("@Points",Points)
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
        /// 跟新玩家游戏币消费情况
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="Money">游戏币</param>
        /// <returns>返回跟新是否成功</returns>
        public Boolean UpdateGameMoney(string UserName, float Money)
        {
            try
            {
                string sql = "update game_users set gamemoney =gamemoney + @Money where username=@UserName";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@UserName", UserName),
                    new SqlParameter("@Money",Money)
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
        /// 查询某个推广来源在某个游戏下的推广用户
        /// </summary>
        /// <param name="GameId">游戏Id</param>
        /// <param name="From">推广来源</param>
        /// <param name="Server">注册区服</param>
        /// <returns>返回用户名集合</returns>
        public List<string> GetUsers(int GameId, string From)
        {
            List<string> list = new List<string>();
            try
            {
                string sql = "select username from game_users where from_url=@From and reggame=@GameId";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@GameId", GameId),
                    new SqlParameter("@From",From)
                };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        list.Add(reder["username"].ToString());
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
        /// 添加一个用户
        /// </summary>
        /// <param name="gu">用户</param>
        /// <returns></returns>
        public Boolean AddUser(GameUser gu)
        {
            try
            {
                string sql = "insert into game_users(username,pwd,nick,sex,phone,tel,realname,email,qq,birthday,cards,photo,source,userdesc,isvalidate,isvaliphone,isvalicard,islock,isspreader,gradeid,addtime,lastlogintime,points,money,gamemoney,rebatemoney,ip,from_url,reggame,spvalue,annalID)"
                           + "values (@UserName,@PWD,@Nick,@Sex,@Phone,@Tel,@RealName,@Email,@QQ,@Birthday,@Cards,@Photo,@Source,@UserDesc,@IsValiDate,@IsValiPhone,@IsValiCard,@IsLock,@IsSpreader,@GradeId,@AddTime,@LastLoginTime,@Points,@Money,@GameMoney,@RebateMoney,@IP,@From_URL,@RegGame,@Spvalue,@annalID)";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@UserName",gu.UserName),
                    new SqlParameter("@PWD",gu.PWD),
                    new SqlParameter("@Nick",gu.Nick), 
                    new SqlParameter("@Sex", gu.Sex),
                    new SqlParameter("@Phone",gu.Phone),
                    new SqlParameter("@Tel",gu.Tel),
                    new SqlParameter("@RealName", gu.RealName),
                    new SqlParameter("@Email",gu.Email),
                    new SqlParameter("@QQ",gu.QQ), 
                    new SqlParameter("@Birthday", gu.BirthDay),
                    new SqlParameter("@Cards",gu.Cards),
                    new SqlParameter("@Photo",gu.Photo),
                    new SqlParameter("@Source",gu.Source), 
                    new SqlParameter("@UserDesc", gu.UserDesc),
                    new SqlParameter("@IsValiDate",gu.IsValiDate),
                    new SqlParameter("@IsValiPhone",gu.IsValiPhone), 
                    new SqlParameter("@IsValiCard", gu.IsValiCard),
                    new SqlParameter("@IsLock",gu.IsLock),
                    new SqlParameter("@IsSpreader",gu.IsSpreader),
                    new SqlParameter("@GradeId", gu.GradeId),
                    new SqlParameter("@AddTime",gu.AddTime),
                    new SqlParameter("@LastLoginTime",gu.LastLoginTime), 
                    new SqlParameter("@Points", gu.Points),
                    new SqlParameter("@Money",gu.Money),
                    new SqlParameter("@GameMoney",gu.GameMoney),
                    new SqlParameter("@RebateMoney",gu.RebateMoney), 
                    new SqlParameter("@IP", gu.Ip), 
                    new SqlParameter("@From_URL", gu.From_Url),
                    new SqlParameter("@RegGame",gu.RegGame),
                    new SqlParameter("@Spvalue",gu.SpValue),
                    new SqlParameter("@annalID",gu.annalID)
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
        /// 获取现有的推广来源
        /// </summary>
        /// <returns>推广来源集合</returns>
        public List<string> GetAllFrom()
        {
            List<string> list = new List<string>();
            try
            {
                string sql = "select Distinct from_url from game_users";
                using (SqlDataReader reder = db.GetReader(sql))
                {
                    while (reder.Read())
                    {
                        if (string.IsNullOrEmpty(reder["from_url"].ToString()))
                        {
                            list.Add("非推广来源用户");
                        }
                        else
                        {
                            list.Add(reder["from_url"].ToString());
                        }
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
        /// 获取用户数据条数
        /// </summary>
        /// <param name="WhereStr">条件</param>
        /// <returns>返回数据条数</returns>
        public Double GetGameUserCount(string WhereStr)
        {
            return cs.GetDataCount(WhereStr, "vw_GameUser");
        }

        /// <summary>
        /// 通过分页获取用户数据
        /// </summary>
        /// <param name="PageSize">页大小</param>
        /// <param name="PageNum">页码</param>
        /// <param name="WhereStr">条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns>返回用户数据集</returns>
        public DataTable GetAllGameUser(int PageSize, int PageNum, string WhereStr, string OrderBy)
        {
            return cs.GetAllData(PageSize, PageNum, WhereStr, OrderBy, "vw_GameUser");
        }

        /// <summary>
        /// 删除一个用户
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <returns>返回是否删除成功</returns>
        public Boolean DelGameUser(string UserName)
        {
            try
            {
                string sql = "delete from game_users where username=@UserName";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@UserName",UserName)
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
        /// 获取用户信息
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="PWD">密码</param>
        /// <returns>返回用户信息</returns>
        public GameUser GetGameUser(string UserName, string PWD)
        {
            GameUser gu = null;
            try
            {
                string sql = "select * from game_users where username = @UserName and pwd=@PWD";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@UserName",UserName),
                     new SqlParameter("@PWD",PWD)
                };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        gu = new GameUser((int)reder["id"], reder["username"].ToString(), reder["pwd"].ToString(), reder["nick"].ToString(),
                            reder["sex"].ToString(), reder["phone"].ToString(), reder["tel"].ToString(), reder["realname"].ToString(),
                            reder["email"].ToString(), reder["qq"].ToString(), reder["birthday"].ToString(), reder["cards"].ToString(),
                            reder["photo"].ToString(), (int)reder["source"], reder["userdesc"].ToString(), (int)reder["isvalidate"],
                            (int)reder["isvaliphone"], (int)reder["isvalicard"], (int)reder["islock"], (int)reder["isspreader"], (int)reder["gradeid"],
                            (DateTime)reder["addtime"], (DateTime)reder["lastlogintime"], float.Parse(reder["points"].ToString()),
                            float.Parse(reder["money"].ToString()), float.Parse(reder["gamemoney"].ToString()), float.Parse(reder["rebatemoney"].ToString()),
                            reder["ip"].ToString(), reder["from_url"].ToString(), (int)reder["reggame"], reder["spvalue"].ToString(),
                            reder["annalID"].ToString());
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
            return gu;
        }

        /// <summary>
        /// 更新玩家信息
        /// </summary>
        /// <param name="gu">玩家信息</param>
        /// <returns>返回是否更新成功</returns>
        public Boolean UpdateUser(GameUser gu)
        {
            try
            {
                string sql = "Update game_users set pwd=@PWD,nick=@Nick,sex=@Sex,phone=@Phone,tel=@Tel,realname=@RealName,email=@Email,qq=@QQ,birthday=@Birthday,cards=@Cards,photo=@Photo,source=@Source,userdesc=@UserDesc,islock=@IsLock,isspreader=@IsSpreader,gradeid=@GradeId,RegGame=@RegGame where id=@Id";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@PWD",gu.PWD),
                    new SqlParameter("@Nick",gu.Nick), 
                    new SqlParameter("@Sex", gu.Sex),
                    new SqlParameter("@Phone",gu.Phone),
                    new SqlParameter("@Tel",gu.Tel),
                    new SqlParameter("@RealName", gu.RealName),
                    new SqlParameter("@Email",gu.Email),
                    new SqlParameter("@QQ",gu.QQ), 
                    new SqlParameter("@Birthday", gu.BirthDay),
                    new SqlParameter("@Cards",gu.Cards),
                    new SqlParameter("@Photo",gu.Photo),
                    new SqlParameter("@Source",gu.Source), 
                    new SqlParameter("@UserDesc", gu.UserDesc),
                    new SqlParameter("@IsLock",gu.IsLock),
                    new SqlParameter("@IsSpreader",gu.IsSpreader),
                    new SqlParameter("@GradeId", gu.GradeId),
                     new SqlParameter("@RegGame", gu.RegGame),
                    new SqlParameter("@Id",gu.Id)
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
        /// 获取蹦蹦网的推广用户id
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns></returns>
        public List<int> GetSpreadUserByBengBeng(int gameId,string fromUrl)
        {
            List<int> listId = new List<int>();
            try
            {
                string sql = "select id from game_users where from_url = @fromUrl and reggame = @gameId";
                SqlParameter[] sp = new SqlParameter[]
               {
                   new SqlParameter("@gameId",gameId),
                   new SqlParameter("@fromUrl",fromUrl)
               };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        int id = (int)reder["id"];
                        listId.Add(id);
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
            return listId;
        }

        /// <summary>
        /// 获取下级推广用户
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <returns>返回用户集合</returns>
        public List<GameUser> GetSpreadUser(int UserId)
        {
            List<GameUser> list = new List<GameUser>();
            try
            {
                string sql = "select * from game_users where source=@UserId";
                SqlParameter[] sp = new SqlParameter[]
               {
                   new SqlParameter("@UserId",UserId)
               };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        GameUser gu = new GameUser((int)reder["id"], reder["username"].ToString(), reder["pwd"].ToString(), reder["nick"].ToString(), reder["sex"].ToString(), reder["phone"].ToString(), reder["tel"].ToString(), reder["realname"].ToString(),
 reder["email"].ToString(), reder["qq"].ToString(), reder["birthday"].ToString(), reder["cards"].ToString(), reder["photo"].ToString(), (int)reder["source"], reder["userdesc"].ToString(), (int)reder["isvalidate"], (int)reder["isvaliphone"], (int)reder["isvalicard"], (int)reder["islock"], (int)reder["isspreader"], (int)reder["gradeid"], (DateTime)reder["addtime"], (DateTime)reder["lastlogintime"], float.Parse(reder["points"].ToString()), float.Parse(reder["money"].ToString()), float.Parse(reder["gamemoney"].ToString()), float.Parse(reder["rebatemoney"].ToString()), reder["ip"].ToString(), reder["from_url"].ToString(), (int)reder["reggame"], reder["spvalue"].ToString(), reder["annalID"].ToString());
                        list.Add(gu);
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
        /// 通过分页获取推广用户
        /// </summary>
        /// <param name="PageSize">页大小</param>
        /// <param name="PageNum">页码</param>
        /// <param name="WhereStr">条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns></returns>
        public List<GameUser> GetSpreadUser(int PageSize, int PageNum, string WhereStr, string OrderBy)
        {
            List<GameUser> list = new List<GameUser>();
            try
            {
                SqlParameter[] sp = new SqlParameter[] { 
                new SqlParameter("@PageSize",PageSize),
                new SqlParameter("@PageNum",PageNum),
                new SqlParameter("@TableName","game_users"),
                new SqlParameter("@WhereStr",WhereStr),
                new SqlParameter("@OrderBy",OrderBy)
                };
                using (SqlDataReader reder = db.GetReaderByProc("Proc_Page", sp))
                {
                    while (reder.Read())
                    {
                        GameUser gu = new GameUser((int)reder["id"], reder["username"].ToString(), reder["pwd"].ToString(), reder["nick"].ToString(), reder["sex"].ToString(), reder["phone"].ToString(), reder["tel"].ToString(), reder["realname"].ToString(),
 reder["email"].ToString(), reder["qq"].ToString(), reder["birthday"].ToString(), reder["cards"].ToString(), reder["photo"].ToString(), (int)reder["source"], reder["userdesc"].ToString(), (int)reder["isvalidate"], (int)reder["isvaliphone"], (int)reder["isvalicard"], (int)reder["islock"], (int)reder["isspreader"], (int)reder["gradeid"], (DateTime)reder["addtime"], (DateTime)reder["lastlogintime"], float.Parse(reder["points"].ToString()), float.Parse(reder["money"].ToString()), float.Parse(reder["gamemoney"].ToString()), float.Parse(reder["rebatemoney"].ToString()), reder["ip"].ToString(), reder["from_url"].ToString(), (int)reder["reggame"], reder["spvalue"].ToString(), reder["annalID"].ToString());
                        list.Add(gu);
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
        /// 检测邮箱是否已经注册
        /// </summary>
        /// <param name="Email">邮箱</param>
        /// <returns>返回是否注册</returns>
        public Boolean ExitEmail(string Email)
        {
            try
            {
                string sql = "select COUNT(*) from game_users where Email=@Email";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@Email", Email)
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
