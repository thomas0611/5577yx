using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.DAL;
using Game.Model;
using System.Data;

namespace Game.Manager
{
    public class GameUserManager
    {
        GameUserServers gus = new GameUserServers();

        /// <summary>
        /// 获取用户是否存在
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <returns>返回是否存在</returns>
        public Boolean IsGameUser(string UserName)
        {
            return gus.IsGameUser(UserName);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <returns>返回用户</returns>
        public GameUser GetGameUser(int UserId)
        {
            return gus.GetGameUser(UserId);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <returns>返回用户</returns>
        public GameUser GetGameUser(string UserName)
        {
            return gus.GetGameUser(UserName);
        }

        /// <summary>
        /// 跟新玩家最后一次登录时间
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <returns>是否更新成功</returns>
        public Boolean UpdateLastLogin(int UserId)
        {
            return gus.UpdateLastLogin(UserId);
        }

        /// <summary>
        /// 更新玩家平台币
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="Money">平台币数量</param>
        /// <returns>返回跟新是否成功</returns>
        public Boolean UpdateUserMoney(string UserName, float Money, string IsAdd)
        {
            return gus.UpdateUserMoney(UserName, Money, IsAdd);
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
            return gus.UpdateUserFlMoney(UserName, Money, IsAdd);
        }

        /// <summary>
        /// 更新玩家积分
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="Points">积分数量</param>
        /// <returns>返回跟新是否成功</returns>
        public Boolean UpdateUserPoints(string UserName, float Points, string IsAdd)
        {
            return gus.UpdateUserPoints(UserName, Points, IsAdd);
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
            return gus.GetUsers(GameId, From);
        }

        /// <summary>
        /// 添加一个用户
        /// </summary>
        /// <param name="gu">用户</param>
        /// <returns></returns>
        public Boolean AddUser(GameUser gu)
        {
            return gus.AddUser(gu);
        }

        /// <summary>
        /// 获取现有的推广来源
        /// </summary>
        /// <returns>推广来源集合</returns>
        public List<string> GetAllFrom()
        {
            return gus.GetAllFrom();
        }

        /// <summary>
        /// 获取用户数据条数
        /// </summary>
        /// <param name="WhereStr">条件</param>
        /// <returns>返回数据条数</returns>
        public Double GetGameUserCount(string WhereStr)
        {
            return gus.GetGameUserCount(WhereStr);
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
            return gus.GetAllGameUser(PageSize, PageNum, WhereStr, OrderBy);
        }

        /// <summary>
        /// 删除一个用户
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <returns>返回是否删除成功</returns>
        public Boolean DelGameUser(string UserName)
        {
            return gus.DelGameUser(UserName);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="PWD">密码</param>
        /// <returns>返回用户信息</returns>
        public GameUser GetGameUser(string UserName, string PWD)
        {
            return gus.GetGameUser(UserName, PWD);
        }

        /// <summary>
        /// 更新玩家信息
        /// </summary>
        /// <param name="gu">玩家信息</param>
        /// <returns>返回是否更新成功</returns>
        public Boolean UpdateUser(GameUser gu)
        {
            return gus.UpdateUser(gu);
        }

        /// <summary>
        /// 获取下级推广用户
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <returns>返回用户集合</returns>
        public List<GameUser> GetSpreadUser(int UserId)
        {
            return gus.GetSpreadUser(UserId);
        }

        /// <summary>
        /// 获取蹦蹦网推广的用户
        /// </summary>
        /// <param name="gameId"></param>
        /// <returns>返回用户的id集合</returns>
        public List<int> GetSpreadUserByBengBeng(int gameId,string fromUrl)
        {
            return gus.GetSpreadUserByBengBeng(gameId,fromUrl);
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
            return gus.GetSpreadUser(PageSize, PageNum, WhereStr, OrderBy);
        }

        /// <summary>
        /// 检测邮箱是否已经注册
        /// </summary>
        /// <param name="Eamil">邮箱</param>
        /// <returns>返回是否注册</returns>
        public Boolean ExitEmail(string Eamil)
        {
            return gus.ExitEmail(Eamil);
        }
    }
}
