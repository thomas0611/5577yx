using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Game.Manager;
using Game.Model;

namespace Game.Controllers
{
    public class TgController : Controller
    {
        //
        // GET: /Tg/
        GameUserManager gum = new GameUserManager();
        SysMsgManager smm = new SysMsgManager();
        ValiDateCodeManager vdcm = new ValiDateCodeManager();
        GamesManager gm = new GamesManager();
        LockManager alm = new LockManager();
        
        public ActionResult Index()
        {
            try
            {
                string Action = Request["Action"];
                string annalID = Request["annalID"];
                if (string.IsNullOrEmpty(Action))
                {
                    return Redirect("about:blank");
                }
                else if (alm.IsLock(Action))
                {
                    return Redirect("about:blank");
                }
                else if (alm.IsLock(BBRequest.GetIP()))
                {
                    return Redirect("about:blank");
                }
                else
                {
                    int s = 0;
                    string[] a = DESEncrypt.Decrypt(Action).Split('|');
                    string b = a[0];
                    string c = a[1];
                    if (int.TryParse(a[0], out s) && int.TryParse(a[1], out s))
                    {
                        GameUser gu = new GameUser();
                        gu = gum.GetGameUser(int.Parse(a[0]));
                        if (gu != null)
                        {
                            if (gu.IsSpreader > 0)
                            {
                                ViewData["Action"] = Action;
                            }
                            else
                            {
                                return Redirect("about:blank");
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(annalID))
                {
                    Session["annalID"] = annalID;
                }
            }
            catch (Exception)
            {
                return Redirect("about:blank");
            }
            return View("~/Views/SpreadCenter/Tg.cshtml");
        }

        public string DoTg()
        {
            GameUser gu = new GameUser();
            string UserName = Request["UserName"].Trim();
            string Pwd = Request["PWD"].Trim();
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
            try
            {
                string Action = Request["Action"];
                int Source = 0;
                int RegGame = 0;
                Games g = new Games();
                if (string.IsNullOrEmpty(Action))
                {
                    return "缺少参数！";
                }
                else if (alm.IsLock(Action) || alm.IsLock(BBRequest.GetIP()))
                {
                    return "参数错误！";
                }
                else
                {
                    int s = 0;
                    string[] a = DESEncrypt.Decrypt(Action).Split('|');
                    string b = a[0];
                    string c = a[1];
                    if (int.TryParse(a[0], out s) && int.TryParse(a[1], out s))
                    {
                        gu = gum.GetGameUser(int.Parse(a[0]));
                        if (gu != null)
                        {
                            if (gu.IsSpreader > 0)
                            {
                                Source = int.Parse(a[0]);
                                RegGame = int.Parse(a[1]);
                                g = gm.GetGame(RegGame);
                                if (!(g.tjqf > 0))
                                {
                                    return "游戏还未设置推荐服务器！";
                                }
                            }
                            else
                            {
                                return "参数错误！";
                            }
                        }
                    }
                }
                gu = new GameUser(0, Request["UserName"], DESEncrypt.Md5(Request["PWD"], 32), "", "0", "", "", "", ""
                  , "", "", "", "1", Source, "", 0, 0, 0, 0, 0, 0, DateTime.Now, DateTime.Now, 0, 0, 0, 0, BBRequest.GetIP(),
                  "", RegGame, "", "");
                if (Session["annalID"] != null)
                {
                    string annalID = Session["annalID"].ToString().Trim();
                    if (!string.IsNullOrEmpty(annalID))
                    {
                        gu.annalID = annalID;
                        gu.From_Url = "BengBeng";
                        string SelUrl = "http://www.bengbeng.com/reannal.php?adID=3278&annalID=" + annalID + "&idCode=" + gu.UserName + "&doukey=" + DESEncrypt.Md5("3278" + annalID + gu.UserName + "06bd24c6124b2dd7", 32);
                        string SleRes = Utils.GetWebPageContent(SelUrl);
                        Dictionary<string, string> Jd = Common.Json.JsonToArray(SleRes);
                        if (Jd["result"] != "1")
                        {
                            return Jd["message"];
                        }
                    }
                    else
                    {
                        return "缺少参数！";
                    }
                }
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
                    GameServer gs = new ServersMananger().GetGameServer(g.tjqf);
                    return g.GameNo + "|" + gs.QuFu;
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
    }
}
