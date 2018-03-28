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
    public class qjllController : Controller
    {
        //
        // GET: /qjll/

        CommonGame cg = new CommonGame();
        GamesManager gm = new GamesManager();
        Games g = new GamesManager().GetGame("qjll");
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
            return cg.InitServers("qjll");
        }

        public ActionResult LoginGame(int S)
        {
            return cg.LoginGame("qjll", S);
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
            return View();
        }

    }
}
