using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanDoWeb.Model.Models
{
    public class Categoriess
    {
        public IEnumerable<Categories> Categorias { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
