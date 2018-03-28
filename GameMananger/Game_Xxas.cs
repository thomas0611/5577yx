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
    public class Game_Xxas : IGame
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
        /// 仙侠傲世登陆接口
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="ServerId">服务器Id</param>
        /// <param name="IsPC">是否客户端</param>
        /// <returns>返回登陆地址</returns>
        public string Login(int UserId, int ServerId, int IsPC)
        {
            gu = gus.GetGameUser(UserId);                                  //获取当前登录用户
            gs = gss.GetGameServer(ServerId);                              //获取用户要登录的服务器
            tstamp = Utils.GetTimeSpan();                                  //获取时间戳
            Sign = DESEncrypt.Md5(gu.UserName + tstamp + "0" + DESEncrypt.Md5(gc.LoginTicket, 32), 32);//获取验证码
            string LoginUrl = "http://" + gs.ServerNo + "." + gc.LoginCom + "/api/login.php?account=" + gu.UserName + "&time=" + tstamp + "&flag=" + Sign + "&fcm=0&server_id=" + gs.ServerNo;
            return LoginUrl;
        }

        /// <summary>
        /// 仙侠傲世充值接口
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
                Sign = DESEncrypt.Md5(OrderNo + gu.UserName + PayGold + order.PayMoney + tstamp + DESEncrypt.Md5(gc.PayTicket, 32), 32);
                string PayUrl = "http://" + gs.ServerNo + "." + gc.LoginCom + "/api/recharge.php?ordernum=" + OrderNo + "&account=" + gu.UserName + "&money=" + order.PayMoney + "&gold=" + PayGold + "&time=" + tstamp + "&flag=" + Sign + "&server_id=" + gs.ServerNo;
                GameUserInfo gui = Sel(gu.Id, gs.Id);                       //获取玩家查询信息
                if (gui.Message == "Success")                               //判断玩家是否存在
                {
                    if (order.State == 1)                                   //判断订单状态是否为支付状态
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
                                    return "充值成功！错误：更新订单状态失败！";
                                }
                            case "-1":
                                return "充值失败！错误原因：参数不完整！";
                            case "-2":
                                return "充值失败！错误原因：验证错误！";
                            case "-3":
                                return "充值失败！错误原因：非法访问Ip！";
                            case "-4":
                                return "充值失败！错误原因：请求超时！";
                            case "-5":
                                return "充值失败！错误原因：账号或角色不存在！";
                            case "-6":
                                return "充值失败！错误原因：无法提交重复订单！";
                            case "-7":
                                return "充值失败！错误原因：服务器账号不存在！";
                            case "-10":
                                return "充值失败！错误原因：充值失败！";
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
                return "充值失败！错误原因：用户不存在";
            }
        }

        /// <summary>
        /// 仙侠傲世查询接口
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
            Sign = DESEncrypt.Md5(gu.UserName + tstamp + DESEncrypt.Md5(gc.SelectTicket, 32), 32);         //获取验证码
            string SelUrl = "http://" + gs.ServerNo + "." + gc.ExistCom + "/api/check_role.php?account=" + gu.UserName + "&time=" + tstamp + "&flag=" + Sign + "&server_id=" + gs.ServerNo;
            try
            {
                string SelResult = Utils.GetWebPageContent(SelUrl);             //获取返回结果
                switch (SelResult)
                {
                    case "-1":
                        gui.Message = "查询失败！参数不全！";
                        break;
                    case "-2":
                        gui.Message = "查询失败！验证失败！";
                        break;
                    case "-3":
                        gui.Message = "查询失败！用户不存在！";
                        break;
                    default:
                        Dictionary<string, string> Jd = Json.JsonToArray(SelResult);
                        gui = new GameUserInfo(Jd["role_id"], gu.UserName, Utils.ConvertUnicodeStringToChinese(Jd["role_name"]), int.Parse(Jd["level"]), gs.Name, os.GetOrderInfo(gu.UserName), "Success");
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
        public Game_Xxas()
        {
            game = games.GetGame("xxas");                                   //获取游戏
            gc = gcs.GetGameConfig(game.Id);                                //获取游戏参数
        }
    }
}
