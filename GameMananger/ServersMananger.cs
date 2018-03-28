using Game.DAL;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Data;

namespace Game.Manager
{
    public class ServersMananger
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
        /// 获取GameId下的所有服务器
        /// </summary>
        /// <param name="GameId">游戏Id</param>
        /// <param name="Top">前几条数据</param>
        /// <returns>返回服务器集合</returns>
        public List<GameServer> GetServersByGame(int GameId, int Top)
        {
            return gss.GetServersByGame(GameId, Top);
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
            return gss.GetGameServer(GameId, Qf);
        }

        /// <summary>
        /// 获取服务器数据总条数
        /// </summary>
        /// <param name="WhereStr">条件</param>
        /// <returns>返回条数</returns>
        public Double GetServerCount(string WhereStr)
        {
            return gss.GetServerCount(WhereStr);
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
            return gss.GetAllServer(PageSize, PageNum, WhereStr, OrderBy);
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
            DataTable dt = gss.GetAllCollect(PageSize, PageNum, WhereStr, OrderBy);
            return dt;
        }

        /// <summary>
        /// 更新服务器信息
        /// </summary>
        /// <param name="gs">服务器</param>
        /// <returns>返回是否更新成功</returns>
        public Boolean UpdateServer(GameServer gs)
        {
            return gss.UpdateServer(gs);
        }

        /// <summary>
        /// 添加服务器信息
        /// </summary>
        /// <param name="gs">服务器</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddServer(GameServer gs)
        {
            return gss.AddServer(gs);
        }

        /// <summary>
        /// 删除服务器
        /// </summary>
        /// <param name="ServerId">服务器Id</param>
        /// <returns>返回是否删除成功</returns>
        public Boolean DelServer(int ServerId)
        {
            return gss.DelServer(ServerId);
        }

        /// <summary>
        /// 获取最新开服
        /// </summary>
        /// <param name="Top">前几条</param>
        /// <returns>返回最新开服信息</returns>
        public List<GameServer> GetNewsServer(int Top)
        {
            return gss.GetNewsServer(Top);
        }

    }
}
