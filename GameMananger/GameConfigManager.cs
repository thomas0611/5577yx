using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Model;
using Game.DAL;

namespace Game.Manager
{
    public class GameConfigManager
    {
        GameConfigServers gcs = new GameConfigServers();

        /// <summary>
        /// 获取GameId的配置参数
        /// </summary>
        /// <param name="GameId">游戏Id</param>
        /// <returns></returns>
        public GameConfig GetGameConfig(int GameId)
        {
            return gcs.GetGameConfig(GameId);
        }

        /// <summary>
        /// 更新游戏配置
        /// </summary>
        /// <param name="GameId">游戏Id</param>
        /// <returns>返回是否更新成功</returns>
        public Boolean UpdateGameConfig(GameConfig gc)
        {
            return gcs.UpdateGameConfig(gc);
        }

        /// <summary>
        /// 添加游戏配置
        /// </summary>
        /// <param name="GameId">游戏Id</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddGameConfig(GameConfig gc)
        {
            return gcs.AddGameConfig(gc);
        }
    }
}
