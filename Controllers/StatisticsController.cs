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
    public class StatisticsController : Controller
    {
        //
        // GET: /Statistics/
        RoleCompetenceManager rcm = new RoleCompetenceManager();
        GameUserManager gum = new GameUserManager();
        OrderManager om = new OrderManager();
        GamesManager gm = new GamesManager();
        SourceChangeManager scm = new SourceChangeManager();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SourceChange()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 128))
                {
                    if (master.UserName == "odin33774006")
                    {
                        ViewData["cz"] = " <th>操作</th>";
                    }
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        public ActionResult Collect()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 123))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        public ActionResult PromoAnalysis()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 126))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        public ActionResult PromoByBengBeng()
        {
            return View();
        }
        /// <summary>
        /// 获取推广用户
        /// </summary>
        /// <returns></returns>
        public ActionResult PromoUser(int userId)
        {
            //int GameId = int.Parse(Request["GameId"]);
            //int ServerId = int.Parse(Request["ServerId"]);
            List<GameUser> ListUser = gum.GetSpreadUser(userId);
            ViewData["listUser"] = ListUser;
            return View();
        }

        /// <summary>
        /// 获取用户角色等级信息
        /// </summary>
        /// <returns></returns>
        public ActionResult PromoUserInfo()
        {
            int GameId = int.Parse(Request ["GameId"]);
            int ServerId = int.Parse(Request["ServerId"]);
            List<int> ListUser = gum.GetSpreadUserByBengBeng(GameId,"BengBeng");
            List<GameUserInfo> ListUserInfo = new List<GameUserInfo>();
            foreach(int user in ListUser.Take(15))
            {
                GameUserInfo gui = new GameUserInfo();
                gui = gm.GetGameUserInfo(GameId,user,ServerId);
                if(gui.Message == "Success")
                {
                    ListUserInfo.Add(gui);
                }
            }
            ViewData["listUser"] = ListUserInfo;
            ViewData["GameId"] = GameId;
            ViewData["ServerId"] = ServerId;
            return View();
        }

        /// <summary>
        /// 获取充值排行
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSumMoney()
        {
            return View();
        }

        public ActionResult PromoDetial(int Id)
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 1261))
                {
                    GameUser gu = gum.GetGameUser(Id);
                    if (gu.IsSpreader > 0)
                    {
                        ViewData["SpreadCount"] = om.GetAllSpreadCount(Id);
                        ViewData["UserName"] = gu.UserName;
                        ViewData["SpreadMoney"] = om.GetSumMoney(Id, "");
                        Session[Keys.SESSION_USER] = Id;
                    }
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
            return View();
        }

        public Boolean DelSourceChange(int SCId)
        {
            return scm.DelSourceChange(SCId);
        }
    }
}
