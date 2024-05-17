using Microsoft.AspNetCore.Mvc;
using Project.DataAccess.Repository.IRepository;

namespace BanDoWeb.Components
{
    public class Carousel:ViewComponent
    {
        private readonly IUnitOfWork unitOfWork;

        public Carousel(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(unitOfWork.Product.GetAll().Take(2));
        }
    }
}
