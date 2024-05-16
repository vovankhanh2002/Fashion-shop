using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanDoWeb.Model.Models
{
    public class Navbar
    {
        public int Id { get; set; }
        public string? TitleNavBar { get; set; }
        public string? UrlNavBar { get; set; }
        public DateTime? DateSetNavbar { get; set; }
        public DateTime? DateOutNavbar { get; set; }
    }
}
