using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Game.Model;

namespace Game.DAL
{
    public class GamesServers
    {
        DBHelper db = new DBHelper();
        CommonServer cs = new CommonServer();

        /// <summary>
        /// 根据游戏编号获取游戏
        /// </summary>
        /// <param name="GameNo">游戏编号</param>
        /// <returns>返回游戏</returns>
        public Games GetGame(string GameNo)
        {
            Games g = new Games();
            try
            {
                string sql = "select * from games where gameno=@GameNo";
                SqlParameter[] sp = new SqlParameter[]
               {
                   new SqlParameter("@GameNo",GameNo)
               };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        g.Id = (int)reder["id"];
                        g.Name = reder["name"].ToString();
                        g.GameNo = reder["gameno"].ToString();
                        g.GameListImg = reder["gamelistimg"].ToString();
                        g.IndexTjImg = reder["indextjimg"].ToString();
                        g.IndexHbImg = reder["indexhbimg"].ToString();
                        g.IndexHdImg = reder["indexhdimg"].ToString();
                        g.HdImg = reder["hdimg"].ToString();
                        g.GameDesc = reder["gamedesc"].ToString();
                        g.GameCom = reder["gamecom"].ToString();
                        g.GameBBS = reder["gamebbs"].ToString();
                        g.NewHand = reder["newhand"].ToString();
                        g.Is_Top = int.Parse(reder["is_top"].ToString());
                        g.Is_Red = int.Parse(reder["is_red"].ToString());
                        g.Is_Hot = int.Parse(reder["is_hot"].ToString());
                        g.Is_Slide = int.Parse(reder["is_slide"].ToString());
                        g.Is_Lock = int.Parse(reder["is_lock"].ToString());
                        g.Sort_Id = int.Parse(reder["sort_id"].ToString());
                        g.AddTime = (DateTime)reder["addtime"];
                        g.GameMoneyScale = int.Parse(reder["gamemoneyscale"].ToString());
                        g.GameMoneyName = reder["gamemoneyname"].ToString();
                        g.IsRole = int.Parse(reder["isrole"].ToString());
                        g.Pic1 = reder["pic1"].ToString();
                        g.Pic2 = reder["pic2"].ToString();
                        g.Pic3 = reder["pic3"].ToString();
                        g.Pic4 = reder["pic4"].ToString();
                        g.P1 = int.Parse(reder["p1"].ToString());
                        g.P2 = int.Parse(reder["p2"].ToString());
                        g.GameProperty = reder["gameproperty"].ToString();
                        g.tjqf = int.Parse(reder["tjqf"].ToString());
                        g.game_url_g = reder["game_url_g"].ToString();
                        g.game_url_hd = reder["game_url_hd"].ToString();
                        g.game_url_xzq = reder["game_url_xzq"].ToString();
                    }
                }
                return g;
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
        /// 根据游戏Id获取游戏
        /// </summary>
        /// <param name="GameId">游戏Id</param>
        /// <returns>返回游戏</returns>
        public Games GetGame(int GameId)
        {
            Games g = new Games();
            try
            {
                string sql = "select * from games where id=@GameId";
                SqlParameter[] sp = new SqlParameter[]
               {
                   new SqlParameter("@GameId",GameId)
               };
                using (SqlDataReader reder = db.GetReader(sql, sp))
                {
                    while (reder.Read())
                    {
                        g.Id = (int)reder["id"];
                        g.Name = reder["name"].ToString();
                        g.GameNo = reder["gameno"].ToString();
                        g.GameListImg = reder["gamelistimg"].ToString();
                        g.IndexTjImg = reder["indextjimg"].ToString();
                        g.IndexHbImg = reder["indexhbimg"].ToString();
                        g.IndexHdImg = reder["indexhdimg"].ToString();
                        g.HdImg = reder["hdimg"].ToString();
                        g.GameDesc = reder["gamedesc"].ToString();
                        g.GameCom = reder["gamecom"].ToString();
                        g.GameBBS = reder["gamebbs"].ToString();
                        g.NewHand = reder["newhand"].ToString();
                        g.Is_Top = int.Parse(reder["is_top"].ToString());
                        g.Is_Red = int.Parse(reder["is_red"].ToString());
                        g.Is_Hot = int.Parse(reder["is_hot"].ToString());
                        g.Is_Slide = int.Parse(reder["is_slide"].ToString());
                        g.Is_Lock = int.Parse(reder["is_lock"].ToString());
                        g.Sort_Id = int.Parse(reder["sort_id"].ToString());
                        g.AddTime = (DateTime)reder["addtime"];
                        g.GameMoneyScale = int.Parse(reder["gamemoneyscale"].ToString());
                        g.GameMoneyName = reder["gamemoneyname"].ToString();
                        g.IsRole = int.Parse(reder["isrole"].ToString());
                        g.Pic1 = reder["pic1"].ToString();
                        g.Pic2 = reder["pic2"].ToString();
                        g.Pic3 = reder["pic3"].ToString();
                        g.Pic4 = reder["pic4"].ToString();
                        g.P1 = int.Parse(reder["p1"].ToString());
                        g.P2 = int.Parse(reder["p2"].ToString());
                        g.GameProperty = reder["gameproperty"].ToString();
                        g.tjqf = int.Parse(reder["tjqf"].ToString());
                        g.game_url_g = reder["game_url_g"].ToString();
                        g.game_url_hd = reder["game_url_hd"].ToString();
                        g.game_url_xzq = reder["game_url_xzq"].ToString();
                    }
                }
                return g;
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
        /// 获取所有游戏
        /// </summary>
        /// <returns>游戏集合</returns>
        public List<Games> GetAll()
        {
            List<Games> list = new List<Games>();
            try
            {
                string sql = "select * from games";
                using (SqlDataReader reder = db.GetReader(sql))
                {
                    while (reder.Read())
                    {
                        Games g = new Games();
                        g.Id = (int)reder["id"];
                        g.Name = reder["name"].ToString();
                        g.GameNo = reder["gameno"].ToString();
                        g.GameListImg = reder["gamelistimg"].ToString();
                        g.IndexTjImg = reder["indextjimg"].ToString();
                        g.IndexHbImg = reder["indexhbimg"].ToString();
                        g.IndexHdImg = reder["indexhdimg"].ToString();
                        g.HdImg = reder["hdimg"].ToString();
                        g.GameDesc = reder["gamedesc"].ToString();
                        g.GameCom = reder["gamecom"].ToString();
                        g.GameBBS = reder["gamebbs"].ToString();
                        g.NewHand = reder["newhand"].ToString();
                        g.Is_Top = int.Parse(reder["is_top"].ToString());
                        g.Is_Red = int.Parse(reder["is_red"].ToString());
                        g.Is_Hot = int.Parse(reder["is_hot"].ToString());
                        g.Is_Slide = int.Parse(reder["is_slide"].ToString());
                        g.Is_Lock = int.Parse(reder["is_lock"].ToString());
                        g.Sort_Id = int.Parse(reder["sort_id"].ToString());
                        g.AddTime = (DateTime)reder["addtime"];
                        g.GameMoneyScale = int.Parse(reder["gamemoneyscale"].ToString());
                        g.GameMoneyName = reder["gamemoneyname"].ToString();
                        g.IsRole = int.Parse(reder["isrole"].ToString());
                        g.Pic1 = reder["pic1"].ToString();
                        g.Pic2 = reder["pic2"].ToString();
                        g.Pic3 = reder["pic3"].ToString();
                        g.Pic4 = reder["pic4"].ToString();
                        g.P1 = int.Parse(reder["p1"].ToString());
                        g.P2 = int.Parse(reder["p2"].ToString());
                        g.GameProperty = reder["gameproperty"].ToString();
                        g.tjqf = int.Parse(reder["tjqf"].ToString());
                        g.game_url_g = reder["game_url_g"].ToString();
                        g.game_url_hd = reder["game_url_hd"].ToString();
                        g.game_url_xzq = reder["game_url_xzq"].ToString();
                        list.Add(g);
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
        /// 获取所有游戏
        /// </summary>
        /// <param name="WhereStr">条件</param>
        /// <returns>游戏集合</returns>
        public List<Games> GetAll(string WhereStr)
        {
            List<Games> list = new List<Games>();
            try
            {
                string sql = "select * from games " + WhereStr;
                using (SqlDataReader reder = db.GetReader(sql))
                {
                    while (reder.Read())
                    {
                        Games g = new Games();
                        g.Id = (int)reder["id"];
                        g.Name = reder["name"].ToString();
                        g.GameNo = reder["gameno"].ToString();
                        g.GameListImg = reder["gamelistimg"].ToString();
                        g.IndexTjImg = reder["indextjimg"].ToString();
                        g.IndexHbImg = reder["indexhbimg"].ToString();
                        g.IndexHdImg = reder["indexhdimg"].ToString();
                        g.HdImg = reder["hdimg"].ToString();
                        g.GameDesc = reder["gamedesc"].ToString();
                        g.GameCom = reder["gamecom"].ToString();
                        g.GameBBS = reder["gamebbs"].ToString();
                        g.NewHand = reder["newhand"].ToString();
                        g.Is_Top = int.Parse(reder["is_top"].ToString());
                        g.Is_Red = int.Parse(reder["is_red"].ToString());
                        g.Is_Hot = int.Parse(reder["is_hot"].ToString());
                        g.Is_Slide = int.Parse(reder["is_slide"].ToString());
                        g.Is_Lock = int.Parse(reder["is_lock"].ToString());
                        g.Sort_Id = int.Parse(reder["sort_id"].ToString());
                        g.AddTime = (DateTime)reder["addtime"];
                        g.GameMoneyScale = int.Parse(reder["gamemoneyscale"].ToString());
                        g.GameMoneyName = reder["gamemoneyname"].ToString();
                        g.IsRole = int.Parse(reder["isrole"].ToString());
                        g.Pic1 = reder["pic1"].ToString();
                        g.Pic2 = reder["pic2"].ToString();
                        g.Pic3 = reder["pic3"].ToString();
                        g.Pic4 = reder["pic4"].ToString();
                        g.P1 = int.Parse(reder["p1"].ToString());
                        g.P2 = int.Parse(reder["p2"].ToString());
                        g.GameProperty = reder["gameproperty"].ToString();
                        g.tjqf = int.Parse(reder["tjqf"].ToString());
                        g.game_url_g = reder["game_url_g"].ToString();
                        g.game_url_hd = reder["game_url_hd"].ToString();
                        g.game_url_xzq = reder["game_url_xzq"].ToString();
                        list.Add(g);
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
        /// 获取游戏总条数
        /// </summary>
        /// <param name="WhereStr">条件</param>
        /// <returns>游戏数</returns>
        public Double GetGamesCount(string WhereStr)
        {
            return cs.GetDataCount(WhereStr, "games");
        }

        /// <summary>
        /// 获取游戏分页数据
        /// </summary>
        /// <param name="PageSize">页大小</param>
        /// <param name="PageNum">页码</param>
        /// <param name="WhereStr">条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns>返回游戏数据集合</returns>
        public List<Games> GetAll(int PageSize, int PageNum, string WhereStr, string OrderBy)
        {
            List<Games> list = new List<Games>();
            try
            {
                SqlParameter[] sp = new SqlParameter[] { 
                new SqlParameter("@PageSize",PageSize),
                new SqlParameter("@PageNum",PageNum),
                new SqlParameter("@TableName","games"),
                new SqlParameter("@WhereStr",WhereStr),
                new SqlParameter("@OrderBy",OrderBy)
                };
                using (SqlDataReader reder = db.GetReaderByProc("Proc_Page", sp))
                {
                    while (reder.Read())
                    {
                        Games g = new Games();
                        g.Id = (int)reder["id"];
                        g.Name = reder["name"].ToString();
                        g.GameNo = reder["gameno"].ToString();
                        g.GameListImg = reder["gamelistimg"].ToString();
                        g.IndexTjImg = reder["indextjimg"].ToString();
                        g.IndexHbImg = reder["indexhbimg"].ToString();
                        g.IndexHdImg = reder["indexhdimg"].ToString();
                        g.HdImg = reder["hdimg"].ToString();
                        g.GameDesc = reder["gamedesc"].ToString();
                        g.GameCom = reder["gamecom"].ToString();
                        g.GameBBS = reder["gamebbs"].ToString();
                        g.NewHand = reder["newhand"].ToString();
                        g.Is_Top = int.Parse(reder["is_top"].ToString());
                        g.Is_Red = int.Parse(reder["is_red"].ToString());
                        g.Is_Hot = int.Parse(reder["is_hot"].ToString());
                        g.Is_Slide = int.Parse(reder["is_slide"].ToString());
                        g.Is_Lock = int.Parse(reder["is_lock"].ToString());
                        g.Sort_Id = int.Parse(reder["sort_id"].ToString());
                        g.AddTime = (DateTime)reder["addtime"];
                        g.GameMoneyScale = int.Parse(reder["gamemoneyscale"].ToString());
                        g.GameMoneyName = reder["gamemoneyname"].ToString();
                        g.IsRole = int.Parse(reder["isrole"].ToString());
                        g.Pic1 = reder["pic1"].ToString();
                        g.Pic2 = reder["pic2"].ToString();
                        g.Pic3 = reder["pic3"].ToString();
                        g.Pic4 = reder["pic4"].ToString();
                        g.P1 = int.Parse(reder["p1"].ToString());
                        g.P2 = int.Parse(reder["p2"].ToString());
                        g.GameProperty = reder["gameproperty"].ToString();
                        g.tjqf = int.Parse(reder["tjqf"].ToString());
                        g.game_url_g = reder["game_url_g"].ToString();
                        g.game_url_hd = reder["game_url_hd"].ToString();
                        g.game_url_xzq = reder["game_url_xzq"].ToString();
                        list.Add(g);
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
        /// 更新游戏信息
        /// </summary>
        /// <param name="g">游戏</param>
        /// <returns>返回是否更新成功</returns>
        public Boolean UpdateGame(Games g)
        {
            try
            {
                string sql = "update games set name=@Name,gameno=@GameNo,gamelistimg=@GameListImg,indextjimg=@IndexTjImg," +
               "indexhbimg=@IndexHbImg,indexhdimg=@IndexHdImg,hdimg=@HdImg,gamedesc=@GameDesc,gamecom=@GameCom,gamebbs=@GameBBS," +
                "newhand=@NewHand,is_top=@Is_Top,is_red=@Is_Red,is_hot=@Is_Hot,is_slide=@Is_Slide,is_lock=@Is_Lock,sort_id=@Sort_Id"
                 + ",gamemoneyscale=@GameMoneyScale,gamemoneyname=@GameMoneyName,isrole=@IsRole,pic1=@Pic1,pic2=@Pic2,pic3=@Pic3," +
                "pic4=@Pic4,gameproperty=@GameProperty,tjqf=@tjqf,game_url_g=@game_url_g,game_url_hd=@game_url_hd,"
                + "game_url_xzq=@game_url_xzq"
                + " where id=@Id ";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@Name",g.Name),
                    new SqlParameter("@GameNo",g.GameNo),
                    new SqlParameter("@GameListImg",string.IsNullOrEmpty(g.GameListImg)?"":g.GameListImg), 
                    new SqlParameter("@IndexTjImg",string.IsNullOrEmpty(g.IndexTjImg)?"":g.IndexTjImg),
                    new SqlParameter("@IndexHbImg",string.IsNullOrEmpty(g.IndexHbImg)?"":g.IndexHbImg),
                    new SqlParameter("@IndexHdImg",string.IsNullOrEmpty(g.IndexHdImg)?"":g.IndexHdImg),
                    new SqlParameter("@HdImg",string.IsNullOrEmpty(g.HdImg)?"":g.HdImg),
                    new SqlParameter("@GameDesc",string.IsNullOrEmpty(g.GameDesc)?"":g.GameDesc),
                    new SqlParameter("@GameCom",string.IsNullOrEmpty(g.GameCom)?"":g.GameCom),
                    new SqlParameter("@GameBBS",string.IsNullOrEmpty(g.GameBBS)?"":g.GameBBS),
                    new SqlParameter("@NewHand",string.IsNullOrEmpty(g.NewHand)?"":g.NewHand), 
                    new SqlParameter("@Is_Top", g.Is_Top),
                    new SqlParameter("@Is_Red",g.Is_Red),
                    new SqlParameter("@Is_Hot",g.Is_Hot),
                    new SqlParameter("@Is_Slide",g.Is_Slide),
                    new SqlParameter("@Is_Lock",g.Is_Lock),
                    new SqlParameter("@Sort_Id",g.Sort_Id),
                    new SqlParameter("@GameMoneyScale",g.GameMoneyScale), 
                    new SqlParameter("@GameMoneyName",string.IsNullOrEmpty(g.GameMoneyName)?"":g.GameMoneyName),
                    new SqlParameter("@IsRole",g.IsRole),
                    new SqlParameter("@Pic1",string.IsNullOrEmpty(g.Pic1)?"":g.Pic1),
                    new SqlParameter("@Pic2",string.IsNullOrEmpty(g.Pic2)?"":g.Pic2),
                    new SqlParameter("@Pic3",string.IsNullOrEmpty(g.Pic3)?"":g.Pic3),
                    new SqlParameter("@Pic4",string.IsNullOrEmpty(g.Pic4)?"":g.Pic4),
                    new SqlParameter("@GameProperty",string.IsNullOrEmpty(g.GameProperty)?"":g.GameProperty),
                    new SqlParameter("@tjqf",g.tjqf),
                    new SqlParameter("@game_url_g",string.IsNullOrEmpty(g.game_url_g)?"":g.game_url_g),
                    new SqlParameter("@game_url_hd",string.IsNullOrEmpty(g.game_url_hd)?"":g.game_url_hd),
                    new SqlParameter("@game_url_xzq",string.IsNullOrEmpty(g.game_url_xzq)?"":g.game_url_xzq),
                    new SqlParameter("@Id",g.Id)
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
        /// 添加游戏信息
        /// </summary>
        /// <param name="g">游戏</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddGame(Games g)
        {
            try
            {
                string sql = "insert into Games(name,gameno,gamelistimg,indextjimg,indexhbimg,indexhdimg,hdimg,gamedesc," +
               "gamecom,gamebbs,newhand,is_top,is_red,is_hot,is_slide,is_lock,sort_id,gamemoneyscale,gamemoneyname,isrole,pic1," +
               "pic2,pic3,pic4,gameproperty,tjqf,game_url_g,game_url_hd,game_url_xzq)"
               + "values (@Name,@GameNo,@GameListImg,@IndexTjImg,@IndexHbImg,@IndexHdImg,@HdImg,@GameDesc,@GameCom,@GameBBS," +
                "@NewHand,@Is_Top,@Is_Red,@Is_Hot,@Is_Slide,@Is_Lock,@Sort_Id,@GameMoneyScale,@GameMoneyName,@IsRole,@Pic1,@Pic2," +
                "@Pic3,@Pic4,@GameProperty,@tjqf,@game_url_g,@game_url_hd,@game_url_xzq)";
                SqlParameter[] sp = new SqlParameter[] 
                {
                     new SqlParameter("@Name",g.Name),
                    new SqlParameter("@GameNo",g.GameNo),
                    new SqlParameter("@GameListImg",string.IsNullOrEmpty(g.GameListImg)?"":g.GameListImg), 
                    new SqlParameter("@IndexTjImg",string.IsNullOrEmpty(g.IndexTjImg)?"":g.IndexTjImg),
                    new SqlParameter("@IndexHbImg",string.IsNullOrEmpty(g.IndexHbImg)?"":g.IndexHbImg),
                    new SqlParameter("@IndexHdImg",string.IsNullOrEmpty(g.IndexHdImg)?"":g.IndexHdImg),
                    new SqlParameter("@HdImg",string.IsNullOrEmpty(g.HdImg)?"":g.HdImg),
                    new SqlParameter("@GameDesc",string.IsNullOrEmpty(g.GameDesc)?"":g.GameDesc),
                    new SqlParameter("@GameCom",string.IsNullOrEmpty(g.GameCom)?"":g.GameCom),
                    new SqlParameter("@GameBBS",string.IsNullOrEmpty(g.GameBBS)?"":g.GameBBS),
                    new SqlParameter("@NewHand",string.IsNullOrEmpty(g.NewHand)?"":g.NewHand), 
                    new SqlParameter("@Is_Top", g.Is_Top),
                    new SqlParameter("@Is_Red",g.Is_Red),
                    new SqlParameter("@Is_Hot",g.Is_Hot),
                    new SqlParameter("@Is_Slide",g.Is_Slide),
                    new SqlParameter("@Is_Lock",g.Is_Lock),
                    new SqlParameter("@Sort_Id",g.Sort_Id),
                    new SqlParameter("@GameMoneyScale",g.GameMoneyScale), 
                    new SqlParameter("@GameMoneyName",string.IsNullOrEmpty(g.GameMoneyName)?"":g.GameMoneyName),
                    new SqlParameter("@IsRole",g.IsRole),
                    new SqlParameter("@Pic1",string.IsNullOrEmpty(g.Pic1)?"":g.Pic1),
                    new SqlParameter("@Pic2",string.IsNullOrEmpty(g.Pic2)?"":g.Pic2),
                    new SqlParameter("@Pic3",string.IsNullOrEmpty(g.Pic3)?"":g.Pic3),
                    new SqlParameter("@Pic4",string.IsNullOrEmpty(g.Pic4)?"":g.Pic4),
                    new SqlParameter("@GameProperty",string.IsNullOrEmpty(g.GameProperty)?"":g.GameProperty),
                    new SqlParameter("@tjqf",g.tjqf),
                    new SqlParameter("@game_url_g",string.IsNullOrEmpty(g.game_url_g)?"":g.game_url_g),
                    new SqlParameter("@game_url_hd",string.IsNullOrEmpty(g.game_url_hd)?"":g.game_url_hd),
                    new SqlParameter("@game_url_xzq",string.IsNullOrEmpty(g.game_url_xzq)?"":g.game_url_xzq),
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
        /// 删除游戏
        /// </summary>
        /// <param name="GameId">游戏Id</param>
        /// <returns>返回是否删除成功</returns>
        public Boolean DelGame(int GameId)
        {
            try
            {
                string sql = "delete from games where id=@Id";
                SqlParameter[] sp = new SqlParameter[] 
                {
                    new SqlParameter("@Id",GameId)
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
        /// 获取游戏
        /// </summary>
        /// <param name="Top">前几条</param>
        /// <param name="Type">类型</param>
        /// <returns>返回游戏集合</returns>
        public List<Games> GetAll(int Top, string Type)
        {
            List<Games> list = new List<Games>();
            try
            {
                string sql = "select top " + Top + " * from games where " + Type + "=1 order by addtime desc";
                using (SqlDataReader reder = db.GetReader(sql))
                {
                    while (reder.Read())
                    {
                        Games g = new Games();
                        g.Id = (int)reder["id"];
                        g.Name = reder["name"].ToString();
                        g.GameNo = reder["gameno"].ToString();
                        g.GameListImg = reder["gamelistimg"].ToString();
                        g.IndexTjImg = reder["indextjimg"].ToString();
                        g.IndexHbImg = reder["indexhbimg"].ToString();
                        g.IndexHdImg = reder["indexhdimg"].ToString();
                        g.HdImg = reder["hdimg"].ToString();
                        g.GameDesc = reder["gamedesc"].ToString();
                        g.GameCom = reder["gamecom"].ToString();
                        g.GameBBS = reder["gamebbs"].ToString();
                        g.NewHand = reder["newhand"].ToString();
                        g.Is_Top = int.Parse(reder["is_top"].ToString());
                        g.Is_Red = int.Parse(reder["is_red"].ToString());
                        g.Is_Hot = int.Parse(reder["is_hot"].ToString());
                        g.Is_Slide = int.Parse(reder["is_slide"].ToString());
                        g.Is_Lock = int.Parse(reder["is_lock"].ToString());
                        g.Sort_Id = int.Parse(reder["sort_id"].ToString());
                        g.AddTime = (DateTime)reder["addtime"];
                        g.GameMoneyScale = int.Parse(reder["gamemoneyscale"].ToString());
                        g.GameMoneyName = reder["gamemoneyname"].ToString();
                        g.IsRole = int.Parse(reder["isrole"].ToString());
                        g.Pic1 = reder["pic1"].ToString();
                        g.Pic2 = reder["pic2"].ToString();
                        g.Pic3 = reder["pic3"].ToString();
                        g.Pic4 = reder["pic4"].ToString();
                        g.P1 = int.Parse(reder["p1"].ToString());
                        g.P2 = int.Parse(reder["p2"].ToString());
                        g.GameProperty = reder["gameproperty"].ToString();
                        g.tjqf = int.Parse(reder["tjqf"].ToString());
                        g.game_url_g = reder["game_url_g"].ToString();
                        g.game_url_hd = reder["game_url_hd"].ToString();
                        g.game_url_xzq = reder["game_url_xzq"].ToString();
                        list.Add(g);
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
