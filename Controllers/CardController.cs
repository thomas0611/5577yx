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
    public class CardController : Controller
    {
        //
        // GET: /Card/
        RoleCompetenceManager rcm = new RoleCompetenceManager();
        CardManager cm = new CardManager();
        GamesManager gm = new GamesManager();

        public ActionResult Index()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 1125))
                {
                    ViewData["Style"] = "display:none";
                    if (rcm.GetRoleCompetence(master.RoleId, 11252))
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

        public ActionResult AddCard()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 11252))
                {
                    ViewData["Function"] = "AddData('/Card/DoAddCard')";
                    ViewData["GameId"] = 0;
                    ViewData["IsLock"] = true;
                    return View("Card");
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        [ValidateInput(false)]
        public Boolean DoAddCard()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return false;
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 11252))
                {
                    cardsname cn = new cardsname();
                    cn.gameid = int.Parse(Request["GameId"]);
                    cn.serverid = int.Parse(Request["ServerId"]);
                    cn.cardname = Request["CardName"];
                    cn.urls = Request["Url"];
                    cn.islock = Request["IsLock"] == "on" ? 1 : 0;
                    cn.img = Request["Img"];
                    cn.carddesc = Request["CardDesc"];
                    return cm.AddCard(cn);
                }
                else
                {
                    return false;
                }
            }
        }

        public ActionResult EditCard(int CardId)
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 11251))
                {
                    ViewData["Function"] = "UpdateData('/Card/UpdateCard')";
                    cardsname cn = new cardsname();
                    cn = cm.GetCard(CardId);
                    ViewData["CardId"] = cn.id;
                    ViewData["GameId"] = cn.gameid;
                    ViewData["ServerId"] = cn.serverid;
                    ViewData["CardName"] = cn.cardname;
                    ViewData["Url"] = cn.urls;
                    ViewData["IsLock"] = cn.islock == 1 ? true : false; ;
                    ViewData["Img"] = cn.img;
                    ViewData["CardDesc"] = cn.carddesc;
                    return View("Card");
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        [ValidateInput(false)]
        public Boolean UpdateCard()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return false;
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 11251))
                {
                    cardsname cn = new cardsname();
                    cn.id = int.Parse(Request["CardId"]);
                    cn.gameid = int.Parse(Request["GameId"]);
                    cn.serverid = int.Parse(Request["ServerId"]);
                    cn.cardname = Request["CardName"];
                    cn.urls = Request["Url"];
                    cn.islock = Request["IsLock"] == "on" ? 1 : 0;
                    cn.img = Request["Img"];
                    cn.carddesc = Request["CardDesc"];
                    return cm.UpdateCard(cn);
                }
                else
                {
                    return false;
                }
            }
        }

        public Boolean DelCard(int CardId)
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return false;
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 11253))
                {
                    return cm.DelCard(CardId);
                }
                else
                {
                    return false;
                }
            }
        }

        public ActionResult ImportCard(int CardId)
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 11254))
                {
                    ViewData["CardId"] = CardId;
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        public Boolean DoImportCard()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return false;
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 11254))
                {
                    try
                    {
                        cards c = new cards();
                        int CardId = int.Parse(Request["CardId"]);
                        string CardTextContent = Request["CardTextContent"].Replace("\n", "|");
                        string[] CardContent = CardTextContent.Split('|');
                        foreach (string Card in CardContent)
                        {
                            if (!string.IsNullOrEmpty(Card) && !cm.ExitCard(CardId, Card))
                            {
                                c.cardnum = Card;
                                c.cardnameid = CardId;
                                c.addtime = DateTime.Now;
                                c.state = 0;
                                cm.AddCard(c);
                            }
                        }
                        return true;
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public ActionResult CardLog()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 11255))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }
    }
}
