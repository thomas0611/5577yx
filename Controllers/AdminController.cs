using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Game.Manager;
using Game.Model;

namespace Game.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/
        RoleCompetenceManager rcm = new RoleCompetenceManager();

        public ActionResult Index()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 1))
                {
                    ViewData["UserName"] = master.UserName;
                    ViewData["MasterRole"] = master.RoleType;
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        public void DelAdminLogin()
        {
            string UserName = Request["UserName"];
            string PassWord = Request["PassWord"];
            string VerifyCode = Request["VerifyCode"];
            MasterManager mm = new MasterManager();
            if (VerifyCode.ToLower() != Session[Keys.SESSION_CODE].ToString().ToLower())
            {
                Response.Write("您输入的验证码不正确！");
                return;
            }
            if (!mm.IsManager(UserName))
            {
                Response.Write("您输入的用户名不存在！");
                return;
            }
            Master master = mm.GetManager(UserName, DESEncrypt.Md5(PassWord, 32));
            if (master == null)
            {
                Response.Write("您输入的密码错误！");
                return;
            }
            Session[Keys.SESSION_ADMIN_INFO] = master;
            Session.Timeout = 45;
            AddMasterLog(master);
        }

        private void AddMasterLog(Master master)
        {
            manager_log ml = new manager_log();
            ml.user_id = master.Id;
            ml.user_name = master.UserName;
            ml.action_type = "Login";
            ml.note = master.RoleType + "登录";
            ml.login_ip = BBRequest.GetIP();
            ml.login_time = DateTime.Now;
            new MasterManager().AddMasterLog(ml);
        }

        public ActionResult LoginOut()
        {
            Session.Remove(Keys.SESSION_ADMIN_INFO);
            return RedirectToAction("Login", "Admin");
        }

        public ActionResult Center()
        {
            SiteConfig sc = new SiteConfigManager().loadConfig(Utils.GetXmlMapPath("Configpath"));
            ViewData["webname"] = sc.webname;
            ViewData["webcompany"] = sc.webcompany;
            return View();
        }

        public GameUserInfo GetGameUserInfo()
        {
            GamesManager gm = new GamesManager();
            int GameId = Convert.ToInt32(Request["GameId"]);
            int UserId = Convert.ToInt32(Request["UserId"]);
            int ServerId = Convert.ToInt32(Request["ServerId"]);

            GameUserInfo gui= gm.GetGameUserInfo(GameId ,UserId ,ServerId );
            return gui;
        }

        public ActionResult GameUserInfo()
        {
            return View();
        }
    }
}
