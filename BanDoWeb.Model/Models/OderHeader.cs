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
    public class OderHeader
    {
        public int Id { get; set; }
        public string ApplicationUserId { get; set; }
        [ForeignKey("ApplicationUserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUsers { get; set; }
        [Required]
        public DateTime? OderDate { get; set; }

        public DateTime? ShippingDate { get; set; }
        public double? OderTotal { get; set; }
        public string? OderStatus { get; set; }
        public string? PaymentStatus { get; set; }
        public string? Tracking { get; set; }
        public string? Carries { get; set; } = "null";
        public DateTime? PaymentDate { get; set; }
        public DateTime? PaymentOutDate { get; set; }
        public string? SessionId { get; set; } = "null";
        public string? PaymentIntenId { get; set; } = "null";

        [Required]
        public string LastName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string StreetAddress { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        public string? PostalCode { get; set; }
        [Required]
        public int PhoneNumber { get; set; }

    }
}
