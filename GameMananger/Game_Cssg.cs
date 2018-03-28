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
    class Game_Cssg
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
        /// 登陆接口
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
            string is_adult = "1";
            string back_url = "www.5577yx.com/" + game.GameNo;
            sign = DESEncrypt.Md5(gu.Id.ToString() + gc.AgentId + game.GameNo + gs.ServerNo + time + is_adult + "#" + gc.LoginTicket, 32);
            string LoginUrl = "http://" + gs.ServerNo + "." + gc.LoginCom + "?uid=" + gu.Id + "&platform=" + gc.AgentId + "&gkey="
                + game.GameNo + "&skey=" + gs.ServerNo + "&time=" + time + "&is_adult=" + is_adult + "&back_url=" + back_url + "&type=web" + "&sign=" + sign;
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
            if (gus.IsGameUser(gu.UserName))
            {
                time = Utils.GetTimeSpan();
                string s = (gu.Id.ToString() + gc.AgentId + game.GameNo + gs.ServerNo + time + order.OrderNo + PayGold + order.PayMoney + "#" + gc.PayTicket);
                sign = DESEncrypt.Md5(s, 32);
                //gkey=$gkey&skey=$skey&platform=$platform&order_id=xxx&uid=xxx&coins=$coins&money=$money&time=$time&sign=$sign&role_name=$role_name&role_id=$role_id
                string PayUrl = "http://" + gs.ServerNo + "." + gc.PayCom + "?gkey=" + game.GameNo + "&skey=" + gs.ServerNo + "&platform=" + gc.AgentId
                    + "&order_id=" + order.OrderNo + "&uid=" + gu.Id + "&coins=" + PayGold + "&money=" + order.PayMoney + "&time=" + time + "&sign=" + sign
                    + "&role_name=" + "&role_id=";
                GameUserInfo gui = Sel(gu.Id, gs.Id);
                if (gui.Message == "Success")
                {
                    if (order.State == 1)
                    {
                        try
                        {
                            string PayResult = Utils.GetWebPageContent(PayUrl);
                            switch (PayResult)
                            {
                                case "0":
                                    if (os.UpdateOrder(order.OrderNo))
                                    {
                                        gus.UpdateGameMoney(gu.UserName, order.PayMoney);
                                        return "充值成功";
                                    }
                                    else
                                    {
                                        return "充值失败！错误原因：更新订单状态失败！";
                                    }

                                case "1":
                                    return "订单重复";
                                case "-1":
                                    return "参数不全";
                                case "-2":
                                    return "签名错误";
                                case "-3":
                                    return "用户不存在";
                                case "-4":
                                    return "请求超时";
                                default:
                                    return "充值失败，未知错误";
                            }
                        }
                        catch (Exception ex)
                        {
                            return ex.Message ;
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
            sign = DESEncrypt.Md5(gu.Id.ToString() + gc.AgentId + game.GameNo + gs.ServerNo + time + "#" + gc.SelectTicket, 32);  //获取验证参数
            //http://[domain]/checkuser.html?uid=$uid&platform=$flatform&gkey=$gkey&skey=$skey&time=$time&sign=$sign
            string SelUrl = "http://" + gs.ServerNo + "." + gc.ExistCom + "?uid=" + gu.Id + "&platform=" + gc.AgentId + "&gkey=" + game.GameNo + "&skey="
            + gs.ServerNo + "&time=" + time + "&sign=" + sign ;      //获取查询地址
            try
            {
                string SelResult = Utils.GetWebPageContent(SelUrl);             //获取返回结果
                switch (SelResult)
                { 
                    case "0":
                        Dictionary<string, string> Jd = Json.JsonToArray(SelResult);
                        gui = new GameUserInfo(gu.Id.ToString(), gu.UserName, Utils.UrlDecode(Jd["nickname"]), int.Parse(Jd["level"]), gs.QuFu, os.GetOrderInfo(gu.UserName), "Success");
                        break;
                    case "-1":
                        gui.Message = "未创建角色";
                        break;
                    case "2":
                        gui.Message = "参数错误";
                        break;
                    default :
                        gui.UserName = "没有角色";
                        gui.Message = "error";
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
                

        /// <summary>
        /// 实例化参数
        /// </summary>
        public Game_Cssg()
        {
            game = games.GetGame("Cssg");                                   //获取游戏
            gc = gcs.GetGameConfig(game.Id);                                //获取游戏参数
        }
    }
}
