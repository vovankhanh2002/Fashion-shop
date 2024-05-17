using Microsoft.AspNetCore.Mvc;
using Project.DataAccess.Repository.IRepository;

namespace BanDoWeb.Components
{
    public class Categori : ViewComponent
    {
        private readonly IUnitOfWork unitOfWork;

        public Categori(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(unitOfWork.Category.GetAll());
        }
    }
}
