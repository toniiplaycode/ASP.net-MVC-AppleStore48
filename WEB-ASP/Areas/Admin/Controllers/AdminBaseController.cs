using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace WEB_ASP.Areas.Admin.Controllers
{
    public class AdminBaseController : Controller
    {
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            string nameAdmin = Session["nameAdmin"].ToString();
            if (nameAdmin.ToLower() != "manager")
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { 
                    controller= "HomeAdmin",
                    action="Err",
                    statusCode="403"
                }));
            }
        }

    }
}