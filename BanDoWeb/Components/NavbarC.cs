using BanDoWeb.Access.Dbcontext;
using Microsoft.AspNetCore.Mvc;
using Project.DataAccess.Repository.IRepository;
using System.Security.Claims;

namespace BanDoWeb.Components
{
    public class NavbarC: ViewComponent
    {
        private IUnitOfWork _unitOfWork;

        public NavbarC(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if(claim != null)
            {
                ViewBag.countCart = _unitOfWork.ShoppingCart.GetAllWhere(i => i.ApplicationUserId == claim.Value).Count();
            }
            else
            {
                ViewBag.countCart = 0;
            }
            return View(_unitOfWork.Category.GetAll());
        }
    }
}
