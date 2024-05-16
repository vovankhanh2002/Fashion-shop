using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanDoWeb.Model.Models
{
    public class Company
    {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter the name")]
        public string Name { get; set; }
        public string? StreetAddress { get; set; }
        public int? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }
        [Phone(ErrorMessage = "Please enter a valid Phone No")]
        public string? PhoneNumber { get; set; }
        public DateTime? DateTime { get; set; }
    }
}
