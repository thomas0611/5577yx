using Game.DAL;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Manager
{
    public class LotteryManager
    {
        GameUserManager gum = new GameUserManager();
        OrderManager om = new OrderManager();
        GamesManager gm = new GamesManager();
        LotteryServers ls = new LotteryServers();

        /// <summary>
        /// 增加积分策略
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="PayMoney">充值金额</param>
        /// <returns>返回是否执行成功</returns>
        public Boolean AddUserPoints(string UserName, float PayMoney)
        {
            return gum.UpdateUserPoints(UserName, PayMoney, "+");
        }

        /// <summary>
        /// 扣除积分策略
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <returns>返回是否执行成功</returns>
        public Boolean DeducUserPoints(string UserName)
        {
            GameUser gu = gum.GetGameUser(UserName);
            if (gu.Points >= 10)
            {
                return gum.UpdateUserPoints(UserName, 10, "-");
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 进行抽奖
        /// </summary>
        /// <returns></returns>
        public string LotteryRes(string UserName)
        {
            int LotteryNum = 0;
            string LotteryText = "";
            LotteryLog ll = new LotteryLog(0, UserName, 10, 0, "", DateTime.Now);
            Random ran = new Random();
            int RandKey = ran.Next(1, 100000);
            if (RandKey > 899 && RandKey <= 999)
            {
                if (RandKey % 2 == 0)
                {
                    LotteryNum = 2;
                }
                else
                {
                    LotteryNum = 10;
                }
                AddUserMoney(UserName, 100);
                LotteryText = "恭喜您获得平台币一百元，现已到账，请查收！请再接再厉向大奖发力！";
                ll.LotterName = "平台币一百元";
            }
            else if (RandKey > 7725 && RandKey <= 7925)
            {
                AddUserMoney(UserName, 10);
                LotteryText = "恭喜您获得平台币十元，现已到账，请查收！请再接再厉向大奖发力！";
                ll.LotterName = "平台币十百元";
                if (RandKey % 2 == 0)
                {
                    LotteryNum = 4;
                }
                else
                {
                    LotteryNum = 12;
                }
            }
            else if (RandKey > 33332 && RandKey <= 34332)
            {
                AddUserMoney(UserName, 5);
                LotteryText = "恭喜您获得平台币五元，现已到账，请查收！请再接再厉向大奖发力！";
                ll.LotterName = "平台币五元";
                if (RandKey % 2 == 0)
                {
                    LotteryNum = 5;
                }
                else
                {
                    LotteryNum = 13;
                }
            }
            else if (RandKey > 55553 && RandKey <= 57553)
            {
                LotteryText = "恭喜您获得平台币一元，现已到账，请查收！请再接再厉向大奖发力！";
                ll.LotterName = "平台币一元";
                if (RandKey % 2 == 0)
                {
                    LotteryNum = 7;
                }
                else
                {
                    LotteryNum = 15;
                }
            }
            else
            {
                LotteryText = "很遗憾这次您啥也没中，不过还是要感谢您的参与！预祝您下次中大奖！";
                ll.LotterName = "谢谢参与";
                int RandKey2 = ran.Next(1, 6);
                switch (RandKey2)
                {
                    case 1:
                        LotteryNum = 3;
                        break;
                    case 2:
                        LotteryNum = 6;
                        break;
                    case 3:
                        LotteryNum = 8;
                        break;
                    case 4:
                        LotteryNum = 11;
                        break;
                    case 5:
                        LotteryNum = 14;
                        break;
                    case 6:
                        LotteryNum = 16;
                        break;
                    default:
                        break;
                }
            }
            ll.LotteryNum = LotteryNum;
            ls.AddLotteryLog(ll);
            return LotteryNum + "|" + LotteryText;
        }

        /// <summary>
        /// 抽奖系统自动发放奖励
        /// </summary>
        /// <param name="UserName">用户名</param>
        /// <param name="Money">金额</param>
        /// <returns>返回发放结果</returns>
        public string AddUserMoney(string UserName, float Money)
        {
            try
            {
                Orders order = new Orders();
                order = om.GetOrder("J", "0", 0, 2, 6, UserName, Money, "抽奖系统");
                order.State = 1;
                if (om.AddOrder(order))
                {
                    return gm.PayManager(order.OrderNo);
                }
                else
                {
                    return "添加订单失败！";
                }
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        /// <summary>
        /// 获取最新获奖记录
        /// </summary>
        /// <returns>返回中奖记录集合</returns>
        public List<LotteryLog> GetLotteryLog()
        {
            return ls.GetLotteryLog();
        }
    }
}
