using Game.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Game.Controllers
{
    public class GameGiftController : Controller
    {
        //
        // GET: /GameGift/
        CardManager cm = new CardManager();

        public ActionResult Index()
        {
            ViewData["GiftCount"] = cm.GetCardCount("");
            return View();
        }

    }
}
