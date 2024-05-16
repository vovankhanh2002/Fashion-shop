using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanDoWeb.Model.Models
{
    public class Reviews
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string MessageReview { get; set; }
        public int Star { get; set; }
        public DateTime DateTime { get; set; }
        public string ApplicationId { get; set; }
        [ForeignKey("ApplicationId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }
    }
}
