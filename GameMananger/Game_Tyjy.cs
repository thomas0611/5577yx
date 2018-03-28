using Common;
using Game.DAL;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Game.Manager
{
    class Game_Tyjy
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
        

        /// <summary>
        /// 桃园结义登陆接口
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ServerId"></param>
        /// <param name="IsPC"></param>
        /// <returns></returns>
        public string Login(int UserId, int ServerId, int IsPC)
        {
            gu = gus.GetGameUser(UserId);
            gs = gss.GetGameServer(ServerId);
            tstamp = Utils.GetTimeSpan();
            string flag = DESEncrypt.Md5(gu.UserName + tstamp + gc.LoginTicket + 1 + gs.ServerNo, 32);
            string username = HttpContext.Current.Server.UrlEncode(gu.UserName);
            string LoginUrl = "http://" + gs.ServerNo + "." + gc.LoginCom + "?username=" + username + "&time=" + tstamp + "&cm=1" + "&flag=" + flag + "&server=" + gs.ServerNo;
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
                tstamp = Utils.GetTimeSpan();
                string username = HttpContext.Current.Server.UrlEncode(order.UserName);
                string flag=DESEncrypt.Md5(order.OrderNo + order.UserName + PayGold + tstamp + gc.PayTicket ,32);
                string PayUrl = "http://" + gs.ServerNo + "." + gc.PayCom + "?order=" + order.OrderNo + "&username=" + username +  "&server=" + gs.QuFu + "&gold=" + PayGold + "&time=" + tstamp + "&flag=" + flag;
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
                                
                            case "-1":
                                return "必要参数格式不对或者缺失";
                                
                            case "-2":
                                return "验证串匹配不正确";
                            case "-3":
                                return "充值所对应的用户在游戏服里面还没有创建角色";
                            case "-4":
                                return "请求的时间戳超时";
                            case "-5":
                                return "ip受限";
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
            tstamp = Utils.GetTimeSpan();                                   //获取时间戳
            string username = Utils.UrlEncode (gu.UserName);
                     
            GameUserInfo gui = new GameUserInfo();                          //定义返回查询结果信息    
            string flag = DESEncrypt.Md5(gu.UserName + tstamp + gc.SelectTicket , 32);              //获取验证参数
            string SelUrl = "http://" + gs.ServerNo + "." + gc.ExistCom + "?username=" + username + "&time=" + tstamp + "&flag=" + flag ;      //获取查询地址
            try
            {
                string SelResult = Utils.GetWebPageContent(SelUrl);             //获取返回结果
                Dictionary<string, string> Jd = Json.JsonToArray(SelResult);
                //if (Jd.ContainsKey("code"))
              //  {
                    //switch (Jd["code"])
                    //{
                    //    case "-1":
                    //        gui.Message = "提交参数不全";
                    //        break;
                    //    case "-2":
                    //        gui.Message = "验证失败";
                    //        break;
                    //    case "-3":
                    //        gui.Message = "没有创建角色";
                    //        break;
                    //    case "-7":
                    //        gui.Message = "超时，超过240秒";
                    //        break;
                    //    case "0" :
                    //        gui = new GameUserInfo(gu.Id.ToString(), gu.UserName, Utils.ConvertUnicodeStringToChinese(Jd["nick_name"]), int.Parse(Jd["level"]), gs.Name, os.GetOrderInfo(gu.UserName), "Success");
                    //        break;
                    //}
               // }
                gui = new GameUserInfo(gu.Id.ToString(), gu.UserName, Utils.ConvertUnicodeStringToChinese(Jd["nick_name"]), int.Parse(Jd["level"]), gs.Name, os.GetOrderInfo(gu.UserName), "Success");
                
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
        public Game_Tyjy()
        {
            game = games.GetGame("Tyjy");                                   //获取游戏
            gc = gcs.GetGameConfig(game.Id);                                //获取游戏参数
        }
    }
}
