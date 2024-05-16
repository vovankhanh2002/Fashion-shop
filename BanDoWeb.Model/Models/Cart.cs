using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanDoWeb.Model.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public Product product { get; set; }
    }
    public class CartVM
    {
        List<Cart> carts = new List<Cart>();
        public IEnumerable<Cart> cartsItem()
        {
            return carts;
        }
        public void AddToCart(Product product, int Quanlity = 1)
        {
            Cart cart = new Cart();
            cart.Id = product.Id;
            cart.product = product;
            carts.Add(cart);
        }
        public void DeleteToCart(int id)
        {
            carts.Remove(carts.SingleOrDefault(i => i.Id == id));
        }
        public void DeleteAllCart()
        {
            carts.Clear();
        }
    }
}
