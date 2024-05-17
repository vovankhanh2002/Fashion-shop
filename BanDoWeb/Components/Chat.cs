using BanDoWeb.Access.Dbcontext;
using BanDoWeb.Model.Models;
using Microsoft.AspNetCore.Mvc;
using Project.DataAccess.Repository.IRepository;
using System.Security.Claims;

namespace BanDoWeb.Components
{
    public class Chat: ViewComponent
    {
        private IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Chat(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View(_unitOfWork.ApplicationUser.GetById( i => i.Id == userId));
        }
    }
}
