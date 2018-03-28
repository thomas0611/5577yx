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
    public class Game_Qh : IGame
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
        /// 枪魂登录接口
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="ServerId">服务器Id</param>
        /// <param name="IsPC">是否PC端</param>
        /// <returns>返回登录地址</returns>
        public string Login(int UserId, int ServerId, int IsPC)
        {
            gu = gus.GetGameUser(UserId);                                   //获取当前登录用户
            gs = gss.GetGameServer(ServerId);                              //获取用户要登录的服务器
            tstamp = Utils.GetTimeSpan();
            Sign = DESEncrypt.Md5(gu.UserName + tstamp + gc.LoginTicket + gs.QuFu, 32);               //获取验证参数
            string LoginUrl = "http://" + gs.ServerNo + "." + gc.LoginCom + "?username=" + gu.UserName + "&time=" + tstamp + "&flag=" + Sign + "&serverid=" + gs.QuFu + "";       //生成登录地址
            return LoginUrl;
        }

        /// <summary>
        /// 枪魂充值接口
        /// </summary>
        /// <param name="OrderNo"></param>
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
                Sign = DESEncrypt.Md5(gu.UserName + tstamp + OrderNo + PayGold + order.PayMoney + gs.QuFu + gc.PayTicket, 32);                //获取验证参数
                string PayUrl = "http://" + gs.ServerNo + "." + gc.PayCom + "?username=" + gu.UserName + "&time=" + tstamp + "&orderid=" + OrderNo + "&coin=" + PayGold + "&money=" + order.PayMoney + "&flag=" + Sign + "&serverid=" + gs.QuFu;
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
                                    return "充值失败！错误原因：更新订单状态失败！";
                                }
                            case "-1":
                                return "充值失败！错误原因：验证失败！";
                            case "-2":
                                return "充值失败！错误原因：参数不正确！";
                            case "-3":
                                return "充值失败！错误原因：无法提交重复订单！";
                            case "-4":
                                return "充值失败！错误原因：服务器内部错误！";
                            case "-5":
                                return "充值失败！错误原因：其他错误！";
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
        /// 枪魂查询接口
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="ServerId">服务器Id</param>
        /// <returns>返回用户角色信息</returns>
        public GameUserInfo Sel(int UserId, int ServerId)
        {
            gu = gus.GetGameUser(UserId);                                   //获取查询用户
            gs = gss.GetGameServer(ServerId);                              //获取查询用户所在区服
            tstamp = Utils.GetTimeSpan();                                   //获取时间戳
            GameUserInfo gui = new GameUserInfo();                          //定义返回查询结果信息    
            Sign = DESEncrypt.Md5(gu.UserName + tstamp + gc.SelectTicket + gs.QuFu, 32);              //获取验证参数
            string SelUrl = "http://" + gs.ServerNo + "." + gc.ExistCom + "?username=" + gu.UserName + "&time=" + tstamp + "&flag=" + Sign + "&serverid=" + gs.QuFu;      //获取查询地址
            try
            {
                string SelResult = Utils.GetWebPageContent(SelUrl);             //获取返回结果
                Dictionary<string, string> Jd = Json.JsonToArray(SelResult);
                gui = new GameUserInfo(Jd["userid"], gu.UserName, Utils.ConvertUnicodeStringToChinese(Jd["nickname"]), int.Parse(Jd["level"]), gs.Name, os.GetOrderInfo(gu.UserName), "Success");

            }
            catch (Exception)
            {
                gui.UserName = "没有角色";
                gui.Message = "error";
            }
            return gui;
        }

        /// <summary>
        /// 实例化参数
        /// </summary>
        public Game_Qh()
        {
            game = games.GetGame("qh");                                   //获取游戏
            gc = gcs.GetGameConfig(game.Id);                                //获取游戏参数
        }
    }
}
