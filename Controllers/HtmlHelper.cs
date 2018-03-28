using Common;
using Game.Manager;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace Game.Controllers
{
    public class HtmlHelper : IRequiresSessionState
    {
        RoleCompetenceManager rcm = new RoleCompetenceManager();
        OrderManager om = new OrderManager();
        LinksManager lm = new LinksManager();
        GamesManager gm = new GamesManager();

        public string NewsHtml(DataTable dt)
        {
            string HtmlStr = "";
            Boolean IsEdit = rcm.GetRoleCompetence(GetUser().RoleId, 11211);
            Boolean IsDel = rcm.GetRoleCompetence(GetUser().RoleId, 11213);
            foreach (DataRow row in dt.Rows)
            {
                string Type = "";
                switch ((int)row["Type"])
                {
                    case 1:
                        Type = "游戏新闻";
                        break;
                    case 2:
                        Type = "游戏公告";
                        break;
                    case 3:
                        Type = "游戏攻略";
                        break;
                    case 4:
                        Type = "游戏活动";
                        break;
                    case 5:
                        Type = "游戏资料";
                        break;
                    default:
                        break;
                }
                string IsTopImageURL = (int)row["IsTop"] == 1 ? "../Images/Admin/ico-1.png" : "../Images/Admin/ico-1_.png";
                string IsTopImageAlt = (int)row["IsTop"] == 1 ? "取消置顶" : "设置置顶";
                string IsRedImageURL = (int)row["IsRed"] == 1 ? "../Images/Admin/ico-2.png" : "../Images/Admin/ico-2_.png";
                string IsRedImageAlt = (int)row["IsRed"] == 1 ? "取消推荐" : "设置推荐";
                string IsHotImageURL = (int)row["IsHot"] == 1 ? "../Images/Admin/ico-3.png" : "../Images/Admin/ico-3_.png";
                string IsHotImageAlt = (int)row["IsHot"] == 1 ? "取消热门" : "设置热门";
                string PropertyHtml = "<img src='" + IsTopImageURL + "' title='" + IsTopImageAlt + "' /><img src='" + IsRedImageURL + "'title='" + IsRedImageAlt + "' /> <img src='" + IsHotImageURL + "' title='" + IsHotImageAlt + "' />";
                string EditHtml = IsEdit ? "<a href=\"/News/EditNews?NewsId=" + row["Id"] + "\">" + row["Title"] + "</a>" : "<a>" + row["Title"] + "</a>";
                string DelHtml = IsDel ? "<a href=\"javascript:InitPageContent('News')\" onclick=\"Del('/News/DelNews?NewsId=" + row["Id"] + "')\">删除</a>" : "";
                HtmlStr += "<tr><td>" + EditHtml + "</td><td>" + row["GameName"] + "</td><td>" + Type + "</td><td>" + row["ReleaseTime"] + "</td><td>" + row["SortId"] + "</td><td>" + PropertyHtml + "</td><td>" + DelHtml + "</td></tr>";
            }
            return HtmlStr;
        }

        public string GameHtml(List<Games> list)
        {
            string HtmlStr = "";
            Boolean IsEdit = rcm.GetRoleCompetence(GetUser().RoleId, 11231);
            Boolean IsDel = rcm.GetRoleCompetence(GetUser().RoleId, 11233);
            Boolean IsServer = rcm.GetRoleCompetence(GetUser().RoleId, 1124);
            Boolean IsAddServer = rcm.GetRoleCompetence(GetUser().RoleId, 11242);
            foreach (Games g in list)
            {
                string IsTopImageURL = g.Is_Red == 1 ? "../Images/Admin/ico-2.png" : "../Images/Admin/ico-2_.png";
                string IsTopImageAlt = g.Is_Red == 1 ? "取消推荐" : "设置推荐";
                string IsRedImageURL = g.Is_Hot == 1 ? "../Images/Admin/ico-3.png" : "../Images/Admin/ico-3_.png";
                string IsRedImageAlt = g.Is_Hot == 1 ? "取消热门" : "设置热门";
                string IsHotImageURL = g.Is_Lock == 1 ? "../Images/Admin/ico-5.png" : "../Images/Admin/ico-5_.png";
                string IsHotImageAlt = g.Is_Lock == 1 ? "取消上线" : "设置上线";
                string PropertyHtml = "<img src='" + IsTopImageURL + "' title='" + IsTopImageAlt + "' /><img src='" + IsRedImageURL + "'title='" + IsRedImageAlt + "' /> <img src='" + IsHotImageURL + "' title='" + IsHotImageAlt + "' />";
                string EditHtml = IsEdit ? "<a href=\"EditGame?GameId=" + g.Id + "\">" + g.Name + "</a>" : "<a>" + g.Name + "</a>";
                string DelHtml = IsDel ? " <a href=\"javascript:InitPageContent('Game')\" onclick=\"Del('/Game/DelGame?GameId=" + g.Id + "')\">删除</a> " : " ";
                string ServerHtml = IsServer ? " <a href=\"/Servers/?GameId=" + g.Id + "\">服务器列表</a> " : " ";
                string AddServerHtml = IsAddServer ? " <a href=\"/Servers/AddServer?GameId=" + g.Id + "\">添加服务器</a> " : " ";
                HtmlStr += "<tr><td>" + g.Id + "</td><td>" + g.GameNo + "</td><td>" + EditHtml + "</td><td>" + g.AddTime + "</td><td>" + g.Sort_Id + "</td><td>" + PropertyHtml + "</td><td>" + AddServerHtml + ServerHtml + DelHtml + "</td></tr>";
            }
            return HtmlStr;
        }

        public string ServersHtml(DataTable dt)
        {
            string HtmlStr = "";
            Boolean IsEdit = rcm.GetRoleCompetence(GetUser().RoleId, 11241);
            Boolean IsDel = rcm.GetRoleCompetence(GetUser().RoleId, 11243);
            foreach (DataRow row in dt.Rows)
            {
                string State = "";
                switch ((int)row["State"])
                {
                    case 1:
                        State = "即将开启";
                        break;
                    case 2:
                        State = "维护";
                        break;
                    case 3:
                        State = "流畅";
                        break;
                    case 4:
                        State = "火爆";
                        break;
                    default:
                        break;

                }
                string EditHtml = IsEdit ? "<a href=\"EditServer?ServerId=" + row["Id"] + "\">" + row["ServerName"] + "</a>" : "<a>" + row["ServerName"] + "</a>";
                string DelHtml = IsDel ? "<a href=\"javascript:InitPageContent('Server')\" onclick=\"Del('/Servers/DelServer?ServerId=" + row["Id"] + "')\">删除</a>" : " ";
                HtmlStr += "<tr><td>" + row["Id"] + "</td><td>" + row["ServerNo"] + "</td><td>" + row["GameName"] + "</td><td>" + row["Qufu"] + "</td><td>" + EditHtml + "</td><td>" + State + "</td><td>" + row["StartTime"] + "</td><td>" + row["SortId"] + "</td><td>" + DelHtml + "</td></tr>";
            }
            return HtmlStr;
        }

        public string CardsHtml(DataTable dt)
        {
            string HtmlStr = "";
            Boolean IsEdit = rcm.GetRoleCompetence(GetUser().RoleId, 11251);
            Boolean IsDel = rcm.GetRoleCompetence(GetUser().RoleId, 11253);
            Boolean IsImport = rcm.GetRoleCompetence(GetUser().RoleId, 11254);
            Boolean IsCardLog = rcm.GetRoleCompetence(GetUser().RoleId, 11255);
            foreach (DataRow row in dt.Rows)
            {
                string EditHtml = IsEdit ? "<a href=\"EditCard?CardId=" + row["Id"] + "\">" + row["CardName"] + "</a>" : "<a>" + row["CardName"] + "</a>";
                string DelHtml = IsDel ? " <a href=\"javascript:InitPageContent('Card')\" onclick=\"Del('/Card/DelCard?CardId=" + row["Id"] + "')\">删除</a> " : " ";
                string ImportHtml = IsImport ? " <a href=\"/Card/ImportCard?CardId=" + row["Id"] + "\">导入卡号</a> " : " ";
                string CardLogHtml = IsCardLog ? " <a href=\"CardLog\">获取记录</a> " : " ";
                HtmlStr += "<tr><td>" + row["Id"] + "</td><td>" + EditHtml + "</td><td>" + row["GameName"] + "</td><td>" + row["CardCount"] + "</td><td>" + row["RemainCard"] + "</td><td>" + ImportHtml + CardLogHtml + DelHtml + "</td></tr>";
            }
            return HtmlStr;
        }

        public string OrderHtml(DataTable dt)
        {
            string HtmlStr = "";
            foreach (DataRow row in dt.Rows)
            {
                string Type = row["Type"].ToString() == "1" ? "游戏币" : "平台币";
                var State = "";
                switch ((int)row["State"])
                {
                    case 0:
                        State = "<span style=\"color:red\">未支付</span>";
                        break;
                    case 1:
                        State = "<span style=\"color:#F7654B\">已付款</span>";
                        break;
                    case 2:
                        State = "<span style=\"color:blue\">已完成</span>";
                        break;
                    default:
                        break;
                }
                HtmlStr += "<tr><td><a href=\"/Order/EditOrder?orderNo=" + row["OrderNo"] + "\">" + row["OrderNo"] + "</a></td><td>" + Type + "</td><td>" + row["PayTypeName"] + "</td><td>" + State + "</td><td>" + row["UserName"] + "</td><td>" + row["PayMoney"] + "</td><td>" + row["GameName"] + "-" + row["ServerName"] + "</td><td>" + row["PayTime"] + "</td><td>" + row["Bz"] + "</td><td>" + row["Source"] + "</td><td>" + row["AdminUserName"] + "</td></tr>";
                //HtmlStr += "<tr><td><a href=\"/Order/EditOrder?orderNo=" + row["OrderNo"] + "\">" + row["OrderNo"] + "</a></td><td>" + Type + "</td><td>" + row["PayTypeName"] + "</td><td>" + State + "</td><td>" + row["UserName"] + "</td><td>" + row["PayMoney"] + "</td><td>" + row["Source"] + "</td><td>" + row["PayTime"] + "</td><td>" + row["Bz"] + "</td><td>" + row["GameName"] + "-" + row["ServerName"] + "</td><td>" + row["AdminUserName"] + "</td></tr>";
            }
            return HtmlStr;
        }

        public string SourceChangeHtml(DataTable dt)
        {
            string HtmlStr = "";
            foreach (DataRow row in dt.Rows)
            {
                string DelHtml = GetUser().UserName == "odin33774006" ? "<td><a href=\"javascript:InitPageContent('SourceChange')\" onclick=\"Del('/Statistics/DelSourceChange?SCId=" + row["Id"] + "')\">删除</a> </td>" : "";
                HtmlStr += "<tr><td>" + row["Id"] + "</td><td>" + row["UserName"] + "</td><td>" + row["SourceName"] + "</td><td>" + row["SourceChangeName"] + "</td><td>" + row["DateChange"] + "</td><td>" + row["Operator"] + "</td>"+DelHtml+"</tr>";
            }
            return HtmlStr;
        }

        public string CollectHtml(DataTable dt, int MaxMoney, int MinMoney)
        {
            string HtmlStr = "";
            foreach (DataRow row in dt.Rows)
            {
                string PayMoney = row["PayMoney"].ToString().Trim();
                if (double.Parse(PayMoney) >= MaxMoney)
                {
                    PayMoney = "<span style=\"color: red\">" + PayMoney + "</span>";
                }
                else if (double.Parse(PayMoney) <= MinMoney)
                {
                    PayMoney = "<span style=\"color: green\">" + PayMoney + "</span>";
                }
                TimeSpan ts = DateTime.Now.Subtract((DateTime)row["StartTime"]);
                string tsStr = (ts.Days > 0 ? ts.Days + "天" : "") + (ts.Hours > 0 ? ts.Hours + "小时" : "") + (ts.Minutes > 0 ? ts.Minutes + "分钟" : "");
                HtmlStr += "<li><div class=\"box\" style=\"height:100px;\"><dl><dt><a>服务器名称：</a>" + row["GameName"] + "-" + row["ServerName"] + "</dt><dt><a>已开服时间：</a>" + tsStr + "</td><dt><a>充值金额：</a>" + PayMoney + "</dt></dl></div><div class=\"clear\"></div></li>";
            }
            return HtmlStr;
        }

        public string PayMoneyOrderHtml(DataTable dt,int gameId,int serverId)
        {
            string htmlStr = "";
            int i = 1;
            try
            {
                foreach (DataRow row in dt.Rows)
                {
                    string source = row["sourcename"].ToString() == "" ? "用户注册" : row["sourcename"].ToString();
                    GameUserInfo gui = gm.GetGameUserInfo(gameId, int.Parse(row["userid"].ToString()), serverId);
                    string userInfo = Utils.ConvertUnicodeStringToChinese(gui.UserName) + "[" + gui.Level + "级]";
                    //string userInfo = gui.UserName + "[" + gui.Level + "级]";
                    //htmlStr += "<tr><td>" + i++ + "</td><td>" + row["username"] + "</td><td>" + row["sumMoney"] + "</td><td>" + source + "</td></tr>";
                    htmlStr += "<tr><td>" + i++ + "</td><td>" + row["username"] + "</td><td>" + userInfo + "</td><td>" + row["sumMoney"] + "</td><td>" + source + "</td></tr>";
                }
            }
            catch (Exception)
            {
                htmlStr = "查询角色失败;";
            }
            return htmlStr;
        }

        public string PromoHtml(DataTable dt)
        {
            string HtmlStr = "";
            foreach (DataRow row in dt.Rows)
            {
                string Spereader = (int)row["Spereader"] == 1 ? "普通推广员" : "推广团长";
                HtmlStr += "<tr><td>" + row["Id"] + "</td><td><a>" + row["UserName"] + "</a></td><td>" + Spereader + "</td><td>" + row["Promo"] + "</td><td>" + om.GetSumMoney((int)row["Id"], "") + "</td><td><a href=\"PromoDetial?Id=" + row["Id"] + "\">推广详情</a></td></tr>";
            }
            return HtmlStr;
        }

        public string GameUserHtml(DataTable dt)
        {
            string HtmlStr = "";
            Boolean IsEdit = rcm.GetRoleCompetence(GetUser().RoleId, 1311);
            foreach (DataRow row in dt.Rows)
            {
                if (string.IsNullOrEmpty(row["Source"].ToString()))
                {
                    row["Source"] = "用户注册";
                }
                string EditHtml = IsEdit ? "<a href=\"/GameUser/EidtUser?Id=" + row["Id"] + "\">" + row["UserName"] + "</a>" : "<a>" + row["UserName"] + "</a>";
                HtmlStr += "<tr><td>" + row["Id"] + "</td><td>" + EditHtml + "</td><td>" + row["Money"] +
                    "</td><td>" + row["Points"] + "</td><td>" + row["AddTime"] + "</td><td>" + row["LastLoginTime"] +
                    "</td><td>" + row["Ip"] + "</td><td>" + row["Source"] + "</td></tr>";
            }
            return HtmlStr;
        }

        public string ServersHtml(List<GameServer> gsList)
        {
            string HtmlStr = "";
            foreach (GameServer gs in gsList)
            {
                HtmlStr += "<option value=\"" + gs.QuFu + "\">" + gs.Name + "</option>";
            }
            return HtmlStr;
        }

        public string YqLinkHtml(string Top)
        {
            string YqLinks = "";
            List<link> YqList = new List<link>();
            if (Top == "All")
            {
                YqList = lm.GetAllLinks();
            }
            else
            {
                YqList = lm.GetAllLinks(int.Parse(Top));
            }
            foreach (link yq in YqList)
            {
                //string HtmlImg = yq.Is_image == 1 ? "<img src='" + yq.Img_url + "' style='width:50px;height:20;'>" : yq.Title;
                //YqLinks += " <a class=\"spanlink2\" href=\"" + yq.Site_url + "\" target=\"_blank\">" + HtmlImg + "</a> ";
                YqLinks += "<li><a href=\"" + yq.Site_url + "\"target=\"_blank\">"+ yq.Title + "</a></li>";
            }
            return YqLinks;
        }

        public string HotGameHtml(List<Games> Hotlist)
        {
            string HtmlStr = "";
            foreach (Games g in Hotlist)
            {
                HtmlStr += "<div class=\"hot_gm\"><a href=\"/" + g.GameNo + "/Servers\" target=\"_blank\"><img src=\"" + g.IndexHdImg + "\" title=\"点击进入游戏\"></a></div>";
            }
            return HtmlStr;
        }

        public string TjGameHtml(List<Games> GameList)
        {
            string HtmlStr = "";
            ServersMananger sm = new ServersMananger();
            foreach (Games g in GameList)
            {
                //List<GameServer> listServer = sm.GetServersByGame(g.Id, 3);
                //listServer.ElementAt(0);
                string serverName = sm.GetGameServer(g.tjqf).Name;
                string qufu = sm.GetGameServer(g.tjqf).QuFu;
                string p = g.GameDesc;
                string type = g.GameProperty .Split('|')[0];
                string descript = p.Length < 50 ? p : p.Substring(0,50);
                //HtmlStr += "<div class=\"hot_gm\"><a href=\"/" + g.GameNo + "/Servers\" target=\"_blank\"><img src=\"" + g.IndexHdImg + "\" width=\"230\" height=\"148\" title=\"点击进入游戏\"></a></div>";
                HtmlStr += "<li><img src=\"" + g.IndexHdImg + "\"style=\"width:210px;height:270px;\"/><div class=\"into\" style=\"display: block; bottom: -252px;\"><div class=\"into_wrap\"><div class=\"t\"><span class=\"nm\">" + g.Name + "</span><span class=\"rol\">" + type + "</span></div><p><a href=\"/" + g.GameNo + "\"target=\"_blank\">" + descript + "...</a></p><a href=\"/" + g.GameNo + "\" class=\"to\" target=\"_blank\">进入官网</a><div class=\"going\" id=\"JPYX_73_1\"><span class=\"sev\"><a class=\"lines\" href=\"/" + g.GameNo + "/LoginGame?S=" + qufu + "\" target=\"_blank\">" + serverName + "</a></span><a class=\"g\" href=\"/" + g.GameNo + "/LoginGame?S=" + qufu + "\" target=\"_blank\">go</a><span class=\"new\">new</span></div><div class=\"going\" id=\"JPYX_73_2\"><span class=\"sev\"><a class=\"lines\" href=\"/" + g.GameNo + "/LoginGame?S=1\" target=\"_blank\">双线1服</a></span><a class=\"g\" href=\"/" + g.GameNo + "/LoginGame?S=1\" target=\"_blank\">go</a><span class=\"ht\">hot</span></div></div><div class=\"into_bg\"></div></li>";
            }
            return HtmlStr;
        }

        public string IndexNewsHtml(List<News> Newslist)
        {
            string HtmlStr = "";
            foreach (News n in Newslist)
            {
                HtmlStr += "<div class=\"sygonggao_list\"><a href=\"/NewsCenter/News?N=" + n.Id + "\" style=\"color:" + n.NameColor + "\" target=\"_blank\" title=\"" + n.Title + "\">" + (n.Title.Length < 15 ? n.Title : n.Title.Substring(0, 15)) + "</a></div>";
            }
            return HtmlStr;
        }

        public string GameHtml2(List<Games> list)
        {
            string HtmlStr = "";
            foreach (Games g in list)
            {
                //HtmlStr += "<dl><dt><span class=\"index_youxi_list_span_img\" onclick=\"window.open('/" + g.GameNo + "/Servers')\"><img src=\"" + g.IndexTjImg + "\" width=\"118\" height=\"86\" title=\"点击进入游戏\"></span></dt><dd class=\"first\"><p class=\"t\">" + "<a href=\"/" + g.GameNo + "/Servers\" target=\"_blank\">" + g.Name + "</a></p><p>类别：" + g.GameProperty.Split('|')[2] + "</p><p>状态：推荐</p></dd><dd class=\"second\"><span class=\"index_span_btn3\"><a href=\"/" + g.GameNo + "/\" target=\"_blank\">进入官网</a></span>" + "</dd></dl>";
                string tjqf = "#";
                if (g.tjqf > 0)
                {
                    GameServer gsTjqf = new ServersMananger().GetGameServer(g.tjqf);
                    tjqf = gsTjqf.QuFu;
                }
                HtmlStr += "<li><div class=\"posb\"><a href=\"/" + g.GameNo + "\"target=\"_blank\" class=\"img\"><img src=\"" + g.IndexTjImg + "\"></a><div class=\"det\"><strong>" + g.Name + "</strong><span>状态：推荐</span><a href=\"/" + g.GameNo + "/\" class=\"web\" target=\"_blank\">进入官网</a><div class=\"btn_box\"><a class=\"gnew\" href=\"/" + g.GameNo + "/LoginGame?S=" + tjqf + "\"target=\"_blank\">进入新区</a><button class=\"gmore\"></button></div></div></div></li>";
            }
            return HtmlStr;
        }

        public Master GetUser()
        {
            HttpSessionState Session = HttpContext.Current.Session;
            HttpResponse Response = HttpContext.Current.Response;
            Master master = new Master();
            try
            {
                master = Session[Keys.SESSION_ADMIN_INFO] as Master;
            }
            catch (Exception)
            {
                Response.Write("<script>location.href='/Admin/Login';</script>");
            }
            return master;
        }

        public string AllGameHtml()
        {
            List<Games> list = new List<Games>();
            list = gm.GetAll();
            string HtmlStr = "";
            foreach (Games g in list)
            {
                HtmlStr += "<option value=\"" + g.Id + "\">" + g.Name + "</option>";
            }
            return HtmlStr;
        }

        public string GetNewServerHtml(List<GameServer> NewServerList)
        {

            //string Style = "style=\"background-color: #FFF4E1\"";
            //foreach (GameServer gs in NewServerList)
            //{
            //    Games g = new Games();
            //    g = gm.GetGame(gs.GameId);
            //    HtmlStr += "<tr><td " + Style + ">&nbsp;</td><td " + Style + ">" + gs.StartTime.ToString("MM-dd") + "</td><td " + Style + ">" + gs.StartTime.ToString("HH:mm") + "</td><td class=\"name\" " + Style + "><a href=\"/" + g.GameNo + "/\" class=\"a2\" target=\"_blank\" title=\"" + gs.Name + "\">" + g.Name + "</a></td><td class=\"qu\" " + Style + "><span style=\"width: 50px; display: block; float: right; text-align: center; line-height: 15px; font-size: 12px\"><a href=\"/" + g.GameNo + "/LoginGame?S=" + gs.QuFu + "\" class=\"a1\"target=\"_blank\">" + gs.QuFu + "区</a></span></td></tr>";
            //}
            string HtmlStr = "";
            int i = 1;
            string c = "";
            foreach (GameServer gs in NewServerList)
            {
                if (i == 1) c = "class=\"num1\"";
                else if (i == 2) c = "class=\"num2\"";
                else if (i == 3) c = "class=\"num3\"";
                else c = "";
                Games g = new Games();
                g = gm.GetGame(gs.GameId);
                HtmlStr += "<li><i  "+ c +">" + i++ + "</i><span class=\"cl1\">" + gs.StartTime.ToString("MM-dd") + " "+ gs.StartTime.ToString("HH") + "时</span><a href=\"/" + g.GameNo + "/LoginGame?S=" + gs.QuFu + "\" target=\"_blank\"><span>"+ gs.Name + "</span>" + g.Name + "</a></li>";
            }
            return HtmlStr;
        }
    }
}
