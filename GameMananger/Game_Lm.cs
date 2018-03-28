using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Model;
using Game.DAL;
using Common;

namespace Game.Manager
{
    public class Game_Lm : IGame
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
        /// 猎魔登陆接口
        /// </summary>
        /// <param name="Id">用户Id</param>
        /// <param name="ServerId">服务器Id</param>
        /// <param name="IsPc">是否PC端登陆</param>
        /// <returns></returns>
        public string Login(int Id, int ServerId, int IsPc)
        {
            gu = gus.GetGameUser(Id);                                  //获取当前登录用户
            gs = gss.GetGameServer(ServerId);                              //获取用户要登录的服务器
            tstamp = Utils.GetTimeSpan();                                  //获取时间戳
            Sign = DESEncrypt.Md5(gc.AgentId + "|" + gu.Id + "|1233|" + gs.ServerNo + "|" + tstamp + "|1|" + gc.LoginTicket, 32);            //获取验证码
            string LoginDelURL = "http://" + gc.LoginCom + "?site=" + gc.AgentId + "&uid=" + gu.Id + "&game=1233&num=" + gs.ServerNo + "&fcm=1&time=" + tstamp + "&sign=" + Sign;
            string LoginResult = Utils.GetWebPageContent(LoginDelURL);
            string[] R = LoginResult.Replace("[", "").Replace("]", "").Split(',');
            string LoginUrl = R[1].Replace("\"", "").Replace("\\", ""); ;
            return LoginUrl;
        }

        /// <summary>
        /// 猎魔充值接口
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
                Sign = DESEncrypt.Md5(gc.AgentId + "|" + gu.Id + "|1233|" + gs.ServerNo + "|" + order.PayMoney + "|" + tstamp + "|" + OrderNo + "|" + gc.PayTicket, 32);
                string PayUrl = "http://" + gc.PayCom + "?site=" + gc.AgentId + "&uid=" + gu.Id + "&game=1233&num=" + gs.ServerNo + "&money=" + order.PayMoney + "&time=" + tstamp + "&orderId=" + OrderNo + "&sign=" + Sign;
                GameUserInfo gui = Sel(gu.Id, gs.Id);                       //获取玩家查询信息
                if (gui.Message == "Success")                               //判断玩家是否存在
                {
                    if (order.State == 1)                                   //判断订单状态是否为支付状态
                    {
                        string PayResult = Utils.GetWebPageContent(PayUrl);         //获取充值结果
                        string[] b = PayResult.Replace("[", "").Replace("]", "").Split(',');
                        if (b[0] == "1")
                        {
                            if (os.UpdateOrder(order.OrderNo))                  //更新订单状态为已完成
                            {
                                gus.UpdateGameMoney(gu.UserName, order.PayMoney);     //跟新玩家游戏消费情况
                                return "充值成功！";
                            }
                            else
                            {
                                return "充值失败！错误原因：更新订单状态失败！";
                            }
                        }
                        else
                        {
                            switch (b[1])
                            {
                                case "1":
                                    return "充值失败！错误原因：站点不存在！";
                                case "2":
                                    return "充值失败！错误原因：验证错误！";
                                case "3":
                                    return "充值失败！错误原因：服务器不存在！";
                                case "4":
                                    return "充值失败！错误原因：服务器未开启！";
                                case "5":
                                    return "充值失败！错误原因：服务器维护中！";
                                case "6":
                                    return "充值失败！错误原因：金额错误！";
                                case "7":
                                    return "充值失败！错误原因：角色不存在！";
                                case "8":
                                    return "充值失败！错误原因：数据库错误！";
                                case "9":
                                    return "充值失败！错误原因：卡类型错误！";
                                case "10":
                                    return "充值失败！错误原因：角色不存在！";
                                case "11":
                                    return "充值失败！错误原因：卡已发完！";
                                case "12":
                                    return "充值失败！错误原因：IP限制！";
                                case "13":
                                    return "充值失败！错误原因：游戏不存在！";
                                case "255":
                                    return "充值失败！错误原因：未知错误！";
                                default:
                                    return "充值失败！未知错误！";
                            }
                        }
                    }
                    else
                    {
                        return "充值失败！错误原因：无法提交未支付订单！";
                    }
                }
                else
                {
                    return gui.Message;
                }
            }
            else
            {
                return "充值失败！错误原因：用户不存在";
            }
        }

        /// <summary>
        /// 猎魔查询接口
        /// </summary>
        /// <param name="Id">用户Id</param>
        /// <param name="ServerId">服务器Id</param>
        /// <returns>返回用户信息</returns>
        public GameUserInfo Sel(int Id, int ServerId)
        {
            gu = gus.GetGameUser(Id);                                   //获取查询用户
            gs = gss.GetGameServer(ServerId);                              //获取查询用户所在区服
            tstamp = Utils.GetTimeSpan();                                   //获取时间戳
            GameUserInfo gui = new GameUserInfo();                          //定义返回查询结果信息                           
            Sign = DESEncrypt.Md5(gc.AgentId + "|" + gu.Id + "|1233|" + gs.ServerNo + "|" + tstamp + "|" + gc.SelectTicket, 32);         //获取验证码
            string SelUrl = "http://" + gc.ExistCom + "?site=" + gc.AgentId + "&uid=" + gu.Id + "&game=1233&num=" + gs.ServerNo + "&time=" + tstamp + "&sign=" + Sign;
            string SelResult = Utils.GetWebPageContent(SelUrl);             //获取返回结果
            if (SelResult == "[1,1]")
            {
                gui.Message = "Success";
            }
            else
            {
                gui.Message = "角色不存在！";
            }
            return gui;
        }

        /// <summary>
        /// 实例化接口参数
        /// </summary>
        public Game_Lm()
        {
            game = games.GetGame("lm");                                   //获取游戏
            gc = gcs.GetGameConfig(game.Id);                                //获取游戏参数
        }
    }
}
