using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.DAL;
using Game.Model;
using Common;

namespace Game.Manager
{
    public class Game_Nz : IGame
    {
        Games game = new Games();                                           //实例化游戏
        GamesServers games = new GamesServers();                            //实例化获取游戏相关数据
        GameConfig gc = new GameConfig();                                   //实例化游戏参数
        GameConfigServers gcs = new GameConfigServers();                    //实例化获取游戏参数相关数据
        GameServer gs = new GameServer();                                   //实例化游戏服务器
        GameServerServers gss = new GameServerServers();                    //实例化获取游戏服务器相关数据
        GameUser gu = new GameUser();                                       //实例化用户
        GameUserServers gus = new GameUserServers();                        //实例化获取用户相关数据
        Orders order = new Orders();                                        //实例化订单
        OrdersServers os = new OrdersServers();                             //实例化获取订单相关数据
        string tstamp;                                                      //定义时间戳
        string Sign;                                                        //定义验证参数

        /// <summary>
        /// 怒斩登录接口
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="ServerId">服务器Id</param>
        /// <returns>返回登录地址</returns>
        public string Login(int UserId, int ServerId, int IsPC)
        {
            gu = gus.GetGameUser(UserId);                                   //获取当前登录用户
            gs = gss.GetGameServer(ServerId);                              //获取用户要登录的服务器
            tstamp = Utils.GetTimeSpan();                                   //获取时间戳
            string sign = DESEncrypt.Md5(gu.Id + gu.UserName + tstamp + "1" + gc.LoginTicket, 32);
            string LoginUrl = "http://" + gs.ServerNo + "." + gc.LoginCom + "login.php?u=" + UserId + "&n=" + gu.UserName + "&t=" + tstamp + "&cm=1&p=" + sign + "&type=ie&s=" + gs.ServerNo;
            return LoginUrl;
        }

        /// <summary>
        /// 怒斩充值接口
        /// </summary>
        /// <param name="OrderNo">订单号</param>
        /// <returns>返回充值结果</returns>
        public string Pay(string OrderNo)
        {
            order = os.GetOrder(OrderNo);                                   //获取用户的充值订单
            gu = gus.GetGameUser(order.UserName);                           //获取充值用户
            gs = gss.GetGameServer(order.ServerId);                        //获取用户要充值的服务器
            string PayGold = (order.PayMoney * game.GameMoneyScale).ToString();     //计算支付的游戏币
            if (gus.IsGameUser(gu.UserName))                                //判断用户是否属于平台
            {
                tstamp = Utils.GetTimeSpan();                               //获取时间戳
                Sign = DESEncrypt.Md5(order.PayMoney + gu.Id.ToString() + OrderNo + tstamp + gc.PayTicket, 32);
                string PayUrl = "http://" + gs.ServerNo + "." + gc.PayCom + "exchange.php?amount=" + order.PayMoney + "&u=" + gu.Id + "&order_no=" + OrderNo + "&time=" + tstamp + "&sign=" + Sign;
                GameUserInfo gui = Sel(gu.Id, gs.Id);                       //获取玩家查询信息
                if (gui.Message == "Success")                               //判断玩家是否存在
                {
                    if (order.State == 1)                                       //判断订单状态是否为支付状态
                    {
                        string PayResult = Utils.GetWebPageContent(PayUrl);     //获取充值结果
                        switch (PayResult)                                      //对充值结果进行解析
                        {
                            case "success":
                                if (os.UpdateOrder(order.OrderNo))              //更新订单状态为已完成
                                {
                                    gus.UpdateGameMoney(gu.UserName, order.PayMoney);     //跟新玩家游戏消费情况
                                    return "充值成功！";
                                }
                                else
                                {
                                    return "充值失败！错误原因：更新订单状态失败！";
                                }
                            case "err_time":
                                return "充值失败！错误原因：订单请求超时！";
                            case "err_sign":
                                return "充值失败！错误原因：验证参数错误！";
                            case "err_repeat":
                                return "充值失败！错误原因：无法提交重复订单！";
                            case "fail":
                                return "充值失败！错误原因：其它错误！";
                            default:
                                return "充值失败！未知错误！";
                        }
                    }
                    else
                    {
                        return "充值失败！错误原因：无法提交未支付订单！";
                    }
                }
                else
                {
                    return "充值失败！角色不存在！";
                }
            }
            else
            {
                return "充值失败！用户不存在！";
            }
        }

        /// <summary>
        /// 怒斩查询接口
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ServerId"></param>
        /// <returns></returns>
        public GameUserInfo Sel(int UserId, int ServerId)
        {
            //怒斩尚未提供用户信息查询接口
            GameUserInfo gui = new GameUserInfo();                          //定义返回查询结果信息 
            gui.Message = "Success";
            return gui;
        }

        /// <summary>
        /// 初始化接口参数
        /// </summary>
        public Game_Nz()
        {
            game = games.GetGame("nz");                                     //获取游戏
            gc = gcs.GetGameConfig(game.Id);                                //获取游戏参数
        }
    }
}
