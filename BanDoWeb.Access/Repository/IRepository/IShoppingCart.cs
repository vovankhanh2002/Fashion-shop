using BanDoWeb.Model.Models;
using Project.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanDoWeb.Access.Repository.IRepository
{
    public interface IShoppingCart : IRepository<ShoppingCart>
    {
        void updateShoppingCart(ShoppingCart shoppingCart);
        int InCount(ShoppingCart shoppingCart, int quantity);
        int DesCout(ShoppingCart shoppingCart, int quantity);
        string AddColor(ShoppingCart shoppingCart, List<string> lstColor);
        string AddSize(ShoppingCart shoppingCart, List<string> lstSize);
    }
}
