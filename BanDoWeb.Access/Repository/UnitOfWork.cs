using BanDoWeb.Access.Dbcontext;
using BanDoWeb.Access.Repository;
using BanDoWeb.Access.Repository.IRepository;
using BanDoWeb.Model.Models;
using Project.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public INavbar Navbar { get; set; }
        public ICategories Category { get; set; }
        public ICompany Company { get; set; }
        public IProduct Product { get; set ; }
        public IApplicationUser ApplicationUser { get; set; }
        public IShoppingCart ShoppingCart { get; set; }
        public IOderHeader OderHeader { get; set; }
        public IOderDetail OderDetail { get; set ; }
        public IChatApp ChatApp { get; set; }
        public INumberOfVisits NumberOfVisits { get; set; }
        public INewsletter Newsletter { get; set; }
        public IContact Contact { get; set; }
        public IReviews Review { get; set; }
        public ISlideimage Slideimage { get; set; }

        private readonly DbcontextBanDo _dbContext;
        public UnitOfWork(DbcontextBanDo dbContext)
        {
            _dbContext = dbContext;
            Navbar = new NavbarRepon(dbContext);
            Category = new CategoriesRepon(dbContext);
            Company = new CompanyRepon(dbContext);
            Product = new ProductRepon(dbContext);
            ApplicationUser = new ApplicationUserRepon(dbContext);
            ShoppingCart = new ShoppingCartRepon(dbContext);
            OderHeader = new OderHeaderRepon(dbContext);
            OderDetail = new OderDetailRepon(dbContext);
            ChatApp = new ChatAppRepon(dbContext);
            NumberOfVisits = new NumberOfVisitsRepon(dbContext);
            Newsletter = new NewsletterRepon(dbContext);
            Contact = new ContactRepon(dbContext);
            Review = new ReviewsRepon(dbContext);
            Slideimage = new SlideimageRepon(dbContext);
        }


        public void Save()
        {
            _dbContext.SaveChanges();
        }
    }
}
