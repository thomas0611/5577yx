using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Game.Controllers
{
    public class yjxyController : Controller
    {
        //
        // GET: /yjxy/
        CommonGame cg = new CommonGame();

        public ActionResult Index()
        {
            return Servers();
        }

        public ActionResult Servers()
        {
            return cg.InitServers("yjxy");
        }

        public ActionResult LoginGame(int S)
        {
            return cg.LoginGame("yjxy", S);
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
