using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEB_ASP.Models;

namespace WEB_ASP.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class HomeAdminController : Controller
    {
        DB_Entities db = new DB_Entities(); // khai báo đối tượng database toàn cục
        public void saveImg(Product model, HttpPostedFileBase Img, string imgOfCol) // hàm dùng để lưu ảnh vào folder và lưu URL vào DB 
        {
            if (ModelState.IsValid && Img != null && Img.ContentLength > 0)
            {
                var fileName = Path.GetFileName(Img.FileName);
                var fileNameExtension = Path.GetExtension(Img.FileName); // lấy đuôi mở rộng
                if (fileNameExtension != ".jpg" || fileNameExtension != ".png")
                {
                    fileName = Path.ChangeExtension(fileName, ".jpg, .png"); // đổi đuôi webp thành jpg  và png
                }
                // tạo 1 fole upload để quản lý sản phẩm thêm vào
                var filePath = Path.Combine(Server.MapPath("~/Upload/"), fileName);
                Img.SaveAs(filePath);

                //lưu ảnh vào folder asset 
                var filePath_Asset = Path.Combine(Server.MapPath("~/asset/img/Product/"), fileName);
                Img.SaveAs(filePath_Asset);
                if (imgOfCol == "Image")
                {
                    model.Image = "asset/img/Product/" + fileName;
                }

                if (imgOfCol == "Image_description")
                {
                    model.Image_description = "asset/img/Product/" + fileName;
                }

                if (imgOfCol == "Image_description2")
                {
                    model.Image_description2 = "asset/img/Product/" + fileName;
                }
            }
        }


        [AdminAuthorize]
        public ActionResult Index(string actionProduct, string message, int? type, string search, int page = 1)
        {
            ViewBag.actionProduct = actionProduct; // toast message
            ViewBag.message = message; // toast message

            List<Product> danhSachSanPham = db.Products.OrderBy(m => m.IdCategory).Where(m => m.IdCategory == type || type == null).ToList();

            // Tìm kiếm
            var checkSearch = String.IsNullOrEmpty(search);
            if (!checkSearch)
            {
                danhSachSanPham = db.Products.Where(m => m.Name_product.ToLower().Contains(search.ToLower())).OrderBy(m => m.IdCategory).ToList();
            }

            ViewBag.search = search;
            ViewBag.type = type;

            // phân trang
            const int PageSize = 10;
            var count = danhSachSanPham.Count();
            var data = danhSachSanPham.Skip((page - 1) * PageSize).Take(PageSize).ToList();
            ViewBag.MaxPage = (count / PageSize) - (count % PageSize == 0 ? 1 : 0) + 1;
            ViewBag.Page = page;

            return View(data);
        }

        [Route("Err/{statusCode}")]
        public ActionResult Err(int statusCode)
        {
            return View(nameof(Err), statusCode);
        }
    }
}