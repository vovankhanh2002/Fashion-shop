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
    public class OderDetail
    {
        public int Id { get; set; }
        [Required]
        public int OderHeaderId { get; set; }
        [ForeignKey("OderHeaderId")]
        [ValidateNever]
        public OderHeader OderHeaders { get; set; }

        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }

        public int Count { get; set; }
        public double Price { get; set; }
        public string Tittle { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
    }
}
