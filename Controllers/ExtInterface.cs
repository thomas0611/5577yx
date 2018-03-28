using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Game.Manager;
using Game.Model;
using Common;

namespace Game.Controllers
{
    public class ExtInterfaceController : Controller
    {
        OnlineLogManager olm = new OnlineLogManager();
        GameUserManager gum = new GameUserManager();
        GamesManager gm = new GamesManager();
        ServersMananger sm = new ServersMananger();

        public string BengbengSel()
        {
            string UserName = Request["idCode"];
            string GameNo = Request["GameNo"];
            string Code = Request["Code"];
            if (Code == DESEncrypt.Md5(UserName + GameNo + "06bd24c6124b2dd7", 32))
            {
                try
                {
                    GameUser gu = gum.GetGameUser(UserName);
                    Games game = gm.GetGame(GameNo);
                    List<string> list = olm.GetServerList(game.Id, gu.Id.ToString());
                    GameUserInfo gui = new GameUserInfo();
                    GameServer gs = new GameServer();
                    foreach (string server in list)
                    {
                        GameUserInfo gui2 = gm.GetGameUserInfo(game.Id, gu.Id, int.Parse(server));
                        if (gui2.Level > gui.Level)
                        {
                            gui = gui2;
                            gs = sm.GetGameServer(int.Parse(server));
                        }
                    }
                    if (gui.Level > 0)
                    {
                        string Status = "0";
                        if (!string.IsNullOrEmpty(gu.annalID) && gu.From_Url == "BengBeng")
                        {
                            Status = "1";
                        }
                        string Res = "{\"Result\":{\"Status\":\"" + Status + "\",\"UserID\":\"" + gu.Id + "\",\"UserName\":\"" + gu.UserName + "\",\"UserServer\":\"" + gs.QuFu + "\",\"ServerName\":\"" + gui.ServerName + "\",\"UserRole\":\"" + gui.UserName + "\",\"UserLevel\":\"" + gui.Level + "\",\"ChongZhi\":\"" + gui.Money + "\"}}";
                        return Res;
                    }
                    else
                    {
                        return "没有等级大于0的角色！";
                    }
                }
                catch (Exception ex)
                {
                    return "查询异常：" + ex.Message;
                }
            }
            else
            {
                return "验证失败！";
            }
        }
    }
}
