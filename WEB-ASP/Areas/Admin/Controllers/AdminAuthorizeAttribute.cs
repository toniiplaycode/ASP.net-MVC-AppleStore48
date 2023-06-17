using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;   
using System.Web.Routing;

namespace WEB_ASP.Areas.Admin.Controllers
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            // Kiểm tra xem người dùng đã đăng nhập hay chưa
            if (httpContext.Session["EmailAdmin"] != null)
            {
                // Người dùng chưa đăng nhập, không có quyền truy cập
                return true;
            }
            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            // Chuyển hướng người dùng đến trang đăng nhập
            if(filterContext.HttpContext.Session["EmailAdmin"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "AccountAdmin", action = "Login" }));
            }
            else
            {

            }
        }
    }
}