using Common;
using Game.DAL;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Manager
{
    public class Game_Jhwj : IGame
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
        /// 江湖问剑登录接口
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="ServerId">服务器Id</param>
        /// <param name="IsPC">是否PC端登录</param>
        /// <returns>返回登录接口</returns>
        public string Login(int UserId, int ServerId, int IsPC)
        {
            gu = gus.GetGameUser(UserId);                                  //获取当前登录用户
            gs = gss.GetGameServer(ServerId);                              //获取用户要登录的服务器
            tstamp = Utils.GetTimeSpan();                                  //获取时间戳
            Sign = DESEncrypt.Md5("fcm" + "n" + "gid" + "221" + "pid" + gc.AgentId + "sid" + gs.QuFu + "time" + tstamp + "uid" + gu.UserName + gc.LoginTicket, 32);            //获取验证码
            string LoginUrl = "http://" + gc.LoginCom + "?pid=" + gc.AgentId + "&gid=221&sid=" + gs.QuFu + "&uid=" + gu.UserName + "&time=" + tstamp + "&fcm=n&sign= " + Sign;
            if (IsPC == 1)
            {
                LoginUrl += "&client=pc";
            }
            else
            {
                LoginUrl += "&client=web";
            }
            return LoginUrl;
        }

        public string Pay(string OrderNo)
        {
            order = os.GetOrder(OrderNo);                                   //获取用户的充值订单
            gu = gus.GetGameUser(order.UserName);                           //获取充值用户
            gs = gss.GetGameServer(order.ServerId);                        //获取用户要充值的服务器
            string PayGold = (order.PayMoney * game.GameMoneyScale).ToString();     //计算支付的游戏币
            if (gus.IsGameUser(gu.UserName))                                //判断用户是否属于平台
            {
                tstamp = Utils.GetTimeSpan();                               //获取时间戳
                Sign = DESEncrypt.Md5(("amount=" + order.PayMoney + "&gid=221&money=" + order.PayMoney + "&oid=" + OrderNo + "&pid=" + gc.AgentId + "&point=" + PayGold + "&sid=" + gs.QuFu + "&uid=" + gu.UserName + "&utime=" + tstamp).ToUpper() + gc.PayTicket, 32);
                string PayUrl = "http://" + gc.PayCom + "?pid=" + gc.AgentId + "&gid=221&sid=" + gs.QuFu + "&uid=" + gu.UserName + "&point=" + PayGold + "&amount=" + order.PayMoney + "&money=" + order.PayMoney + "&oid=" + OrderNo + "&utime=" + tstamp + "&sign=" + Sign;
                GameUserInfo gui = Sel(gu.Id, gs.Id);                       //获取玩家查询信息
                if (gui.Message == "Success")                               //判断玩家是否存在
                {
                    if (order.State == 1)                                   //判断订单状态是否为支付状态
                    {
                        try
                        {
                            string PayResult = Utils.GetWebPageContent(PayUrl);         //获取充值结果
                            switch (PayResult)
                            {
                                case "1":
                                    if (os.UpdateOrder(order.OrderNo))                  //更新订单状态为已完成
                                    {
                                        gus.UpdateGameMoney(gu.UserName, order.PayMoney);     //跟新玩家游戏消费情况
                                        return "充值成功！";
                                    }
                                    else
                                    {
                                        return "充值成功！发生错误：更新订单状态失败！";
                                    }
                                case "11":
                                    return "充值失败！错误原因：金额错误！";
                                case "2006209":
                                    return "充值失败！错误原因：参数错误！";
                                case "13":
                                    return "充值失败！错误原因：请求方式错误！";
                                case "17":
                                    return "充值失败！错误原因：验证失败！";
                                case "10":
                                    return "充值失败！错误原因：充值服务器不存在！";
                                case "14":
                                    return "充值失败！错误原因：请求超时！";
                                case "18":
                                    return "充值失败！错误原因：非法请求的IP！";
                                case "19":
                                    return "充值失败！错误原因：充值游戏不存在！";
                                case "20":
                                    return "充值失败！错误原因：充值比例错误！";
                                case "-10":
                                    return "充值失败！错误原因：无效的服务器！";
                                case "-11":
                                    return "充值失败！错误原因：无效的玩家帐号！";
                                case "12":
                                case "-12":
                                    return "充值失败！错误原因：无法提交重复订单！";
                                case "-14":
                                    return "充值失败！错误原因：无效的时间！";
                                case "-15":
                                    return "充值失败！错误原因：充值金额错误！";
                                case "-17":
                                    return "充值失败！错误原因：无效的时间！";
                                case "-18":
                                    return "充值失败！错误原因：验证失败！";
                                default:
                                    return "充值失败！未知错误！";
                            }
                        }
                        catch (Exception)
                        {
                            return "充值失败！错误原因：充值失败！";
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

        public GameUserInfo Sel(int UserId, int ServerId)
        {
            gu = gus.GetGameUser(UserId);                                   //获取查询用户
            gs = gss.GetGameServer(ServerId);                              //获取查询用户所在区服
            tstamp = Utils.GetTimeSpan();                                   //获取时间戳
            GameUserInfo gui = new GameUserInfo();                          //定义返回查询结果信息                           
            Sign = DESEncrypt.Md5(gc.SelectTicket + "pid" + gc.AgentId + "sid" + gs.QuFu + "time" + tstamp + "uid" + gu.UserName, 32).ToUpper();         //获取验证码
            string SelUrl = "http://" + gc.ExistCom + "?pid=" + gc.AgentId + "&sid=" + gs.QuFu + "&time=" + tstamp + "&uid=" + gu.UserName + "&sign=" + Sign;
            try
            {
                string SelResult = Utils.GetWebPageContent(SelUrl);             //获取返回结果
                switch (SelResult)
                {
                    case "-1":
                        gui.Message = "查询失败！内部错误！";
                        break;
                    case "-100":
                        gui.Message = "查询失败！参数错误！";
                        break;
                    case "-200":
                        gui.Message = "查询失败！系统错误！";
                        break;
                    case "-300":
                        gui.Message = "查询失败！验证失败！";
                        break;
                    case "-400":
                        gui.Message = "查询失败！请求超时！";
                        break;
                    case "-500":
                        gui.Message = "查询失败！数据操作失败！";
                        break;
                    case "-600":
                        gui.Message = "查询失败！帐号无效！";
                        break;
                    case "-700":
                        gui.Message = "查询失败！充值服务器不存在！";
                        break;
                    case "-1406":
                        gui.Message = "查询失败！角色不存在！";
                        break;
                    case "-11000":
                        gui.Message = "查询失败！非法访问的IP！";
                        break;
                    case "100":
                        gui.Message = "查询失败！平台请求参数错误！";
                        break;
                    case "700":
                    case "555":
                        gui.Message = "查询失败！平台不存在！";
                        break;
                    case "556":
                        gui.Message = "查询失败！内部分析错误！";
                        break;
                    case "300":
                        gui.Message = "查询失败！平台验证失败！";
                        break;
                    default:
                        Dictionary<string, string> Jd = Json.JsonToArray(SelResult);
                        gui = new GameUserInfo(Jd["userid"], gu.UserName, Utils.ConvertUnicodeStringToChinese(Jd["rolename"]), int.Parse(Jd["level"]), gs.Name, os.GetOrderInfo(gu.UserName), "Success");
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

        public Game_Jhwj()
        {
            game = games.GetGame("jhwj");                                   //获取游戏
            gc = gcs.GetGameConfig(game.Id);                                //获取游戏参数
        }
    }
}
