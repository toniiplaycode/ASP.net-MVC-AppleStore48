using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using WEB_ASP.Models;


namespace WEB_ASP.Areas.Admin.Controllers
{
    public class InfoOrder_Detail
    {
        public string Name { get; set; }
        public string img { get; set; }
        public double price { get; set; }
        public int quantity { get; set; }
        public double Total_money { get; set; }
    }

    [AdminAuthorize]
    public class OrderAdminController : Controller
    {
        DB_Entities _db = new DB_Entities();
        // GET: Admin/Order
        [AdminAuthorize]
        public ActionResult Order(string actionOrder, string message, int? type, int page = 1)
        {
            ViewBag.actionOrder = actionOrder; // toast message
            ViewBag.message = message; // toast message

            List<Order> listOrder = _db.Orders.OrderByDescending(m => m.IdOrder).Where(m => m.IdOrder == type || type == null).ToList();

            ViewBag.type = type;
            // phân trang
            const int PageSize = 10;
            var count = listOrder.Count();
            var data = listOrder.Skip((page - 1) * PageSize).Take(PageSize).ToList();
            ViewBag.MaxPage = (count / PageSize) - (count % PageSize == 0 ? 1 : 0) + 1;
            ViewBag.Page = page;

            return View(data);
        }


        [AdminAuthorize]
        public ActionResult Order_Detail(int id)
        {
            var detail = _db.Order_Details.Where(d => d.IdOrder == id).ToList();
            ViewBag.Detail = detail;

            double total = 0;
            foreach (var item in detail)
            {
                //double? item.UntiPriceSale = double; 
                total += item.UntiPriceSale * item.QuantitySale;
            }
            ViewBag.Total = total;

            var order = _db.Orders.FirstOrDefault(d => d.IdOrder == id);
            return View(order);
        }
        public ActionResult DeleteOrder(int id)
        {
            List<Order_Details> _ListOderDetail = (from o in _db.Order_Details where o.IdOrder == id select o).ToList();
            foreach (var item in _ListOderDetail)
            {
                _db.Order_Details.Remove(item);
            }
            _db.SaveChanges();
            List<Order> _order = (from o in _db.Orders where o.IdOrder == id select o).ToList();

            _db.Orders.RemoveRange(_order);
            _db.SaveChanges();

            return RedirectToAction("Order", "OrderAdmin");
        }

        public ActionResult OrderBrowsing(int id) // duyet don hang
        {

            var updateStatus = _db.Orders.Find(id);
            updateStatus.Idstatus = 2;

            _db.SaveChanges();
            return RedirectToAction("Order", "OrderAdmin", new { actionOrder = id, message = "Bạn đã hủy đơn hàng" });
        }
    }

    [AdminAuthorize]
    public class SecureOrderController : AdminBaseController
    {
        DB_Entities _db = new DB_Entities();
        public ActionResult CancalOrder(int id)  // huy don hang
        {
            var updateStatus = _db.Orders.Find(id);
            updateStatus.Idstatus = 5;

            _db.SaveChanges();
            return RedirectToAction("Order", "OrderAdmin");
        }
    }
}