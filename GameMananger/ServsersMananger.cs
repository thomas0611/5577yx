using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Model;
using Game.DAL;

namespace Game.Manager
{
    public class ServsersMananger
    {
        GameServerServers gss = new GameServerServers();

        /// <summary>
        /// 获取GameId下的所有服务器
        /// </summary>
        /// <param name="GameId">游戏Id</param>
        /// <returns>返回服务器集合</returns>
        public List<GameServer> GetServersByGame(int GameId)
        {
            return gss.GetServersByGame(GameId);
        }

        /// <summary>
        /// 获取服务器信息
        /// </summary>
        /// <param name="ServerId">服务器Id</param>
        /// <returns>返回服务器</returns>
        public GameServer GetGameServer(int ServerId)
        {
            return gss.GetGameServer(ServerId);
        }
        
        /// <summary>
        /// 获取服务器信息
        /// </summary>
        /// <param name="GameId">游戏Id</param>
        /// <param name="Qf">区服</param>
        /// <returns>返回服务器</returns>
        public GameServer GetGameServer(int GameId, int Qf) 
        {
            return gss.GetGameServer(GameId,Qf);
        }
    }
}
