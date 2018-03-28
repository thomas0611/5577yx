using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Game.Model;
using Game.Manager;
using Common;
using System.Xml;

namespace Game.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/
        GameUserManager gum = new GameUserManager();
        SysMsgManager smm = new SysMsgManager();
        ValiDateCodeManager vdcm = new ValiDateCodeManager();
        NewsManager nm = new NewsManager();
        GamesManager gm = new GamesManager();
        ServersMananger sm = new ServersMananger();
        HtmlHelper hh = new HtmlHelper();
        LockManager alm = new LockManager();

        public ActionResult Index()
        {
            List<News> GGlist = new List<News>();
            GGlist = nm.GetNews(7, 2);
            ViewData["NewsGG"] = hh.IndexNewsHtml(GGlist);

            List<News> HDlist = new List<News>();
            HDlist = nm.GetNews(7, 4);
            ViewData["NewsHD"] = hh.IndexNewsHtml(HDlist);

            string IndexPic = "";
            string[] pic = new string[5];
            string filePath = Utils.GetXmlMapPath("Picpath");
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNode xn = doc.SelectSingleNode("models");
            int i = 0;
            foreach (XmlElement xe in xn.ChildNodes)
            {
                if (xe.NodeType != XmlNodeType.Comment && xe.Name.ToLower() == "model")
                {
                    pic[i] = xe.Attributes["name"].Value + "|" + xe.Attributes["url"].Value + "|" + xe.Attributes["url1"].Value + "|" + xe.Attributes["link"].Value;
                    i++;
                }
            }
            foreach (string item in pic)
            {
                IndexPic += "<li><a href=\"http://" + item.Split('|')[3] + "\" target=\"_blank\"> <img title=\"\" src=\"" + item.Split('|')[1] + "\" width=\"688px\" height=\"294px\"></a></li>";
            }
            ViewData["IndexPic"] = IndexPic;

            List<Games> Hotlist = new List<Games>();
            Hotlist = gm.GetAll(3, "is_hot");
            ViewData["HotGame"] = hh.TjGameHtml(Hotlist);
            //ViewData["HotGame"] = Hotlist;
            List<Games> Tjlist = new List<Games>();
            Tjlist = gm.GetAll(6, "is_red");
            ViewData["TjGame"] = hh.GameHtml2(Tjlist);
            return View();
        }

        public ActionResult Login()
        {
            if (Utils.GetCookie("6qmgamesame") != "")
            {
                string value = Utils.GetCookie("6qmgamesame");
                string UserName = DESEncrypt.DesDecrypt(value.Split('|')[0]);
                string PWD = DESEncrypt.DesDecrypt(value.Split('|')[1]);
                ViewData["UserName"] = UserName;
                ViewData["PWD"] = PWD;
            }
            return View();
        }

        public string DoLogin()
        {
            string UserName = Request["UserName"].Trim();
            string PWD = Request["PWD"].Trim();
            string Code = Request["Code"].Trim();
            if (string.IsNullOrEmpty(UserName))
            {
                return "请输入用户名！";
            }
            if (string.IsNullOrEmpty(PWD))
            {
                return "请输入密码！";
            }
            if (string.IsNullOrEmpty(Code))
            {
                return "请输入验证码！";
            }
            if (Session[Keys.SESSION_CODE] == null)
            {
                return "验证码已过期，请刷新验证码！";
            }
            if (Code.ToLower() != Session[Keys.SESSION_CODE].ToString().ToLower())
            {
                return "您输入的验证码不正确！";
            }
            if (!gum.IsGameUser(UserName))
            {
                return "您输入的用户不存在！";
            }
            GameUser gu = new GameUser();
            gu = gum.GetGameUser(UserName, DESEncrypt.Md5(PWD, 32));
            if (gu == null)
            {
                return "您输入的用户或密码有误！";
            }
            if (gu.IsLock == 1)
            {
                return "您输入的用户或密码有误！";
            }
            if (alm.IsLock(BBRequest.GetIP()))
            {
                return "您输入的用户或密码有误！";
            }
            Session[Keys.SESSION_USER] = gu.Id;
            Session.Timeout = 120;
            gum.UpdateLastLogin(gu.Id);
            if (Request["ck1"] == "on")
            {
                Utils.WriteCookie("6qmgamesame", DESEncrypt.DesEncrypt(UserName) + "|" + DESEncrypt.DesEncrypt(PWD), 1440);
            }
            else
            {
                Utils.WriteCookie("6qmgamesame", "", -10);
            }
            return "True";
        }

        public string KsLogin()
        {
            string UserName = Request["UserName"].Trim();
            string PWD = Request["PWD"].Trim();
            if (string.IsNullOrEmpty(UserName))
            {
                return "请输入用户名！";
            }
            if (string.IsNullOrEmpty(PWD))
            {
                return "请输入密码！";
            }
            if (!gum.IsGameUser(UserName))
            {
                return "您输入的用户不存在！";
            }
            GameUser gu = new GameUser();
            gu = gum.GetGameUser(UserName, DESEncrypt.Md5(PWD, 32));
            if (gu == null)
            {
                return "您输入的用户或密码有误！";
            }
            if (gu.IsLock == 1)
            {
                return "您输入的用户或密码有误！";
            }
            if (alm.IsLock(BBRequest.GetIP()))
            {
                return "您输入的用户或密码有误！";
            }
            Session[Keys.SESSION_USER] = gu.Id;
            Session.Timeout = 120;
            gum.UpdateLastLogin(gu.Id);
            if (Request["ck1"] == "on")
            {
                Utils.WriteCookie("6qmgamesame", DESEncrypt.DesEncrypt(UserName) + "|" + DESEncrypt.DesEncrypt(PWD), 1440);
            }
            else
            {
                Utils.WriteCookie("6qmgamesame", "", -10);
            }
            return "True";
        }

        public ActionResult Reg()
        {
            return View();
        }

        public string DoReg()
        {
            GameUser gu = new GameUser();
            string UserName = Request["UserName"].Trim();
            string Pwd = Request["PWD"].Trim();
            string Card = Request["Card"].Trim();
            string Email = Request["Email"].Trim();
            string Code = Request["Code"].Trim();
            if (!DevRegHel.RegName(UserName))
            {
                return "您输入的用户名不可用！";
            }
            if (!DevRegHel.RegPwd(Pwd))
            {
                return "您输入的密码不可用！";
            }
            if (!DevRegHel.RegCard(Card))
            {
                return "您输入的身份证不可用！";
            }
            if (!DevRegHel.RegEmail(Email))
            {
                return "您输入的邮箱不可用！";
            }
            if (gum.IsGameUser(UserName))
            {
                return "您输入的用户名已被注册！";
            }
            if (Session[Keys.SESSION_CODE] == null)
            {
                return "验证码已过期，请刷新验证码！";
            }
            if (Session[Keys.SESSION_CODE].ToString().ToUpper() != Code.ToUpper())
            {
                return "您输入的验证码不正确！";
            }
            if (Request["Ck"].Trim() != "on")
            {
                return "请务必确认您已经阅读服务条款！";
            }
            if (alm.IsLock(BBRequest.GetIP()))
            {
                return "您暂时不能注册！";
            }
            if (gum.ExitEmail(Request["Email"]))
            {
                return "您的邮箱已经注册过！";
            }
            gu = new GameUser(0, Request["UserName"], DESEncrypt.Md5(Request["PWD"], 32), "", Request["Sex"], "", "", Request["RealName"], Request["Email"]
                , "", Request["Birthday"], Request["Card"], "1", 0, "", 0, 0, 0, 0, 0, 0, DateTime.Now, DateTime.Now, 0, 0, 0, 0, BBRequest.GetIP(),
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
                    Session[Keys.SESSION_USER] = Id;
                    Session.Timeout = 120;
                    validatecode vdc = new validatecode();
                    vdcm.DelValiDateCode(Id, 1);
                    vdc.type = 1;
                    vdc.userid = Id;
                    vdc.sendtime = DateTime.Now;
                    vdc.code = Guid.NewGuid().ToString() + DateTime.Now.Minute + DateTime.Now.Millisecond;
                    vdc.email = gu.Email.Trim();
                    vdcm.AddValiDateCode(vdc);
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

        public Boolean IsGameUser()
        {
            return gum.IsGameUser(Request["UserName"]);
        }

        public ActionResult Left()
        {
            int UserId = BBRequest.GetUserId();
            if (UserId > 0)
            {
                GameUser gu = gum.GetGameUser(UserId);
                ViewData["UserNameed"] = gu.UserName;
                ViewData["Money"] = gu.Money;
                ViewData["Photo"] = gu.Photo + ".jpg";
                List<OnlineLog> Ollist = new List<OnlineLog>();
                Ollist = new OnlineLogManager().GetOnlineLog(gu.Id, 2);
                string HtmlStr = "";
                foreach (OnlineLog ol in Ollist)
                {
                    Games g = gm.GetGame(ol.GameId);
                    GameServer gs = sm.GetGameServer(ol.ServerId);
                    HtmlStr += " <li ><a href=\"/" + g.GameNo + "/LoginGame?S=" + gs.QuFu + "\" target=\"_blank\" class=\"dub\"><span>" + sm.GetGameServer(ol.ServerId).Name + "</span><span class=\"g\">go</span></a>" + "<a class=\"n\" href=\"#\" target=\"_blank\">" + gm.GetGame(ol.GameId).Name + "</a></li>";
                }
                ViewData["OnlineLogHtml"] = HtmlStr;
                ViewData["LoginStyle"] = "style=\"display: none;\"";
            }
            else
            {
                if (Utils.GetCookie("6qmgamesame") != "")
                {
                    string value = Utils.GetCookie("6qmgamesame");
                    string UserName = DESEncrypt.DesDecrypt(value.Split('|')[0]);
                    string PWD = DESEncrypt.DesDecrypt(value.Split('|')[1]);
                    ViewData["UserName"] = UserName;
                    ViewData["PWD"] = PWD;
                }
                ViewData["LoginedStyle"] = "style=\"display: none;\"";
            }
            List<GameServer> list = new List<GameServer>();
            list = sm.GetNewsServer(6);
            ViewData["NewServerHtml"] = hh.GetNewServerHtml(list);
            return PartialView();
        }

        public ActionResult Top()
        {
            int UserId = BBRequest.GetUserId();
            if (UserId > 0)
            {
                GameUser gu = gum.GetGameUser(UserId);
                ViewData["HeadInfo"] = "<span style=\"color: sandybrown; font-weight: bold\">" + gu.UserName + "</span>";
            }
            else
            {
                ViewData["HeadInfo"] = "体验更优的服务请您先<a href=\"/Home/Login\">登录</a>或<a href=\"/Home/Reg\">注册</a>";
            }
            string Controller = Request.RequestContext.RouteData.Values["Controller"].ToString();
            switch (Controller)
            {
                case "Home":
                    ViewData["HomeOn"] = "active";
                    break;
                case "GameCenter":
                    ViewData["GameCenterOn"] = "active";
                    break;
                case "UserCenter":
                    ViewData["UserCenterOn"] = "active";
                    break;
                case "PayCenter":
                    ViewData["PayCenterOn"] = "active";
                    break;
                case "GameGift":
                    ViewData["GameGiftOn"] = "active";
                    break;
                case "NewsCenter":
                    ViewData["NewsCenterOn"] = "active";
                    break;
                default:
                    break;
            }
            return View();
        }

        public ActionResult Footer()
        {
            ViewData["YqLink"] = hh.YqLinkHtml("12");
            return View();
        }
    }
}
