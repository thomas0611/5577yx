using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Game.Manager;
using Game.Model;

namespace Game.Controllers
{
    public class NewsCenterController : Controller
    {
        //
        // GET: /NewsCenter/
        NewsManager nm = new NewsManager();
        GamesManager gm = new GamesManager();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult News(int N)
        {
            News news = new News();
            news = nm.GetNews(N);
            ViewData["NewsTitle"] = news.Title;
            ViewData["Game"] = gm.GetGame(news.GameId).Name;
            ViewData["Time"] = news.ReleaseTime;
            ViewData["NewsContent"] = news.NewsContent;

            return View();
        }

    }
}
