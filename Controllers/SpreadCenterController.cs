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
    public class SpreadCenterController : Controller
    {
        //
        // GET: /SpreadCenter/
        GameUserManager gum = new GameUserManager();
        OrderManager om = new OrderManager();
        GamesManager gm = new GamesManager();
        SysMsgManager smm = new SysMsgManager();
        ValiDateCodeManager vdcm = new ValiDateCodeManager();
        LockManager alm = new LockManager();

        public ActionResult Index()
        {
            ViewData["UserCenterOn"] = "current";
            ViewData["YejiOn"] = "chosen";
            int UserId = BBRequest.GetUserId();
            if (UserId > 0)
            {
                GameUser gu = gum.GetGameUser(UserId);
                if (gu.IsSpreader > 0)
                {
                    ViewData["Photo"] = gu.Photo;
                    ViewData["SpreadCount"] = om.GetAllSpreadCount(UserId);
                    ViewData["UserName"] = gu.UserName;
                    ViewData["SpreadMoney"] = om.GetSumMoney(UserId, "");
                    ViewData["Style"] = "display:none";
                    if (gu.IsSpreader == 2)
                    {
                        ViewData["CaoZuo"] = "<th>操作</th>";
                        ViewData["Style"] = "";
                    }
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "SpreadCenter");
            }
            return View();
        }

        public ActionResult SpreadGame()
        {
            ViewData["UserCenterOn"] = "current";
            ViewData["GameOn"] = "chosen";
            int UserId = BBRequest.GetUserId();
            if (UserId > 0)
            {
                GameUser gu = gum.GetGameUser(UserId);
                if (gu.IsSpreader > 0)
                {
                    ViewData["Photo"] = gu.Photo;
                    ViewData["SpreadCount"] = om.GetAllSpreadCount(UserId);
                    ViewData["UserName"] = gu.UserName;
                    if (gu.IsSpreader != 2)
                    {
                        ViewData["Style"] = "display:none";
                    }

                    List<Games> list = new List<Games>();
                    list = gm.GetAll("where is_lock=1 order by sort_id ");
                    ViewData["Action"] = DESEncrypt.Encrypt(UserId + "|" + list[list.Count - 1].Id);
                    ViewData["GameName"] = list[list.Count - 1].Name;
                    string HtmlGame = "";
                    foreach (Games g in list)
                    {
                        string Action = DESEncrypt.Encrypt(UserId + "|" + g.Id);
                        HtmlGame += "<li style=\"width: 210px;\"><a onclick=\"GetSpreadText('" + g.Name + "','" + Action + "')\"><img src=\"" + g.GameListImg + "\" width=\"200px\" height=\"110px\"></a><label for=\"male\">" + g.Name + "</label></li>";
                    }
                    ViewData["HtmlGame"] = HtmlGame;
                }
                else
                {
                    return RedirectToAction("Login", "SpreadCenter");
                }
            }
            else
            {
                return RedirectToAction("Login", "SpreadCenter");
            }
            return View();
        }

        public ActionResult Login()
        {
            if (Utils.GetCookie("6qmgamesame") != "")
            {
                string value = Utils.GetCookie("6qmgamesame");
                string UserName = DESEncrypt.DesDecrypt(value.Split('|')[0]);
                ViewData["UserName"] = UserName;
            }
            int UserId = BBRequest.GetUserId();
            if (UserId > 0)
            {
                Session.RemoveAll();
            }
            return View();
        }

        public ActionResult AddUnderUser()
        {
            ViewData["UserCenterOn"] = "current";
            ViewData["UnderOn"] = "chosen";
            int UserId = BBRequest.GetUserId();
            if (UserId > 0)
            {
                GameUser gu = gum.GetGameUser(UserId);
                if (gu.IsSpreader > 1)
                {
                    ViewData["Photo"] = gu.Photo;
                    ViewData["SpreadCount"] = om.GetAllSpreadCount(UserId);
                    ViewData["UserName"] = gu.UserName;
                    ViewData["SpreadMoney"] = om.GetSumMoney(UserId, "");
                    if (gu.IsSpreader != 2)
                    {
                        ViewData["Style"] = "display:none";
                    }
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "SpreadCenter");
                }
            }
            else
            {
                return RedirectToAction("Login", "SpreadCenter");
            }
        }

        public string AddUnderSpreader()
        {
            GameUser gu = new GameUser();
            string UserName = Request["UserName"].Trim();
            string Pwd = Request["PWD"].Trim();
            int UserId = BBRequest.GetUserId();
            if (UserId > 0)
            {
                GameUser Spreader = gum.GetGameUser(UserId);
                if (Spreader.IsSpreader > 1)
                { }
                else
                {
                    return "您不是推广团长，不能添加推广员！";
                }
            }
            else
            {
                return "您还未登陆，不能添加推广员！";
            }
            if (!DevRegHel.RegName(UserName))
            {
                return "您输入的用户名不可用！";
            }
            if (!DevRegHel.RegPwd(Pwd))
            {
                return "您输入的密码不可用！";
            }
            if (alm.IsLock(BBRequest.GetIP()))
            {
                return "您暂时不能注册！";
            }
            gu = new GameUser(0, Request["UserName"], DESEncrypt.Md5(Request["PWD"], 32), "", "0", "", "", "", ""
                  , "", "", "", "1", UserId, "", 0, 0, 0, 0, 1, 0, DateTime.Now, DateTime.Now, 0, 0, 0, 0, BBRequest.GetIP(),
                  "", 0, "", "");
            try
            {
                if (gum.AddUser(gu))
                {
                    int Id = gum.GetGameUser(UserName).Id;
                    sysmsg sm = new sysmsg();
                    sm.title = "注册成功消息";
                    sm.type = 2;
                    sm.userid = Id;
                    sm.fromid = 0;
                    sm.msg = "恭喜您成功注册5577游戏账号，您可凭借此账号登录5577游戏旗下任何一款游戏，祝您游戏愉快。如果您在游戏过程中遇到任何问题，欢迎您致电客服咨询。";
                    smm.AddSysMsg(sm);
                    //Session[Keys.SESSION_USER] = Id;
                    //Session.Timeout = 120;
                    //validatecode vdc = new validatecode();
                    //vdcm.DelValiDateCode(Id, 1);
                    //vdc.type = 1;
                    //vdc.userid = Id;
                    //vdc.sendtime = DateTime.Now;
                    //vdc.code = Guid.NewGuid().ToString() + DateTime.Now.Minute + DateTime.Now.Millisecond;
                    //vdc.email = gu.Email.Trim();
                    //vdcm.AddValiDateCode(vdc);
                    //string ucode = DESEncrypt.encryptstring1(vdc.userid.ToString());
                    //string tcode = DESEncrypt.encryptstring1(vdc.type.ToString());
                    //string scode = vdc.code.ToString();
                    //string vicode = vdc.sendtime.ToString("yyyy-MM-ddHH:mm:ss");
                    return "True";
                }
                else
                {
                    return "注册失败！";
                }
            }
            catch (Exception ex)
            {
                gu.IsLock = 1;
                gu.UserDesc = "此用户为注册失败用户！失败原因：" + ex.Message;
                gum.UpdateUser(gu);
                //gum.DelGameUser(UserName);
                return "注册失败！错误：" + ex.Message;
            }
        }

        public Boolean ResetSpreader()
        {
            int UserId = int.Parse(Request["UId"]);
            GameUser gu = gum.GetGameUser(UserId);
            gu.Sex = "0";
            gu.Phone = "";
            gu.RealName = "";
            gu.Email = "";
            gu.QQ = "";
            gu.Cards = "";
            gu.BirthDay = "";
            gu.UserDesc = "";
            gu.IsLock = 1;
            gu.PWD = DESEncrypt.Md5("111111", 32);
            return gum.UpdateUser(gu);
        }

        public ActionResult Default()
        {
            int UserId = BBRequest.GetUserId();
            if (UserId > 0)
            {
                GameUser gu = gum.GetGameUser(UserId);
                if (gu.IsSpreader > 0)
                {
                    ViewData["SpreadCount"] = om.GetAllSpreadCount(UserId);
                    ViewData["UserName"] = gu.UserName;
                    ViewData["SpreadMoney"] = om.GetSumMoney(UserId, "");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "SpreadCenter");
            }
            return View();
        }

        public ActionResult SpreadPaymoney()
        {
            return IsLogined();
        }

        public ActionResult SpreadUser()
        {
            return IsLogined();
        }

        public ActionResult SpreadSum()
        {
            return IsLogined();
        }

        public ActionResult IsLogined()
        {
            int UserId = BBRequest.GetUserId();
            if (UserId > 0)
            {
                GameUser gu = gum.GetGameUser(UserId);
                if (gu.IsSpreader > 0)
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                return RedirectToAction("Login", "SpreadCenter");
            }
        }
    }
}
