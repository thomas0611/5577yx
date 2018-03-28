using Common;
using Game.Manager;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Game.Controllers
{
    public class CommonGame : Controller
    {
        Games g = new Games();
        GamesManager gm = new GamesManager();
        NewsManager nm = new NewsManager();
        List<GameServer> gsList = new List<GameServer>();
        ServersMananger sm = new ServersMananger();
        GameUserManager gum = new GameUserManager();
        CardManager cm = new CardManager();

        public ActionResult InitServers(string GameNo)
        {
            ViewData["GameCenterOn"] = "current ";
            g = gm.GetGame(GameNo);
            ViewData["Title"] = g.Name + "服务器列表";
            ViewData["IndexHbImg"] = g.IndexHbImg;
            string NewsGG = "";
            List<News> GGlist = new List<News>();
            GGlist = nm.GetNews(5, 2, g.Id);
            foreach (News n in GGlist)
            {
                NewsGG += "<li><span style=\"margin: 20px;\"><a href=\"/NewsCenter/News?N=" + n.Id + "\" style=\"text-decoration: none;\">" + n.Title + "</a></span></li>";
            }
            ViewData["NewsGG"] = NewsGG;
            string[] Str = g.GameProperty.Split('|');
            ViewData["GameLx"] = Str[0];
            ViewData["GameTz"] = Str[1];
            ViewData["GameCzlx"] = Str[2];
            ViewData["GameHm"] = Str[3];
            ViewData["GameSfms"] = Str[4];

            string ServerHtml = "";
            string SelHtml = "";
            gsList = sm.GetServersByGame(g.Id);
            for (int i = 0; i < Math.Ceiling(gsList.Count / 20f); i++)
            {
                SelHtml += " <a>" + (i * 20 + 1) + "-" + ((i + 1) * 20) + "</a>";
                ServerHtml += "<div style=\"display: none\">";
                for (int j = (i * 20); j <= ((20 * i + 19) >= gsList.Count ? gsList.Count - 1 : (20 * i + 19)); j++)
                {
                    if (gsList[j].StartTime < DateTime.Now && gsList[j].State == 1)
                    {
                        gsList[j].State = 4;
                        sm.UpdateServer(gsList[j]);
                    }
                    else if (gsList[j].StartTime > DateTime.Now && gsList[j].State != 1)
                    {
                        gsList[j].State = 1;
                        sm.UpdateServer(gsList[j]);
                    }
                    switch (gsList[j].State)
                    {
                        case 1:
                            ServerHtml += "<a class=\"li_jikai\">" + gsList[j].Name + "</a>";
                            break;
                        case 2:
                            ServerHtml += "<a class=\"li_weihu\">" + gsList[j].Name + "</a>";
                            break;
                        case 3:
                            ServerHtml += "<a class=\"li_liuchang\" href=\"/" + g.GameNo + "/LoginGame?S=" + gsList[j].QuFu + "\" target=\"_blank\">" + gsList[j].Name + "</a>";
                            break;
                        case 4:
                            ServerHtml += "<a class=\"li_hot\" href=\"/" + g.GameNo + "/LoginGame?S=" + gsList[j].QuFu + "\" target=\"_blank\">" + gsList[j].Name + "</a>";
                            break;
                        default:
                            break;
                    }
                }
                ServerHtml += "</div>";
            }
            ViewData["SelHtml"] = SelHtml;
            ViewData["ServerHtml"] = ServerHtml;

            string ZlHtml = "";
            List<News> zlList = new List<News>();
            zlList = nm.GetNews(200, 5, g.Id);
            foreach (News zl in zlList)
            {
                ZlHtml += "<li><a href=\"/NewsCenter/News?N=" + zl.Id + "\" target=\"_blank\">" + zl.Title + "</a></li>";
            }
            ViewData["ZlHtml"] = ZlHtml;

            ViewData["Pic1"] = g.Pic1;
            ViewData["Pic2"] = g.Pic2;
            ViewData["Pic3"] = g.Pic3;
            ViewData["Pic4"] = g.Pic4;

            ViewData["GameCom"] = g.GameCom;
            ViewData["BBS"] = g.GameBBS;
            ViewData["game_url_hd"] = g.game_url_hd;
            ViewData["Dlq"] = string.IsNullOrEmpty(g.game_url_xzq) ? "" : "<a href=\"" + g.game_url_xzq + "\" class=\"dlq\" target=\"_blank\">&gt; 登录器地址 </a>";
            return View("~/Views/GameCenter/GameServer.cshtml");
        }

        public ActionResult LoginGame(string GameNo, int Qf)
        {
            g = gm.GetGame(GameNo);
            GameServer gs = new GameServer();
            gs = sm.GetGameServer(g.Id, Qf);
            ViewData["Title"] = "5577yx-" + g.Name;
            ViewData["ServerName"] = g.Name + "-" + gs.Name;
            ViewData["GameNo"] = g.GameNo;
            ViewData["Qf"] = Qf;
            if (gs.State == 1 && gs.StartTime < DateTime.Now)
            {
                gs.State = 4;
                sm.UpdateServer(gs);
            }
            if (gs.State != 1 && gs.StartTime > DateTime.Now)
            {
                gs.State = 1;
                sm.UpdateServer(gs);
            }
            if (gs.State == 1 && gs.StartTime > DateTime.Now)
            {
                TimeSpan ts = gs.StartTime - DateTime.Now;
                ViewData["TimeSpan"] = ts.TotalMilliseconds;
            }
            if (gs.State == 1 || gs.State == 2)
            {
                ViewData["State"] = gs.State;
                return View("~/Views/GameCenter/LoginGame.cshtml");
            }
            int UserId = BBRequest.GetUserId();
            if (UserId > 0)
            {
                ViewData["LoginUrl"] = gm.LoginUrl(g.Id, UserId, gs.Id, 0);
                gum.UpdateLastLogin(UserId);
                OnlineLog ol = new OnlineLog(0, UserId, g.Id, gs.Id, DateTime.Now, 0, 0);
                new OnlineLogManager().AddOnlineLog(ol);
            }
            else
            {
                if (Utils.GetCookie("6qmgamesame") != "")
                {
                    string value = Utils.GetCookie("6qmgamesame");
                    string UserName = DESEncrypt.DesDecrypt(value.Split('|')[0]);
                    string PWD = DESEncrypt.DesDecrypt(value.Split('|')[1]);
                    GameUser gu = gum.GetGameUser(UserName, DESEncrypt.Md5(PWD, 32));
                    if (gu != null)
                    {
                        BBRequest.WriteUserId(gu.Id);
                        gum.UpdateLastLogin(gu.Id);
                        ViewData["LoginUrl"] = gm.LoginUrl(g.Id, gu.Id, gs.Id, 0);
                        OnlineLog ol = new OnlineLog(0, gu.Id, g.Id, gs.Id, DateTime.Now, 0, 0);
                        new OnlineLogManager().AddOnlineLog(ol);
                    }
                    else
                    {
                        return RedirectToAction("Login", "Home");
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Home");
                }
            }

            return View("~/Views/GameCenter/LoginGame.cshtml");
        }

        public ActionResult GameGift(int CardId)
        {
            cardsname c = new cardsname();
            c = cm.GetCard(CardId);
            g = gm.GetGame(c.gameid);
            ViewData["CardName"] = c.cardname;
            ViewData["Img"] = c.img;
            ViewData["CardCount"] = cm.GetCardCount(CardId);
            ViewData["CardDes"] = c.carddesc;
            ViewData["CardId"] = CardId;
            ViewData["GameNo"] = g.GameNo;
            return View("~/Views/GameGift/GetGift.cshtml");
        }

        public string DoGetGift(int CardId, string CardNum)
        {
            cardsname cn = new cardsname();
            cn = cm.GetCard(CardId);
            g = gm.GetGame(cn.gameid);
            if (cm.GetCardCount(CardId) < 1)
            {
                return "<span><b>温馨提示：</b><lable style=\"color:red\">该新手卡已经被抢空啦！</span>";
            }
            int UserId = BBRequest.GetUserId();
            if (UserId <= 0)
            {
                if (Utils.GetCookie("6qmgamesame") != "")
                {
                    string value = Utils.GetCookie("6qmgamesame");
                    string UserName = DESEncrypt.DesDecrypt(value.Split('|')[0]);
                    string PWD = DESEncrypt.DesDecrypt(value.Split('|')[1]);
                    GameUser gu = gum.GetGameUser(UserName, DESEncrypt.Md5(PWD, 32));
                    if (gu != null)
                    {
                        BBRequest.WriteUserId(gu.Id);
                        gum.UpdateLastLogin(gu.Id);
                        UserId = BBRequest.GetUserId();
                    }
                    else
                    {
                        return "<span><b>温馨提示：</b><lable style=\"color:red\">登录后才可领取！</lable></span>";
                    }
                }
                else
                {
                    return "<span><b>温馨提示：</b><lable style=\"color:red\">登录后才可领取！</lable></span>";
                }
            }
            if (CardNum == null)
            {
                if (cm.ExitCardLog(UserId, CardId))
                {
                    return "<span id=\"showno\" ><b>温馨提示：</b><lable style=\"color:red\">您已经领取过该新手卡啦！</span>";
                }
                else
                {
                    cards c = new cards();
                    c = cm.GetCards(CardId);
                    CardNum = c.cardnum;
                    cardslog cl = new cardslog();
                    cl.userid = UserId;
                    cl.cardid = CardId;
                    cl.cardsid = c.id;
                    cm.AddCardLog(cl);
                    cm.UpdateCard(1, c.id);
                    sysmsg sysmsg1 = new sysmsg();
                    sysmsg1.msgid = 0;
                    sysmsg1.title = "新手卡信息：" + cn.cardname;
                    sysmsg1.type = 2;
                    sysmsg1.userid = UserId;
                    sysmsg1.msg = "领取新手卡成功！卡号：" + c.cardnum;
                    sysmsg1.fromid = 0;
                    new SysMsgManager().AddSysMsg(sysmsg1);
                    return "<span id=\"showno\" ><b>领取成功!激活码：</b> <label id=\"lblno\" style=\"color:red\"> " + CardNum + " </label></span>";
                }
            }
            else
            {
                return "<span id=\"showno\" ><b>领取成功!激活码：</b> <label id=\"lblno\" style=\"color:red\"> " + CardNum + " </label></span>";
            }
        }
    }
}