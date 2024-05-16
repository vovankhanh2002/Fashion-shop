using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanDoWeb.Model.Models
{
    public class ShoppingCartVM
    {
        public IEnumerable<ShoppingCart> LstshoppingCarts { get; set; }
        public int CartTotal { get; set; }
        public OderHeader OrderHeader { get; set; }
    }
}
