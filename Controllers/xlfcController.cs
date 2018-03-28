using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Game.Model;
using Game.Manager;

namespace Game.Controllers
{
    public class xlfcController : Controller
    {
        //
        // GET: /xlfc/

        CommonGame cg = new CommonGame();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Servers()
        {
            return cg.InitServers("xlfc");
        }
    }
}
