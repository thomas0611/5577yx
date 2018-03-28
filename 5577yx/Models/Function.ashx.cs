using Common;
using Game.Manager;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace _5577yx.Models
{
    /// <summary>
    /// Function 的摘要说明
    /// </summary>
    public class Function : IHttpHandler, IRequiresSessionState
    {
        GameUserManager gum = new GameUserManager();
        GamesManager gm = new GamesManager();
        ServersMananger sm = new ServersMananger();
        CardManager cm = new CardManager();
        OrderManager om = new OrderManager();

        public void ProcessRequest(HttpContext context)
        {
            string method = context.Request["method"];
            context.Response.ContentType = "text/html; charset=utf-8";
            switch (method)
            {
                case "InitHeadInfo":
                    InitHeadInfo(context);
                    break;
                case "InitLeftInfo":
                    InitLeftInfo(context);
                    break;
                case "GetCookie":
                    GetCookie(context);
                    break;
                case "GetGetOnlineLog":
                    GetGetOnlineLog(context);
                    break;
                case "GetNewsServer":
                    GetNewsServer(context);
                    break;
                case "MakeOrder":
                    MakeOrder(context);
                    break;
                case "zfbeOrder":
                    zfbeOrder(context);
                    break;
                case "CheckMoney":
                    CheckMoney(context);
                    break;
                case "GetCardCount":
                    GetCardCount(context);
                    break;
                case "GetSpreadMoney":
                    GetSpreadMoney(context);
                    break;
                case "GetXiaoxi":
                    GetXiaoxi(context);
                    break;
                case "UpdateUserSource":
                    UpdateUserSource(context);
                    break;
                default:
                    break;
            }
        }

        public void InitHeadInfo(HttpContext context)
        {
            GameUser gu = GetUser(context);
            if (gu != null)
            {
                context.Response.Write("<span style=\"color: sandybrown; font-weight: bold\">" + gu.UserName + "</span>");
            }
            else
            {
                context.Response.Write("体验更优的服务请您先[<a href=\"/Home/Login\">登录</a>]或[<a href=\"/Home/Reg\">注册</a>]");
            }
        }

        public void InitLeftInfo(HttpContext context)
        {
            GameUser gu = GetUser(context);
            string JsonStr = "";
            if (gu != null)
            {
                JsonStr = Json.ToJson(gu);
            }
            context.Response.Write(JsonStr);
        }

        public void GetCookie(HttpContext context)
        {
            if (Utils.GetCookie("6qmgamesame") != "")
            {
                string value = Utils.GetCookie("6qmgamesame");
                string UserName = DESEncrypt.DesDecrypt(value.Split('|')[0]);
                string PWD = DESEncrypt.DesDecrypt(value.Split('|')[1]);
                context.Response.Write(UserName + "|" + PWD);
            }
        }

        public void GetGetOnlineLog(HttpContext context)
        {
            List<OnlineLog> list = new List<OnlineLog>();
            list = new OnlineLogManager().GetOnlineLog(GetUser(context).Id, 2);
            string HtmlStr = "";
            foreach (OnlineLog ol in list)
            {
                Games g = gm.GetGame(ol.GameId);
                GameServer gs = sm.GetGameServer(ol.ServerId);
                HtmlStr += " <span ><a href=\"/" + g.GameNo + "/LoginGame?S=" + gs.QuFu + "\" target=\"_blank\">" + gm.GetGame(ol.GameId).Name + "-" + sm.GetGameServer(ol.ServerId).Name + " </a></span><br/>";
            }
            context.Response.Write(HtmlStr);
        }

        public void GetNewsServer(HttpContext context)
        {
            List<GameServer> list = new List<GameServer>();
            list = sm.GetNewsServer(6);
            string HtmlStr = "";
            string Style = "style=\"background-color: #FFF4E1\"";
            foreach (GameServer gs in list)
            {
                Games g = new Games();
                g = gm.GetGame(gs.GameId);
                HtmlStr += "<tr><td " + Style + ">&nbsp;</td><td " + Style + ">" + gs.StartTime.ToString("MM-dd") + "</td><td " + Style + ">" + gs.StartTime.ToString("HH:mm") + "</td><td class=\"name\" " + Style + "><a href=\"/" + g.GameNo + "/\" class=\"a2\" target=\"_blank\" title=\"" + gs.Name + "\">" + g.Name + "</a></td><td class=\"qu\" " + Style + "><span style=\"width: 50px; display: block; float: right; text-align: center; line-height: 15px; font-size: 12px\"><a href=\"/" + g.GameNo + "/LoginGame?S=" + gs.QuFu + "\" class=\"a1\"target=\"_blank\">" + gs.QuFu + "区</a></span></td></tr>";
            }
            context.Response.Write(HtmlStr);
        }

        public void MakeOrder(HttpContext context)
        {
            int Type = int.Parse(context.Request["Type"]);
            int GameId = int.Parse(context.Request["GameId"]);
            int ServerId = int.Parse(context.Request["ServerId"]);
            string SelMoney = context.Request["SelMoney"];
            string TxtMoney = context.Request["TxtMoney"];
            string UserName = context.Request["UserName"].Trim();
            string Bank = context.Request["Bank"];
            int PayType = int.Parse(context.Request["PayType"]);
            Orders order = new Orders();
            OrderManager om = new OrderManager();
            GameUserManager gum = new GameUserManager();
            GamesManager gm = new GamesManager();
            GameUser gu = new GameUser();
            try
            {
                gu = gum.GetGameUser((int)context.Session[Keys.SESSION_USER]);
            }
            catch (Exception)
            {
                if (PayType == 7 || PayType == 15)
                {
                    context.Response.Write("/Home/Login");
                    return;
                }
            }
            float PayMoney = 0;
            string OrderCode = Type == 1 ? "G" : "P";
            if (PayType == 15)
            {
                OrderCode = "F";
            }
            try
            {
                if (!string.IsNullOrEmpty(TxtMoney))
                {
                    PayMoney = float.Parse(TxtMoney.Trim());
                }
                else
                {
                    PayMoney = float.Parse(SelMoney);
                }
                order = om.GetOrder(OrderCode, GameId.ToString(), ServerId, Type, PayType, UserName, PayMoney, gu.UserName);
                if (PayType == 7 || PayType == 15)
                {
                    if (om.AddOrder(order))
                    {
                        string Result = gm.PayManager(order.OrderNo);
                        context.Response.Write("您的订单" + order.OrderNo + "已经处理。处理结果：" + Result + "|Error");
                        return;
                    }
                    else
                    {
                        context.Response.Write("添加订单失败！|Error");
                        return;
                    }
                }
                else
                {
                    if (om.AddOrder(order))
                    {
                        context.Response.Write("/PayCenter/PayOrder?Order=" + DESEncrypt.encryptstring1(order.OrderNo) + "&Bank=" + Bank + "|Success");
                    }
                    else
                    {
                        context.Response.Write("添加订单失败！|Error");
                        return;
                    }
                }
            }
            catch (Exception e)
            {
                context.Response.Write(e.Message + "|Error");
                return;
            }
        }

        public void zfbeOrder(HttpContext context)
        {
            Orders order = new Orders();
            string OrderNo = context.Request["OrderNo"];
            OrderManager om = new OrderManager();
            order = om.GetOrder(OrderNo);
            if (order.PayTypeId == 8)
            {
                //支付类型
                string payment_type = "1";
                //必填，不能修改
                //服务器异步通知页面路径
                string notify_url = ConfigurationManager.AppSettings["zfbmsg"].ToString();
                //需http://格式的完整路径，不能加?id=123这类自定义参数

                //页面跳转同步通知页面路径
                string return_url = ConfigurationManager.AppSettings["zfbreurl"].ToString();
                //需http://格式的完整路径，不能加?id=123这类自定义参数，不能写成http://localhost/

                //卖家支付宝帐户
                string seller_email = ConfigurationManager.AppSettings["zfbuser"].ToString();
                //必填

                //商户订单号
                string out_trade_no = order.OrderNo;
                //商户网站订单系统中唯一订单号，必填

                //订单名称

                string subject = order.Type == 1 ? "5577yx平台游戏币" : "5577yx平台币" + "充值";
                //必填

                //付款金额
                string total_fee = order.PayMoney.ToString();
                //必填

                //订单描述

                string body = order.GameName + order.ServerName;
                //商品展示地址
                string show_url = "";
                //需以http://开头的完整路径，例如：http://www.xxx.com/myorder.html

                //防钓鱼时间戳
                string anti_phishing_key = Common.Submit.Query_timestamp();
                //若要使用请调用类文件submit中的query_timestamp函数

                //客户端的IP地址
                string exter_invoke_ip = "";
                //非局域网的外网IP地址，如：221.0.0.1

                //把请求参数打包成数组
                SortedDictionary<string, string> sParaTemp = new SortedDictionary<string, string>();
                sParaTemp.Add("partner", Config.Partner);
                sParaTemp.Add("_input_charset", Config.Input_charset.ToLower());
                sParaTemp.Add("service", "create_direct_pay_by_user");
                sParaTemp.Add("payment_type", payment_type);
                sParaTemp.Add("notify_url", notify_url);
                sParaTemp.Add("return_url", return_url);
                sParaTemp.Add("seller_email", seller_email);
                sParaTemp.Add("out_trade_no", out_trade_no);
                sParaTemp.Add("subject", subject);
                sParaTemp.Add("total_fee", total_fee);
                sParaTemp.Add("body", body);
                sParaTemp.Add("show_url", show_url);
                sParaTemp.Add("anti_phishing_key", anti_phishing_key);
                sParaTemp.Add("exter_invoke_ip", exter_invoke_ip);
                //建立请求
                string sHtmlText = Submit.BuildRequest(sParaTemp, "post", "确认");
                context.Response.Write(sHtmlText);
            }
            else
            {
                context.Response.Write("NoZfb");
            }
        }

        public void CheckMoney(HttpContext context)
        {
            int GameId = int.Parse(context.Request["GameId"]);
            int Type = int.Parse(context.Request["Type"]);
            string SelMoney = context.Request["SelMoney"];
            string TxtMoney = context.Request["TxTMoney"];
            int PayType = int.Parse(context.Request["PayType"]);
            Games game = new Games();
            GamesManager gm = new GamesManager();
            game = gm.GetGame(GameId);
            float Money = string.IsNullOrEmpty(TxtMoney) ? float.Parse(SelMoney.Trim()) : float.Parse(TxtMoney.Trim());
            float PayMoney = 0;
            switch (PayType)
            {
                case 7:
                    PayMoney = Money / 10;
                    break;
                case 5:
                case 9:
                    PayMoney = Money * 0.8F;
                    break;
                case 2:
                case 3:
                case 4:
                    PayMoney = Money * 0.9F;
                    break;
                default:
                    PayMoney = Money;
                    break;
            }
            string GameMoney = Type == 1 ? PayMoney * game.GameMoneyScale + "游戏币" : PayMoney * 10 + "平台币";
            context.Response.Write(Money + "|" + GameMoney);
        }

        public void GetCardCount(HttpContext context)
        {
            int CardId = int.Parse(context.Request["CardId"]);
            context.Response.Write(cm.GetCardCount(CardId));
        }

        public void GetSpreadMoney(HttpContext context)
        {
            string WhereStr = context.Request["WhereStr"];
            int UserId = BBRequest.GetUserId();
            string[] re = WhereStr.Split('|');
            WhereStr = "";
            if (!string.IsNullOrEmpty(re[0]))
            {
                WhereStr += " and o.gameid = " + re[0];
            }
            else
            {
                WhereStr += " and 1=1";
            }
            if (string.IsNullOrEmpty(re[1]) || string.IsNullOrEmpty(re[2]))
            {
                WhereStr += " and 1=1";
            }
            else
            {
                WhereStr += " and o.paytime>='" + re[1] + "' and o.paytime<='" + re[2] + "'";
            }
            context.Response.Write(om.GetSumMoney(UserId, WhereStr));
        }

        private void GetXiaoxi(HttpContext context)
        {
            GameUser gu = GetUser(context);
            string XiaoxiHtml = "";
            if (gu != null)
            {
                if (string.IsNullOrEmpty(gu.Email))
                {
                    XiaoxiHtml += "<a href=\"/UserCenter/UserEmail\" style=\"color:red\">您还未绑定邮箱！</a><br/>";
                }
                if (string.IsNullOrEmpty(gu.Cards))
                {
                    XiaoxiHtml += "<a href=\"/UserCenter/UserCard\" style=\"color:red\">您还未绑定身份证！</a>";
                }
            }
            context.Response.Write(XiaoxiHtml);
        }

        private void UpdateUserSource(HttpContext context)
        {
            string Ud = context.Request["Ud"];
            int UserId = int.Parse(context.Request["Id"]);
            GameUser gu = gum.GetGameUser(UserId);
            if (Ud == "U")
            {
                gu.IsSpreader = 1;
            }
            else
            {
                gu.IsSpreader = 0;
            }
            context.Response.Write(gum.UpdateUser(gu));
        }

        public GameUser GetUser(HttpContext context)
        {
            GameUser gu = null;
            if (context.Session[Keys.SESSION_USER] != null)
            {
                gu = gum.GetGameUser((int)context.Session[Keys.SESSION_USER]);
            }
            return gu;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}