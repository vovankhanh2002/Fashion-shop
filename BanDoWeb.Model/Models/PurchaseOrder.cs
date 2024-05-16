using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanDoWeb.Model.Models
{
    public class PurchaseOrder
    {
        public IEnumerable<OderDetail> oderDetails { get; set; }
        public IEnumerable<OderHeader> oderHeaders { get; set; }
    }
}
