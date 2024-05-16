using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanDoWeb.Model.Models
{
    public class Dashboard
    {
        public int newMember { get; set; }
        public int Sales { get; set; }
        public int orderDetail { get; set; }
        public IEnumerable<OderHeader> oderHeaders { get; set; }
        public int applicationUsers { get; set; }
    }
}
