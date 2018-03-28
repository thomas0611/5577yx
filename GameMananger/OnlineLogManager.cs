using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.DAL;
using Game.Model;

namespace Game.Manager
{
    public class OnlineLogManager
    {
        OnlineLogServers ols = new OnlineLogServers();

        /// <summary>
        /// 获取玩家是否在某游戏的某服务器登录过
        /// </summary>
        /// <param name="GameId">游戏Id</param>
        /// <param name="UserId">用户Id</param>
        /// <param name="ServerId">服务器Id</param>
        /// <returns>返回是否</returns>
        public Boolean IsLogin(int GameId, int UserId, int ServerId)
        {
            return ols.IsLogin(GameId, UserId, ServerId);
        }

        /// <summary>
        /// 获取某玩家在某游戏登陆过的服务器集合
        /// </summary>
        /// <param name="gameid">游戏Id</param>
        /// <param name="userid">用户Id</param>
        /// <returns>服务器集合</returns>
        public List<string> GetServerList(int GameId, string UserId)
        {
            return ols.GetServerList(GameId, UserId);
        }

        /// <summary>
        /// 查询玩家在游戏最后一次登录的服务器
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="GameId">游戏Id</param>
        /// <returns>返回登陆记录</returns>
        public OnlineLog GetLastLogin(int UserId, int GameId)
        {
            return ols.GetLastLogin(UserId, GameId);
        }

        /// <summary>
        /// 获取玩家登录记录
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="Top">几条</param>
        /// <returns>返回玩家登录记录</returns>
        public List<OnlineLog> GetOnlineLog(int UserId, int Top)
        {
            return ols.GetOnlineLog(UserId, Top);
        }

        /// <summary>
        /// 添加一次登录日志
        /// </summary>
        /// <param name="ol">登录日志</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddOnlineLog(OnlineLog ol)
        {
            return ols.AddOnlineLog(ol);
        }
    }
}
