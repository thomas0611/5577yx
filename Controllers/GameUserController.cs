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
    public class GameUserController : Controller
    {
        //
        // GET: /GameUser/
        RoleCompetenceManager rcm = new RoleCompetenceManager();
        GameUserManager gum = new GameUserManager();
        GamesManager gm = new GamesManager();
        SourceChangeManager scm = new SourceChangeManager();
        LockManager alm = new LockManager();

        public ActionResult Index()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 131))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        public ActionResult EidtUser(int Id)
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 1311))
                {
                    GameUser gu = gum.GetGameUser(Id);
                    ViewData["UserId"] = gu.Id;
                    ViewData["UserName"] = gu.UserName;
                    ViewData["Sex"] = gu.Sex == "0" ? "男" : "女";
                    ViewData["Phone"] = gu.Phone;
                    ViewData["RealName"] = gu.RealName;
                    ViewData["Email"] = gu.Email;
                    ViewData["QQ"] = gu.QQ;
                    ViewData["Cards"] = gu.Cards;
                    ViewData["IsSpreader"] = gu.IsSpreader;
                    ViewData["GradeId"] = gu.GradeId;
                    ViewData["Money"] = gu.Money;
                    ViewData["GameMoney"] = gu.GameMoney;
                    ViewData["Points"] = gu.Points;
                    ViewData["BirthDay"] = gu.BirthDay;
                    ViewData["Source"] = gu.Source > 0 ? gum.GetGameUser(gu.Source).UserName : "用户注册";
                    ViewData["RegGame"] = gu.RegGame;
                    ViewData["AddTime"] = gu.AddTime;
                    ViewData["LastLoginTime"] = gu.LastLoginTime;
                    ViewData["Ip"] = gu.Ip;
                    ViewData["IsValiDate"] = gu.IsValiDate == 1 ? "已验证" : "未验证";
                    ViewData["IsLock"] = gu.IsLock == 1 ? "锁定" : "正常";
                    ViewData["UserDesc"] = gu.UserDesc;
                    return View("GameUser");
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        public Boolean UpdateGameUser()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return false;
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 1311))
                {
                    string Type = Request["Type"];
                    int UserId = int.Parse(Request["UserId"]);
                    GameUser gu = gum.GetGameUser(UserId);
                    int OldSouce = gu.Source;
                    if (Type == "Save")
                    {
                        int IsSpread = int.Parse(Request["IsSpread"]);
                        string Source = Request["Source"];
                        int RegGame = int.Parse(Request["RegGame"]);
                        int IsLock = int.Parse(Request["IsLock"]);
                        if (!string.IsNullOrEmpty(Source))
                        {
                            if (Source != "用户注册")
                            {
                                GameUser spread = gum.GetGameUser(Source);
                                if (spread.IsSpreader > IsSpread)
                                {
                                    gu.Source = spread.Id;
                                    gu.RegGame = RegGame;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                gu.Source = 0;
                                gu.RegGame = 0;
                            }
                            SourceChange sc = new SourceChange(0, gu.UserName, OldSouce, gu.Source, DateTime.Now, master.UserName);
                            scm.AddSourceChange(sc);
                        }
                        gu.IsSpreader = IsSpread;
                        gu.IsLock = IsLock;
                    }
                    else
                    {
                        gu.Sex = "0";
                        gu.Phone = "";
                        gu.RealName = "";
                        gu.Email = "";
                        gu.QQ = "";
                        gu.Cards = "";
                        gu.BirthDay = "";
                        gu.UserDesc = "";
                        gu.PWD = DESEncrypt.Md5("111111", 32);
                    }
                    return gum.UpdateUser(gu);
                }
                else
                {
                    return false;
                }
            }
        }

        public ActionResult IpLock()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 134))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        public Boolean AddLock()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return false;
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 1341))
                {
                    string Lock = Request["LockInfo"];
                    string LockInfo = "Ip锁定";
                    try
                    {
                        int s = 0;
                        string[] a = DESEncrypt.Decrypt(Lock).Split('|');
                        string b = a[0];
                        string c = a[1];
                        if (int.TryParse(a[0], out s) && int.TryParse(a[1], out s))
                        {
                            GameUser gu = new GameUser();
                            gu = gum.GetGameUser(int.Parse(a[0]));
                            Games g = gm.GetGame(int.Parse(a[1]));
                            if (gu != null)
                            {
                                if (gu.IsSpreader > 0)
                                {
                                    LockInfo = gu.UserName + "的" + g.Name + "链接";
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {
                    }
                    if (!string.IsNullOrEmpty(Lock) && !alm.IsLock(Lock))
                    {
                        return alm.AddLock(Lock, master.UserName, LockInfo);
                    }
                }
            }
            return false;
        }

        public Boolean DelLock()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return false;
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 1341))
                {
                    string Lock = Request["Lock"];
                    if (!string.IsNullOrEmpty(Lock) && alm.IsLock(Lock))
                    {
                        return alm.DelLock(Lock);
                    }
                }
            }
            return false;
        }
    }
}
