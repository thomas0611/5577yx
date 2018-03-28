using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.DAL;
using Game.Model;
using Common;

namespace Game.Manager
{
    public class Game_Tj : IGame
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
        string Auth;                                                        //定义验证码

        /// <summary>
        /// 天界登录接口
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="ServerId">服务器Id</param>
        /// <returns>返回登录地址</returns>
        public string Login(int UserId, int ServerId, int IsPC)
        {
            gu = gus.GetGameUser(UserId);                                   //获取要登录用户
            gs = gss.GetGameServer(ServerId);                              //获取要登录的服务器
            tstamp = Utils.GetTimeSpan();                                   //获取时间戳
            Base64Protector bp = new Base64Protector();
            Auth = bp.Base64Code(string.Format("sid={0}&uid={1}&time={2}&indulge={3}", gs.ServerNo, gu.UserName, tstamp, "n"));
            Sign = DESEncrypt.Md5(Auth + gc.LoginTicket, 32);       //获取验证参数
            string LoginUrl = "http://" + gc.LoginCom + "?auth=" + Auth + "&sign=" + Sign;
            return LoginUrl;
        }

        /// <summary>
        /// 天界充值接口
        /// </summary>
        /// <param name="OrderNo">订单号</param>
        /// <returns>返回充值结果</returns>
        public string Pay(string OrderNo)
        {
            order = os.GetOrder(OrderNo);                                   //获取用户的充值订单
            gu = gus.GetGameUser(order.UserName);                           //获取充值用户
            gs = gss.GetGameServer(order.ServerId);                        //获取用户要登录的服务器
            if (gus.IsGameUser(gu.UserName))                                //判断用户是否属于平台
            {
                tstamp = Utils.GetTimeSpan();                                   //获取时间戳
                string PayGold = (order.PayMoney * game.GameMoneyScale).ToString();     //计算支付的游戏币
                Auth = gc.AgentId + OrderNo.Substring(0, 19) + gu.UserName + "212" + gs.ServerNo + order.PayMoney + PayGold;
                Sign = DESEncrypt.Md5(Auth + gc.PayTicket, 32);                 //获取验证参数
                string PayUrl = "http://" + gc.PayCom + "jointoppay/?pid=" + gc.AgentId + "&order_id=" + OrderNo.Substring(0, 19) + "&uid=" + gu.UserName + "&gid=" + "212" + "&sid=" + gs.ServerNo + "&order_amount=" + order.PayMoney + "&point=" + PayGold + "&money=" + order.PayMoney + "&sign=" + Sign;
                GameUserInfo gui = Sel(gu.Id, gs.Id);                         //获取玩家查询信息
                if (gui.Message == "Success")                                   //判断玩家是否存在
                {
                    if (order.State == 1)                                       //判断订单状态是否为支付状态
                    {
                        string PayResult = Utils.GetWebPageContent(PayUrl);         //获取充值返回结果
                        switch (PayResult)                                          //对充值结果进行解析
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
                            case "-10":
                                return "充值失败！无效服务器编号！";
                            case "-11":
                                return "充值失败！无效玩家帐号！";
                            case "-12":
                                return "充值失败！无法提交重复订单！";
                            case "-14":
                                return "充值失败！无效时间戳！";
                            case "-15":
                                return "充值失败！冲值金额错误！";
                            case "-16":
                                return "充值失败！充值元宝数错误！";
                            case "-17":
                                return "充值失败！校验码错误！";
                            case "-18":
                                return "充值失败！非法的访问IP！";
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
                    return "角色不存在！";
                }
            }
            else
            {
                return "充值失败！用户不存在！";
            }
        }

        /// <summary>
        /// 天界查询接口
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="ServerId">服务器Id</param>
        /// <returns>放回查询结果</returns>
        public GameUserInfo Sel(int UserId, int ServerId)
        {
            gu = gus.GetGameUser(UserId);                                   //获取查询用户信
            gs = gss.GetGameServer(ServerId);                              //获取查询用户所在区服
            tstamp = Utils.GetTimeSpan();                                   //获取时间戳
            Sign = DESEncrypt.Md5("secret_key" + "api_key" + gc.AgentId + "server_id" + gs.ServerNo + "timestamp" + tstamp + "user_id" + gu.UserName, 32);     //获取验证参数
            string SelUrl = "http://" + gc.ExistCom + "?api_key=" + gc.AgentId + "&user_id=" + gu.UserName + "&server_id=" + gs.ServerNo + "timestamp" + tstamp + "&sign=" + Sign;
            string SelResult = Utils.GetWebPageContent(SelUrl);             //获取返回结果
            GameUserInfo gui = new GameUserInfo();                          //定义返回查询结果信息
            try
            {
                switch (SelResult)
                {
                    case "ERROR_-1":
                        gui.Message = "系统内部错误！";
                        break;
                    case "ERROR_-100":
                        gui.Message = "传入参数不符合规则！";
                        break;
                    case "ERROR_-200":
                        gui.Message = "系统错误！";
                        break;
                    case "ERROR_-500":
                        gui.Message = "数据库操作失误！";
                        break;
                    case "ERROR_-1406":
                        gui.Message = "角色不存在！";
                        break;
                    case "ERROR_-1814":
                        gui.Message = "超过方法最大调用次数！";
                        break;
                    case "ERROR_-11000":
                        gui.Message = "非法IP访问";
                        break;
                    default:
                        gui = new GameUserInfo(gu.Id.ToString(), gu.UserName, SelResult, 0, gs.Name, os.GetOrderInfo(gu.UserName), "Success");
                        break;
                }
            }
            catch (Exception)
            {
                gui.Message = "Error";
            }
            return gui;
        }

        /// <summary>
        /// 初始化接口参数
        /// </summary>
        public Game_Tj()
        {
            game = games.GetGame("tj");                                     //获取游戏
            gc = gcs.GetGameConfig(game.Id);                                //获取游戏参数
        }
    }
}
