using Game.Manager;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Common;
using System.Web.SessionState;
using Game.Controllers;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System.Text;

namespace _5577yx.Models
{
    /// <summary>
    /// AdminFunction 的摘要说明
    /// </summary>
    public class AdminFunction : IHttpHandler, IRequiresSessionState
    {
        GamesManager gm = new GamesManager();
        ServersMananger sm = new ServersMananger();
        NewsManager nm = new NewsManager();
        CardManager cm = new CardManager();
        OrderManager om = new OrderManager();
        MasterManager mm = new MasterManager();
        SourceChangeManager scm = new SourceChangeManager();
        GameUserManager gum = new GameUserManager();
        LinksManager lm = new LinksManager();
        RoleCompetenceManager rcm = new RoleCompetenceManager();
        SysMsgManager smm = new SysMsgManager();
        HtmlHelper hh = new HtmlHelper();
        LockManager Lkm = new LockManager();

        public void ProcessRequest(HttpContext context)
        {
            string method = context.Request["method"];
            context.Response.ContentType = "text/html";
            switch (method)
            {
                case "GetGames":
                    GetGames(context);
                    break;
                case "GetGamesIdName":
                    GetGamesIdName(context );
                    break;
                case "GetServers":
                    GetServers(context);
                    break;
                case "GetServersIdName":
                    GetServerIdName(context);
                    break;
                case "GetNewsCount":
                    GetDataCount(context, "News");
                    break;
                case "GetAllNews":
                    GetAllNews(context);
                    break;
                case "GetGamesCount":
                    GetDataCount(context, "Game");
                    break;
                case "GetAllGame":
                    GetAllGames(context);
                    break;
                case "GetServerCount":
                    GetDataCount(context, "Server");
                    break;
                case "GetAllServer":
                    GetAllServer(context);
                    break;
                case "GetCardCount":
                    GetDataCount(context, "Card");
                    break;
                case "GetAllCard":
                    GetAllCard(context);
                    break;
                case "GetOrderCount":
                    GetDataCount(context, "Order");
                    break;
                case "GetAllOrder":
                    GetAllOrder(context);
                    break;
                case "GetOrderInfo":
                    GetOrderInfo(context);
                    break;
                case "GetSourceChangeCount":
                    GetDataCount(context, "SourceChange");
                    break;
                case "GetAllSourceChange":
                    GetAllSourceChange(context);
                    break;
                case "GetAllCollect":
                    GetAllCollect(context);
                    break;
                case "GetPromoCount":
                    GetPromoCount(context);
                    break;
                case "GetAllPromo":
                    GetAllPromo(context);
                    break;
                case "GetGameUserCount":
                    GetDataCount(context, "GameUser");
                    break;
                case "GetAllGameUser":
                    GetAllGameUser(context);
                    break;
                case "GetMasterLogCount":
                    GetDataCount(context, "MasterLog");
                    break;
                case "GetAllMasterLog":
                    GetAllMasterLog(context);
                    break;
                case "GetLinksCount":
                    GetDataCount(context, "Links");
                    break;
                case "GetAllLinks":
                    GetAllLinks(context);
                    break;
                case "GetCardLogCount":
                    GetDataCount(context, "CardLog");
                    break;
                case "GetAllCardLog":
                    GetAllCardLog(context);
                    break;
                case "GetMasterCount":
                    GetDataCount(context, "Master");
                    break;
                case "GetAllMaster":
                    GetAllMaster(context);
                    break;
                case "GetAllGift":
                    GetAllGift(context);
                    break;
                case "GetAllIndexNews":
                    GetAllIndexNews(context);
                    break;
                case "GetSysMsgCount":
                    GetDataCount(context, "SysMsg");
                    break;
                case "GetAllSysMsg":
                    GetAllSysMsg(context);
                    break;
                case "GetPayHistory1Count":
                    GetDataCount(context, "PayHistory1");
                    break;
                case "GetPayHistory2Count":
                    GetDataCount(context, "PayHistory2");
                    break;
                case "GetAllPayHistory1":
                    GetAllPayHistory1(context);
                    break;
                case "GetAllPayHistory2":
                    GetAllPayHistory2(context);
                    break;
                case "GetSpreadUserCount":
                    GetDataCount(context, "InitSpread");
                    break;
                case "GetAllSpreadUser":
                    GetAllSpreadUser(context);
                    break;
                case "GetAllNextSpreadUser":
                    GetAllSpreadUser(context);
                    break;
                case "GetSpreadPayCount":
                    GetDataCount(context, "SpreadPay");
                    break;
                case "GetAllSpreadPay":
                    GetAllSpreadPay(context);
                    break;
                case "GetSpreadGameCount":
                    GetDataCount(context, "SpreadGame");
                    break;
                case "GetAllSpreadGame":
                    GetAllSpreadGame(context);
                    break;
                case "GetMasterRoleCount":
                    GetDataCount(context, "GetMasterRoleCount");
                    break;
                case "GetAllMasterRole":
                    GetAllMasterRole(context);
                    break;
                case "GetRoleCom":
                    GetRoleCom(context);
                    break;
                case "GetLockCount":
                    GetDataCount(context, "GetLockCount");
                    break;
                case "GetAllLock":
                    GetAllLock(context);
                    break;
                case "GetMenu":
                    GetMenu(context);
                    break;
                case "CheckUser":
                    CheckUser(context);
                    break;
                case "DoCz":
                    DoCz(context);
                    break;
                case "DoBatCz":
                    DoBatCz(context);
                    break;
                case "GetExcel":
                    ExportExcel(context);
                    break;
                case "GetSumMoney":
                    GetSumMoney(context);
                    break;
                default:
                    break;
            }
        }

        public void GetGames(HttpContext context)
        {
            List<Games> list = new List<Games>();
            list = gm.GetAll();
            string JsonStr = Json.ListToJson(list);
            context.Response.Write(JsonStr);
        }

