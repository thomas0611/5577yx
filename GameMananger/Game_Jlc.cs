using System;
using Common;
using Game.DAL;
using Game.Model;

namespace Game.Manager
{
    public class Game_Jlc : IGame
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
        /// 九龙朝登陆接口
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
            string url = string.Format("sid={0}&uid={1}&time={2}&indulge={3}",
                                      gs.ServerNo, gu.UserName, tstamp, "n");     //获取验证字符串
            Base64Protector bp = new Base64Protector();
            Auth = bp.Base64Code(url);                                      //获取验证码
            Sign = DESEncrypt.Md5(Auth + gc.LoginTicket, 32);               //获取验证参数
            string LoginUrl = "http://" + gc.LoginCom + "?auth=" + Auth + "&sign=" + Sign;       //生成登录地址
            if (IsPC == 1)
            {
                LoginUrl += "&play_gamecode&isclient=1";
            }
            return LoginUrl;
        }

        /// <summary>
        /// 九龙朝充值接口
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
                Sign = DESEncrypt.Md5("sid=" + gs.ServerNo + "&uid=" + gu.UserName + "&oid=" + OrderNo + "&money=" + order.PayMoney + "&gold=" + PayGold + "&time=" + tstamp + gc.PayTicket, 32);                 //获取验证参数
                string PayUrl = "http://" + gc.PayCom + "?sid=" + gs.ServerNo + "&uid=" + gu.UserName + "&oid=" + OrderNo + "&money=" + order.PayMoney + "&gold=" + PayGold + "&time=" + tstamp + "&sign=" + Sign;
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
                            case "-10":
                                return "充值失败！错误原因：服务器编号错误或者不存在！";
                            case "-11":
                                return "充值失败！错误原因：无效的玩家账号！";
                            case "-12":
                                return "充值失败！错误原因：无法提交重复订单！";
                            case "-14":
                                return "充值失败！错误原因：无效时间戳！";
                            case "-15":
                                return "充值失败！错误原因：充值金额错误！";
                            case "-16":
                                return "充值失败！错误原因：虚拟币数量错误！";
                            case "-17":
                                return "充值失败！错误原因：校验码错误！";
                            case "-18":
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
                    return gui.Message;
                }
            }
            else
            {
                return "充值失败！用户不存在！";
            }
        }

        /// <summary>
        /// 九龙朝用户信息查询接口
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="ServerId">服务器Id</param>
        /// <returns>返回查询结果</returns>
        public GameUserInfo Sel(int UserId, int ServerId)
        {
            //九龙朝暂未提供查询接口
            //gu = gus.GetGameUser(UserId);                                   //获取查询用户
            //gs = gss.GetGameServer(ServerId);                              //获取查询用户所在区服
            //tstamp = Utils.GetTimeSpan();                                   //获取时间戳
            GameUserInfo gui = new GameUserInfo();                          //定义返回查询结果信息  
            gui.Message = "Success";
            return gui;
        }

        /// <summary>
        /// 实例化接口参数
        /// </summary>
        public Game_Jlc()
        {
            game = games.GetGame("Jlc");                                   //获取游戏
            gc = gcs.GetGameConfig(game.Id);                                //获取游戏参数
        }
    }
}
