using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WEB_ASP.Models
{
    [Serializable]
    public class CartItem
    {
        public Product _shopping_product { get; set; }
        public int _cart_quantity { get; set; }
    }

    //#region hang

    public class CartModel
    {
        List<CartItem> items = new List<CartItem>();
        public IEnumerable<CartItem> Items
        {
            get { return items; }
        }

        public void Add(Product _product, int _quantity)
        {
            var item = items.FirstOrDefault(s => s._shopping_product.IdProduct == _product.IdProduct);
            if(item == null)
            {
                items.Add(new CartItem
                {
                    _shopping_product = _product,
                    _cart_quantity = _quantity
                });
            }
            else
            {
                item._cart_quantity += _quantity;    
            }
        }

        public void update(int id, int _quantity)
        {
            var item = items.Find(s => s._shopping_product.IdProduct == id);
            if (item != null)
            {
                item._cart_quantity = _quantity;
            }
        }

        public double Total_Money()
        {
            var total = items.Sum(s => s._shopping_product.Price * s._cart_quantity);
            return total;
        }

        public void Remove_CartItem(int id)
        {
            items.RemoveAll(s => s._shopping_product.IdProduct == id);
        }

        // tong so san pham trong gio hang
        public int Total_Quantity()
        {
            return items.Sum(s => s._cart_quantity);
        }
        public void ClearCart()
        {
            items.Clear(); // xoa toan bo gio hang khi order
        }
    }
}            