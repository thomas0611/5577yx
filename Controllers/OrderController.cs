using Common;
using Game.Manager;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Game.Controllers
{
    public class OrderController : Controller
    {
        //
        // GET: /Order/
        RoleCompetenceManager rcm = new RoleCompetenceManager();
        OrderManager om = new OrderManager();

        public ActionResult Index()
        {
            if (Session[Keys.SESSION_ADMIN_INFO] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                Master master = Session[Keys.SESSION_ADMIN_INFO] as Master;
                if (rcm.GetRoleCompetence(master.RoleId, 1131))
                {
                    return View();
                }
                else
                {
                    return RedirectToAction("Login", "Admin");
                }
            }
        }

        public ActionResult EditOrder()
        {
            string orderNo = Request["orderNo"];
            ViewData["id"] = om.GetOrder(orderNo).Id;
            ViewData["orderNo"] = orderNo;
            ViewData["state"] = om.GetOrder(orderNo).State;
            return View();
        }

        public Boolean DoEdit()
        { 
            int id = int.Parse(Request["id"]);
            int state = int.Parse(Request["state"]);
            string orderNo = Request["orderNo"];
            DataTable dt = om.GetComOrder(orderNo);
            DataRow dr = dt.Rows[0];
            try
            {
                string file = "~/upload/" + "201404011010110109539.txt";
                string Path = HttpContext.Server.MapPath(file);
                using (StreamWriter sw = System.IO.File.AppendText(Path))
                {
                    string OrderState = Convert.ToInt32(dr["State"]) == 2 ? "已完成" : "未支付";
                    string NewState = state == 2 ? "已完成" : "未支付";
                    string str = DateTime.Now.ToString() + "|" + dr["OrderNo"] + "|" + dr["PayTypeName"] + "|" + dr["PayMoney"] + "|" + dr["PayTime"] + "|" + OrderState + "->" + NewState + "|" + dr["UserName"] + "|" + dr["Source"] + "|" + dr["Bz"] + "|" + dr["GameName"] + "|" + dr["ServerName"];
                    sw.WriteLine(str);
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message) ;
            }
            return om.ChangeOrderState(id, state);
        }
    }
}
