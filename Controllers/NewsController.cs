using Common;
using Game.Manager;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Game.Controllers.Admin
{
    public class NewsController : Controller
    {
        //
        // GET: /News/
        RoleCompetenceManager rcm = new RoleCompetenceManager();
        NewsManager nm = new NewsManager();

        public ActionResult Index()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 1121))
                {
                    ViewData["Style"] = "display:none";
                    if (rcm.GetRoleCompetence(master.RoleId, 11212))
                    {
                        ViewData["style"] = "display:block";
                    }
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        public ActionResult AddNews()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 11212))
                {
                    ViewData["Source"] = "本站";
                    ViewData["SortId"] = 99;
                    ViewData["Function"] = "AddData('/News/DoAddNews')";
                    return View("News");
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        [ValidateInput(false)]
        public Boolean DoAddNews()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return false;
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 11212))
                {
                    News n = new News();
                    n.GameId = int.Parse(Request["GameId"]);
                    n.Type = int.Parse(Request["Type"]);
                    n.Title = Request["Title"];
                    n.NameColor = Request["NameColor"];
                    n.IsHot = Request["IsHot"] == "on" ? 1 : 0;
                    n.IsRed = Request["IsRed"] == "on" ? 1 : 0;
                    n.IsTop = Request["IsTop"] == "on" ? 1 : 0;
                    n.KeyWord = string.IsNullOrEmpty(Request["KeyWord"]) ? "" : Request["KeyWord"];
                    n.Source = string.IsNullOrEmpty(Request["Source"]) ? "本站" : Request["Source"];
                    n.SortId = int.Parse(string.IsNullOrEmpty(Request["SortId"]) ? "99" : Request["SortId"]);
                    n.Photo = string.IsNullOrEmpty(Request["Photo"]) ? "" : Request["Photo"];
                    n.NewsContent = Request["NewsContent"];
                    return nm.AddNews(n);
                }
                else
                {
                    return false;
                }
            }
        }

        public ActionResult EditNews(int NewsId)
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 11211))
                {
                    News news = new News();
                    news = nm.GetNews(NewsId);
                    ViewData["GameId"] = news.GameId;
                    ViewData["Type"] = news.Type;
                    ViewData["Title"] = news.Title;
                    ViewData["NameColor"] = news.NameColor;
                    ViewData["IsHot"] = news.IsHot == 1 ? true : false;
                    ViewData["IsRed"] = news.IsRed == 1 ? true : false;
                    ViewData["IsTop"] = news.IsTop == 1 ? true : false;
                    ViewData["KeyWord"] = news.KeyWord;
                    ViewData["Source"] = news.Source;
                    ViewData["SortId"] = news.SortId;
                    ViewData["Photo"] = news.Photo;
                    ViewData["NewsContent"] = news.NewsContent;
                    ViewData["NewsId"] = news.Id;
                    ViewData["Function"] = "UpdateData('/News/UpdateNews')";
                    return View("News");
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        [ValidateInput(false)]
        public Boolean UpdateNews()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return false;
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 11211))
                {
                    News n = new News();
                    n.GameId = int.Parse(Request["GameId"]);
                    n.Type = int.Parse(Request["Type"]);
                    n.Title = Request["Title"];
                    n.NameColor = Request["NameColor"];
                    n.IsHot = Request["IsHot"] == "on" ? 1 : 0;
                    n.IsRed = Request["IsRed"] == "on" ? 1 : 0;
                    n.IsTop = Request["IsTop"] == "on" ? 1 : 0;
                    n.KeyWord = Request["KeyWord"];
                    n.Source = Request["Source"];
                    n.SortId = int.Parse(Request["SortId"]);
                    n.Photo = string.IsNullOrEmpty(Request["Photo"]) ? "" : Request["Photo"];
                    n.NewsContent = Request["NewsContent"];
                    n.Id = int.Parse(Request["NewsId"]);
                    return nm.UpdateNews(n);
                }
                else
                {
                    return false;
                }
            }
        }

        public Boolean DelNews(int NewsId)
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return false;
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 11213))
                {
                    return nm.DelNews(NewsId);
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
