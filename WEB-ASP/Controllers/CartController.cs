using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WEB_ASP.Models;

namespace WEB_ASP.Controllers
{
    public class CartController : Controller
    {
        DB_Entities _db = new DB_Entities();

        // GET: ShoppingCart
        public CartModel GetCart()
        {
            CartModel cart = Session["CartModel"] as CartModel;
            if (cart == null || Session["CartModel"] == null)
            {
                cart = new CartModel();
                Session["CartModel"] = cart;
            }
            return cart;
        }

        //phuong thuc them vao gio hang

        [HttpPost]
        public ActionResult AddToCart(FormCollection _product)
        {
            int id = int.Parse(_product["IdProduct"]);
            int quantity = int.Parse(_product["qty"]);

            var product = _db.Products.SingleOrDefault(s => s.IdProduct == id);
            if (product != null)
            {
                GetCart().Add(product, quantity);
            }
            return RedirectToAction("ShowToCart", "Cart");
        }
        //trang gio hang
        public ActionResult ShowToCart()
        {
            CartModel cart = Session["CartModel"] as CartModel;
            return View(cart);
        }


        public ActionResult RemoveCart(int id)
        {
            CartModel cart = Session["CartModel"] as CartModel;
            cart.Remove_CartItem(id);
            return RedirectToAction("ShowToCart", "Cart");
        }

        public PartialViewResult BagCart()
        {
            int _sum_item = 0;
            CartModel cart = Session["CartModel"] as CartModel;
            if (cart != null)
            {
                _sum_item = cart.Total_Quantity();
            }
            ViewBag.Sum_Quantity = _sum_item;

            return PartialView("BagCart");
        }

        public ActionResult Shopping_Success()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckOut(FormCollection form)
        {
            try
            {
                string city = form["city"];
                string district = form["district"];
                string commune = form["commune"];

                CartModel cart = Session["cartmodel"] as CartModel;
                Order _order = new Order();

                if (!string.IsNullOrEmpty(form["NameCus"]) && !string.IsNullOrEmpty(form["phone"]) && !string.IsNullOrEmpty(form["email"]))
                {
                    _order.NameCus = form["NameCus"];
                    _order.Phone = form["phone"];
                    _order.Email = form["email"];
                }
                else
                {
                    // Thông báo cho người dùng về việc thiếu thông tin khách hàng
                    ViewBag.error_null_address = "vui lòng nhập đày đủ thông tin";
                    return View();
                }

                string address = city + ", " + district + ", " + commune;
                _order.Address_Delivery = address;
                // lưu trạng thái đơn hàng là mới đặt
                _order.Idstatus = 1;
                _order.OrderDate = DateTime.Now;
                // lưu id khách hàng = với id tài khoản đăng nhập
                if (Session["id"] != null)
                {
                    _order.IdCustomer = int.Parse(Session["id"].ToString());
                }

                // luu lai tong tien cua don hang
                _order.Total_Money = Convert.ToDouble(cart.Total_Money());


                List<Order_Details> chiTietDonDatHangList = new List<Order_Details>();
                foreach (var item in cart.Items)
                {
                    Order_Details _order_details = new Order_Details();
                    _order_details.IdOrder = _order.IdOrder;
                    _order_details.IdProduct = item._shopping_product.IdProduct;
                    _order_details.UntiPriceSale = item._shopping_product.Price;
                    _order_details.QuantitySale = item._cart_quantity;

                    chiTietDonDatHangList.Add(_order_details);
                }
                _order.Order_Details = chiTietDonDatHangList;
                // chay add thu di
                _db.Orders.Add(_order);
                _db.SaveChanges();
                cart.ClearCart();
                return RedirectToAction("Shopping_Success", "Cart");
            }

            catch (Exception ex)
            {
                return Content("Error Checkout. Please information of Customer....");
            }
        }

        public JsonResult Update(string CartModels)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(CartModels);
            CartModel cart = Session["CartModel"] as CartModel;

            foreach (var item in cart.Items)
            {
                var jsonItem = jsonCart.SingleOrDefault(x => x._shopping_product.IdProduct == item._shopping_product.IdProduct);
                if (jsonItem != null)
                {
                    cart.update(jsonItem._shopping_product.IdProduct, jsonItem._cart_quantity);
                }
            }
            Session["CartModel"] = cart;
            return Json(new
            {
                status = true

            });

        }

    }
}