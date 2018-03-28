using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.DAL;
using Game.Model;
using Common;

namespace Game.Manager
{
    public class Game_Dxz : IGame
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
        /// 大侠传登录接口
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="ServerId">服务器Id</param>
        /// <returns>返回登录地址</returns>
        public string Login(int UserId, int ServerId, int IsPC)
        {
            gu = gus.GetGameUser(UserId);                                   //获取当前登录用户
            gs = gss.GetGameServer(ServerId);                              //获取用户要登录的服务器
            tstamp = Utils.GetTimeSpan();                                   //获取时间戳
            string url = string.Format("op_id={0}&sid={1}&game_id={2}&account={3}&adult_flag={4}&game_time={5}&ip={6}&ad_info={7}&time={8}",
                                      gc.AgentId, gs.ServerNo, 8, gu.UserName, 1, 13359459, BBRequest.GetIP(), "", tstamp);     //获取验证字符串
            Base64Protector bp = new Base64Protector();
            Auth = bp.Base64Code(url);                                      //获取验证码
            Sign = DESEncrypt.Md5(Auth + gc.LoginTicket, 32);               //获取验证参数
            string LoginUrl = "http://" + gc.LoginCom + "?auth=" + Auth + "&verify=" + Sign;       //生成登录地址
            return LoginUrl;
        }

        /// <summary>
        /// 大侠传充值接口
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
                string url = string.Format("op_id={0}&sid={1}&game_id={2}&account={3}&order_id={4}&game_money={5}&u_money={6}&time={7}",
                                            gc.AgentId, gs.ServerNo, 8, gu.UserName, OrderNo, PayGold, order.PayMoney, tstamp);        //获取验证字符串
                Base64Protector bp = new Base64Protector();
                Auth = bp.Base64Code(url);                                      //获取验证码
                Sign = DESEncrypt.Md5(Auth + gc.PayTicket, 32);                 //获取验证参数
                string PayUrl = "http://" + gc.PayCom + "?auth=" + Auth + "&verify=" + Sign;
                GameUserInfo gui = Sel(gu.Id, gs.Id);                           //获取玩家查询信息
                if (gui.Message == "Success")                                     //判断玩家是否存在
                {
                    if (order.State == 1)                                       //判断订单状态是否为支付状态
                    {
                        string PayResult = Utils.GetWebPageContent(PayUrl);         //获取充值结果
                        PayResult = PayResult.Substring(0, PayResult.IndexOf('}')); //处理充值结果
                        PayResult = PayResult.Replace(PayResult.Substring(0, PayResult.LastIndexOf('{') + 1), "");
                        switch (PayResult.Substring(9))                             //对充值结果进行解析
                        {
                            case "0":
                                if (os.UpdateOrder(order.OrderNo))                  //更新订单状态为已完成
                                {
                                    gus.UpdateGameMoney(gu.UserName, order.PayMoney);     //跟新玩家游戏消费情况
                                    return "充值成功！";
                                }
                                else
                                {
                                    return "充值失败！错误原因：更新订单状态失败！";
                                }
                            case "1":
                                return "充值失败！错误原因：商家错误或者不存在！";
                            case "2":
                                return "充值失败！错误原因：服务器不存在！";
                            case "3":
                                return "充值失败！错误原因：游戏不存在！";
                            case "4":
                                return "充值失败！错误原因：无效时间戳！";
                            case "5":
                                return "充值失败！错误原因：检验码错误！";
                            case "6":
                                return "充值失败！错误原因：缺少参数！";
                            case "7":
                                return "充值失败！错误原因：订单号不存在！";
                            case "8":
                                return "充值失败！错误原因：角色不存在！";
                            case "9":
                                return "充值失败！错误原因：排行榜类型不存在！";
                            case "10":
                                return "充值失败！错误原因：方法名字错误！";
                            case "11":
                                return "充值失败！错误原因：非法IP！";
                            case "12":
                                return "充值失败！错误原因：充值金额不对！";
                            case "13":
                                return "充值失败！错误原因：游戏币金额不对！";
                            case "14":
                                return "充值失败！错误原因：游戏币和游戏币币比列不对！";
                            case "100":
                                return "充值失败！未知原因";
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
        /// 大侠传查询接口
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="ServerId">服务器Id</param>
        /// <returns></returns>
        public GameUserInfo Sel(int UserId, int ServerId)
        {
            gu = gus.GetGameUser(UserId);                                   //获取查询用户
            gs = gss.GetGameServer(ServerId);                              //获取查询用户所在区服
            tstamp = Utils.GetTimeSpan();                                   //获取时间戳
            GameUserInfo gui = new GameUserInfo();                          //定义返回查询结果信息               
            string url = string.Format("op_id={0}&sid={1}&game_id={2}&accounts={3}&time={4}",
                                          gc.AgentId, gs.ServerNo, 8, gu.UserName, tstamp);         //获取验证字符串
            Base64Protector bp = new Base64Protector();
            Auth = bp.Base64Code(url);                                      //获取验证码
            Sign = DESEncrypt.Md5(Auth + gc.SelectTicket, 32);              //获取验证参数
            string SelUrl = "http://" + gc.ExistCom + "?auth=" + Auth + "&verify=" + Sign;      //获取查询地址
            string SelResult = Utils.GetWebPageContent(SelUrl);             //获取返回结果
            try
            {
                SelResult = SelResult.Substring(0, SelResult.IndexOf('}'));                      //处理返回结果
                SelResult = SelResult.Replace(SelResult.Substring(0, SelResult.LastIndexOf('{') + 1), "");
                string[] b = SelResult.Split(',');
                gui = new GameUserInfo(b[0].Substring(12), gu.UserName, Utils.ConvertUnicodeStringToChinese(b[1].Substring(7).Replace("\"", "")), int.Parse(b[6].Substring(8).Replace("\"", "")), gs.Name, os.GetOrderInfo(gu.UserName), "Success");
            }
            catch (Exception)
            {
                gui.UserName = "没有角色";
                gui.Message = "Error";
            }
            return gui;
        }

        /// <summary>
        /// 初始化接口参数
        /// </summary>
        public Game_Dxz()
        {
            game = games.GetGame("dxz");                                     //获取游戏
            gc = gcs.GetGameConfig(game.Id);                                //获取游戏参数
        }
    }
}
