using System.Linq;
using System.Web.Mvc;
using WEB_ASP.Models;

namespace WEB_ASP.Controllers
{
    public class check
    {
        public int idstatus { get; set; }
        public int idorder { get; set; }
        public double total_money { get; set; }
    }
    public class order_detail
    {
        public int Id { get; set; }
        public int IdOrder { get; set; }
        public int IdProduct { get; set; }
        public double UntiPriceSale { get; set; }
        public int QuantitySale { get; set; }
        public int idstatus { get; set; }
        public double total_money { get; set; }
        public string img { get; set; }
        public string name { get; set; }
        public string category { get; set; }


    }
    public class InfoCustomerController : Controller
    {
        DB_Entities _db = new DB_Entities();
        // GET: InfoCustomer
        public ActionResult Index()
        {
            var id = Session["id"] as WEB_ASP.Models.Account;
            var account = _db.Accounts.Find(id);
            return View(account);
        }
        public ActionResult InfoCus()
        {
            return View();
        }
        [HttpPost]
        public ActionResult EditInfoCustomr(FormCollection _form)
        {
            int IdCustomer = int.Parse(Session["id"].ToString());
            Account _use = _db.Accounts.Find(IdCustomer);

            _use.FullName = _form["full_name"];
            _use.User_Name = _form["username"];
            _use.Phone = _form["phone"];

            _db.SaveChanges();
            Session["UserName"] = _form["username"];

            return RedirectToAction("infoCus", "infoCustomer");
        }

        public ActionResult OrderCustomer()
        {
            var IdCustomer = int.Parse(Session["id"].ToString());
            using (var _db = new DB_Entities())
            {
                var result = (from dh in _db.Orders
                              join ctdh in _db.Order_Details on dh.IdOrder equals ctdh.IdOrder
                              where dh.IdCustomer == IdCustomer
                              orderby dh.Idstatus
                              select new order_detail
                              {
                                  Id = ctdh.Id,
                                  IdProduct = ctdh.IdProduct,
                                  IdOrder = ctdh.IdOrder,
                                  UntiPriceSale = ctdh.UntiPriceSale,
                                  QuantitySale = ctdh.QuantitySale,
                                  idstatus = dh.Idstatus,
                                  total_money = dh.Total_Money,
                                  img = ctdh.Product.Image,
                                  name = ctdh.Product.Name_product,
                                  category = ctdh.Product.Category.NameCaregory
                              }).ToList();

                var result1 = (from dh in _db.Orders
                               join ctdh in _db.Order_Details on dh.IdOrder equals ctdh.IdOrder
                               where dh.IdCustomer == IdCustomer
                               orderby dh.Idstatus
                               select new check
                               {
                                   idorder = dh.IdOrder,
                                   idstatus = dh.Idstatus,
                                   total_money = dh.Total_Money
                               }).ToList();

                result = result.OrderBy(o => o.IdOrder).ToList();
                ViewBag.list = result;
                ViewBag.listcheck = result1;
                return View(result);
            }

        }
        public ActionResult CancalOrder(int id)  // huy don hang
        {
            var updateStatus = _db.Orders.Find(id);
            updateStatus.Idstatus = 5;

            _db.SaveChanges();
            return RedirectToAction("OrderCustomer", "InfoCustomer");
        }
    }
}