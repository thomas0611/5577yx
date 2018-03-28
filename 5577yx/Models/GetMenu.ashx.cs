using Common;
using Game.Manager;
using Game.Model;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace _5577yx.Models
{
    /// <summary>
    /// GetMenu 的摘要说明
    /// </summary>
    public class GetMenu : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string action = context.Request["action"];
            switch (action)
            {
                case "GetRootMenu":
                    GetRootMenu(context);
                    break;
                case "GetCompetence":
                    GetCompetence(context);
                    break;
            }
        }

        public void GetRootMenu(HttpContext context)
        {
            List<Menu> list = new RoleCompetenceManager().GetAllMenu(2);
            string strTxt = Json.ListToJson(list);
            context.Response.Write(strTxt);
        }

        public void GetCompetence(HttpContext context)
        {
            List<Competence> List = new RoleCompetenceManager().GetAllCompetence();
            string StrTxt = Json.ListToJson(List);
            context.Response.Write(StrTxt);
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