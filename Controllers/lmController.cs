using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Game.Controllers
{
    public class lmController : Controller
    {
        //
        // GET: /lm/
        CommonGame cg = new CommonGame();

        public ActionResult Index()
        {
            return Servers();
        }

        public ActionResult Servers()
        {
            return cg.InitServers("lm");
        }

        public ActionResult LoginGame(int S)
        {
            return cg.LoginGame("lm", S);
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
