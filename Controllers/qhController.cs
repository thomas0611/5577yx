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
    public class qhController : Controller
    {
        //
        // GET: /qh/
        CommonGame cg = new CommonGame();
        Games g = new Games();
        GamesManager gm = new GamesManager();
        ServersMananger sm = new ServersMananger();
        HtmlHelper hh = new HtmlHelper();
        NewsManager nm = new NewsManager();


        public ActionResult Index()
        {
            g = gm.GetGame("qh");
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
            ViewData["YqLink"] = hh.YqLinkHtml("10");

            List<News> Newlist = new List<News>();
            Newlist = nm.GetNews(7, 2, g.Id);
            string NewsHtml = "";
            foreach (News n in Newlist)
            {
                NewsHtml += "<li><span>" + n.ReleaseTime + "</span><a href=\"/NewsCenter/News?N=" + n.Id + "\" style=\"color:" + n.NameColor + "\" target=\"_blank\" title=\"" + n.Title + "\">" + (n.Title.Length < 15 ? n.Title : n.Title.Substring(0, 15)) + "</a></li>";
            }
            ViewData["News"] = NewsHtml;

            List<News> GGNewlist = new List<News>();
            GGNewlist = nm.GetNews(7, 4, g.Id);
            string GGNewsHtml = "";
            foreach (News n in GGNewlist)
            {
                GGNewsHtml += "<li><span>" + n.ReleaseTime + "</span><a href=\"/NewsCenter/News?N=" + n.Id + "\" style=\"color:" + n.NameColor + "\" target=\"_blank\" title=\"" + n.Title + "\">" + (n.Title.Length < 15 ? n.Title : n.Title.Substring(0, 15)) + "</a></li>";
            }
            ViewData["GGNews"] = GGNewsHtml;
            return View();
        }

        public ActionResult Servers()
        {
            return cg.InitServers("qh");
        }

        public ActionResult LoginGame(int S)
        {
            return cg.LoginGame("qh", S);
        }

        public ActionResult Gift(int G)
        {
            return cg.GameGift(G);
        }

        public string DoGetGift(int G)
        {
            return cg.DoGetGift(G, null);
        }
    }
}