        public void GetGamesIdName(HttpContext context)
        {
            List<Games> list = gm.GetAll();
            StringBuilder Json = new StringBuilder();
            Json.Append("[");
            if (list.Count > 0)
            {
                int i = 0;
                foreach (Games g in list)
                {
                    Json.Append("{");
                    Json.Append("\"" + "Id" + "\":" + "\"" + g.Id.ToString() + "\",");
                    Json.Append("\"" + "Name" + "\":" + "\"" + g.Name + "\"");
                    Json.Append("}");
                    if (i++ < list.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]");
            context.Response.Write(Json);
        }

        public void GetServers(HttpContext context)
        {
            int GameId = int.Parse(context.Request["GameId"]);
            List<GameServer> list = new List<GameServer>();
            list = sm.GetServersByGame(GameId);
            string JsonStr = Json.ListToJson(list);
            context.Response.Write(JsonStr);
        }

        public void GetServerIdName(HttpContext context)
        {
            int GameId = int.Parse(context.Request["GameId"]);
            List<GameServer> list = sm.GetServersByGame(GameId);
            StringBuilder Json = new StringBuilder();
            Json.Append("[");
            if (list.Count > 0)
            {
                int i = 0;
                foreach (GameServer gs in list)
                {
                    Json.Append("{");
                    Json.Append("\"" + "Id" + "\":" + "\"" + gs.Id + "\",");
                    Json.Append("\"" + "Name" + "\":" + "\"" + gs.Name + "\"");
                    Json.Append("}");
                    if (i++ < list.Count - 1)
                    {
                        Json.Append(",");
                    }
                }
            }
            Json.Append("]");
            context.Response.Write(Json);
        }

        public void GetDataCount(HttpContext context, string Code)
        {
            string WhereStr = context.Request["WhereStr"];
            int UserId = BBRequest.GetUserId();
            GameUser gu = new GameUser();
            switch (Code)
            {
                case "News":
                    context.Response.Write(nm.GetNewsCount(WhereStr));
                    break;
                case "Game":
                    context.Response.Write(gm.GetGamesCount(WhereStr));
                    break;
                case "Server":
                    context.Response.Write(sm.GetServerCount(WhereStr));
                    break;
                case "Card":
                    context.Response.Write(cm.GetCardCount(WhereStr));
                    break;
                case "Order":
                    context.Response.Write(om.GetOrderCount(WhereStr));
                    break;
                case "SourceChange":
                    context.Response.Write(scm.GetSourceChangeCount(WhereStr));
                    break;
                case "GameUser":
                    context.Response.Write(gum.GetGameUserCount(WhereStr));
                    break;
                case "MasterLog":
                    context.Response.Write(mm.GetMasterLogCount(WhereStr));
                    break;
                case "Links":
                    context.Response.Write(lm.GetLinksCount(WhereStr));
                    break;
                case "CardLog":
                    context.Response.Write(cm.GetCardLogCount(WhereStr));
                    break;
                case "Master":
                    context.Response.Write(mm.GetMasterCount(WhereStr));
                    break;
                case "SysMsg":
                    context.Response.Write(smm.GetSysMsgCount(BBRequest.GetUserId()));
                    break;
                case "PayHistory1":
                    gu = gum.GetGameUser(UserId);
                    WhereStr = "where UserName='" + gu.UserName + "'";
                    context.Response.Write(om.GetOrderCount(WhereStr));
                    break;
                case "PayHistory2":
                    gu = gum.GetGameUser(UserId);
                    WhereStr = "where UserName='" + gu.UserName + "' and State=2";
                    context.Response.Write(om.GetOrderCount(WhereStr));
                    break;
                case "InitSpread":
                    string[] re = WhereStr.Split('|');
                    string Coded = context.Request["Code"];
                    if (re.Length == 4)
                    {
                        context.Response.Write(om.GetSpreadCount(int.Parse(re[3]), false));
                    }
                    else
                    {
                        if (Coded == "All")
                        {
                            context.Response.Write(om.GetSpreadCount(UserId, false));
                        }
                        else
                        {
                            context.Response.Write(om.GetSpreadCount(UserId, true));
                        }
                    }
                    break;
                case "SpreadPay":
                    context.Response.Write(om.GetSpreadPayCount(UserId, WhereStr));
                    break;
                case "SpreadGame":
                    context.Response.Write(om.GetSpreadGameCount(UserId, WhereStr));
                    break;
                case "GetMasterRoleCount":
                    context.Response.Write(mm.GetMasterRoleCount(WhereStr));
                    break;
                case "GetLockCount":
                    context.Response.Write(Lkm.GetAllLockCount(WhereStr));
                    break;
                case "Under":
                    context.Response.Write(om.GetSpreadUserInfo(UserId, WhereStr));
                    break;
                default:
                    break;
            }
        }

        public void GetAllNews(HttpContext context)
        {
            int PageSize = int.Parse(context.Request["PageSize"]);
            int PageNum = int.Parse(context.Request["PageNum"]);
            string WhereStr = context.Request["WhereStr"].Replace("where", "");
            DataTable dt = new DataTable();
            dt = nm.GetNews(PageSize, PageNum, WhereStr, "SortId asc,ReleaseTime desc");
            context.Response.Write(hh.NewsHtml(dt));
        }

        public void GetAllGames(HttpContext context)
        {
            int PageSize = int.Parse(context.Request["PageSize"]);
            int PageNum = int.Parse(context.Request["PageNum"]);
            string WhereStr = context.Request["WhereStr"].Replace("where", "");
            List<Games> list = new List<Games>();
            list = gm.GetAll(PageSize, PageNum, WhereStr, "sort_id asc,addtime desc");
            context.Response.Write(hh.GameHtml(list));
        }

        public void GetAllServer(HttpContext context)
        {
            int PageSize = int.Parse(context.Request["PageSize"]);
            int PageNum = int.Parse(context.Request["PageNum"]);
            string WhereStr = context.Request["WhereStr"].Replace("where", "");
            DataTable dt = new DataTable();
            dt = sm.GetAllServer(PageSize, PageNum, WhereStr, "SortId asc,StartTime desc");
            context.Response.Write(hh.ServersHtml(dt));
        }

        public void GetAllCard(HttpContext context)
        {
            int PageSize = int.Parse(context.Request["PageSize"]);
            int PageNum = int.Parse(context.Request["PageNum"]);
            string WhereStr = context.Request["WhereStr"].Replace("where", "");
            DataTable dt = new DataTable();
            dt = cm.GetCards(PageSize, PageNum, WhereStr, "");
            context.Response.Write(hh.CardsHtml(dt));
        }

        public void GetAllOrder(HttpContext context)
        {
            int PageSize = int.Parse(context.Request["PageSize"]);
            int PageNum = int.Parse(context.Request["PageNum"]);
            string WhereStr = context.Request["WhereStr"].Replace("where", "");
            DataTable dt = new DataTable();
            dt = om.GetAllOrders(PageSize, PageNum, WhereStr, "PayTime desc");
            context.Response.Write(hh.OrderHtml(dt));
        }

        /// <summary>
        /// 获取指定服的充值排名
        /// </summary>
        /// <param name="context"></param>
        public void GetSumMoney(HttpContext context)
        {
            int gameId = int.Parse(context.Request["gameId"]);
            int serverId = int.Parse(context.Request["serverId"]);
            DataTable dt = om.GetSumMoneyOrder(gameId,serverId);
            context.Response.Write(hh.PayMoneyOrderHtml(dt,gameId,serverId));
        }

        public void GetOrderInfo(HttpContext context)
        {
            string WhereStr = context.Request["WhereStr"];
            Double totalCount = om.GetOrderCount(WhereStr);
            Double CompleteCount = om.GetOrderCount(WhereStr + " and State=2");
            Double UnfinishedCount = totalCount - CompleteCount;
            Double SumOrderMoney = om.GetSumMoney(WhereStr);
            Double SdOrderMoney = om.GetSumMoney(WhereStr + "and PayType=6 ");
            Double PtOrderMoney = om.GetSumMoney(WhereStr + "and Type=2 ");
            context.Response.Write(totalCount + "|" + CompleteCount + "|" + UnfinishedCount + "|" + SumOrderMoney + "|" + SdOrderMoney + "|" + PtOrderMoney);
        }

        public void GetAllSourceChange(HttpContext context)
        {
            int PageSize = int.Parse(context.Request["PageSize"]);
            int PageNum = int.Parse(context.Request["PageNum"]);
            string WhereStr = context.Request["WhereStr"].Replace("where", "");
            DataTable dt = new DataTable();
            dt = scm.GetAllSourceChange(PageSize, PageNum, WhereStr, "DateChange desc");
            context.Response.Write(hh.SourceChangeHtml(dt));
        }

        public void GetAllCollect(HttpContext context)
        {
            int PageSize = int.Parse(context.Request["PageSize"]);
            int PageNum = int.Parse(context.Request["PageNum"]);
            string WhereStr = context.Request["WhereStr"].Replace("where", "");
            int MinMoney = int.Parse(context.Request["MinMoney"]);
            int MaxMoney = int.Parse(context.Request["MaxMoney"]);
            DataTable dt = new DataTable();
            dt = sm.GetAllCollect(PageSize, PageNum, WhereStr, "StartTime desc");
            context.Response.Write(hh.CollectHtml(dt, MaxMoney, MinMoney));
        }

        public void GetPromoCount(HttpContext context)
        {
            string WhereStr = context.Request["WhereStr"];
            string LinkName = context.Request["LinkName"];
            if (!string.IsNullOrEmpty(LinkName))
            {
                try
                {
                    string[] a = DESEncrypt.Decrypt(LinkName).Split('|');
                    string b = a[0];
                    string c = a[1];
                    int s = 0;
                    if (int.TryParse(a[0], out s) && int.TryParse(a[1], out s))
                    {
                        GameUser gu = gum.GetGameUser(int.Parse(a[0]));
                        if (gu != null)
                        {
                            WhereStr += " and username ='" + gu.UserName + "'";
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
            context.Response.Write(om.GetPromoCount(WhereStr));
        }

        public void GetAllPromo(HttpContext context)
        {
            int PageSize = int.Parse(context.Request["PageSize"]);
            int PageNum = int.Parse(context.Request["PageNum"]);
            string WhereStr = context.Request["WhereStr"].Replace("where", "");
            string LinkName = context.Request["LinkName"];
            if (!string.IsNullOrEmpty(LinkName))
            {
                try
                {
                    string[] a = DESEncrypt.Decrypt(LinkName).Split('|');
                    string b = a[0];
                    string c = a[1];
                    int s = 0;
                    if (int.TryParse(a[0], out s) && int.TryParse(a[1], out s))
                    {
                        GameUser gu = gum.GetGameUser(int.Parse(a[0]));
                        if (gu != null)
                        {
                            WhereStr += " and username ='" + gu.UserName + "'";
                        }
                    }
                }
                catch (Exception)
                {

                }
            }
            DataTable dt = new DataTable();
            dt = om.GetAllPromo(PageSize, PageNum, WhereStr, "");
            context.Response.Write(hh.PromoHtml(dt));
        }

        public void GetAllGameUser(HttpContext context)
        {
            int PageSize = int.Parse(context.Request["PageSize"]);
            int PageNum = int.Parse(context.Request["PageNum"]);
            string WhereStr = context.Request["WhereStr"].Replace("where", "");
            DataTable dt = new DataTable();
            dt = gum.GetAllGameUser(PageSize, PageNum, WhereStr, "LastLoginTime desc");

            context.Response.Write(hh.GameUserHtml(dt));
        }

        public void GetAllMasterLog(HttpContext context)
        {
            int PageSize = int.Parse(context.Request["PageSize"]);
            int PageNum = int.Parse(context.Request["PageNum"]);
            string WhereStr = context.Request["WhereStr"].Replace("where", "");
            List<manager_log> list = new List<manager_log>();
            list = mm.GetAllMasterLog(PageSize, PageNum, WhereStr, "login_time desc");
            string HtmlStr = "";
            foreach (manager_log ml in list)
            {
                HtmlStr += "<tr><td>" + ml.id + "</td><td><a>" + ml.user_name + "</a></td><td>" + ml.note +
                    "</td><td>" + ml.login_ip + "</td><td>" + ml.login_time + "</td><td><a>删除</a></td></tr>";
            }
            context.Response.Write(HtmlStr);
        }

        public void GetAllLinks(HttpContext context)
        {
            int PageSize = int.Parse(context.Request["PageSize"]);
            int PageNum = int.Parse(context.Request["PageNum"]);
            string WhereStr = context.Request["WhereStr"].Replace("where", "");
            List<link> list = new List<link>();
            list = lm.GetAllLinks(PageSize, PageNum, WhereStr, "sort_id asc,add_time desc");
            string HtmlStr = "";
            Boolean IsEdit = rcm.GetRoleCompetence(GetUser(context).RoleId, 1431);
            Boolean IsDel = rcm.GetRoleCompetence(GetUser(context).RoleId, 1433);
            foreach (link l in list)
            {
                string ImageHtml = l.Is_image == 1 ? "<img src='" + l.Img_url + "' width='50' height='20'>" : "文字链接";
                string IsRedHtml = l.Is_red == 1 ? "<img src='../Images/Admin/ico-2.png' title='取消推荐'>" : "<img src='../Images/Admin/ico-2_.png' title='设置推荐'>";
                string IsLockHtml = l.Is_lock == 1 ? "<img src='../Images/Admin/ico-5.png' title='通过审核'>" : "<img src='../Images/Admin/ico-5_.png' title='暂未通过审核'>";
                string EditHtml = IsEdit ? " <a href=\"/Settings/LinkEdit?Id=" + l.Id + "\">" + l.Title + "</a> " : "<a>" + l.Title + "</a> ";
                string DelHtml = IsDel ? "<a href=\"javascript:InitPageContent('Links')\" onclick=\"Del('/Settings/DelLink?Id=" + l.Id + "')\">删除</a>" : " ";
                HtmlStr += "<tr><td>" + l.Id + "</td><td>" + EditHtml + "</td><td>" + ImageHtml + "</td><td>" + l.Add_time +
                    "</td><td>" + l.Sort_id + "</td><td>" + IsRedHtml + IsLockHtml + "</td><td>" + DelHtml + "</td></tr>";
            }
            context.Response.Write(HtmlStr);
        }

        public void GetAllCardLog(HttpContext context)
        {
            int PageSize = int.Parse(context.Request["PageSize"]);
            int PageNum = int.Parse(context.Request["PageNum"]);
            string WhereStr = context.Request["WhereStr"].Replace("where", "");
            DataTable dt = new DataTable();
            dt = cm.GetAllCardLog(PageSize, PageNum, WhereStr, "Time desc");
            string HtmlStr = "";
            foreach (DataRow row in dt.Rows)
            {
                HtmlStr += "<tr><td>" + row["Id"] + "</td><td>" + row["UserName"] + "</td><td>" + row["Time"] + "</td><td>" + row["CardName"] + "</td><td>" + row["CardNum"] + "</td></tr>";
            }
            context.Response.Write(HtmlStr);
        }

        public void GetAllMaster(HttpContext context)
        {
            int PageSize = int.Parse(context.Request["PageSize"]);
            int PageNum = int.Parse(context.Request["PageNum"]);
            string WhereStr = context.Request["WhereStr"].Replace("where", "");
            List<Master> list = new List<Master>();
            list = mm.GetAllMaster(PageSize, PageNum, WhereStr, "");
            string HtmlStr = "";
            Boolean IsEdit = rcm.GetRoleCompetence(GetUser(context).RoleId, 1441);
            Boolean IsDel = rcm.GetRoleCompetence(GetUser(context).RoleId, 1443);
            foreach (Master m in list)
            {
                string EditHtml = IsEdit ? "<a href=\"EditMaster?M=" + m.Id + "\">" + m.UserName + "</a>" : "<a>" + m.UserName + "</a>";
                string DelHtml = IsDel ? "<a href=\"javascript:InitPageContent('Master')\" onclick=\"Del('/Master/DelMaster?Mid=" + m.Id + "')\">删除</a>" : "";
                string State = m.State == 0 ? "<img title=\"正常\" src=\"../Images/Admin/icon_correct.png\" />" : "<img title=\"禁用\" src=\"../Images/Admin/icon_disable.png\" />";
                HtmlStr += "<tr><td>" + m.Id + "</td><td>" + EditHtml + "</td><td>" + m.RoleType + "</td><td>" + m.RealName + "</td><td>" + m.Phone + "</td><td>" + m.Email + "</td><td>" + m.AddTime + "</td><td>" + State + "</td><td>" + DelHtml + "</td></tr>";
            }
            context.Response.Write(HtmlStr);
        }

        public void GetAllGift(HttpContext context)
        {
            int PageSize = int.Parse(context.Request["PageSize"]);
            int PageNum = int.Parse(context.Request["PageNum"]);
            string WhereStr = context.Request["WhereStr"].Replace("where", "");
            DataTable dt = new DataTable();
            dt = cm.GetCards(PageSize, PageNum, WhereStr, "");
            string HtmlStr = "";
            foreach (DataRow row in dt.Rows)
            {
                HtmlStr += "<div id=\"picl\"><ul><li><a href=\"/" + gm.GetGame((int)row["GameId"]).GameNo + "/Gift?G=" + row["Id"] + "\"><img src=\"" + row["Img"] + "\" width=\"190\" height=\"105\" title=\"" + row["CardName"] + "\"></a> <a class=\"orange_link\"href=\"/" + gm.GetGame((int)row["GameId"]).GameNo + "/Gift?G=" + row["Id"] + "\">领取</a> </li> </ul> </div>";
            }
            context.Response.Write(HtmlStr);
        }

        public void GetAllIndexNews(HttpContext context)
        {
            int PageSize = int.Parse(context.Request["PageSize"]);
            int PageNum = int.Parse(context.Request["PageNum"]);
            string WhereStr = context.Request["WhereStr"].Replace("where", "");
            DataTable dt = new DataTable();
            dt = nm.GetNews(PageSize, PageNum, WhereStr, "SortId asc,ReleaseTime desc");
            string HtmlStr = "";
            foreach (DataRow row in dt.Rows)
            {
                string Type = "";
                switch ((int)row["Type"])
                {
                    case 2:
                        Type = "[官方公告]";
                        break;
                    case 4:
                        Type = "[精彩活动]";
                        break;
                    default:
                        break;
                }
                HtmlStr += "<li><span class=\"rt\">" + row["ReleaseTime"] + "</span>" + Type + "<a href=\"/NewsCenter/News?N=" + row["Id"] + "\"style=\"color: " + row["NameColor"] + "\">" + row["Title"] + "</a></li><li class=\"bk8 hr\"></li>";
            }
            context.Response.Write(HtmlStr);
        }

        public void GetAllSysMsg(HttpContext context)
        {
            int PageSize = int.Parse(context.Request["PageSize"]);
            int PageNum = int.Parse(context.Request["PageNum"]);
            List<sysmsg> list = new List<sysmsg>();
            list = smm.GetAllSysMsg(PageSize, PageNum, "userid=" + BBRequest.GetUserId(), "addtime desc");
            string HtmlStr = "";
            foreach (sysmsg sm in list)
            {
                string Type = sm.type == 1 ? "平台消息" : "站内信";
                string State = sm.state == 1 ? "已读" : "未读";
                HtmlStr += "<tr><td align=\"center\">" + Type + "</td><td class=\"t\"><a href=\"javascript:;\" onclick=\"ShowMsg('" + sm.id + "')\">" + sm.title + "</a></td><td align=\"center\">未读</td><td align=\"center\">" + sm.addtime + "</td><td align=\"center\"><a href=\"javascript:InitPageContent('SysMsg')\" onclick=\"Del('/UserCenter/DelUserMsg?M=" + sm.id + "')\">删除</a></td><tr id=\"trCon" + sm.id + "\" style=\"display: none;\"><td colspan=\"5\" style=\"padding-left: 10px;\"><div class=\"xiaoxi_con11\"><span style=\"color: #FF9900\">消息内容：</span> <span style=\"color: #F7654B\">" + sm.msg + "</span><br><span style=\"color: #FF9900\">防诈骗提示：</span>请您在游戏中不要相信任何中奖信息，那些都是骗子的骗人招数,请您要妥善保管好您的帐号。</div></td></tr></tr>";
            }
            context.Response.Write(HtmlStr);
        }

        public void GetAllPayHistory1(HttpContext context)
        {
            int PageSize = int.Parse(context.Request["PageSize"]);
            int PageNum = int.Parse(context.Request["PageNum"]);
            int UserId = BBRequest.GetUserId();
            GameUser gu = new GameUser();
            gu = gum.GetGameUser(UserId);
            string WhereStr = "UserName='" + gu.UserName + "' and state=2";
            DataTable dt = new DataTable();
            dt = om.GetAllOrders(PageSize, PageNum, WhereStr, "PayTime desc");
            string HtmlStr = "";
            foreach (DataRow row in dt.Rows)
            {
                string Type = row["Type"].ToString() == "1" ? "游戏币" : "平台币";
                //string State = "";
                //switch ((int)row["State"])
                //{
                //    case 0:
                //        State = "<span style=\"color:red\">未支付</span>";
                //        break;
                //    case 1:
                //        State = "<span style=\"color:#F7654B\">已付款</span>";
                //        break;
                //    case 2:
                //        State = "<span style=\"color:blue\">已完成</span>";
                //        break;
                //    default:
                //        break;
                //}
                    //HtmlStr += "<tr><td>" + row["PayTime"] + "</td><td>" + row["OrderNo"] + "</td><td>" + row["PayMoney"] + "</td><td>" + Type + "</td><td>" + "State" + "</td></tr>";
                    HtmlStr += "<tr><td>" + row["PayTime"] + "</td><td>" + row["OrderNo"] + "</td><td>" + row["PayMoney"] + "</td><td>" + Type + "</td><td>" + "已完成" + "</td></tr>";
            }
            context.Response.Write(HtmlStr);
        }

        public void GetAllPayHistory2(HttpContext context)
        {
            int PageSize = int.Parse(context.Request["PageSize"]);
            int PageNum = int.Parse(context.Request["PageNum"]);
            int UserId = BBRequest.GetUserId();
            GameUser gu = new GameUser();
            gu = gum.GetGameUser(UserId);
            string WhereStr = " UserName='" + gu.UserName + "' and State=2";
            DataTable dt = new DataTable();
            dt = om.GetAllOrders(PageSize, PageNum, WhereStr, "PayTime desc");
            string HtmlStr = "";
            foreach (DataRow row in dt.Rows)
            {
                string Consumerobjects = "";
                if ((int)row["GameId"] == 0 || (int)row["ServerId"] == 0)
                {
                    Consumerobjects = "充值平台币";
                }
                else
                {
                    Consumerobjects = row["GameName"] + "-" + row["ServerName"];
                }
                HtmlStr += "<tr><td>" + row["PayTime"] + "</td><td>" + row["PayMoney"] + "</td><td>" + Consumerobjects + "</td></tr>";
            }
            context.Response.Write(HtmlStr);
        }

        public void GetAllSpreadUser(HttpContext context)
        {
            int PageSize = int.Parse(context.Request["PageSize"]);
            int PageNum = int.Parse(context.Request["PageNum"]);
            int UserId = 0;
            string WhereStr = context.Request["WhereStr"];
            string Code = context.Request["Code"];
            string WhereStr2 = "";
            UserId = BBRequest.GetUserId();
            string[] re = WhereStr.Split('|');

            if (!string.IsNullOrEmpty(re[0]))
            {
                WhereStr2 += " and o.gameid = " + re[0];
            }
            else
            {
                WhereStr2 += " and 1=1";
            }
            if (string.IsNullOrEmpty(re[1]) || string.IsNullOrEmpty(re[2]))
            {
                WhereStr2 += " and 1=1";
            }
            else
            {
                WhereStr2 += " and o.paytime>='" + re[1] + "' and o.paytime<='" + re[2] + "'";
            }
            if (Code == "All")
            {
                WhereStr = "source='" + UserId + "' ";
            }
            else
            {
                if (re.Length == 4)
                {
                    UserId = int.Parse(re[3]);
                }
                WhereStr = "source='" + UserId + "' ";
            }

            List<GameUser> list = new List<GameUser>();
            list = gum.GetSpreadUser(PageSize, PageNum, WhereStr, "addtime desc");
           // int GameId = int.Parse(context.Request["GameId"]);
            //int ServerId = int.Parse(context .Request["ServerId"]);
            string HtmlStr = "";
            foreach (GameUser gu in list)
            {
                //if (Code == "UnderDetail")
                //{
                //    HtmlStr += "<tr><td><a style=\"width: 20px; margin-right: 5px;\" onclick=\"ShowUser(this,'" + gu.Id + "')\"><img src=\"/Images/add01.png\" height=\"20\" /></a><span>" + gu.UserName + "</span></td><td>" + gu.AddTime + "</td><td>" + om.GetSumMoney(gu.Id, WhereStr2) + "</td><td>" + om.GetAllSpreadCount(gu.Id) + "</td><td><a  href=\"javascript:InitPageContent('UnderUserDetail')\" onclick=\"UpdateUserSource('D','" + gu.Id + "')\">撤销推广员</a></td></tr>";
                //}
                //else
                //{
                GameUser spread = gum.GetGameUser(UserId);
                if (spread.IsSpreader == 2)
                {
                    if (gu.IsSpreader == 1)
                    {
                        HtmlStr += "<tr><td><a style=\"width: 20px; margin-right: 5px;\" onclick=\"ShowUser(this,'" + gu.Id + "')\">" + gu.UserName + "</a></td><td>" + gu.AddTime + "</td><td>" + om.GetSumMoney(gu.Id, WhereStr2) + "</td><td>" + om.GetAllSpreadCountS(gu.Id) + "/" + om.GetAllSpreadCount(gu.Id) + "</td><td><a href=\"javascript:InitPageContent('SpreadUser')\" onclick=\"Reset('/SpreadCenter/ResetSpreader?UId=" + gu.Id + "')\">重置推广员</a></td></tr>";
                    }
                    else
                    {
                        HtmlStr += "<tr><td>" + gu.UserName + "</td><td>" + gu.AddTime + "</td><td>" + om.GetSumMoney(gu.Id, WhereStr2) + "</td><td>" + gu.Ip + "</td><td><a href=\"javascript:InitPageContent('SpreadUser')\" onclick=\"UpdateUserSource('U','" + gu.Id + "')\">提升为推广员</a></td></tr>";
                    }
                }
                else
                {
                    HtmlStr += "<tr><td>" + gu.UserName + "</td><td>" + gu.AddTime + "</td><td>" + om.GetSumMoney(gu.Id, WhereStr2) + "</td><td>" + gu.Ip + "</td><td width=\"181\"></td></tr>";
                }
                
            }
            context.Response.Write(HtmlStr);
        }

        public void GetAllSpreadPay(HttpContext context)
        {
            int PageSize = int.Parse(context.Request["PageSize"]);
            int PageNum = int.Parse(context.Request["PageNum"]);
            int UserId = UserId = BBRequest.GetUserId();
            string WhereStr = context.Request["WhereStr"];
            List<Orders> list = new List<Orders>();
            list = om.GetAllSpreadPay(PageSize, PageNum, UserId, WhereStr);
            string HtmlStr = "";
            foreach (Orders o in list)
            {
                GameUserInfo gui = gm.GetGameUserInfo(o.GameId, gum.GetGameUser(o.UserName).Id, o.ServerId);
                HtmlStr += "<tr><td>" + o.UserName + "</td><td>" + o.PayTime + "</td><td>" + o.GameName + "-" + o.ServerName + "</td><td>" + gui.UserName + "[" + gui.Level + "级]" + "</td><td>" + o.PayMoney + "</td></tr>";
            }
            context.Response.Write(HtmlStr);
        }

        public void GetAllSpreadGame(HttpContext context)
        {
            int PageSize = int.Parse(context.Request["PageSize"]);
            int PageNum = int.Parse(context.Request["PageNum"]);
            string WhereStr = context.Request["WhereStr"];
            int UserId = UserId = BBRequest.GetUserId();
            Dictionary<string, string> list = new Dictionary<string, string>();
            list = om.GetAllSpreadGame(PageSize, PageNum, UserId, WhereStr);
            string HtmlStr = "";
            foreach (KeyValuePair<string, string> kv in list)
            {
                GameServer gs = sm.GetGameServer(int.Parse(kv.Key));
                string GameName = gm.GetGame(gs.GameId).Name;
                HtmlStr += "<tr><td>" + GameName + "-" + gs.Name + "</td><td>" + kv.Value + "</td></tr>";
            }
            context.Response.Write(HtmlStr);
        }

        private void GetAllMasterRole(HttpContext context)
        {
            int PageSize = int.Parse(context.Request["PageSize"]);
            int PageNum = int.Parse(context.Request["PageNum"]);
            string WhereStr = context.Request["WhereStr"];
            List<MasterRole> list = new List<MasterRole>();
            list = mm.GetAllMasterRole(PageSize, PageNum, WhereStr, "");
            string HtmlStr = "";
            Boolean IsEdit = rcm.GetRoleCompetence(GetUser(context).RoleId, 1452);
            Boolean IsDel = rcm.GetRoleCompetence(GetUser(context).RoleId, 1454);
            Boolean IsDetail = rcm.GetRoleCompetence(GetUser(context).RoleId, 1451);
            foreach (MasterRole mr in list)
            {
                string EditHtml = IsEdit ? "<a href=\"EditRole?R=" + mr.RoleId + "\">编辑权限</a>" : "";
                string DelHtml = IsDel ? "<a>删除</a>" : "";
                string DetaileHtml = IsDetail ? "<a href=\"RoleDetail?R=" + mr.RoleId + "\">查看权限</>" : "";
                HtmlStr += "<tr><td>" + mr.RoleId + "</td><td>" + mr.RoleName + "</td><td>" + DetaileHtml + " " + EditHtml + " " + DelHtml + "</td></tr>";
            }
            context.Response.Write(HtmlStr);
        }

        private void GetRoleCom(HttpContext context)
        {
            int CId = int.Parse(context.Request["C"]);
            List<Competence> list = new List<Competence>();
            list = rcm.GetChildrenCompetence(CId, GetUser(context).RoleId);
            string HtmlStr = "<tr><td></td><td>";
            foreach (Competence Com in list)
            {
                HtmlStr += "<table><tr><td></td><td><input type=\"checkbox\" name=\"RItem\" value=\"" + Com.CompetenceId + "\" onclick=\"ShowChildren(this)\" />" + Com.CompetenceName + "权限</td></tr></table>";
            }
            HtmlStr += "</td></tr>";
            context.Response.Write(HtmlStr);
        }

        private void GetAllLock(HttpContext context)
        {
            int PageSize = int.Parse(context.Request["PageSize"]);
            int PageNum = int.Parse(context.Request["PageNum"]);
            string WhereStr = context.Request["WhereStr"].Replace("where", "");

            DataTable dt = Lkm.GetAllLock(PageSize, PageNum, WhereStr, "");
            string HtmlStr = "";
            foreach (DataRow row in dt.Rows)
            {
                HtmlStr += "<tr><td>" + row["Id"] + "</td><td><a>" + row["Lock"] + "</a></td><td>" + row["AddTime"] + "</td><td>" + row["LockInfo"] +
                    "</td><td>" + row["Operator"] + "</td><td><a  href=\"javascript:InitPageContent('Lock')\" onclick=\"Del('/GameUser/DelLock?Lock=" + row["Lock"] + "')\">删除</a></td></tr>";
            }
            context.Response.Write(HtmlStr);
        }

        public void GetMenu(HttpContext context)
        {
            List<Menu> list = new RoleCompetenceManager().GetAllMenu(GetUser(context).RoleId);
            string strTxt = Json.ListToJson(list);
            context.Response.Write(strTxt);
        }

        public void CheckUser(HttpContext context)
        {
            string UserName = context.Request["UserName"].Trim();
            string GameId = context.Request["GameId"].Trim();
            int ServerId = int.Parse(context.Request["ServerId"].Trim());
            int Type = int.Parse(context.Request["Type"].Trim());
            if (!string.IsNullOrEmpty(context.Request["Money"].Trim()))
            {
                float Money = float.Parse(context.Request["Money"].Trim());
                try
                {
                    if (om.CheckOrder(GameId, ServerId, Type, 6, UserName, Money, GetUser(context).UserName))
                    {
                        context.Response.Write("cg");
                    }
                }
                catch (Exception e)
                {
                    context.Response.Write(e.Message);
                }
            }
            else
            {
                context.Response.Write("请输入充值金额！");
            }
        }

        public void DoCz(HttpContext context)
        {
            string GameId = context.Request["GameId"].Trim();
            int ServerId = int.Parse(context.Request["ServerId"].Trim());
            int Type = int.Parse(context.Request["Type"].Trim());
            float Money = float.Parse(context.Request["Money"].Trim());
            OrderManager om = new OrderManager();
            GamesManager gm = new GamesManager();
            string PWD = context.Request["PWD"].Trim();
            string Bz = context.Request["Bz"].Trim();
            string UserList = context.Request["UserList"];
            string[] Result = UserList.Substring(0, UserList.LastIndexOf('|')).Split('|');
            string MasterName = GetUser(context).UserName;
            if (mm.GetMasterByCz(MasterName, DESEncrypt.Md5(PWD, 32)) == null)
            {
                context.Response.Write("fali");
            }
            else
            {
                if (MasterName == "shouchong001")
                {
                    if (Money > 10)
                    {
                        context.Response.Write("outmoney");
                        return;
                    }
                }
                if (!string.IsNullOrEmpty(Bz))
                {
                    string ResHtml = "";
                    foreach (string UserName in Result)
                    {
                        Orders order = new Orders();
                        if (UserName != "undefined" && !string.IsNullOrEmpty(UserName.Trim()))
                        {
                            try
                            {
                                order = om.GetOrder("J", GameId, ServerId, Type, 6, UserName, Money, MasterName);
                                order.State = 1;
                                order.Ip = Bz;
                                if (om.AddOrder(order))
                                {
                                    ResHtml += "<span>您的订单<span style=\"color:Red\">" + order.OrderNo + "</span>已处理。<span style=\"color:Red\">" + gm.PayManager(order.OrderNo) + "</span></span><br/>";
                                }
                                else
                                {
                                    ResHtml += "<span>您的订单<span style=\"color:Red\">" + order.OrderNo + "</span>已处理。<span style=\"color:Red\">添加订单失败！用户名称为：</span>" + UserName + "</span><br/>";
                                }
                            }
                            catch (Exception e)
                            {
                                ResHtml += "<span>您的订单处理异常！<span style=\"color:Red\">" + e.Message + "</span><span>用户名称为：" + UserName +"</span><br/>";
                            }
                        }
                    }
                    context.Response.Write(ResHtml);
                }
                else
                {
                    context.Response.Write("BzIsNull");
                }
            }
        }

        /// <summary>
        /// 批量手动充值
        /// </summary>
        /// <param name="context"></param>
        public void DoBatCz(HttpContext context)
        {
            string GameId = context.Request["GameId"].Trim();
            int ServerId = int.Parse(context.Request["ServerId"].Trim());
            int Type = int.Parse(context.Request["Type"].Trim());
            string PWD = context.Request["PWD"].Trim();
            string Bz = context.Request["Bz"].Trim();
            string operUser = GetUser(context).UserName;
            if (mm.GetMasterByCz(operUser, DESEncrypt.Md5(PWD, 32)) == null)
            {
                context.Response.Write("fail");
                return;
            }
            if(string.IsNullOrEmpty(Bz))
            {
                context.Response.Write("BzIsNull");
                return;
            }
            if(string.IsNullOrEmpty(GameId) || string.IsNullOrEmpty(ServerId.ToString()))
            {
                context.Response.Write("GameIsNull");
                return;
            }
            OrderManager om = new OrderManager();
            GamesManager gm = new GamesManager();
            HttpPostedFile PostUsers = context.Request.Files["Users"];
            string filename = DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(PostUsers.FileName);
            PostUsers.SaveAs(context.Server.MapPath("~/upload/" + filename));
            string UserPath = context.Server.MapPath("~/upload/" + filename);
            List<string> listUser = new List<string>();
            using (StreamReader sr = File.OpenText(UserPath))
            {
                string users = "";
                while (true)
                {
                    users = sr.ReadLine();
                    if (users != null)
                    {
                        listUser.Add(users);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            string ResHtml = "";
            int countS = 0;
            int countF = 0;
            string usersFailed = "";
            foreach (string userName in listUser)
            {
                Orders order = new Orders();
                if(!gum.IsGameUser(userName))
                {
                    ResHtml += "<span style=\"color:Red\">" + userName + ":用户不存在</span><br/>";
                    usersFailed += userName + ",";
                    countF++;
                }
                else 
                {
                    try
                    {
                        order = om.GetOrder("J", GameId, ServerId, Type, 6, userName, 1, operUser);
                        order.State = 1;
                        order.Ip = Bz;
                        if (om.AddOrder(order))
                        {
                            GameUserInfo gui = gm.GetGameUserInfo(int.Parse(GameId), gum.GetGameUser(userName).Id, ServerId);
                            if (gui.Message == "Success")
                            {
                                gm.PayManager(order.OrderNo);
                                countS++;
                            }
                            else
                            {
                                ResHtml += "<span style=\"color:Red\">" + userName + gui.Message + "</span>";
                                countF++;
                            }
                            
                        }
                        else
                        {
                            countF++;
                            usersFailed += userName + ",";
                        }
                    }
                    catch (Exception e)
                    {
                        countF++;
                        usersFailed += userName + ",";
                        ResHtml += "<span>订单处理异常！<span style=\"color:Red\">" + e.Message + "</span></span><br/>";
                    }
                }
                
            }
            ResHtml += "<span style=\"color:green\">充值成功" + countS + "个</span>。<br/><span style=\"color:Red\">订单失败" + countF + "个。用户为：" + usersFailed + "</span><br/>";
            context.Response.Write(ResHtml);

        }

        public void ExportExcel(HttpContext context)
        {
            int GameId = int.Parse(context.Request["GameId"].Trim());
            int ServerId = int.Parse(context.Request["ServerId"].Trim());
            List<int> ListUser = gum.GetSpreadUserByBengBeng(GameId, "BengBeng");
            List<GameUserInfo> ListUserInfo = new List<GameUserInfo>();
            foreach (int user in ListUser)
            {
                GameUserInfo gui = new GameUserInfo();
                gui = gm.GetGameUserInfo(GameId, user, ServerId);
                if (gui.Message == "Success")
                {
                    ListUserInfo.Add(gui);
                }
            }
            HSSFWorkbook book = new HSSFWorkbook();
            ISheet sheet = book.CreateSheet("游戏角色信息");
            IRow rowHeader = sheet.CreateRow(0);
            //string [] strHeader ={"用户ID","角色名","区服","等级"};
            //for (int i=0;i< strHeader.Length ;i ++)
            //{
            //    rowHeader.CreateCell(i ,CellType.STRING).SetCellValue(strHeader[i]);
            //}
            rowHeader.CreateCell(0, CellType.STRING).SetCellValue("用户ID");
            rowHeader.CreateCell(1, CellType.STRING).SetCellValue("角色名");
            rowHeader.CreateCell(2, CellType.STRING).SetCellValue("区服");
            rowHeader.CreateCell(3, CellType.STRING).SetCellValue("等级");
            int rowNum = 1;
            foreach(GameUserInfo gui in ListUserInfo)
            {
                IRow row = sheet.CreateRow(rowNum++);
                row.CreateCell(0,CellType.STRING).SetCellValue(gui.Id);
                row.CreateCell(1, CellType.STRING).SetCellValue(gui.UserName);
                row.CreateCell(2, CellType.STRING).SetCellValue(gui.ServerName);
                row.CreateCell(3, CellType.STRING).SetCellValue(gui.Level);
            }
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xls";
            string filePath = context.Server.MapPath("~/upload/" + fileName);
            using (Stream stream = File.OpenWrite(filePath))
            {
                book.Write(stream);
            }
            context.Response.Write("../../upload/" + fileName); 
        }

        public Master GetUser(HttpContext context)
        {
            Master master = new Master();
            try
            {
                master = context.Session[Keys.SESSION_ADMIN_INFO] as Master;
            }
            catch (Exception)
            {
                context.Response.Write("<script>top.location.href='/Admin/Login';</script>");
            }
            return master;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}