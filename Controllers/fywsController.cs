using Common;
using Game.Controllers;
using Game.Manager;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Game.Controllers
{
    public class fywsController : Controller
    {
        //
        // GET: /fyws/
        CommonGame cg = new CommonGame();
        GamesManager gm = new GamesManager();
        Games g = new GamesManager().GetGame("fyws");
        ServersMananger sm = new ServersMananger();
        HtmlHelper hh = new HtmlHelper();
        NewsManager nm = new NewsManager();
        GameUserManager gum = new GameUserManager();
        public ActionResult Index()
        {
            return Servers();
        }

        public ActionResult Servers()
        {
            return cg.InitServers("fyws");
        }

        public ActionResult LoginGame(int S)
        {
            return cg.LoginGame("fyws", S);
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
            ViewData["NewsList"] = nm.GetNews(6,2,g.Id); 
            return View();
        }

        public ActionResult WdServers()
        {
            int UserId = BBRequest.GetUserId();
            g = gm.GetGame("fyws");
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
                string ServerHtml = "";
                foreach (GameServer gs in gsList)
                {
                    if(gs.State == 3 || gs.State == 4)
                    {
                        ServerHtml += "<a href=\"client://loadgame|http://www.5577yx.com/" + g.GameNo + "/LoginGame?S=" + gs.QuFu + "\" target=\"_self\">" + gs.Name + "</a>";
                    }
                }
                ViewData["gsHtml"] = ServerHtml;

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
