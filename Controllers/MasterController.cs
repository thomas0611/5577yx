using Common;
using Game.Manager;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Game.Controllers
{
    public class MasterController : Controller
    {
        //
        // GET: /Manager/
        RoleCompetenceManager rcm = new RoleCompetenceManager();
        MasterManager mm = new MasterManager();


        public ActionResult Index()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 144))
                {
                    ViewData["Style"] = "display:none";
                    if (rcm.GetRoleCompetence(master.RoleId, 1442))
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

        public ActionResult MasterLog()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 142))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        public ActionResult MasterRole()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 145))
                {
                    ViewData["Style"] = "display:none";
                    if (rcm.GetRoleCompetence(master.RoleId, 1453))
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

        public ActionResult RoleDetail(int R)
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 1451))
                {

                    string JsonStr = Common.Json.ListToJson(rcm.GetAllCompetence(R));
                    ViewData["Json"] = JsonStr;
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        public ActionResult EditRole(int R)
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 1452))
                {

                    MasterRole mr = new MasterRole();
                    mr = mm.GetMasterRole(R);
                    ViewData["RoleId"] = mr.RoleId;
                    ViewData["RoleName"] = mr.RoleName;
                    ViewData["Function"] = "UpdateData('/Master/UpdateMasterRole')";
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        public Boolean UpdateMasterRole()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return false;
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 1452))
                {
                    string RItem = Request["RItem"];
                    string RoleId = Request["RoleId"];
                    rcm.DelAllCom(int.Parse(RoleId));
                    Boolean IsSuccess = false;
                    string[] re = RItem.Split(',');
                    foreach (string rc in re)
                    {
                        IsSuccess = rcm.AddCompentence(int.Parse(RoleId), int.Parse(rc));
                        if (!IsSuccess)
                        {
                            rcm.DelAllCom(int.Parse(RoleId));
                        }
                    }
                    return IsSuccess;
                }
                else
                {
                    return false;
                }
            }
        }

        public ActionResult AddMasterRole()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 1453))
                {
                    ViewData["Function"] = "AddData('/Master/DoAddMasterRole')";
                    return View("EditRole");
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        public Boolean DoAddMasterRole()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return false;
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 1452))
                {
                    string RItem = Request["RItem"];
                    string RoleName = Request["RoleName"];
                    if (mm.AddMasterRole(RoleName))
                    {
                        MasterRole mr = new MasterRole();
                        mr = mm.GetMasterRole(RoleName);
                        Boolean IsSuccess = false;
                        string[] re = RItem.Split(',');
                        foreach (string rc in re)
                        {
                            IsSuccess = rcm.AddCompentence(mr.RoleId, int.Parse(rc));
                            if (!IsSuccess)
                            {
                                rcm.DelAllCom(mr.RoleId);
                            }
                        }
                        return IsSuccess;
                    }
                    else
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

        public ActionResult EditMaster(int M)
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 1441))
                {
                    string RoleHtml = "";
                    Master m = new Master();
                    m = mm.GetMaster(M);
                    ViewData["RoleId"] = m.RoleId;
                    ViewData["UserName"] = m.UserName;
                    ViewData["UserPWD"] = m.UserPWD;
                    ViewData["UserCzPWD"] = m.UserCzPWD;
                    ViewData["RealName"] = m.RealName;
                    ViewData["Phone"] = m.Phone;
                    ViewData["Email"] = m.Email;
                    List<MasterRole> list = mm.GetAllMasterRole();
                    foreach (MasterRole mr in list)
                    {
                        RoleHtml += "<option value=\"" + mr.RoleId + "\">" + mr.RoleName + "</option>";
                    }
                    ViewData["Role"] = RoleHtml;
                    ViewData["Function"] = "UpdateData('/Master/UpdateMaster')";
                    ViewData["MId"] = M;
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }

        }

        public Boolean UpdateMaster()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return false;
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 1441))
                {
                    Master m = new Master();
                    m.Id = int.Parse(Request["MId"]);
                    m = mm.GetMaster(m.Id);
                    int RoleId = int.Parse(Request["RoleId"]);
                    if (RoleId > 0)
                    {
                        m.RoleId = RoleId;
                        m.RoleType = mm.GetMasterRole(RoleId).RoleName;
                    }
                    else
                    {
                        return false;
                    }
                    m.State = Request["rblIsLock"] == "NoLock" ? 0 : 1;
                    m.UserName = Request["UserName"];
                    if (Request["UserPWD"] == Request["UserPWDA"])
                    {
                        m.UserPWD = DESEncrypt.Md5(Request["UserPWD"], 32);
                    }
                    else
                    {
                        if (Request["UserPWD"] == m.UserPWD)
                        {

                        }
                        else
                        {
                            return false;
                        }
                    }
                    if (Request["UserCzPWD"] == Request["UserCzPWDA"])
                    {
                        m.UserCzPWD = DESEncrypt.Md5(Request["UserCzPWD"], 32);
                    }
                    else
                    {
                        if (Request["UserCzPWD"] == m.UserCzPWD)
                        {

                        }
                        else
                        {
                            return false;
                        }
                    }
                    m.RealName = Request["RealName"];
                    m.Phone = Request["Phone"];
                    m.Email = Request["Email"];
                    return mm.UpdateMaster(m);
                }
                else
                {
                    return false;
                }
            }
        }

        public ActionResult AddMaster()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 1442))
                {
                    string RoleHtml = "";
                    List<MasterRole> list = mm.GetAllMasterRole();
                    foreach (MasterRole mr in list)
                    {
                        RoleHtml += "<option value=\"" + mr.RoleId + "\">" + mr.RoleName + "</option>";
                    }
                    ViewData["Role"] = RoleHtml;
                    ViewData["Function"] = "AddData('/Master/DoAddMaster')";
                    return View("EditMaster");
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        public Boolean DoAddMaster()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return false;
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 1442))
                {
                    Master m = new Master();
                    int RoleId = int.Parse(Request["RoleId"]);
                    if (RoleId > 0)
                    {
                        m.RoleId = RoleId;
                        m.RoleType = mm.GetMasterRole(RoleId).RoleName;
                    }
                    else
                    {
                        return false;
                    }
                    m.State = Request["rblIsLock"] == "NoLock" ? 0 : 1;
                    m.UserName = Request["UserName"];
                    if (Request["UserPWD"] == Request["UserPWDA"])
                    {
                        m.UserPWD = DESEncrypt.Md5(Request["UserPWD"], 32);
                    }
                    else
                    {
                        return false;
                    }
                    if (Request["UserCzPWD"] == Request["UserCzPWDA"])
                    {
                        m.UserCzPWD = DESEncrypt.Md5(Request["UserCzPWD"], 32);
                    }
                    else
                    {
                        return false;
                    }
                    m.RealName = Request["RealName"];
                    m.Phone = Request["Phone"];
                    m.Email = Request["Email"];
                    return mm.AddMaster(m);
                }
                else
                {
                    return false;
                }
            }

        }

        public Boolean DelMaster(int Mid)
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return false;
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 142))
                {
                    return mm.DelMaster(Mid);
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
