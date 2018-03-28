using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.DAL;
using Game.Model;
using Common;

namespace Game.Manager
{
    public class Game_Wz : IGame
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
        /// 武尊登陆接口
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="ServerId">服务器Id</param>
        /// <param name="IsPC">是否PC端登陆</param>
        /// <returns>返回登陆地址</returns>
        public string Login(int UserId, int ServerId, int IsPC)
        {
            gu = gus.GetGameUser(UserId);                                   //获取当前登录用户
            gs = gss.GetGameServer(ServerId);                              //获取用户要登录的服务器
            tstamp = Utils.GetTimeSpan();                                   //获取时间戳
            Sign = DESEncrypt.Md5("uname=" + gu.UserName + "&time=" + tstamp + gc.LoginTicket, 32);               //获取验证参数
            string LoginUrl = "http://" + gs.ServerNo + "." + gc.LoginCom + "?uname=" + gu.UserName + "&time=" + tstamp + "&sign=" + Sign + "&isAdult=1";       //生成登录地址
            return LoginUrl;
        }

        /// <summary>
        /// 武尊充值接口
        /// </summary>
        /// <param name="OrderNo">订单号</param>
        /// <returns>返回订单结果</returns>
        public string Pay(string OrderNo)
        {
            order = os.GetOrder(OrderNo);                                   //获取用户的充值订单
            gu = gus.GetGameUser(order.UserName);                           //获取充值用户
            gs = gss.GetGameServer(order.ServerId);                        //获取用户要充值的服务器
            string PayGold = (order.PayMoney * game.GameMoneyScale).ToString();     //计算支付的游戏币
            if (gus.IsGameUser(gu.UserName))                                //判断用户是否属于平台
            {
                tstamp = Utils.GetTimeSpan();                                   //获取时间戳
                Sign = DESEncrypt.Md5("7" + gc.AgentId + gs.QuFu + gu.Id + gu.UserName + OrderNo + order.PayMoney + tstamp + gc.PayTicket, 32);                 //获取验证参数
                string PayUrl = "http://" + gc.PayCom + "?orderid=" + OrderNo + "&partnerid=" + gc.AgentId + "&gameid=7&sid=" + gs.QuFu + "&userid=" + gu.Id + "&username=" + gu.UserName + "&actorid=&actorname=&money=" + order.PayMoney + "&time=" + tstamp + "&sign=" + Sign;
                GameUserInfo gui = Sel(gu.Id, gs.Id);                           //获取玩家查询信息
                if (gui.Message == "Success")                                     //判断玩家是否存在
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
                                    return "充值失败！错误原因：更新订单状态失败！";
                                }
                            case "2":
                                return "充值失败！错误原因：缺少参数！";
                            case "3":
                                return "充值失败！错误原因：验证失败！";
                            case "4":
                                return "充值失败！错误原因：非法的访问IP！";
                            case "5":
                                return "充值失败！错误原因：合作方不存在！";
                            case "6":
                                return "充值失败！错误原因：充值的游戏不存在！";
                            case "7":
                                return "充值失败！错误原因：游戏未登记！";
                            case "8":
                                return "充值失败！错误原因：游戏服务器未登记！";
                            case "9":
                                return "充值失败！错误原因：合作商未开启！";
                            case "10":
                                return "充值失败！错误原因：游戏未开启！";
                            case "11":
                                return "充值失败！错误原因：游戏服务器未开启！";
                            case "12":
                                return "充值失败！错误原因：充值金额不对！";
                            case "13":
                                return "充值失败！错误原因：游戏币金额不对！";
                            case "14":
                                return "充值失败！错误原因：游戏未创建！";
                            case "15":
                                return "充值失败！错误原因：请求超时！";
                            case "-1":
                                return "充值失败！错误原因：玩家角色不存在！";
                            case "-2":
                                return "充值失败！错误原因：无法提交重复的订单号！";
                            case "-3":
                                return "充值失败！错误原因：游戏签名失败！";
                            case "-4":
                                return "充值失败！错误原因：游戏非法的访问IP！";
                            case "-5":
                                return "充值失败！错误原因：游戏缺少参数！";
                            case "-6":
                                return "充值失败！错误原因：充值失败！";
                            case "-7":
                                return "充值失败！错误原因：充值异常！";
                            case "-8":
                                return "充值失败！错误原因：游戏请求超时！";
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
        /// 武尊查询接口
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
            Sign = DESEncrypt.Md5(gu.UserName + gc.SelectTicket, 32);              //获取验证参数
            string SelUrl = "http://" + gs.ServerNo + "." + gc.ExistCom + "?username=" + gu.UserName + "&verify=" + Sign;      //获取查询地址
            string SelResult = Utils.GetWebPageContent(SelUrl);             //获取返回结果
            try
            {
                switch (SelResult)
                {
                    case "-1":
                        gui.UserName = "没有角色";
                        gui.Message = "缺少参数！";
                        break;
                    case "-2":
                        gui.UserName = "没有角色";
                        gui.Message = "验证参数错误！";
                        break;
                    case "0":
                        gui.UserName = "没有角色";
                        gui.Message = "账号下未找到角色！";
                        break;
                    default:
                        SelResult = SelResult.Substring(0, SelResult.IndexOf('}'));                      //处理返回结果
                        SelResult = SelResult.Replace(SelResult.Substring(0, SelResult.LastIndexOf('{') + 1), "");
                        string[] b = SelResult.Split(',');
                        gui = new GameUserInfo(gu.Id.ToString(), gu.UserName, Utils.UrlDecode(b[0].Substring(10).Replace("\"", "")), int.Parse(b[1].Substring(5).Replace("\"", "")), gs.Name, os.GetOrderInfo(gu.UserName), "Success");
                        break;
                }

            }
            catch (Exception)
            {
                gui.UserName = "没有角色";
                gui.Message = "查询失败！";
            }
            return gui;
        }

        /// <summary>
        /// 实例化接口参数
        /// </summary>
        public Game_Wz()
        {
            game = games.GetGame("wz");                                     //获取游戏
            gc = gcs.GetGameConfig(game.Id);                                //获取游戏参数
        }
    }
}
