using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _5577yx.Models
{
    public class ExceptionFilter : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            base.OnException(filterContext);
            string message = filterContext.Exception.Message;
            LogHelper logHelper = new LogHelper();
            logHelper.WriteLog(this.ExceptionType, message);
            //HttpContext.Current.Response.Write(message);
            HttpContext.Current.Response.Redirect("/BBS");
        }
    }
}