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
    public class tgztController : Controller
    {
        //
        // GET: /tgzt/
        CommonGame cg = new CommonGame();
        Games g = new Games();
        GamesManager gm = new GamesManager();
        ServersMananger sm = new ServersMananger();
        HtmlHelper hh = new HtmlHelper();
        NewsManager nm = new NewsManager();

        public ActionResult Index()
        {
            return Servers();
        }

        public ActionResult Servers()
        {
            return cg.InitServers("tgzt");
        }

        public ActionResult LoginGame(int S)
        {
            return cg.LoginGame("tgzt", S);
        }

        public ActionResult Gift(int G)
        {
            return cg.GameGift(G);
        }

        public string DoGetGift(int G)
        {
            return cg.DoGetGift(G, null);
        }

        public ActionResult Kfzc()
        {
            return View();
        }
    }
}
