using Game.Manager;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Game.Controllers
{
    public class AboutController : Controller
    {
        //
        // GET: /About/
        WebInfoManager wim = new WebInfoManager();

        public ActionResult Index()
        {
            int WebInfoId = string.IsNullOrEmpty(Request["Id"]) ? 1 : int.Parse(Request["Id"]);
            sys_onepage wi = wim.GetWebInfo(WebInfoId);
            ViewData["AboutTitle"] = wi.title;
            ViewData["AboutText"] = wi.contents;
            return View();
        }

    }
}
