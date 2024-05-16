using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanDoWeb.Model.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter the name")]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Please enter the price")]
        public double Price { get; set; }
        public int? Views { get; set; }
        public int? Warehouse { get; set; }
        public double? PriceTotal { get; set; }
        public double? OriginalPrice { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public int? Star { get; set; }
        public bool? Active { get; set; }
        
        public string? ImageUrl { get; set; }
        
        [Required(ErrorMessage = "Please enter the categoryID")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Categories Categories { get; set; }
    }
}
