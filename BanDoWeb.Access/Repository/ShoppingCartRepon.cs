using BanDoWeb.Access.Dbcontext;
using BanDoWeb.Access.Repository.IRepository;
using BanDoWeb.Model.Models;
using Project.DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanDoWeb.Access.Repository
{
    public class ShoppingCartRepon : Repository<ShoppingCart>, IShoppingCart
    {
        private DbcontextBanDo _dbcon;
        public ShoppingCartRepon(DbcontextBanDo dbcontext) : base(dbcontext)
        {
            _dbcon = dbcontext;

        }

        public string AddColor(ShoppingCart shoppingCart, List<string> lstColor)
        {
            shoppingCart.Color +="," + lstColor[0];
            return shoppingCart.Color;
        }

        public string AddSize(ShoppingCart shoppingCart, List<string> lstSize)
        {
            shoppingCart.Size += "," + lstSize[0];
            return shoppingCart.Size;
        }

        public int DesCout(ShoppingCart shoppingCart, int quantity)
        {
            shoppingCart.Count -= quantity;
            return shoppingCart.Count;
        }

        public int InCount(ShoppingCart shoppingCart, int quantity)
        {
            shoppingCart.Count += quantity;
            return shoppingCart.Count;
        }

        public void updateShoppingCart(ShoppingCart shoppingCart)
        {
            _dbcon.Update(shoppingCart);
        }
    }
}
