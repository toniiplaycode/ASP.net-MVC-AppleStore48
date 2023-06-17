using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using WEB_ASP.Models;

namespace WEB_ASP.Controllers
{
    public class AccountController : Controller
    {
        private DB_Entities _db = new DB_Entities();

        //GET: Register
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        //POST: Register
        [HttpPost]
        public ActionResult Register(Models.AccountModel _User)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // check user_name 
                    var check = _db.Accounts.FirstOrDefault(s => s.User_Name == _User.User_name);
                    if (check == null)
                    {
                        _User.Password = GetMD5(_User.Password);
                        _db.Configuration.ValidateOnSaveEnabled = false;

                        var account = new Account();
                        account.FullName = _User.Fullname_user;
                        account.Phone = _User.Phone;
                        account.Password = _User.Password;
                        account.User_Name = _User.User_name;
                        _db.Accounts.Add(account);
                        _db.SaveChanges();
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        ViewBag.error = "User name already exists";
                        return View();
                    }
                }
            }

            catch (Exception e)
            {
                Console.WriteLine("Something went wrong.");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Login(string UserName, string password)
        {
            // UserName : ng dùng nhập
            if (ModelState.IsValid)
            {
                var f_password = GetMD5(password);
                var check_pass = _db.Accounts.Where(s => s.User_Name.Equals(UserName)).ToList();
                if (check_pass.Count() == 0)
                {
                    ViewBag.error = "Tài khoản không tồn tại.";
                    return View();
                }

                var data = _db.Accounts.Where(s => s.User_Name.Equals(UserName) && s.Password.Equals(f_password)).ToList();
                if (data.Count() > 0)
                {
                    //add session
                    Session["UserName"] = data.FirstOrDefault().User_Name;
                    Session["id"] = data.FirstOrDefault().Id;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "Thông tin Tài khoản hoặc Mật khảu không chính xác.";
                    return View();
                }
            }
            return View();
        }

        //Logout

        public ActionResult Logout()
        {
            Session.Clear();//remove session
            return RedirectToAction("Index", "Home");
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
