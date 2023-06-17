using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEB_ASP.Models;
using System.Security.Cryptography;
using System.Text;

namespace WEB_ASP.Areas.Admin.Controllers
{
    public class AccountAdminController : Controller
    {
        DB_Entities _db = new DB_Entities();
        // GET: Admin/AccountAdmin
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Login(string Email, string password)
        {
            if (ModelState.IsValid)
            {
                var check_Email = _db.Account_Admin.Where(x => x.Email.Equals(Email)).ToList();
                if (check_Email.Count() == 0)
                {
                    ViewBag.error_email_null = "Email không tồn tại.";
                    return View();
                }
                var data = _db.Account_Admin.Where(s => s.Email.Equals(Email) && s.password.Equals(password)).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["EmailAdmin"] = data.FirstOrDefault().Email;
                    Session["nameAdmin"] = data.FirstOrDefault().UserName;
                    return RedirectToAction("Index", "HomeAdmin");
                }
                else
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng.");
                    ViewBag.error_pass = "Thông tin tài khoản và mật khẩu không chính xác.";
                    return View();
                }
            }
            return View();
        }
        
        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "AccountAdmin");
        }

        //create a string MD5 -ma hao 
        public static string GetMD5(string str)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] fromData = Encoding.UTF8.GetBytes(str);
            byte[] targetData = md5.ComputeHash(fromData);
            string byte2String = null;

            for (int i = 0; i < targetData.Length; i++)
            {
                byte2String += targetData[i].ToString("x2");
            }
            return byte2String;
        }
    }
}