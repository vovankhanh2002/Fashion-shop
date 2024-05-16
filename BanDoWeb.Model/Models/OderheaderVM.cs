using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanDoWeb.Model.Models
{
    public class OderheaderVM
    {
        public OderHeader oderHeader { get; set; }
        public IEnumerable<SelectListItem> OderheaderListItems { get; set; }
        public IEnumerable<OderDetail> oderDetails { get; set; }
    }
}
