using Game.DAL;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Game.Manager
{
    public class CardManager
    {
        CardServer cs = new CardServer();

        /// <summary>
        /// 获取新闻总条数
        /// </summary>
        /// <param name="WhereStr">条件</param>
        /// <returns>返回新闻条数</returns>
        public Double GetCardCount(string WhereStr)
        {
            return cs.GetCardCount(WhereStr);
        }

        /// <summary>
        /// 获取分页新闻数据
        /// </summary>
        /// <param name="PageSize">页面大小</param>
        /// <param name="PageNum">第几页</param>
        /// <param name="WhereStr">条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns>返回新闻数据集合</returns>
        public DataTable GetCards(int PageSize, int PageNum, string WhereStr, string OrderBy)
        {
            return cs.GetCards(PageSize, PageNum, WhereStr, OrderBy);
        }

        /// <summary>
        /// 获取新手卡
        /// </summary>
        /// <param name="CardId">卡Id</param>
        /// <returns>返回新手卡</returns>
        public cardsname GetCard(int CardId)
        {
            return cs.GetCard(CardId);
        }

        /// <summary>
        /// 更新新手卡信息
        /// </summary>
        /// <param name="cn">新手卡</param>
        /// <returns>返回是否更新成功</returns>
        public Boolean UpdateCard(cardsname cn)
        {
            cn.gamename = new GamesManager().GetGame(cn.gameid).Name;
            cn.servername = cn.serverid == 0 ? "" : new ServersMananger().GetGameServer(cn.serverid).Name;
            return cs.UpdateCard(cn);
        }

        /// <summary>
        /// 添加新手卡
        /// </summary>
        /// <param name="cn">新手卡</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddCard(cardsname cn)
        {
            cn.gamename = new GamesManager().GetGame(cn.gameid).Name;
            cn.servername = cn.serverid == 0 ? "" : new ServersMananger().GetGameServer(cn.serverid).Name;
            return cs.AddCard(cn);
        }

        /// <summary>
        /// 删除新手卡
        /// </summary>
        /// <param name="CardId">卡Id</param>
        /// <returns>返回是否删除成功</returns>
        public Boolean DelCard(int CardId)
        {
            return cs.DelCard(CardId);
        }

        /// <summary>
        /// 添加新手卡号
        /// </summary>
        /// <param name="c">卡号</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddCard(cards c)
        {
            return cs.AddCard(c);
        }

        /// <summary>
        /// 检查是否存在该卡号
        /// </summary>
        /// <param name="CardId">卡Id</param>
        /// <param name="CardNum">卡号</param>
        /// <returns>返回是否存在</returns>
        public Boolean ExitCard(int CardId, string CardNum)
        {
            return cs.ExitCard(CardId, CardNum);
        }

        /// <summary>
        /// 获取新手卡领取记录数量
        /// </summary>
        /// <param name="WhereStr">条件</param>
        /// <returns>返回记录数量</returns>
        public Double GetCardLogCount(string WhereStr)
        {
            return cs.GetCardLogCount(WhereStr);
        }

        /// <summary>
        /// 通过分页获取所有新手卡领取记录
        /// </summary>
        /// <param name="PageSize">页大小</param>
        /// <param name="PageNum">页码</param>
        /// <param name="WhereStr">条件</param>
        /// <param name="OrderBy">排序</param>
        /// <returns>返回数据集合</returns>
        public DataTable GetAllCardLog(int PageSize, int PageNum, string WhereStr, string OrderBy)
        {
            return cs.GetAllCardLog(PageSize, PageNum, WhereStr, OrderBy);
        }

        /// <summary>
        /// 获取新手卡剩余数量
        /// </summary>
        /// <param name="CardId">卡Id</param>
        /// <returns>返回新手卡数量</returns>
        public Double GetCardCount(int CardId)
        {
            return cs.GetCardCount(CardId);
        }

        /// <summary>
        /// 检测玩家是否已经领取新手卡
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="CardId">卡Id</param>
        /// <returns>返回是否领取</returns>
        public Boolean ExitCardLog(int UserId, int CardId)
        {
            return cs.ExitCardLog(UserId, CardId);
        }

        /// <summary>
        /// 获取一张未领取的新手卡
        /// </summary>
        /// <param name="CardId">卡Id</param>
        /// <returns>返回新手卡</returns>
        public cards GetCards(int CardId)
        {
            return cs.GetCards(CardId);
        }

        /// <summary>
        /// 添加一天领卡记录
        /// </summary>
        /// <param name="cl">领卡记录</param>
        /// <returns>返回是否添加成功</returns>
        public Boolean AddCardLog(cardslog cl)
        {
            return cs.AddCardLog(cl);
        }

        /// <summary>
        /// 更新新手卡状态
        /// </summary>
        /// <param name="State">状态</param>
        /// <param name="CardsId">卡Id</param>
        /// <returns>返回是否更新成功</returns>
        public Boolean UpdateCard(int State, int CardsId)
        {
            return cs.UpdateCard(State, CardsId);
        }
    }
}
