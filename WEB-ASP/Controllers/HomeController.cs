using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WEB_ASP.Models;

namespace WEB_ASP.Controllers
{
    public class HomeController : Controller
    {
        DB_Entities _db = new DB_Entities();
        public List<Product> filter(int caterogy, string type, int? filterSelect, int page) // hàm dùng để hiển thị sản phẩm, filter và phân trang
        {
            List<Product> danhSachSanPham = _db.Products.Where(m => m.IdCategory == caterogy && (m.Name_product.ToLower().Contains(type) || type == null)).ToList();

            if (filterSelect == 1) // giá từ thấp đến cao
            {
                danhSachSanPham = _db.Products.Where(m => m.IdCategory == caterogy && (m.Name_product.ToLower().Contains(type) || type == null)).OrderByDescending(m => m.Price).ToList();
            }
            else if (filterSelect == 2) // giá từ cao đến thấp
            {
                danhSachSanPham = _db.Products.Where(m => m.IdCategory == caterogy && (m.Name_product.ToLower().Contains(type) || type == null)).OrderBy(m => m.Price).ToList();
            }

            ViewBag.filterSelect = filterSelect;
            ViewBag.type = type;

            // phân trang
            const int PageSize = 8;
            var count = danhSachSanPham.Count();
            var data = danhSachSanPham.Skip((page - 1) * PageSize).Take(PageSize).ToList();
            ViewBag.MaxPage = (count / PageSize) - (count % PageSize == 0 ? 1 : 0) + 1;
            ViewBag.Page = page;

            return (data);
        }

        public ActionResult Index()
        {
            var ProductList = _db.Products.ToList();

            return View(ProductList);
        }

        public ActionResult Iphone(string type, int? filterSelect, int page = 1)
        {
            return View(filter(1, type, filterSelect, page));
        }

        public ActionResult Ipad(string type, int? filterSelect, int page = 1)
        {
            return View(filter(2, type, filterSelect, page));
        }

        public ActionResult Mac(string type, int? filterSelect, int page = 1)
        {
            return View(filter(3, type, filterSelect, page));
        }

        public ActionResult Watch(string type, int? filterSelect, int page = 1)
        {
            return View(filter(4, type, filterSelect, page));
        }

        public ActionResult Sound(string type, int? filterSelect, int page = 1)
        {
            return View(filter(5, type, filterSelect, page));
        }

        public ActionResult Search(string search, int page = 1)
        {
            ViewBag.search = search;
            List<Product> danhSachSanPham = _db.Products.Where(m => m.Name_product.ToLower().Contains(search.ToLower())).ToList();

            // phân trang
            const int PageSize = 8;
            var count = danhSachSanPham.Count();
            var data = danhSachSanPham.Skip((page - 1) * PageSize).Take(PageSize).ToList();
            ViewBag.MaxPage = (count / PageSize) - (count % PageSize == 0 ? 1 : 0) + 1;
            ViewBag.Page = page;

            return View(data);
        }
        public ActionResult Detail(int id)
        {
            var product = _db.Products.Find(id);
            switch (product.IdCategory)
            {
                case 1:
                    ViewBag.type = "Iphone";
                    break;
                case 2:
                    ViewBag.type = "Ipad";
                    break;
                case 3:
                    ViewBag.type = "Mac";
                    break;
                case 4:
                    ViewBag.type = "Watch";
                    break;
                case 5:
                    ViewBag.type = "Sound";
                    break;
            }
            return View(product);
        }

    }
}