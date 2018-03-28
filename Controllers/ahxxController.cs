using Common;
using Game.Controllers;
using Game.Manager;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _5577yx.Views
{
    public class ahxxController : Controller
    {
        //
        // GET: /ahxx/
        CommonGame cg = new CommonGame();
        GamesManager gm = new GamesManager();
        Games g = new GamesManager().GetGame("ahxx");
        ServersMananger sm = new ServersMananger();
        Game.Controllers.HtmlHelper hh = new Game.Controllers.HtmlHelper();
        NewsManager nm = new NewsManager();
        GameUserManager gum = new GameUserManager();

        public ActionResult Index()
        {
            return Servers();
        }

        public ActionResult Servers()
        {
            return cg.InitServers("ahxx");
        }

        public ActionResult LoginGame(int S)
        {
            return cg.LoginGame("ahxx", S);
        }

        public ActionResult Gift(int G)
        {
            return cg.GameGift(G);
        }

        public string DoGetGift(int G)
        {
            return cg.DoGetGift(G, null);
        }

        public ActionResult Wd()
        {
            ViewData["NewsList"] = nm.GetNews(5,2,g.Id); 
            return View();
        }

        public ActionResult WdServers()
        {
            int UserId = BBRequest.GetUserId();
            g = gm.GetGame("ahxx");
            if (UserId > 0)
            {
                GameUser gu = new GameUser();
                gu = gum.GetGameUser(UserId);
                ViewData["UserName"] = gu.UserName;
                ViewData["TjqfHref"] = "#";
                ViewData["TjqfName"] = "暂无";
                ViewData["LLHref"] = "#";
                ViewData["LLName"] = "最近没有玩游戏哦";
                OnlineLog ol = new OnlineLog();
                ol = new OnlineLogManager().GetLastLogin(UserId, g.Id);
                if (ol != null)
                {
                    GameServer Llqf = sm.GetGameServer(ol.ServerId);
                    ViewData["LLHref"] = "client://loadgame|http://www.5577yx.com/" + g.GameNo + "/LoginGame?S=" + Llqf.QuFu;
                    ViewData["LLName"] = Llqf.Name;
                }
                if (g.tjqf > 0)
                {
                    GameServer tjqf = sm.GetGameServer(g.tjqf);
                    ViewData["TjqfHref"] = "client://loadgame|http://www.5577yx.com/" + g.GameNo + "/LoginGame?S=" + tjqf.QuFu;
                    ViewData["TjqfName"] = tjqf.Name;
                }
                List<GameServer> gsList = new List<GameServer>();
                gsList = sm.GetServersByGame(g.Id);
                //string ServerHtml = "";
                List<GameServer> serverList = new List<GameServer>();
                foreach (GameServer gs in gsList)
                {
                    if(gs.State == 3 || gs.State == 4)
                    {
                        serverList.Add(gs);
                        //ServerHtml += "<a href=\"client://loadgame|http://www.5577yx.com/" + g.GameNo + "/LoginGame?S=" + gs.QuFu + "\" target=\"_self\">" + gs.Name + "</a>";
                    }
                }
                //ViewData["gsHtml"] = ServerHtml;
                ViewData["serverList"] = serverList;
                Utils.WriteCookie2("miniloader", "1", "5577yx.com");

                return View();
            }
            else
            {
                return RedirectToAction("Wd");
            }

        }
    }
}
