using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanDoWeb.Model.Models
{
    public class Categories
    {
        public int Id { get; set; }
        public string NameCategori { get; set; }
        public string? ImageUrl { get; set; }
    }
}
