using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Model;
using Game.DAL;
using Common;

namespace Game.Manager
{
    public class Game_Jdsj : IGame
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
        /// 绝代双骄登陆接口
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="ServerId">服务器Id</param>
        /// <param name="IsPC">是否PC端登陆</param>
        /// <returns>返回登陆地址</returns>
        public string Login(int UserId, int ServerId, int IsPC)
        {
            gu = gus.GetGameUser(UserId);                                  //获取要登录用户
            gs = gss.GetGameServer(ServerId);                              //获取要登录的服务器
            tstamp = DateTime.Now.ToString("yyyyMMddHHmmss");                                  //获取时间戳
            Sign = DESEncrypt.Md5("uid=" + gu.Id + "&uname=" + gu.UserName + "&lgtime=" + tstamp + "&uip=" + BBRequest.GetIP().Replace(".", "_") + "&type=" + gc.AgentId + "&sid=" + gs.ServerNo + "&key=" + gc.LoginTicket, 32);
            string LoginUrl = "http://" + gc.LoginCom + "?uid=" + gu.Id + "&uname=" + gu.UserName + "&lgtime=" + tstamp + "&uip=" + BBRequest.GetIP().Replace(".", "_") + "&type=" + gc.AgentId + "&sid=" + gs.ServerNo + "&sign=" + Sign;
            return LoginUrl;
        }

