using BanDoWeb.Model.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanDoWeb.Access.Dbcontext
{
    public class DbcontextBanDo  : IdentityDbContext
    {
        public DbcontextBanDo(DbContextOptions<DbcontextBanDo> options) : base(options)
        {

        }
        public DbSet<Navbar> Navbars { get; set; }
        public DbSet<Categories> Categoriess { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<OderHeader> OderHeaders { get; set; }
        public DbSet<OderDetail> OderDetails { get; set; }
        public DbSet<ChatApp> ChatApps { get; set; }
        public DbSet<NumberOfVisits> NumberOfVisits { get; set; }
        public DbSet<Newsletter> Newsletters { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Reviews> Reviews { get; set; }
        public DbSet<SlidedImage> SlidedImages { get; set; }

    }
}
