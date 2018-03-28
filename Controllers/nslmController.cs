﻿using Common;
using Game.Manager;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Game.Controllers
{
    public class nslmController : Controller
    {
        //
        // GET: /gjqx/
        CommonGame cg = new CommonGame();
        Games g = new Games();
        GamesManager gm = new GamesManager();
        ServersMananger sm = new ServersMananger();
        HtmlHelper hh = new HtmlHelper();
        NewsManager nm = new NewsManager();
        GameUserManager gum = new GameUserManager();

        public ActionResult Index()
        {
            g = gm.GetGame("nslm");
            List<GameServer> gsList = new List<GameServer>();
            gsList = sm.GetServersByGame(g.Id);
            ViewData["ServersHtml"] = hh.ServersHtml(gsList);
            ViewData["GameNo"] = g.GameNo;
            ViewData["TjqfHref"] = "#";
            ViewData["TjqfName"] = "暂无推荐区服";
            ViewData["GameDes"] = g.GameDesc;
            if (g.tjqf > 0)
            {
                GameServer tjqf = sm.GetGameServer(g.tjqf);
                ViewData["TjqfHref"] = "/" + g.GameNo + "/LoginGame?S=" + tjqf.QuFu;
                ViewData["TjqfName"] = tjqf.Name;
            }
            ViewData["YqLink"] = hh.YqLinkHtml("20");

            List<News> Newlist = new List<News>();
            Newlist = nm.GetNews(7, 2, g.Id);
            string NewsHtml = "";
            foreach (News n in Newlist)
            {
                NewsHtml += "<p><span class=\"date\">" + n.ReleaseTime + "</span><span class=\"leis\"><a href=\"#\">热点</a></span><a href=\"/NewsCenter/News?N=" + n.Id + "\" title=\"" + n.Title + "\" class=\"zw\">" + (n.Title.Length < 15 ? n.Title : n.Title.Substring(0, 15)) + "</a></p>";
            }
            ViewData["News"] = NewsHtml;

            List<News> GGNewlist = new List<News>();
            GGNewlist = nm.GetNews(7, 4, g.Id);
            string GGNewsHtml = "";
            foreach (News n in GGNewlist)
            {
                GGNewsHtml += "<li><span class=\"date fr\">" + n.ReleaseTime + "</span>[公告]<a href=\"/NewsCenter/News?N=" + n.Id + "\" target=\"_self\"title=\"" + n.Title + "\">" + (n.Title.Length < 15 ? n.Title : n.Title.Substring(0, 15)) + "</strong></a></li>";
            }
            ViewData["GGNews"] = GGNewsHtml;
            return View();
        }

        public ActionResult Servers()
        {
            return cg.InitServers("nslm");
        }

        public ActionResult LoginGame(int S)
        {
            return cg.LoginGame("nslm", S);
        }

        public ActionResult Gift(int G)
        {
            return cg.GameGift(G);
        }

        public string DoGetGift(int G)
        {
            return cg.DoGetGift(G, null);
        }

        public ActionResult Wd()
        {
            return View();
        }

        public ActionResult WdServers()
        {
            int UserId = BBRequest.GetUserId();
            g = gm.GetGame("nslm");
            if (UserId > 0)
            {
                GameUser gu = new GameUser();
                gu = gum.GetGameUser(UserId);
                ViewData["UserName"] = gu.UserName;
                ViewData["TjqfHref"] = "#";
                ViewData["TjqfName"] = "暂无推荐区服";
                ViewData["LLHref"] = "#";
                ViewData["LLName"] = "暂无记录";
                OnlineLog ol = new OnlineLog();
                ol = new OnlineLogManager().GetLastLogin(UserId, g.Id);
                if (ol != null)
                {
                    GameServer Llqf = sm.GetGameServer(ol.ServerId);
                    ViewData["LLHref"] = "/" + g.GameNo + "/LoginGame?S=" + Llqf.QuFu;
                    ViewData["LLName"] = Llqf.Name;
                }
                if (g.tjqf > 0)
                {
                    GameServer tjqf = sm.GetGameServer(g.tjqf);
                    ViewData["TjqfHref"] = "/" + g.GameNo + "/LoginGame?S=" + tjqf.QuFu;
                    ViewData["TjqfName"] = tjqf.Name;
                }
                List<GameServer> gsList = new List<GameServer>();
                gsList = sm.GetServersByGame(g.Id);
                string ServerHtml = "";
                foreach (GameServer gs in gsList)
                {
                    switch (gs.State)
                    {
                        case 1:
                            ServerHtml += "<li><a  class=\"s2\"><span>" + gs.Name + "</span>即将开启</a></li>";
                            break;
                        case 2:
                            ServerHtml += "<li><a  class=\"s1\"><span>" + gs.Name + "</span>停服维护</a></li>";
                            break;
                        case 3:
                            ServerHtml += "<li><a href=\"/" + g.GameNo + "/LoginGame?S=" + gs.QuFu + "\" target=\"_game\" class=\"s6\"><span>" + gs.Name + "</span>流畅</a></li>";
                            break;
                        case 4:
                            ServerHtml += "<li><a href=\"/" + g.GameNo + "/LoginGame?S=" + gs.QuFu + "\" target=\"_game\" class=\"s4\"><span>" + gs.Name + "</span>火爆</a></li>";
                            break;
                        default:
                            break;
                    }
                }
                ViewData["gsHtml"] = ServerHtml;

                Utils.WriteCookie2("miniloader", "1", "5577yx.com");

                return View();
            }
            else
            {
                return RedirectToAction("Wd");
            }

        }
    }
}
