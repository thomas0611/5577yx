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
    class Game_Shmc
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
        string time;                                                      //定义时间戳
        string verify;                                                        //定义验证参数

        /// <summary>
        /// 深海迷城登录接口
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ServerId"></param>
        /// <param name="IsPC"></param>
        /// <returns></returns>
        public string Login(int UserId, int ServerId ,int IsPC)
        {
            gu = gus.GetGameUser(UserId );
            gs = gss.GetGameServer(ServerId );
            time = Utils.GetTimeSpan();
            verify = DESEncrypt.Md5(gu.UserName + gs.ServerNo + gc.AgentId + time + 1 + gc.LoginTicket ,32);
            string LoginUrl = "http://" + gc.LoginCom + "?username=" + gu.UserName + "&pfId=" + gc.AgentId + "&serverId=" + gs.ServerNo + "&time=" + time + "&icard=1" + "&verify=" + verify;
            return LoginUrl;
        }

        /// <summary>
        /// 充值接口
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <returns>返回充值信息</returns>
        public string Pay(string  OrderNo)
        {
            order = os.GetOrder(OrderNo);                                       //获取用户的充值订单
            gu = gus.GetGameUser(order.UserName );                              //获取充值用户
            gs = gss.GetGameServer(order.ServerId);                             //获取用户充值的服务器
            string PayGold = (order.PayMoney * game.GameMoneyScale).ToString(); //计算支付的游戏币
            if (gus.IsGameUser(gu.UserName))                                    //判断用户是否属于平台
            {
                time = Utils.GetTimeSpan();
                verify = DESEncrypt.Md5(order.UserName + order.ServerId + gc.AgentId + time + OrderNo + PayGold + order.PayMoney + gc.PayTicket, 32);
                string PayUrl = "http://" + gc.PayCom + "?username=" + order.UserName + "&pfId=" + gc.AgentId + "&serverId=" + order.ServerId + "&serialId=" + OrderNo + "&time=" + time + "&gameCoin=" + PayGold + "&rmb=" + order.PayMoney + "&verify=" + verify;
                GameUserInfo gui = Sel(gu.Id, gs.Id);                            //查询玩家是否存在
                if (gui.Message  == "Success")                                  
                {
                    
                    if (order.State == 1)
                    {
                        string PayResult = Utils.GetWebPageContent(PayUrl);         //获取充值结果
                        switch (PayResult)
                        { 
                            case "0":
                                if (os.UpdateOrder(order.OrderNo))              //更新订单状态为已完成
                                {
                                    gus.UpdateGameMoney(gu.UserName, order.PayMoney);//更新游戏玩家消费信息
                                    return "充值成功";
                                }
                                else 
                                {
                                    return "充值失败！错误原因：更新订单状态失败！";
                                }
                            case "1":
                                return "参数不能为空";
                            case "2":
                                return "流水号已存在";
                            case "3":
                                return "检验码错误";
                            case "4":
                                return "用户名不存在";
                            case "5":
                                return "参数错误";
                            case "6":
                                return "时间戳过期，长度为3分钟";
                            default:
                                break;
                        }
                    }
                    else
                    {
                        return "充值失败！错误原因：无法提交未支付订单！";
                    }
                }
                return "充值失败！角色不存在";

            }
            else
            {
                return "充值失败！用户不存在！";
            }
           
        }

        /// <summary>
        /// 查询用户是否存在
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ServerId"></param>
        /// <returns></returns>
        public GameUserInfo Sel(int UserId, int ServerId)
        {
            gu = gus.GetGameUser(UserId );
            gs = gss.GetGameServer(ServerId );
            time = Utils.GetTimeSpan();
            verify = DESEncrypt.Md5(gu.UserName + gs.ServerNo + gc.AgentId + time + gc.SelectTicket,32);
            string SelUrl = "http://" + gc.ExistCom + "?username=" + gu.UserName + "&pfId=" + gc.AgentId + "&serverId=" + gs.ServerNo + "&time=" + time + "&verify=" + verify;
            GameUserInfo gui = new GameUserInfo();
            try
            {
                string SelResult = Utils.GetWebPageContent(SelUrl );
                
                switch (SelResult )
                {
                    case "0":
                        gui.Message = "Success";
                        break;
                    case "1":
                        gui.Message = "参数不能为空。";
                        break;
                    case "2":
                        gui.Message = "用户名不存在。";
                        break;
                    case "3":
                        gui.Message = "校验码错误。";
                        break;
                    case "6":
                        gui.Message = "时间戳过期";
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                gui.Message = "查询失败！错误原因:" + ex.Message;
            }
            return gui;
        }

        /// <summary>
        /// 初始化参数
        /// </summary>
        public Game_Shmc()
        {
            game = games.GetGame("shmc");                                   //获取游戏
            gc = gcs.GetGameConfig(game.Id);                                //获取游戏参数
        }
    }
}
