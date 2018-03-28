using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Game.Controllers
{
    public class wdqkController : Controller
    {
        //
        // GET: /wdqk/

        CommonGame cg = new CommonGame();

        public ActionResult Index()
        {
            return Servers();
        }

        public ActionResult Servers()
        {
            return cg.InitServers("wdqk");
        }

        public ActionResult LoginGame(int S)
        {
            return cg.LoginGame("wdqk", S);
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
