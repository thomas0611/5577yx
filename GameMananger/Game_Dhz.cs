using System;
using Common;
using Game.DAL;
using Game.Model;

namespace Game.Manager
{
    public class Game_Dhz : IGame
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
        /// 盗魂传登陆接口
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
            string LoginUrl = "";
            Sign = DESEncrypt.Md5(gc.AgentId + "|" + gu.UserName + "|" + gs.ServerNo + "|" + tstamp + "|" + "2" + "|" + gc.LoginTicket, 32);               //获取验证参数
            if (IsPC == 0)
            {
                string Url = "http://" + gc.LoginCom + "?ptid=" + gc.AgentId + "&account=" + gu.UserName + "&areaid=" + gs.ServerNo + "&tt=" + tstamp + "&fcm=" + 2 + "&sign=" + Sign;       //生成登录地址
                LoginUrl = Url;
            }
            else if (IsPC == 1)
            {
                string Url = "http://" + gc.LoginCom + "?ptid=" + gc.AgentId + "&account=" + gu.UserName + "&areaid=" + gs.ServerNo + "&tt=" + tstamp + "&fcm=" + 2 + "&sign=" + Sign + "&logintype=2";       //生成登录地址
                LoginUrl = Url;
            }
            return LoginUrl;
        }

        /// <summary>
        /// 盗魂传充值接口
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
                Sign = DESEncrypt.Md5(gc.AgentId + "|" + gu.UserName + "|" + gs.ServerNo + "|" + tstamp + "|" + OrderNo + "|" + PayGold + "|" + gc.PayTicket, 32);                 //获取验证参数
                string PayUrl = "http://" + gc.PayCom + "?ptid=" + gc.AgentId + "&account=" + gu.UserName + "&areaid=" + gs.ServerNo + "&tt=" + tstamp + "&orderno=" + OrderNo + "&point=" + PayGold + "&sign=" + Sign;
                GameUserInfo gui = Sel(gu.Id, gs.Id);                           //获取玩家查询信息
                if (gui.Message == "Success")                                   //判断玩家是否存在
                {
                    if (order.State == 1)                                       //判断订单状态是否为支付状态
                    {
                        string PayResult = Utils.GetWebPageContent(PayUrl);         //获取充值结果
                        PayResult = PayResult.Substring(0, PayResult.IndexOf('}'));                      //处理返回结果
                        PayResult = PayResult.Replace(PayResult.Substring(0, PayResult.LastIndexOf('{') + 1), "");
                        string[] b = PayResult.Split(',');
                        switch (b[0].Substring(13))                             //对充值结果进行解析
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
                                return "订单重复，之前已经处理成功！";
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
        /// 盗魂传角色查询接口
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <param name="ServerId">服务器Id</param>
        /// <returns>返回查询结果</returns>
        public Model.GameUserInfo Sel(int UserId, int ServerId)
        {
            gu = gus.GetGameUser(UserId);                                   //获取查询用户
            gs = gss.GetGameServer(ServerId);                              //获取查询用户所在区服
            tstamp = Utils.GetTimeSpan();                                   //获取时间戳
            GameUserInfo gui = new GameUserInfo();                          //定义返回查询结果信息                                                              //获取验证码
            Sign = DESEncrypt.Md5(gc.AgentId + "|" + tstamp + "|" + gc.SelectTicket, 32);              //获取验证参数
            string SelUrl = "http://" + gc.ExistCom + "?type=character&ptid=" + gc.AgentId + "&tt=" + tstamp + "&account=" + gu.UserName + "&sign=" + Sign + "";      //获取查询地址
            string SelResult = Utils.GetWebPageContent(SelUrl);             //获取返回结果
            try
            {
                SelResult = SelResult.Substring(0, SelResult.IndexOf('}'));                      //处理返回结果
                SelResult = SelResult.Replace(SelResult.Substring(0, SelResult.LastIndexOf('{') + 1), "");
                string[] b = SelResult.Split(',');
                gui = new GameUserInfo(b[0].Substring(14), gu.UserName, b[3].Substring(16).Replace("\'", ""), b[12].Substring(8) == "null" ? 0 : int.Parse(b[12].Substring(8)), gs.Name, os.GetOrderInfo(gu.UserName), "Success");
            }
            catch (Exception)
            {
                gui.UserName = "没有角色";
                gui.Message = "error";
            }
            return gui;
        }


        /// <summary>
        /// 实例化接口参数
        /// </summary>
        public Game_Dhz()
        {
            game = games.GetGame("dhz");                                   //获取游戏
            gc = gcs.GetGameConfig(game.Id);                                //获取游戏参数
        }
    }
}
