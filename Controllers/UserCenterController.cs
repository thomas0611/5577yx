
using Common;
using Game.Manager;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace Game.Controllers
{
    public class UserCenterController : Controller
    {
        //
        // GET: /UserCenter/
        GameUserManager gum = new GameUserManager();
        OnlineLogManager olm = new OnlineLogManager();
        GamesManager gm = new GamesManager();
        ServersMananger sm = new ServersMananger();
        SysMsgManager smm = new SysMsgManager();
        ValiDateCodeManager vdcm = new ValiDateCodeManager();
        HtmlHelper hh = new HtmlHelper();

        public ActionResult Index()
        {
            ViewData["MyInfoOn"] = "ch_son_p_cur";
            ViewData["UserInfoON"] = "ch_id_cur";
            ViewData["menu1On"] = "pay_menu_cur1";
            ViewData["menu2On"] = "reddot";
            ViewData["menu3On"] = "reddot";
            int UserId = BBRequest.GetUserId();
            if (UserId > 0)
            {
                GameUser gu = gum.GetGameUser(UserId);
                ViewData["Photo"] = gu.Photo;
                ViewData["UserName"] = gu.UserName;
                ViewData["Money"] = gu.Money;
                ViewData["FlMoney"] = gu.RebateMoney;
                ViewData["vip"] = gu.GradeId > 0 ? "vip1_12.jpg" : "vip11_0.jpg";
                if (gu.IsSpreader <= 0)
                {
                    ViewData["IsSpread"] = "display:none";
                }
                List<GameServer> NewServerlist = new List<GameServer>();
                NewServerlist = sm.GetNewsServer(6);
                ViewData["NewServerHtml"] = hh.GetNewServerHtml(NewServerlist);
                ViewData["MsgCount"] = smm.GetSysMsgCount(gu.Id);
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
            string LoginOnline = "";
            List<OnlineLog> list = new List<OnlineLog>();
            list = olm.GetOnlineLog(UserId, 4);
            foreach (OnlineLog ol in list)
            {
                Games g = new Games();
                g = gm.GetGame(ol.GameId);
                GameServer gs = new GameServer();
                gs = sm.GetGameServer(ol.ServerId);
                if (gs != null)
                {
                    LoginOnline += "<a href=\"/" + g.GameNo + "/LoginGame?S=" + gs.QuFu + "\" target=\"_blank\"><li><img src=\"" + gs.Img + "\"><br><span class=\"span_lanse\">" + g.Name + "</span>&nbsp;" + gs.Name + "</li></a>";
                }
            }
            ViewData["LoginOnline"] = LoginOnline;
            return View();
        }

        public ActionResult MyInfoEdit()
        {
            ViewData["MyInfoEditOn"] = "ch_son_p_cur";
            ViewData["UserInfoON"] = "ch_id_cur";
            ViewData["menu1On"] = "pay_menu_cur1";
            ViewData["menu2On"] = "reddot";
            ViewData["menu3On"] = "reddot";
            int UserId = BBRequest.GetUserId();
            if (UserId > 0)
            {
                GameUser gu = new GameUser();
                gu = gum.GetGameUser(UserId);
                ViewData["Photo"] = gu.Photo;
                ViewData["UserName"] = gu.UserName;
                ViewData["Sex"] = gu.Sex == "0" ? "男" : "女";
                ViewData["RealName"] = gu.RealName;
                ViewData["BirthDay"] = gu.BirthDay;
                if (!string.IsNullOrEmpty(gu.Email))
                {
                    string Email = gu.Email.Substring(0, 4);
                    int Length = gu.Email.LastIndexOf('@') - 4;
                    for (int i = 0; i < Length; i++)
                    {
                        Email += "*";
                    }
                    Email += gu.Email.Substring(gu.Email.LastIndexOf('@'), gu.Email.Length - gu.Email.LastIndexOf('@'));
                    ViewData["Email"] = Email;
                }
                if (gu.IsSpreader <= 0)
                {
                    ViewData["IsSpread"] = "display:none";
                }
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
            return View();
        }

        public Boolean DoInfoEdit()
        {
            int UserId = BBRequest.GetUserId();
            if (UserId < 1)
            {
                RedirectToAction("Login", "Home");
            }
            GameUser gu = new GameUser();
            gu = gum.GetGameUser(UserId);
            gu.Photo = string.IsNullOrEmpty(Request["Photo"]) ? gu.Photo : Request["Photo"];
            gu.RealName = string.IsNullOrEmpty(Request["RealName"]) ? gu.RealName : Request["RealName"];
            gu.Cards = string.IsNullOrEmpty(Request["Card"]) ? gu.Cards : Request["Card"];
            gu.BirthDay = string.IsNullOrEmpty(Request["BirthDay"]) ? gu.BirthDay : Request["BirthDay"];
            gu.Sex = string.IsNullOrEmpty(Request["Sex"]) ? gu.Sex : Request["Sex"];
            gu.Email = string.IsNullOrEmpty(Request["Email"]) ? gu.Email : Request["Email"];
            gu.PWD = string.IsNullOrEmpty(Request["PWD"]) ? gu.PWD : DESEncrypt.Md5(Request["PWD"], 32);
            return gum.UpdateUser(gu);
        }

        public ActionResult UserSafe()
        {
            ViewData["UserSafeOn"] = "ch_son_p_cur";
            ViewData["UserInfo2On"] = "ch_id_cur";
            ViewData["menu1On"] = "reddot";
            ViewData["menu2On"] = "pay_menu_cur1";
            ViewData["menu3On"] = "reddot";
            int UserId = BBRequest.GetUserId();
            if (UserId < 1)
            {
                return RedirectToAction("Login", "Home");
            }
            GameUser gu = new GameUser();
            gu = gum.GetGameUser(UserId);
            if (string.IsNullOrEmpty(gu.Email) && string.IsNullOrEmpty(gu.Cards))
            {
                ViewData["Safe"] = "<span class=\"weak\">弱</span>";
                ViewData["SafeInfo"] = "请您务必及时绑定邮箱或身份证，以确保您账户的安全！";
            }
            else if (string.IsNullOrEmpty(gu.Email) || string.IsNullOrEmpty(gu.Cards))
            {
                ViewData["Safe"] = "<span class=\"mid\">适中</span>";
                ViewData["SafeInfo"] = "请您及时绑定邮箱和身份证，以完善您账户的安全！";
            }
            else if (!string.IsNullOrEmpty(gu.Email) && !string.IsNullOrEmpty(gu.Cards))
            {
                ViewData["Safe"] = "<span class=\"good\">强</span>";
                ViewData["SafeInfo"] = "您的账户很安全，请继续保持！";
            }
            if (string.IsNullOrEmpty(gu.Email))
            {
                ViewData["EamilStyle"] = "no";
                ViewData["EmailText"] = "您还暂未绑定邮箱！";
            }
            else
            {
                ViewData["EamilStyle"] = "yes";
                ViewData["EmailText"] = "您已经绑定邮箱！";
            }
            if (string.IsNullOrEmpty(gu.Cards))
            {
                ViewData["CardsStyle"] = "no";
                ViewData["CardsText"] = "您还暂未绑定邮箱！";
            }
            else
            {
                ViewData["CardsStyle"] = "yes";
                ViewData["CardsText"] = "您已经绑定邮箱！";
            }
            if (gu.IsSpreader <= 0)
            {
                ViewData["IsSpread"] = "display:none";
            }
            return View();
        }

        public ActionResult UserMsg()
        {
            ViewData["UserMsgOn"] = "ch_son_p_cur";
            ViewData["UserInfo2On"] = "ch_id_cur";
            ViewData["menu1On"] = "reddot";
            ViewData["menu2On"] = "pay_menu_cur1";
            ViewData["menu3On"] = "reddot";
            int UserId = BBRequest.GetUserId();
            if (UserId < 1)
            {
                return RedirectToAction("Login", "Home");
            }
            GameUser gu = gum.GetGameUser(UserId);
            if (gu.IsSpreader <= 0)
            {
                ViewData["IsSpread"] = "display:none";
            }
            return View();
        }

        public Boolean DelUserMsg(int M)
        {
            int UserId = BBRequest.GetUserId();
            if (UserId < 1)
            {
                return false;
            }
            else
            {
                return smm.DelSysMsg(M);
            }
        }

        public ActionResult UserCard()
        {
            ViewData["UserCardOn"] = "ch_son_p_cur";
            ViewData["UserInfo2On"] = "ch_id_cur";
            ViewData["menu1On"] = "reddot";
            ViewData["menu2On"] = "pay_menu_cur1";
            ViewData["menu3On"] = "reddot";
            GameUser gu = new GameUser();
            int UserId = BBRequest.GetUserId();
            if (UserId < 1)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                gu = gum.GetGameUser(UserId);
                if (gu.IsSpreader <= 0)
                {
                    ViewData["IsSpread"] = "display:none";
                }
            }
            if (string.IsNullOrEmpty(gu.Cards))
            {
                ViewData["CardStyle"] = "no";
                ViewData["ShowStyle1"] = "display:none";
                ViewData["ShowStyle2"] = "";
            }
            else
            {
                ViewData["CardStyle"] = "yes";
                ViewData["ShowStyle1"] = "";
                ViewData["ShowStyle2"] = "display:none";
                string Cards = gu.Cards.Substring(0, 6);
                if (gu.Cards.Length == 15)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        Cards += "*";
                    }
                    Cards += gu.Cards.Substring(12, 3);
                }
                else if (gu.Cards.Length == 18)
                {
                    for (int i = 0; i < 8; i++)
                    {
                        Cards += "*";
                    }
                    Cards += gu.Cards.Substring(14, 4);
                }
                ViewData["Card"] = Cards;
                string RealName = gu.RealName.Substring(0, 1);
                for (int i = 0; i < gu.RealName.Length - 1; i++)
                {
                    RealName += "*";
                }
                ViewData["RealName"] = RealName;
            }
            return View();
        }

        public ActionResult UserEmail()
        {
            ViewData["UserEmailOn"] = "ch_son_p_cur";
            ViewData["UserInfo2On"] = "ch_id_cur";
            ViewData["menu1On"] = "reddot";
            ViewData["menu2On"] = "pay_menu_cur1";
            ViewData["menu3On"] = "reddot";
            GameUser gu = new GameUser();
            int UserId = BBRequest.GetUserId();
            if (UserId < 1)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                gu = gum.GetGameUser(UserId);
                if (gu.IsSpreader <= 0)
                {
                    ViewData["IsSpread"] = "display:none";
                }
            }
            if (string.IsNullOrEmpty(gu.Email))
            {
                ViewData["EmailStyle"] = "no";
                ViewData["ShowStyle1"] = "display:none";
                ViewData["ShowStyle2"] = "";
            }
            else
            {
                ViewData["EmailStyle"] = "yes";
                ViewData["ShowStyle1"] = "";
                ViewData["ShowStyle2"] = "display:none";
                string Email = gu.Email.Substring(0, 4);
                int Length = gu.Email.LastIndexOf('@') - 4;
                for (int i = 0; i < Length; i++)
                {
                    Email += "*";
                }
                Email += gu.Email.Substring(gu.Email.LastIndexOf('@'), gu.Email.Length - gu.Email.LastIndexOf('@'));
                ViewData["Email"] = Email;
            }
            return View();
        }

        public ActionResult UserMiBao()
        {
            ViewData["UserMiBaoOn"] = "ch_son_p_cur";
            ViewData["UserInfo2On"] = "ch_id_cur";
            ViewData["menu1On"] = "reddot";
            ViewData["menu2On"] = "pay_menu_cur1";
            ViewData["menu3On"] = "reddot";
            GameUser gu = new GameUser();
            int UserId = BBRequest.GetUserId();
            if (UserId < 1)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                gu = gum.GetGameUser(UserId);
                if (gu.IsSpreader <= 0)
                {
                    ViewData["IsSpread"] = "display:none";
                }
            }
            return View();
        }

        public ActionResult UpdatePwd()
        {
            ViewData["UpdatePwdOn"] = "ch_son_p_cur";
            ViewData["UserInfo2On"] = "ch_id_cur";
            ViewData["menu1On"] = "reddot";
            ViewData["menu2On"] = "pay_menu_cur1";
            ViewData["menu3On"] = "reddot";

            GameUser gu = new GameUser();
            int UserId = BBRequest.GetUserId();
            if (UserId < 1)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                gu = gum.GetGameUser(UserId);
                if (gu.IsSpreader <= 0)
                {
                    ViewData["IsSpread"] = "display:none";
                }
            }

            ViewData["UserName"] = gu.UserName;
            ViewData["Mima"] = gu.PWD;
            return View();
        }

        public ActionResult LoginOut()
        {
            Session.Remove(Keys.SESSION_USER);
            return RedirectToAction("Index", "Home");
        }

        public ActionResult PayHistory()
        {
            ViewData["PayHistoryOn"] = "ch_son_p_cur";
            ViewData["UserInfo3On"] = "ch_id_cur";
            ViewData["menu1On"] = "reddot";
            ViewData["menu2On"] = "reddot";
            ViewData["menu3On"] = "pay_menu_cur1";

            GameUser gu = new GameUser();
            int UserId = BBRequest.GetUserId();
            if (UserId < 1)
            {
                return RedirectToAction("Login", "Home");
            }
            else
            {
                gu = gum.GetGameUser(UserId);
                if (gu.IsSpreader <= 0)
                {
                    ViewData["IsSpread"] = "display:none";
                }
            }

            ViewData["Money"] = gu.Money;
            return View();
        }

        public ActionResult FindPwd()
        {
            return View();
        }

        public string DoFindPwd()
        {
            string UserName = Request["UserName"];
            string Code = Request["Code"];
            try
            {
                if (string.IsNullOrEmpty(UserName.Trim()))
                {
                    return "请输入用户名|Error";
                }
                if (!gum.IsGameUser(UserName))
                {
                    return "您填写的用户名不存在！|Error";
                }
                if (string.IsNullOrEmpty(Code.Trim()))
                {
                    return "请输入验证码！|Error";
                }
                if (Session[Keys.SESSION_CODE] == null)
                {
                    return "验证码已过期，请重新获取验证码！|Error";
                }
                if (Code.ToLower() != Session[Keys.SESSION_CODE].ToString().ToLower())
                {
                    return "验证码错误！|Error";
                }

                GameUser gu = new GameUser();
                gu = gum.GetGameUser(UserName);
                int userid = gu.Id;
                DateTime dt1 = DateTime.Now.AddHours(-47);
                validatecode vcode = new validatecode();
                if (string.IsNullOrEmpty(gu.Email))
                {
                    return "您还未绑定邮箱！请联系客服！|Error";
                }
                if (vdcm.ExitValiDateCode(userid, 2, dt1))
                {
                    vcode = vdcm.GetValiDateCode(userid, 2);
                    string ucode = DESEncrypt.encryptstring1(vcode.userid.ToString());
                    string tcode = DESEncrypt.encryptstring1(vcode.type.ToString());
                    string scode = vcode.code.ToString();
                    string vicode = vcode.sendtime.ToString("yyyy-MM-ddHH:mm:ss");
                    string email = ConfigurationManager.AppSettings["email"].ToString();
                    string pwd = ConfigurationManager.AppSettings["password"].ToString();
                    string emailserver = ConfigurationManager.AppSettings["emailserver"].ToString();
                    string emailuser = ConfigurationManager.AppSettings["emailuser"].ToString();
                    string content_email = "Email 密码找回<br/><br/>" +
                                       "这封信是由 『5577游戏平台』 发送的。<br/><br/>" +

                                       "您收到这封邮件，是由于在 『5577游戏平台』 进行了密码找回操作。如果您并没有访问过 『乐扣游戏平台』，或没有进行上述操作，请忽略这封邮件。您不需要退订或进行其他进一步的操作。<br/><br/>" +


                                       "----------------------------------------------------------------------<br/>" +
                                       "密码找回说明<br/>" +
                                       "----------------------------------------------------------------------<br/><br/>" +

                                       "如果您是 『5577游戏平台』 的新用户，或在找回密码时使用了本地址，我们需 要对您的地址有效性进行验证以避免垃圾邮件或地址被滥用。<br/><br/>" +

                                       "您只需点击下面的链接即可修改您的帐号密码：<br/>" +
                                       "<a href=\"http://www.5577yx.com/UserCenter/ValiDateCode?ucode=" + ucode + "&tcode=" + tcode + "&scode=" + scode + "&vicode=" + vicode + "\">http://www.5577yx.com/UserCenter/ValiDateCodeucode=" + ucode + "&tcode=" + tcode + "&scode=" + scode + "&vicode=" + vicode + "</a><br/>" +
                                       "(如果上面不是链接形式，请将该地址手工粘贴到浏览器地址栏再访问)<br/><br/>" +

                                       "感谢您的访问，祝您使用愉快！<br/><br/>" +

                                       "此致<br/>" +
                                       "『5577游戏平台』 管理团队.<br/>";

                    new SendHelper().SendEmail(vcode.email, "5577yx邮箱密码找回", content_email);
                }
                else
                {
                    vdcm.DelValiDateCode(userid, 2);
                    vcode.type = 2;
                    vcode.userid = userid;
                    vcode.sendtime = DateTime.Now;
                    vcode.code = Guid.NewGuid().ToString() + DateTime.Now.Minute + DateTime.Now.Millisecond;
                    vcode.email = gu.Email;
                    vdcm.AddValiDateCode(vcode);
                    string ucode = DESEncrypt.encryptstring1(userid.ToString());
                    string tcode = DESEncrypt.encryptstring1("2");
                    string scode = vcode.code.ToString();
                    string vicode = vcode.sendtime.ToString("yyyy-MM-ddHH:mm:ss");
                    string email = ConfigurationManager.AppSettings["email"].ToString();
                    string pwd = ConfigurationManager.AppSettings["password"].ToString();
                    string emailserver = ConfigurationManager.AppSettings["emailserver"].ToString();
                    string emailuser = ConfigurationManager.AppSettings["emailuser"].ToString();
                    string content_email = "Email 密码找回<br/><br/>" +
                                       "这封信是由 『5577游戏平台』 发送的。<br/><br/>" +

                                       "您收到这封邮件，是由于在 『5577游戏平台』 进行了密码找回操作。如果您并没有访问过 『乐扣游戏平台』，或没有进行上述操作，请忽略这封邮件。您不需要退订或进行其他进一步的操作。<br/><br/>" +


                                       "----------------------------------------------------------------------<br/>" +
                                       "密码找回说明<br/>" +
                                       "----------------------------------------------------------------------<br/><br/>" +

                                       "如果您是 『5577游戏平台』 的新用户，或在找回密码时使用了本地址，我们需 要对您的地址有效性进行验证以避免垃圾邮件或地址被滥用。<br/><br/>" +

                                       "您只需点击下面的链接即可修改您的帐号密码：<br/>" +
                                       "<a href=\"http://www.5577yx.com/UserCenter/ValiDateCode?ucode=" + ucode + "&tcode=" + tcode + "&scode=" + scode + "&vicode=" + vicode + "\">http://www.5577yx.com/UserCenter/ValiDateCodeucode=" + ucode + "&tcode=" + tcode + "&scode=" + scode + "&vicode=" + vicode + "</a><br/>" +
                                       "(如果上面不是链接形式，请将该地址手工粘贴到浏览器地址栏再访问)<br/><br/>" +

                                       "感谢您的访问，祝您使用愉快！<br/><br/>" +

                                       "此致<br/>" +
                                       "『5577游戏平台』 管理团队.<br/>";

                    new SendHelper().SendEmail(vcode.email, "5577yx邮箱密码找回", content_email);
                }
            }
            catch (Exception ex)
            {
                return ex.Message + "|Error";
            }
            return "已成功发送邮件，请前往邮箱找回密码！|Success";
        }

        public ActionResult ValiDateCode()
        {
            string ucode = BBRequest.GetQueryString("ucode");
            string tcode = BBRequest.GetQueryString("tcode");
            string scode = BBRequest.GetQueryString("scode");
            string vicode = BBRequest.GetQueryString("vicode");
            ViewData["MsgStyle"] = "";
            ViewData["UpdatePwdStyle"] = "";
            if (string.IsNullOrEmpty(ucode) || string.IsNullOrEmpty(tcode) || string.IsNullOrEmpty(scode) || string.IsNullOrEmpty(vicode))
            {
                ViewData["Msg"] = "验证失败";
                ViewData["MsgText"] = "您的验证信息错误或已过期,请登录用户中心重新发送！";
                ViewData["UpdatePwdStyle"] = "display: none;";
                return View();
            }
            int userid = int.Parse(DESEncrypt.decryptstring1(ucode));
            int type = int.Parse(DESEncrypt.decryptstring1(tcode));
            vicode = vicode.Substring(0, 10) + " " + vicode.Substring(10, 8);
            DateTime dt1 = DateTime.Parse(vicode);
            if (!vdcm.ExitValiDateCode(userid, type, dt1.AddHours(-47)))
            {
                ViewData["Msg"] = "验证失败";
                ViewData["MsgText"] = "您的验证信息错误或已过期,请登录用户中心重新发送！";
                ViewData["UpdatePwdStyle"] = "display: none;";
                return View();
            }
            validatecode vcode = vdcm.GetValiDateCode(userid, type);
            if (vcode.code != scode)
            {
                ViewData["Msg"] = "验证失败";
                ViewData["MsgText"] = "您的验证信息错误或已过期,请登录用户中心重新发送！";
                ViewData["UpdatePwdStyle"] = "display: none;";
                return View();
            }
            ViewData["MsgStyle"] = "display: none;";
            Session[Keys.SESSION_USER] = userid;
            Session["Type"] = type;
            Session.Timeout = 20;
            return View();
        }

        public string DoUpadatePwd()
        {
            string Pwd = Request["PWD"];
            string APwd = Request["APwd"];
            if (string.IsNullOrEmpty(Pwd.Trim()))
            {
                return "请输入密码!";
            }
            if (!DevRegHel.RegPwd(Pwd.Trim()))
            {
                return "密码输入错误!";
            }
            if (Pwd.Trim() != APwd.Trim())
            {
                return "两次输入的密码不一致!";
            }
            int UserId = BBRequest.GetUserId();
            int Type = (int)Session["Type"];
            if (UserId > 0)
            {
                GameUser gu = gum.GetGameUser(UserId);
                gu.PWD = DESEncrypt.Md5(Pwd, 32);
                gum.UpdateUser(gu);
                vdcm.DelValiDateCode(UserId, Type);
            }
            else
            {
                return "您的验证已过期！";
            }
            return null;
        }
    }
}
