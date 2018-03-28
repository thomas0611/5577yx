using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.DAL;
using Game.Model;
using Common;

namespace Game.Manager
{
    public class Game_Dpqk : IGame
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
        /// 斗破乾坤登录接口
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="ServerId">服务器Id</param>
        /// <param name="IsPC">是否PC端登陆</param>
        /// <returns>返回登录地址</returns>
        public string Login(int UserId, int ServerId, int IsPC)
        {
            gu = gus.GetGameUser(UserId);                                   //获取当前登录用户
            gs = gss.GetGameServer(ServerId);                              //获取用户要登录的服务器
            tstamp = Utils.GetTimeSpan();                                   //获取时间戳
            Sign = DESEncrypt.Md5("account=" + gu.UserName + "&agent=" + gc.AgentId + "&fcm=1&fcm_time=-1&serverid=" + gs.ServerNo + "&time=" + tstamp + "&way=1&" + gc.LoginTicket, 32);
            string LoginUrl = "http://" + gs.ServerNo + "." + gc.LoginCom + "?account=" + gu.UserName + "&agent=" + gc.AgentId + "&fcm=1&fcm_time=-1&serverid=" + gs.ServerNo + "&time=" + tstamp + "&way=1&token=" + Sign;
            return LoginUrl;
        }

        /// <summary>
        /// 斗破乾坤充值接口
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
                Sign = DESEncrypt.Md5("account=" + gu.UserName + "&agent=" + gc.AgentId + "&amount=" + PayGold + "&order=" + OrderNo + "&price=" + order.PayMoney + "&serverid=" + gs.ServerNo + "&time=" + tstamp + "&" + gc.PayTicket, 32);
                string PayUrl = "http://" + gs.ServerNo + "." + gc.PayCom + "?account=" + gu.UserName + "&agent=" + gc.AgentId + "&amount=" + PayGold + "&order=" + OrderNo + "&price=" + order.PayMoney + "&serverid=" + gs.ServerNo + "&time=" + tstamp + "&token=" + Sign;
                GameUserInfo gui = Sel(gu.Id, gs.Id);                       //获取玩家查询信息
                if (gui.Message == "Success")                               //判断玩家是否存在
                {
                    if (order.State == 1)                                       //判断订单状态是否为支付状态
                    {
                        string PayResult = Utils.GetWebPageContent(PayUrl);     //获取充值结果
                        switch (PayResult)                                      //对充值结果进行解析
                        {
                            case "1":
                                if (os.UpdateOrder(order.OrderNo))              //更新订单状态为已完成
                                {
                                    gus.UpdateGameMoney(gu.UserName, order.PayMoney);     //跟新玩家游戏消费情况
                                    return "充值成功！";
                                }
                                else
                                {
                                    return "充值失败！错误原因：更新订单状态失败！";
                                }
                            case "-102":
                                return "充值失败！验证参数错误！";
                            case "-201":
                                return "充值失败！角色不存在！";
                            case "-207":
                                return "充值失败！账号不存在！";
                            case "-300":
                                return "充值失败！金额有误！";
                            case "-301":
                                return "充值失败！无效订单！";
                            case "-302":
                                return "充值失败！无非提交重复订单！";
                            case "-303":
                                return "充值失败！充值接口已关闭！";
                            case "-304":
                                return "充值失败！IP限制！";
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
                    return gui.Message;
                }
            }
            else
            {
                return "充值失败！用户不存在！";
            }
        }

        /// <summary>
        /// 斗破乾坤查询接口
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="ServerId">服务器Id</param>
        /// <returns>返回查询结果</returns>
        public GameUserInfo Sel(int UserId, int ServerId)
        {
            gu = gus.GetGameUser(UserId);                                   //获取查询用户
            gs = gss.GetGameServer(ServerId);                              //获取查询用户所在区服
            tstamp = Utils.GetTimeSpan();                                   //获取时间戳
            GameUserInfo gui = new GameUserInfo();                          //定义返回查询结果信息   
            Sign = DESEncrypt.Md5("account=" + gu.UserName + "&action=playerinfo&agent=" + gc.AgentId + "&serverid=" + gs.ServerNo + "&time=" + tstamp + "&" + gc.SelectTicket, 32);              //获取验证参数
            string SelUrl = "http://" + gs.ServerNo + "." + gc.ExistCom + "?account=" + gu.UserName + "&action=playerinfo&agent=" + gc.AgentId + "&serverid=" + gs.ServerNo + "&time=" + tstamp + "&token=" + Sign + "";      //获取查询地址
            string SelResult = Utils.GetWebPageContent(SelUrl);             //获取返回结果
            try
            {
                switch (SelResult)
                {
                    case "-501":
                        gui.Message = "查询账号数量超过限制";
                        break;
                    case "-207":
                        gui.Message = "无效玩家账号";
                        break;
                    default:
                        SelResult = SelResult.Substring(0, SelResult.IndexOf('}'));                      //处理返回结果
                        SelResult = SelResult.Replace(SelResult.Substring(0, SelResult.LastIndexOf('{') + 1), "");
                        string[] b = SelResult.Split(',');
                        switch (b[0].Substring(7))
                        {
                            case "-101":
                                gui.Message = "查询失败！无效访问！";
                                break;
                            case "-102":
                                gui.Message = "查询失败！验证失败！";
                                break;
                            case "-209":
                                gui.Message = "查询失败！无效参数！";
                                break;
                            case "-401":
                                gui.Message = "查询失败！链接超时！";
                                break;
                            case "-304":
                                gui.Message = "查询失败！IP限制！";
                                break;
                            default:
                                gui = new GameUserInfo(gu.Id.ToString(), gu.UserName, b[1].Substring(7).Replace("\"", ""), int.Parse(b[0].Substring(8).Replace("\"", "")), gs.Name, os.GetOrderInfo(gu.UserName), "Success");
                                break;
                        }
                        break;
                }
            }
            catch (Exception)
            {
                gui.UserName = "没有角色";
                gui.Message = "查询失败！查询不到用户信息！";
            }
            return gui;
        }

        /// <summary>
        /// 实例化接口参数
        /// </summary>
        public Game_Dpqk()
        {
            game = games.GetGame("dpqk");                                   //获取游戏
            gc = gcs.GetGameConfig(game.Id);                                //获取游戏参数
        }
    }
}
