using System.IO;
using System.Web;
using System.Web.Mvc;
using WEB_ASP.Models;

namespace WEB_ASP.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class SecureHomeController : AdminBaseController
    {
        DB_Entities db = new DB_Entities();
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
        public ActionResult AddProduct()
        {
            return View();
        }

        [HttpPost]
        [AdminAuthorize]
        public ActionResult AddProduct(Product model, HttpPostedFileBase Image, HttpPostedFileBase Image_description, HttpPostedFileBase Image_description2)
        {
            saveImg(model, Image, "Image");
            saveImg(model, Image_description, "Image_description");
            saveImg(model, Image_description2, "Image_description2");

            db.Products.Add(model);
            db.SaveChanges();

            return RedirectToAction("Index", "HomeAdmin", new { actionProduct = model.Image, message = "Đã thêm mới 1 sản phẩm" });
        }

        [AdminAuthorize]
        public ActionResult EditProduct(int id)
        {
            var model = db.Products.Find(id);
            return View(model);
        }
        [AdminAuthorize]
        [HttpPost]
        public ActionResult EditProduct(Product model, HttpPostedFileBase Image, HttpPostedFileBase Image_description, HttpPostedFileBase Image_description2)
        {
            var updateModel = db.Products.Find(model.IdProduct); // tìm đối tượng theo id

            // gán giá trị mới cho updateModel
            updateModel.Name_product = model.Name_product;
            updateModel.Price = model.Price;
            updateModel.Discription = model.Discription;
            updateModel.Discription2 = model.Discription2;
            updateModel.Discount = model.Discount;
            updateModel.IdCategory = model.IdCategory;

            // nếu có ảnh mới, ảnh mô tả mới, ảnh mô tả 2 mới thì lưu ảnh mới folder và URL mới vào DB
            if (Image != null)
            {
                saveImg(updateModel, Image, "Image");
            }

            if (Image_description != null)
            {
                saveImg(updateModel, Image_description, "Image_description");
            }

            if (Image_description2 != null)
            {
                saveImg(updateModel, Image_description2, "Image_description2");
            }

            db.SaveChanges(); // lưu lại thay đổi

            return RedirectToAction("Index", "HomeAdmin", new { actionProduct = updateModel.Image, message = "Đã sửa 1 sản phẩm" });
        }

        public ActionResult DeleteProduct(int id)
        {
            var deleteModel = db.Products.Find(id); // tìm đối tượng theo id
            db.Products.Remove(deleteModel); // lệnh xoá đối tượng
            db.SaveChanges(); // lưu lại thay đổi

            return RedirectToAction("Index", "HomeAdmin", new { actionProduct = deleteModel.Image, message = "Đã xoá 1 sản phẩm" });
        }

    }
}