        /// <summary>
        /// 绝代双骄充值接口
        /// </summary>
        /// <param name="OrderNo">订单号</param>
        /// <returns>返回充值结果</returns>
        public string Pay(string OrderNo)
        {
            order = os.GetOrder(OrderNo);                                   //获取用户的充值订单
            gu = gus.GetGameUser(order.UserName);                           //获取充值用户
            gs = gss.GetGameServer(order.ServerId);                        //获取用户要登录的服务器
            tstamp = DateTime.Now.ToString("yyyyMMddHHmmss");                                   //获取时间戳
            if (gus.IsGameUser(gu.UserName))                                //判断用户是否属于平台
            {
                string PayGold = (order.PayMoney * game.GameMoneyScale).ToString();     //计算支付的游戏币
                Sign = DESEncrypt.Md5("uid=" + gu.Id + "&uname=" + gu.UserName + "&serverid=" + gs.ServerNo + "&point=" + PayGold + "&amount=" + order.PayMoney + "&oid=" + OrderNo + "&time=" + tstamp + "&type=" + gc.AgentId + "&key=" + gc.PayTicket, 32);           //获取验证参数
                string PayUrl = "http://" + gc.PayCom + "?uid=" + gu.Id + "&uname=" + gu.UserName + "&serverid=" + gs.ServerNo + "&point=" + PayGold + "&amount=" + order.PayMoney + "&oid=" + OrderNo + "&time=" + tstamp + "&type=" + gc.AgentId + "&sign=" + Sign + "&format=xml";
                GameUserInfo gui = Sel(gu.Id, gs.Id);                         //获取玩家查询信息
                if (gui.Message == "Success")                                   //判断玩家是否存在
                {
                    if (order.State == 1)                                       //判断订单状态是否为支付状态
                    {
                        string PayResult = Utils.GetWebPageContent(PayUrl);         //获取充值返回结果
                        Dictionary<string, string> list = XmlHelper.ReadXml2(PayResult);                //解析返回结果
                        switch (list["status"])                                          //对充值结果进行解析
                        {
                            case "400":
                                if (os.UpdateOrder(order.OrderNo))                  //更新订单状态为已完成
                                {
                                    gus.UpdateGameMoney(gu.UserName, order.PayMoney);     //跟新玩家游戏消费情况
                                    return "充值成功！";
                                }
                                else
                                {
                                    return "充值成功！错误原因：更新订单状态失败！";
                                }
                            case "401":
                                return "充值失败！错误原因：缺少用户！";
                            case "402":
                                return "充值失败！错误原因：缺少用户名！";
                            case "403":
                                return "充值失败！错误原因：缺少服务器！";
                            case "404":
                                return "充值失败！错误原因：缺少充值点数！";
                            case "405":
                                return "充值失败！错误原因：缺少充值金额！";
                            case "406":
                                return "充值失败！错误原因：缺少订单！";
                            case "407":
                                return "充值失败！错误原因：缺少充值时间！";
                            case "408":
                                return "充值失败！错误原因：缺少运营商！";
                            case "409":
                                return "充值失败！错误原因：验证！";
                            case "410":
                                return "充值失败！错误原因：非法访问IP！";
                            case "411":
                                return "充值失败！未知错误！验证失败！";
                            case "412":
                                return "充值失败！错误原因：用户角色不存在！";
                            case "413":
                                return "充值失败！错误原因：获取角色信息失败！";
                            case "414":
                                return "充值失败！错误原因：内部错误！";
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
        /// 绝代双骄查询接口
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="ServerId">服务器Id</param>
        /// <returns>返回查询结果</returns>
        public GameUserInfo Sel(int UserId, int ServerId)
        {
            gu = gus.GetGameUser(UserId);                                   //获取查询用户信
            gs = gss.GetGameServer(ServerId);                              //获取查询用户所在区服
            tstamp = Utils.GetTimeSpan();                                   //获取时间戳
            Sign = DESEncrypt.Md5("uid=" + gu.Id + "&uname=" + gu.UserName + "&serverid=" + gs.ServerNo + "&type=" + gc.AgentId + "&key=" + gc.SelectTicket, 32);     //获取验证参数
            string SelUrl = "http://" + gc.ExistCom + "?uid=" + gu.Id + "&uname=" + gu.UserName + "&serverid=" + gs.ServerNo + "&type=" + gc.AgentId + "&sign=" + Sign;
            string SelResult = Utils.GetWebPageContent(SelUrl);             //获取返回结果
            GameUserInfo gui = new GameUserInfo();                          //定义返回查询结果信息
            try
            {
                SelResult = SelResult.Substring(0, SelResult.IndexOf('}'));         //处理返回结果
                SelResult = SelResult.Replace(SelResult.Substring(0, SelResult.LastIndexOf('{') + 1), "");
                string[] b = SelResult.Split(',');
                switch (b[0].Substring(9).Replace("\"", ""))
                {
                    case "601":
                        gui.Message = "查询失败！缺少用户！";
                        break;
                    case "602":
                        gui.Message = "查询失败！缺少用户名！";
                        break;
                    case "603":
                        gui.Message = "查询失败！服务器编号不存在！";
                        break;
                    case "604":
                        gui.Message = "查询失败！缺少运营商标识！";
                        break;
                    case "606":
                        gui.Message = "查询失败！缺少验证参数！";
                        break;
                    case "607":
                        gui.Message = "查询失败！非法的访问IP！";
                        break;
                    case "608":
                        gui.Message = "查询失败！验证失败！";
                        break;
                    case "609":
                        gui.Message = "查询失败！用户不存在！";
                        break;
                    case "610":
                        gui.Message = "查询失败！角色不存在！";
                        break;
                    default:
                        string a = b[0].Replace("\\\"", "");
                        gui = new GameUserInfo(b[0].Replace("\\\"", "").Substring(7), gu.UserName, Utils.ConvertUnicodeStringToChinese(b[2].Replace("\\\\\"", "").Substring(6)), int.Parse(b[3].Replace("\\\"", "").Substring(6)), gs.Name, os.GetOrderInfo(gu.UserName), "Success");
                        break;
                }

            }
            catch (Exception)
            {
                gui.Message = "查询失败！查询不到用户信息！";
            }
            return gui;
        }

        /// <summary>
        /// 实例化接口参数
        /// </summary>
        public Game_Jdsj()
        {
            game = games.GetGame("jdsj");                                     //获取游戏
            gc = gcs.GetGameConfig(game.Id);                                //获取游戏参数
        }
    }
}
