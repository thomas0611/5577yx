using Common;
using Game.Controllers;
using Game.Manager;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _5577yx.Views
{
    public class shmcController : Controller
    {
        //
        // GET: /shmc/

        CommonGame cg = new CommonGame();
        GamesManager gm = new GamesManager();
        Games g = new GamesManager().GetGame("shmc");
        ServersMananger sm = new ServersMananger();
        Game.Controllers.HtmlHelper hh = new Game.Controllers.HtmlHelper();
        NewsManager nm = new NewsManager();
        GameUserManager gum = new GameUserManager();

        public ActionResult Index()
        {
          /*  g = gm.GetGame("tyjy");
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
            return View();   */
            return Servers();
        }

        public ActionResult Servers()
        {
            return cg.InitServers("shmc");
        }

        public ActionResult LoginGame(int S)
        {
            return cg.LoginGame("shmc", S);
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
            g = gm.GetGame("shmc");
            if (UserId > 0)
            {
                GameUser gu = new GameUser();
                gu = gum.GetGameUser(UserId);
                ViewData["UserName"] = gu.UserName;
                ViewData["TjqfHref"] = "#";
                ViewData["TjqfName"] = "暂无";
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
                    //switch (gs.State)
                    //{
                    //    case 1:
                    //        ServerHtml += "<li><a  class=\"s2\"><span>" + gs.Name + "</span>即将开启</a></li>";
                    //        break;
                    //    case 2:
                    //        ServerHtml += "<li><a  class=\"s1\"><span>" + gs.Name + "</span>停服维护</a></li>";
                    //        break;
                    //    case 3:
                    //        ServerHtml += "<li><a href=\"/" + g.GameNo + "/LoginGame?S=" + gs.QuFu + "\" target=\"_game\" class=\"s6\"><span>" + gs.Name + "</span>流畅</a></li>";
                    //        break;
                    //    case 4:
                    //        ServerHtml += "<li><a href=\"/" + g.GameNo + "/LoginGame?S=" + gs.QuFu + "\" target=\"_game\" class=\"s4\"><span>" + gs.Name + "</span>火爆</a></li>";
                    //        break;
                    //    default:
                    //        break;
                    //}
                    ServerHtml += "<li><a href=\"/" + g.GameNo + "/LoginGame?S=" + gs.QuFu + "\" target=\"_game\">"+gs.Name+"</a></li>";
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
