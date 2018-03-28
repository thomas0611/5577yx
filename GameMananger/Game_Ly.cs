using System;
using Common;
using Game.DAL;
using Game.Model;
namespace Game.Manager
{
    public class Game_Ly : IGame
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
        /// 烈焰登陆接口
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="ServerId">服务器Id</param>
        /// <param name="IsPC">是否PC端登陆</param>
        /// <returns>返回登陆地址</returns>
        public string Login(int UserId, int ServerId, int IsPC)
        {
            gu = gus.GetGameUser(UserId);                                  //获取当前登录用户
            gs = gss.GetGameServer(ServerId);                              //获取用户要登录的服务器
            tstamp = Utils.GetTimeSpan();                                  //获取时间戳
            Sign = DESEncrypt.Md5("id=" + gu.UserName + "&now=" + tstamp + "." + gc.LoginTicket, 32);            //获取验证码
            string LoginUrl = "http://" + gs.ServerNo + "." + gc.LoginCom + "?id=" + gu.UserName + "&time=" + tstamp + "&isAdult=1&is_client=" + IsPC + "&sign=" + Sign;
            return LoginUrl;
        }

        /// <summary>
        /// 烈焰充值接口
        /// </summary>
        /// <param name="OrderNo">订单号</param>
        /// <returns>充值结果</returns>
        public string Pay(string OrderNo)
        {
            order = os.GetOrder(OrderNo);                                   //获取用户的充值订单
            gu = gus.GetGameUser(order.UserName);                           //获取充值用户
            gs = gss.GetGameServer(order.ServerId);                        //获取用户要充值的服务器
            string PayGold = (order.PayMoney * game.GameMoneyScale).ToString();     //计算支付的游戏币
            if (gus.IsGameUser(gu.UserName))                                //判断用户是否属于平台
            {
                tstamp = Utils.GetTimeSpan();                               //获取时间戳
                Sign = DESEncrypt.Md5("ly" + gc.AgentId + gs.ServerNo + gu.UserName + OrderNo + order.PayMoney + PayGold + tstamp + gc.PayTicket, 32).ToUpper();
                string PayUrl = "http://" + gc.PayCom + "?game=ly&agent=" + gc.AgentId + "&user=" + gu.UserName + "&order=" + OrderNo + "&money=" + order.PayMoney + "&coin=" + PayGold + "&server=" + gs.ServerNo + "&time=" + tstamp + "&sign=" + Sign;
                GameUserInfo gui = Sel(gu.Id, gs.Id);                       //获取玩家查询信息
                if (gui.Message == "Success")                               //判断玩家是否存在
                {
                    if (order.State == 1)                                   //判断订单状态是否为支付状态
                    {
                        string PayResult = Utils.GetWebPageContent(PayUrl);         //获取充值结果
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
                            case "0":
                                return "充值失败！参数不正确！";
                            case "2":
                                return "充值失败！合作方不存在！";
                            case "3":
                                return "充值失败！金额有误！";
                            case "4":
                                return "充值失败！不存在的服务器！";
                            case "5":
                                return "充值失败！验证失败！";
                            case "6":
                                return "充值失败！充值游戏不存在！";
                            case "7":
                                return "充值失败！玩家角色不存在！";
                            case "8":
                                return "充值失败！时间超时（大于5分钟）！";
                            case "9":
                                return "充值失败！请求过于频繁！";
                            case "-7":
                                return "充值失败！无法提交相同订单！";
                            case "-1":
                                return "充值失败！非法的访问IP！";
                            case "-4":
                                return "充值失败！验证失败！";
                            case "-102":
                                return "充值失败！充值异常，游戏方无响应！";
                            case "-103":
                                return "充值失败！充值异常，充值接口异常！";
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
                    return "充值失败！错误原因：不存在角色！";
                }
            }
            else
            {
                return "充值失败！错误原因：用户不存在";
            }
        }

        /// <summary>
        /// 烈焰用户查询接口
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="ServerId">服务器Id</param>
        /// <returns>查询结果</returns>
        public GameUserInfo Sel(int UserId, int ServerId)
        {
            gu = gus.GetGameUser(UserId);                                   //获取查询用户
            gs = gss.GetGameServer(ServerId);                              //获取查询用户所在区服
            tstamp = Utils.GetTimeSpan();                                   //获取时间戳
            GameUserInfo gui = new GameUserInfo();                          //定义返回查询结果信息                           
            Sign = DESEncrypt.Md5(gu.UserName + "." + tstamp + "." + gc.SelectTicket, 32);         //获取验证码
            string SelUrl = "http://" + gs.ServerNo + "." + gc.ExistCom + "?id=" + gu.UserName + "&time=" + tstamp + "&sign=" + Sign;
            string SelResult = Utils.GetWebPageContent(SelUrl);             //获取返回结果
            try
            {
                SelResult = SelResult.Substring(0, SelResult.IndexOf('}'));         //处理返回结果
                SelResult = SelResult.Replace(SelResult.Substring(0, SelResult.LastIndexOf('{') + 1), "");
                string[] b = SelResult.Split(',');
                gui = new GameUserInfo(gu.Id.ToString(), gu.UserName, Utils.ConvertUnicodeStringToChinese(b[0].Substring(7).Replace("\"", "")), int.Parse(b[1].Substring(8).Replace("\"", "")), gs.Name, os.GetOrderInfo(gu.UserName), "Success");
            }
            catch (Exception)
            {
                gui.Message = "error";
            }
            return gui;
        }

        /// <summary>
        /// 实例化接口参数
        /// </summary>
        public Game_Ly()
        {
            game = games.GetGame("ly");                                   //获取游戏
            gc = gcs.GetGameConfig(game.Id);                                //获取游戏参数
        }
    }
}
