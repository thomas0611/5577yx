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
    class Game_Ahxx
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
        string time;                                                        //定义时间戳
        string sign;                                                        //签名

        public string Login(int UserId, int ServerId, int IsPC)
        {
            gu = gus.GetGameUser(UserId);
            gs = gss.GetGameServer(ServerId);
            time = Utils.GetTimeSpan();
            sign = DESEncrypt.Md5(gu.UserName + gs.QuFu + gc.LoginTicket + time, 32);
            string LoginUrl = "http://" + gs.ServerNo + "." + gc.LoginCom + "?spid=" + gc.AgentId + "&username=" + gu.UserName + "&server_num=" + gs.QuFu + "&fcm=1" + "&time=" + time + "&sign=" + sign;
            return LoginUrl;
        }

        public string Pay(string OrderNo)
        {
            order = os.GetOrder(OrderNo);
            gu = gus.GetGameUser(order.UserName);
            gs = gss.GetGameServer(order.ServerId);
            if (gus.IsGameUser(gu.UserName))
            {
                string Gold = (order.PayMoney * game.GameMoneyScale).ToString();
                time = Utils.GetTimeSpan();
                sign = DESEncrypt.Md5(gu.UserName + Gold + gs.QuFu + OrderNo + gc.PayTicket, 32);
                string PayUrl = "http://" + gs.ServerNo + "." + gc.PayCom + "?spid=" + gc.AgentId + "&username=" + gu.UserName + "&order=" + OrderNo + "&server_num=" + gs.QuFu + "&rmb=" + order.PayMoney + "&gold=" + Gold + "&time=" + time + "&sign=" + sign;
                GameUserInfo gui = Sel(gu.Id, gs.Id);
                if (gui.Message == "Success")
                {
                    if (order.State == 1)
                    {
                        string PayResult = Utils.GetWebPageContent(PayUrl);
                        switch (PayResult)
                        {
                            case "1":
                                if (os.UpdateOrder(order.OrderNo))
                                {
                                    gus.UpdateGameMoney(gu.UserName, order.PayMoney);
                                    return "充值成功";
                                }
                                else
                                {
                                    return "充值失败！错误原因：更新订单状态失败！";
                                }
                            case "2":
                                return "用户名不存在";
                            case "3":
                                return "Sign错误";
                            case "4":
                                return "订单号已存在";
                            case "5":
                                return "充值金额有错(最小金额为:10元宝)";
                            case "6":
                                return "ip限制";
                            case "7":
                                return "平台或区服不存在";
                            case "8":
                                return "参数不全";
                            case "9":
                                return "未知错误";
                            default:
                                return "未知错误";
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

        public GameUserInfo Sel(int UserId, int ServerId)
        {
            gu = gus.GetGameUser(UserId);
            gs = gss.GetGameServer(ServerId);
            time = Utils.GetTimeSpan();
            sign = DESEncrypt.Md5("get_player_info_" + time + "_" + gc.SelectTicket, 32);
            string SelUrl = "http://" + gs.ServerNo + "." + gc.ExistCom + "?spid=" + gc.AgentId + "&users=" + gu.UserName + "&server_num=" + gs.QuFu + "&time=" + time + "&sign=" + sign;
            GameUserInfo gui = new GameUserInfo();
            try
            {
                string SelResult = Utils.GetWebPageContent(SelUrl);             //获取返回结果
                string status = SelResult.Substring(1,SelResult.IndexOf(',')-1);
                Dictionary<string, string> Jd = Json.JsonToArray(SelResult);
                switch (status)
                {
                    case "\"status\":1":
                        gui = new GameUserInfo(gu.Id.ToString(), gu.UserName, Utils.UrlDecode(Jd["name"]), int.Parse(Jd["level"]), gs.QuFu, os.GetOrderInfo(gu.UserName), "Success");
                        break;
                    case "\"status\":2":
                        gui.Message = "参数错误";
                        break;
                    case "\"status\":3":
                        gui.Message = "签名错误";
                        break;
                    case "\"status\":4":
                        gui.Message = "平台或区服错误";
                        break;
                    case "\"status\":5":
                        gui.Message = "查询失败";
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
                gui.UserName = "没有角色";
                gui.Message = "error";
            }
            return gui;
        }

        public Game_Ahxx()
        {
            game = games.GetGame("Ahxx");
            gc = gcs.GetGameConfig(game.Id); 
        }
    }
}
