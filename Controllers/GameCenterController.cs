using Game.Manager;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Game.Controllers
{
    public class GameCenterController : Controller
    {
        //
        // GET: /GameCenter/
        GamesManager gm = new GamesManager();
        HtmlHelper hh = new HtmlHelper();

        public ActionResult Index()
        {
            List<Games> Hotlist = new List<Games>();
            Hotlist = gm.GetAll(3, "is_hot");
            ViewData["HotGame"] = hh.TjGameHtml(Hotlist);

            List<Games> list = new List<Games>();
            list = gm.GetAll("where is_lock=1 order by sort_id");
            ViewData["AllGame"] = hh.GameHtml2(list);
            return View();
        }

    }
}
