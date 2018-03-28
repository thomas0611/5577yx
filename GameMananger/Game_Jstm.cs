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
    public class Game_Jstm: IGame
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
        string sign;                                                        //定义验证参数
        string gameid = "2";                                                //游戏方定义的游戏ID

        /// <summary>
        /// 初始化参数
        /// </summary>
        public Game_Jstm()
        {
            game = games.GetGame("jstm");                                   //获取游戏
            gc = gcs.GetGameConfig(game.Id);                                //获取游戏参数
        }

        /// <summary>
        /// 绝色唐门登录接口
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
            Base64Protector bp = new Base64Protector();
            string au = "account=" + gu.Id.ToString() + "&game_id=" + gameid + "&ip=" + gu.Ip + "&is_audit=1" + "&op_id=" + gc.AgentId + "&sid=" + gs.ServerNo + "&time=" + time ;
            string auth = bp.Base64Code(au);
            sign = DESEncrypt.Md5(auth + gc.LoginTicket,32);
            string LoginUrl = "http://" + gc.LoginCom + "?auth=" + auth + "&sign=" + sign;
            return LoginUrl;
        }

        /// <summary>
        /// 绝色唐门充值接口
        /// </summary>
        /// <param name="OrderNo"></param>
        /// <returns></returns>
        public string Pay(string OrderNo)
        {
            order = os.GetOrder(OrderNo);
            gu = gus.GetGameUser(order.UserName);
            gs = gss.GetGameServer(order.ServerId);
            if (!gus.IsGameUser(gu.UserName))
            {
                return "充值失败！用户不存在！";
            }
            else 
            {
                string PayGold = (order.PayMoney * game.GameMoneyScale).ToString();     //计算支付的游戏币
                time = Utils.GetTimeSpan();
                string au = "account=" + gu.Id + "&game_id=" + gameid + "&game_money=" + PayGold + "&op_id=" + gc.AgentId + "&order_id=" + OrderNo + "&rmb_money=" + order.PayMoney * 100 + "&sid=" + gs.ServerNo + "&time=" + time;
                Base64Protector bp = new Base64Protector();
                string auth = bp.Base64Code(au);
                sign = DESEncrypt.Md5(auth + gc.PayTicket, 32);
                string PayUrl = "http://" + gc.PayCom + "?auth=" + auth + "&sign=" + sign;
                GameUserInfo gui = Sel(gu.Id, gs.Id);
                if (gui.Message == "Success")
                {
                    if (order.State == 1)
                    {
                        string PayResult = Utils.GetWebPageContent(PayUrl);
                        string strResult = PayResult.Substring(1 ,PayResult.IndexOf('}')-1);
                        switch (strResult)
                        {
                            case "\"status\":0":
                                if (os.UpdateOrder(order.OrderNo))
                                {
                                    gus.UpdateGameMoney(gu.UserName, order.PayMoney);
                                    return "充值成功";
                                }
                                else
                                {
                                    return "充值失败！错误原因：更新订单状态失败！";
                                }
                            case "\"status\":1":
                                return "检验码签名错误";
                            case "\"status\":2":
                                return "参数异常";
                            case "\"status\":3":
                                return "无效时间戳";
                            case "\"status\":4":
                                return "op_id 运营商编号不存在";
                            case "\"status\":5":
                                return "game_id 不存在";
                            case "\"status\":6":
                                return "非法IP";
                            case "\"status\":101":
                                return "游戏币和RMB 比列不对";
                            case "\"status\":102":
                                return "该订单已处理(成功)，不再重复充值";
                            case "\"status\":103":
                                return "网络异常（联运方需重新发起同一订单的充值请求）";
                            case "\"status\":104":
                                return "充值失败";
                            default:
                                return "充值失败，未知错误";
                        }
                    }
                    else
                    {
                        return "充值失败！错误原因：无法提交未支付订单！";
                    }
                }
                else return "充值失败！角色不存在！";
            }
        }


        /// <summary>
        /// 绝色唐门查询接口
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="ServerId"></param>
        /// <returns></returns>
        public GameUserInfo Sel(int UserId, int ServerId)
        {
            gu = gus.GetGameUser(UserId);
            gs = gss.GetGameServer(ServerId);
            time = Utils.GetTimeSpan();
            GameUserInfo gui = new GameUserInfo();
            string au = "account=" + gu.Id + "&game_id=" + gameid + "&op_id=" + gc.AgentId + "&sid=" + gs.ServerNo + "&time=" + time;
            Base64Protector bp = new Base64Protector();
            string auth = bp.Base64Code(au);
            sign = DESEncrypt.Md5(auth + gc.SelectTicket ,32);
            string SelUrl = "http://" + gc.ExistCom + "?auth=" + auth + "&sign=" + sign;
            try
            {
                string SelResult = Utils.GetWebPageContent(SelUrl);             //获取返回结果
                string status = SelResult.Substring(SelResult.IndexOf(',') - 1, 1);
                switch (status)
                {
                    case "0":
                        Dictionary<string, string> Jd = Json.JsonToArray(SelResult);
                        if (Jd != null)
                        {
                            gui = new GameUserInfo(gu.Id.ToString(), gu.UserName, Utils.UrlDecode(Jd["name"]), int.Parse(Jd["level"]), gs.QuFu, os.GetOrderInfo(gu.UserName), "Success");
                        }
                        else gui.Message = "角色不存在";
                        break;
                    case "1":
                        gui.Message = "检验码签名错误";
                        break;
                    case "2":
                        gui.Message = "参数异常";
                        break;
                    case "3":
                        gui.Message = "无效时间戳";
                        break;
                    case "4":
                        gui.Message = "op_id 运营商编号不存在";
                        break;
                    case "5":
                        gui.Message = "game_id不存在";
                        break;
                    case "6":
                        gui.Message = "非法IP";
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
    }
}
