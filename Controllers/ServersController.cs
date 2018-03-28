using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Game.Model;
using Game.Manager;
using Common;

namespace Game.Controllers
{
    public class ServersController : Controller
    {
        //
        // GET: /Servers/
        RoleCompetenceManager rcm = new RoleCompetenceManager();
        ServersMananger sm = new ServersMananger();
        HtmlHelper hh = new HtmlHelper();

        public ActionResult Index()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 1124))
                {
                    ViewData["Style"] = "display:none";
                    if (rcm.GetRoleCompetence(master.RoleId, 11242))
                    {
                        ViewData["style"] = "display:block";
                    }
                    int GameId = int.Parse(string.IsNullOrEmpty(Request["GameId"]) ? "0" : Request["GameId"]);
                    ViewData["GameId"] = GameId;
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        public ActionResult AddServer()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 11242))
                {
                    int GameId = int.Parse(string.IsNullOrEmpty(Request["GameId"]) ? "0" : Request["GameId"]);
                    ViewData["AllGameHtml"] = hh.AllGameHtml();
                    ViewData["GameId"] = GameId;
                    ViewData["Sort_Id"] = 99;
                    ViewData["Function"] = "AddData('/Servers/DoAddServer')";
                    return View("Server");
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        public Boolean DoAddServer()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return false;
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 11242))
                {
                    GameServer gs = new GameServer();
                    gs.GameId = int.Parse(Request["GameId"]);
                    gs.ServerNo = Request["ServerNo"];
                    gs.QuFu = Request["QuFu"];
                    gs.Name = Request["ServerName"];
                    gs.Line = Request["Line"];
                    gs.State = int.Parse(Request["State"]);
                    gs.StartTime = DateTime.Parse(Request["StartTime"]);
                    gs.Sort_Id = int.Parse(Request["Sort_Id"]);
                    gs.Img = Request["Img"];
                    gs.ServerDesc = Request["ServerDesc"];
                    return sm.AddServer(gs);
                }
                else
                {
                    return false;
                }
            }
        }

        public ActionResult EditServer(int ServerId)
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 11241))
                {
                    GameServer gs = new GameServer();
                    gs = sm.GetGameServer(ServerId);
                    ViewData["ServerId"] = gs.Id;
                    ViewData["ServerNo"] = gs.ServerNo;
                    ViewData["GameId"] = gs.GameId;
                    ViewData["QuFu"] = gs.QuFu;
                    ViewData["ServerName"] = gs.Name;
                    ViewData["Line"] = gs.Line;
                    ViewData["State"] = gs.State;
                    ViewData["StartTime"] = gs.StartTime;
                    ViewData["Sort_Id"] = gs.Sort_Id;
                    ViewData["Img"] = gs.Img;
                    ViewData["ServerDesc"] = gs.ServerDesc;
                    ViewData["ServerId"] = gs.Id;
                    ViewData["ServerId"] = gs.Id;

                    ViewData["Function"] = "UpdateData('/Servers/UpdateServer')";

                    ViewData["AllGameHtml"] = hh.AllGameHtml();
                    return View("Server");
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        public Boolean UpdateServer()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return false;
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 11241))
                {
                    GameServer gs = new GameServer();
                    gs.Id = int.Parse(Request["ServerId"]);
                    gs.GameId = int.Parse(Request["GameId"]);
                    gs.ServerNo = Request["ServerNo"];
                    gs.QuFu = Request["QuFu"];
                    gs.Name = Request["ServerName"];
                    gs.Line = Request["Line"];
                    gs.State = int.Parse(Request["State"]);
                    gs.StartTime = DateTime.Parse(Request["StartTime"]);
                    gs.Sort_Id = int.Parse(Request["Sort_Id"]);
                    gs.Img = Request["Img"];
                    gs.ServerDesc = Request["ServerDesc"];
                    return sm.UpdateServer(gs);
                }
                else
                {
                    return false;
                }
            }
        }

        public Boolean DelServer(int ServerId)
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return false;
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 11243))
                {
                    return sm.DelServer(ServerId);
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
