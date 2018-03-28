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
    class Game_Fyws :IGame
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
        string sign;                                                      //签名

        /// <summary>
        /// 风云无双登陆接口
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ServerId"></param>
        /// <param name="IsPC"></param>
        /// <returns></returns>
        public string Login(int UserId, int ServerId, int IsPC)
        {
            gu = gus.GetGameUser(UserId);
            gs = gss.GetGameServer(ServerId);
            time = Utils.GetTimeSpan();
            sign = DESEncrypt.Md5("qid=" + gu.Id + "&time=" + time + "&server_id=" + gs.ServerNo + gc.LoginTicket, 32);
            string LoginUrl = "http://" +gs.ServerNo + "." + gc.LoginCom + "?qid=" + gu.Id + "&server_id=" + gs.ServerNo + "&time=" + time + "&sign=" + sign + "&isAdult=1";
            return LoginUrl;
        }

        /// <summary>
        /// 充值接口
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <returns>返回充值结果</returns>
        public string Pay(string OrderNo)
        {
            order = os.GetOrder(OrderNo);
            gu = gus.GetGameUser(order.UserName);
            gs = gss.GetGameServer(order.ServerId);
            string PayGold=(order.PayMoney * game.GameMoneyScale).ToString();
            string orderid = order.OrderNo.Substring(5);
            if (gus.IsGameUser(gu.UserName))
            {
                string s = (gu.Id.ToString() + order.PayMoney + orderid + gs.ServerNo + gc.PayTicket);
                sign = DESEncrypt.Md5(s, 32);
                string PayUrl = "http://" + gs.ServerNo + "." + gc.PayCom + "?qid=" + gu.Id + "&order_amount=" + order.PayMoney + "&order_id=" + orderid + "&server_id=" + gs.ServerNo + "&sign=" + sign;
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
                                return "重复通知";
                                
                            default :
                                return "充值失败，未知错误";
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
        /// 查询接口
        /// </summary>
        /// <returns></returns>
        public GameUserInfo Sel(int UserId, int ServerId)
        {
            gu = gus.GetGameUser(UserId);                                   //获取查询用户
            gs = gss.GetGameServer(ServerId);                              //获取查询用户所在区服
            time = Utils.GetTimeSpan();                                   //获取时间戳
            GameUserInfo gui = new GameUserInfo();                          //定义返回查询结果信息    
            sign = DESEncrypt.Md5("get_player_info_" + time + "_" + gc.SelectTicket, 32);  //获取验证参数
            string SelUrl = "http://" + gs.ServerNo + "." + gc.ExistCom + "?server_id=" + gs.ServerNo + "&users=" + gu.Id + "&time=" + time + "&sign=" + sign ;      //获取查询地址
            try
            {
                string SelResult = Utils.GetWebPageContent(SelUrl);             //获取返回结果
                switch (SelResult)
                { 
                    case "-1":
                        gui.Message = "参数错误";
                        break;
                    case "-3":
                        gui.Message = "参数验证失败";
                        break;
                    case "-4":
                        gui.Message = "缺少参数";
                        break;
                    case "-5":
                        gui.Message = "获取玩家数据失败";
                        break;
                    case "-6":
                        gui.Message = "请求的数据太多";
                        break;
                    case "-7":
                        gui.Message = "玩家都不存在,包含合区后，在原区找不到";
                        break;
                    case "-102":
                        gui.Message = "(全局)获取服务器信息失败或服务不存在";
                        break;
                    case "-103":
                        gui.Message = "(全局)服务器设置异常";
                        break;
                    default :
                        Dictionary<string, string> Jd = Json.JsonToArray(SelResult);
                        gui = new GameUserInfo(gu.Id.ToString(), gu.UserName, Utils.UrlDecode(Jd["name"]), int.Parse(Jd["level"]), gs.QuFu, os.GetOrderInfo(gu.UserName), "Success");
                        break;

                }
                //gui = new GameUserInfo(gu.Id.ToString(), gu.UserName,Utils.ConvertUnicodeStringToChinese(Jd["nick_name"]), int.Parse(Jd["level"]), gs.Name, os.GetOrderInfo(gu.UserName), "Success");
                
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
        public Game_Fyws()
        {
            game = games.GetGame("Fyws");                                   //获取游戏
            gc = gcs.GetGameConfig(game.Id);                                //获取游戏参数
        }
    }
}
