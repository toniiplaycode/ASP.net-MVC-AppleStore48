using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WEB_ASP.Models;

namespace WEB_ASP.Areas.Admin.Controllers
{
    public class Customer
    {
        public string fullname { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string address { get; set; }
    }

    [AdminAuthorize]
    public class CustomerController : AdminBaseController
    {
        DB_Entities _db = new DB_Entities();
        // GET: Admin/Customer
        public ActionResult Index(string actionProduct, string message, int page = 1)
        {
            ViewBag.actionProduct = actionProduct; // toast message
            ViewBag.message = message; // toast message

            //CustomerItem viewModel = new CustomerItem();
            List<Customer> accountOrders = new List<Customer>();
            using (DB_Entities _db = new DB_Entities())
            {
                accountOrders = (from account in _db.Accounts
                                 join order in _db.Orders on account.Id equals order.IdCustomer
                                 group new { account, order } by new { account.FullName, account.Phone, order.Email, order.Address_Delivery } into grouped
                                 select new Customer
                                 {
                                     fullname = grouped.Key.FullName,
                                     phone = grouped.Key.Phone,
                                     email = grouped.Key.Email,
                                     address = grouped.Key.Address_Delivery
                                 }).ToList();
            }

            const int PageSize = 10;
            var count = accountOrders.Count();
            var data = accountOrders.Skip((page - 1) * PageSize).Take(PageSize).ToList();
            ViewBag.MaxPage = (count / PageSize) - (count % PageSize == 0 ? 1 : 0) + 1;
            ViewBag.Page = page;

            ViewBag.list = data;

            return View();
        }
    }
}