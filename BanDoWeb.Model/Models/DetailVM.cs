using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanDoWeb.Model.Models
{
    public class DetailVM
    {
        public Product Product { get; set; }
        public IEnumerable<Reviews> Reviews { get; set; }
        public IEnumerable<SlidedImage> SlidedImages { get; set; }
        public int Star { get; set; }

    }
}
