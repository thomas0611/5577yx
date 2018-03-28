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
    public class GameController : Controller
    {
        //
        // GET: /Game/
        RoleCompetenceManager rcm = new RoleCompetenceManager();
        GamesManager gm = new GamesManager();
        GameConfigManager gcm = new GameConfigManager();

        public ActionResult Index()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 1123))
                {
                    ViewData["Style"] = "display:none";
                    if (rcm.GetRoleCompetence(master.RoleId, 11232))
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

        public ActionResult AddGame()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 11232))
                {
                    ViewData["Function"] = "AddData('/Game/DoAddGame')";
                    ViewData["IsShow"] = "display: none";
                    ViewData["GameId"] = 0;
                    ViewData["IsLock"] = true;
                    ViewData["SortId"] = 99;
                    return View("Game");
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        public Boolean DoAddGame()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return false;
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 11232))
                {
                    Games g = new Games();
                    g.Id = int.Parse(Request["GameId"]);
                    g.GameNo = Request["GameNo"];
                    g.Name = Request["GameName"];
                    g.tjqf = int.Parse(Request["tjqf"]);
                    g.GameProperty = Request["GameLx"] + "|" + Request["GameTz"] + "|" + Request["GameCzlx"] + "|" + Request["GameHm"] + "|" + Request["GameSfms"];
                    g.Is_Red = Request["IsRed"] == "on" ? 1 : 0;
                    g.Is_Hot = Request["IsHot"] == "on" ? 1 : 0;
                    g.Is_Lock = Request["IsLock"] == "on" ? 1 : 0;
                    g.Sort_Id = int.Parse(Request["SortId"]);
                    g.GameMoneyScale = int.Parse(Request["GameMoneyScale"]);
                    g.GameMoneyName = Request["GameMoneyName"];
                    g.GameCom = Request["GameCom"];
                    g.GameBBS = Request["GameBBS"];
                    g.NewHand = Request["NewHand"];
                    g.Pic1 = Request["Pic1"];
                    g.Pic2 = Request["Pic2"];
                    g.Pic3 = Request["Pic3"];
                    g.Pic4 = Request["Pic4"];
                    g.GameListImg = Request["GameListImg"];
                    g.IndexTjImg = Request["IndexTjImg"];
                    g.IndexHbImg = Request["IndexHbImg"];
                    g.IndexHdImg = Request["IndexHdImg"];
                    g.game_url_hd = Request["game_url_hd"];
                    g.game_url_xzq = Request["game_url_xzq"];
                    g.GameDesc = Request["GameDesc"];

                    return gm.AddGame(g);
                }
                else
                {
                    return false;
                }
            }


        }

        public ActionResult EditGame(int GameId)
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 11231))
                {
                    Games g = new Games();
                    g = gm.GetGame(GameId);
                    ViewData["GameId"] = GameId;
                    ViewData["GameNo"] = g.GameNo;
                    ViewData["GameName"] = g.Name;
                    ViewData["tjqf"] = g.tjqf;
                    string[] Str = g.GameProperty.Split('|');
                    ViewData["GameLx"] = Str[0];
                    ViewData["GameTz"] = Str[1];
                    ViewData["GameCzlx"] = Str[2];
                    ViewData["GameHm"] = Str[3];
                    ViewData["GameSfms"] = Str[4];
                    ViewData["IsRed"] = g.Is_Red == 1 ? true : false;
                    ViewData["IsHot"] = g.Is_Hot == 1 ? true : false;
                    ViewData["IsLock"] = g.Is_Lock == 1 ? true : false;
                    ViewData["SortId"] = g.Sort_Id;
                    ViewData["GameMoneyScale"] = g.GameMoneyScale;
                    ViewData["GameMoneyName"] = g.GameMoneyName;
                    ViewData["GameCom"] = g.GameCom;
                    ViewData["GameBBS"] = g.GameBBS;
                    ViewData["NewHand"] = g.NewHand;
                    ViewData["Pic1"] = g.Pic1;
                    ViewData["Pic2"] = g.Pic2;
                    ViewData["Pic3"] = g.Pic3;
                    ViewData["Pic4"] = g.Pic4;
                    ViewData["GameListImg"] = g.GameListImg;
                    ViewData["IndexTjImg"] = g.IndexTjImg;
                    ViewData["IndexHbImg"] = g.IndexHbImg;
                    ViewData["IndexHdImg"] = g.IndexHdImg;
                    ViewData["game_url_hd"] = g.game_url_hd;
                    ViewData["NewHand"] = g.NewHand;
                    ViewData["game_url_xzq"] = g.game_url_xzq;
                    ViewData["GameDesc"] = g.GameDesc;

                    GameConfig gc = new GameConfig();
                    gc = gcm.GetGameConfig(GameId);
                    if (gc != null)
                    {
                        ViewData["AgentId"] = gc.AgentId;
                        ViewData["LoginCom"] = gc.LoginCom;
                        ViewData["PayCom"] = gc.PayCom;
                        ViewData["ExistCom"] = gc.ExistCom;
                        ViewData["LoginTicket"] = gc.LoginTicket;
                        ViewData["PayTicket"] = gc.PayTicket;
                        ViewData["SelectTicket"] = gc.SelectTicket;
                        ViewData["FcmTicket"] = gc.FcmTicket;
                    }
                    ViewData["Function"] = "UpdateData('/Game/UpdateGame')";
                    return View("Game");
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }

        }

        public Boolean UpdateGame()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return false;
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 11231))
                {
                    Games g = new Games();
                    g.Id = int.Parse(Request["GameId"]);
                    g.GameNo = Request["GameNo"];
                    g.Name = Request["GameName"];
                    g.tjqf = int.Parse(Request["tjqf"]);
                    g.GameProperty = Request["GameLx"] + "|" + Request["GameTz"] + "|" + Request["GameCzlx"] + "|" + Request["GameHm"] + "|" + Request["GameSfms"];
                    g.Is_Red = Request["IsRed"] == "on" ? 1 : 0;
                    g.Is_Hot = Request["IsHot"] == "on" ? 1 : 0;
                    g.Is_Lock = Request["IsLock"] == "on" ? 1 : 0;
                    g.Sort_Id = int.Parse(Request["SortId"]);
                    g.GameMoneyScale = int.Parse(Request["GameMoneyScale"]);
                    g.GameMoneyName = Request["GameMoneyName"];
                    g.GameCom = Request["GameCom"];
                    g.GameBBS = Request["GameBBS"];
                    g.NewHand = Request["NewHand"];
                    g.Pic1 = Request["Pic1"];
                    g.Pic2 = Request["Pic2"];
                    g.Pic3 = Request["Pic3"];
                    g.Pic4 = Request["Pic4"];
                    g.GameListImg = Request["GameListImg"];
                    g.IndexTjImg = Request["IndexTjImg"];
                    g.IndexHbImg = Request["IndexHbImg"];
                    g.IndexHdImg = Request["IndexHdImg"];
                    g.game_url_hd = Request["game_url_hd"];
                    g.game_url_xzq = Request["game_url_xzq"];
                    g.GameDesc = Request["GameDesc"];

                    GameConfig gc = new GameConfig();
                    gc.AgentId = Request["AgentId"];
                    gc.LoginCom = Request["LoginCom"];
                    gc.PayCom = Request["PayCom"];
                    gc.ExistCom = Request["ExistCom"];
                    gc.LoginTicket = Request["LoginTicket"];
                    gc.PayTicket = Request["PayTicket"];
                    gc.SelectTicket = Request["SelectTicket"];
                    gc.FcmTicket = Request["FcmTicket"];
                    gc.GameId = int.Parse(Request["GameId"]);
                    if (gcm.GetGameConfig(g.Id) == null)
                    {
                        return (gcm.AddGameConfig(gc) && gm.UpdateGame(g));
                    }
                    else
                    {
                        return (gcm.UpdateGameConfig(gc) && gm.UpdateGame(g));
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public Boolean DelGame(int GameId)
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return false;
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 11233))
                {
                    return gm.DelGame(GameId);
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
