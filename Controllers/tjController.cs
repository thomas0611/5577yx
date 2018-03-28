using Game.Manager;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Game.Controllers
{
    public class tjController : Controller
    {
        //
        // GET: /tj/
        CommonGame cg = new CommonGame();
        Games g = new Games();
        GamesManager gm = new GamesManager();
        ServersMananger sm = new ServersMananger();
        HtmlHelper hh = new HtmlHelper();

        public ActionResult Index()
        {
            return Servers();
        }

        public ActionResult Servers()
        {
            return cg.InitServers("tj");
        }

        public ActionResult LoginGame(int S)
        {
            return cg.LoginGame("tj", S);
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
