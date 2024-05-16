using BanDoWeb.Access.Repository.IRepository;
using BanDoWeb.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public INavbar Navbar { get; set; }
        public ICategories Category { get; set; }
        public ICompany Company { get; set; }
        public IProduct Product { get; set; }
        public IShoppingCart ShoppingCart { get; set; }
        public IApplicationUser ApplicationUser { get; set; }
        public IOderDetail OderDetail { get; set; }
        public IOderHeader OderHeader { get; set; }
        public IChatApp ChatApp { get; set; }
        public INumberOfVisits NumberOfVisits { get; set; }
        public INewsletter Newsletter { get; set; }
        public IContact Contact { get; set; }
        public IReviews Review { get; set; }
        public ISlideimage Slideimage { get; set; }
        void Save();
    }
}
