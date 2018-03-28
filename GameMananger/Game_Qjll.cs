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
    public class Game_Qjll : IGame
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
        /// 奇迹来了登陆接口
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="ServerId">服务器Id</param>
        /// <param name="IsPC">是否PC端登陆</param>
        /// <returns>返回登陆地址</returns>
        public string Login(int UserId, int ServerId, int IsPC)
        {
            gu = gus.GetGameUser(UserId);                                   //获取当前登录用户
            gs = gss.GetGameServer(ServerId);                              //获取用户要登录的服务器
            tstamp = Utils.GetTimeSpan();
            Sign = DESEncrypt.Md5("name=" + gu.UserName + "&platid=" + gc.AgentId + "&platuid=" + gu.Id + "&sid=" + gs.ServerNo + "&tm=" + tstamp + gc.LoginTicket, 32);               //获取验证参数
            string LoginUrl = "http://" + gc.LoginCom + "?name=" + gu.UserName + "&platid=" + gc.AgentId + "&platuid=" + gu.Id + "&sid=" + gs.ServerNo + "&tm=" + tstamp + "&sig=" + Sign;       //生成登录地址
            //string LoginUrl = "http://" + gc.LoginCom + "?name=" + gu.UserName + "&platid=" + gc.AgentId + "&platuid=" + gu.Id + "&sid=" + gs.Id + "&tm=" + tstamp + "&sig=" + Sign;
            return LoginUrl;
        }

        /// <summary>
        /// 奇迹来了充值接口
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
                tstamp = Utils.GetTimeSpan();                                   //获取时间戳
                //Sign = DESEncrypt.Md5("orderid=" + OrderNo + "&money=" + order.PayMoney + "&paypoint=" + PayGold + "&platid=" + gc.AgentId + "&platuid=" + gu.Id + "&sid=" + gs.ServerNo + "&tm=" + tstamp + gc.PayTicket, 32);                //获取验证参数
                Sign = DESEncrypt.Md5("money=" + order.PayMoney + "&orderid=" + order.OrderNo + "&paypoint=" + PayGold + "&platid=" + gc.AgentId + "&platuid=" + gu.Id + "&sid=" + gs.ServerNo + "&tm=" + tstamp + gc.PayTicket,32);
                string PayUrl = "http://" + gc.PayCom + "?orderid=" + OrderNo + "&money=" + order.PayMoney + "&paypoint=" + PayGold + "&platid=" + gc.AgentId + "&platuid=" + gu.Id + "&sid=" + gs.ServerNo + "&tm=" + tstamp + "&sig=" + Sign;
                GameUserInfo gui = Sel(gu.Id, gs.Id);                           //获取玩家查询信息
                if (gui.Message == "Success")                                   //判断玩家是否存在
                {
                    if (order.State == 1)                                       //判断订单状态是否为支付状态
                    {
                        string PayResult = Utils.GetWebPageContent(PayUrl);         //获取充值结果
                        switch (PayResult)                             //对充值结果进行解析
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
                            case "-1":
                                return "充值失败！错误原因：参数错误！";
                            case "-2":
                                return "充值失败！错误原因：请求超时！";
                            case "-3":
                                return "充值失败！错误原因：服务器或平台不存在！";
                            case "-4":
                                return "充值失败！错误原因：验证失败！";
                            case "-5":
                                return "充值失败！错误原因：用户不存在！";
                            case "-6":
                                return "充值失败！错误原因：非法的访问IP！";
                            case "-7":
                                return "充值失败！错误原因：无法提交重复订单！";
                            case "-8":
                                return "充值失败！错误原因：订单重发时前后不一致！";
                            case "0":
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
                    return "充值失败！角色不存在！";
                }
            }
            else
            {
                return "充值失败！用户不存在！";
            }
        }

        /// <summary>
        /// 奇迹来了查询接口
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="ServerId">服务器Id</param>
        /// <returns>返回查询信息</returns>
        public GameUserInfo Sel(int UserId, int ServerId)
        {
            gu = gus.GetGameUser(UserId);                                   //获取查询用户
            gs = gss.GetGameServer(ServerId);                              //获取查询用户所在区服
            tstamp = Utils.GetTimeSpan();                                   //获取时间戳
            GameUserInfo gui = new GameUserInfo();                          //定义返回查询结果信息    
            Sign = DESEncrypt.Md5("platid=" + gc.AgentId + "&platuid=" + gu.Id + "&sid=" + gs.ServerNo + "&tm=" + tstamp + gc.SelectTicket, 32);              //获取验证参数
            string SelUrl = "http://" + gc.ExistCom + "?platid=" + gc.AgentId + "&platuid=" + gu.Id + "&sid=" + gs.ServerNo + "&tm=" + tstamp + "&sig=" + Sign;      //获取查询地址
            string SelResult = Utils.GetWebPageContent(SelUrl);             //获取返回结果
            switch (SelResult)
            {
                case "0":
                    gui.UserName = "没有角色";
                    gui.Message = "查询失败！错误原因：该用户不存在";
                    break;
                case "1":
                    gui.UserName = "没有角色";
                    gui.Message = "Success";
                    break;
                case "-1":
                    gui.UserName = "没有角色";
                    gui.Message = "查询失败！错误原因：参数错误！";
                    break;
                case "-2":
                    gui.UserName = "没有角色";
                    gui.Message = "查询失败！错误原因：请求超时！";
                    break;
                case "-3":
                    gui.UserName = "没有角色";
                    gui.Message = "查询失败！错误原因：服务器或平台不存在！";
                    break;
                case "-4":
                    gui.UserName = "没有角色";
                    gui.Message = "查询失败！错误原因：验证失败！";
                    break;
                default:
                    gui.UserName = "没有角色";
                    gui.Message = "查询失败！未知错误！";
                    break;
            }
            return gui;
        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        public Game_Qjll()
        {
            game = games.GetGame("qjll");                                   //获取游戏
            gc = gcs.GetGameConfig(game.Id);                                //获取游戏参数
        }
    }
}
