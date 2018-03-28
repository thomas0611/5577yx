using Common;
using Game.Manager;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Game.Controllers
{
    public class PayController : Controller
    {
        //
        // GET: /Pay/
        RoleCompetenceManager rcm = new RoleCompetenceManager();


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SdPay()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 1132))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        public ActionResult Dhq()
        {
            return View();
        }

        public ActionResult Flq()
        {
            return View();
        }


    }
}
