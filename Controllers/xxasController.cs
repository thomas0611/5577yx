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
    public class xxasController : Controller
    {
        //
        // GET: /gjqx/
        CommonGame cg = new CommonGame();
        Games g = new Games();
        GamesManager gm = new GamesManager();
        ServersMananger sm = new ServersMananger();
        HtmlHelper hh = new HtmlHelper();
        NewsManager nm = new NewsManager();

        public ActionResult Index()
        {
            g = gm.GetGame("xxas");
            List<GameServer> gsList = new List<GameServer>();
            gsList = sm.GetServersByGame(g.Id, 5);
            string ServerHtml = "";
            foreach (GameServer gs in gsList)
            {
                ServerHtml += "<li><a href=\"/" + g.GameNo + "/LoginGame?S=" + gs.QuFu + "\"><span>" + gs.Name + "</span><span class=\"status\">火爆</span></a></li>";
            }
            ViewData["ServersHtml"] = ServerHtml;
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


            List<News> Newlist = new List<News>();
            Newlist = nm.GetNews(5, 4, g.Id);
            string NewsHtml = "";
            foreach (News n in Newlist)
            {
                NewsHtml += "<li><span class=\"date\">" + n.ReleaseTime.ToString("yyyy-MM-dd") + "</span><a href=\"/NewsCenter/News?N=" + n.Id + "\">[新闻]" + (n.Title.Length < 25 ? n.Title : n.Title.Substring(0, 25)) + "</a></li>";
            }
            ViewData["News"] = NewsHtml;

            List<News> GGNewlist = new List<News>();
            GGNewlist = nm.GetNews(5, 2, g.Id);
            string GGNewsHtml = "";
            foreach (News n in GGNewlist)
            {
                GGNewsHtml += "<li><span class=\"date\">" + n.ReleaseTime.ToString("yyyy-MM-dd") + "</span><a href=\"/NewsCenter/News?N=" + n.Id + "\">[公告]" + (n.Title.Length < 25 ? n.Title : n.Title.Substring(0, 25)) + "</a></li>";
            }
            ViewData["GGNews"] = GGNewsHtml;
            return View();
        }

        public ActionResult Servers()
        {
            return cg.InitServers("xxas");
        }

        public ActionResult LoginGame(int S)
        {
            return cg.LoginGame("xxas", S);
        }

        public ActionResult Gift(int G)
        {
            return cg.GameGift(G);
        }

        public string DoGetGift(int G)
        {
            return cg.DoGetGift(G, null);
        }

        public ActionResult Ffzz()
        {
            return View();
        }

        public ActionResult FfzzDetail()
        {
            return View();
        }
    }
}
