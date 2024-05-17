using BanDoWeb.Model.Models;
using Microsoft.AspNetCore.Mvc;
using Project.DataAccess.Repository.IRepository;

namespace BanDoWeb.Components
{
    public class Product:ViewComponent
    {
        private readonly IUnitOfWork unitOfWork;

        public Product(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var Categories = new Categoriess();
            Categories.Categorias = unitOfWork.Category.GetAll();
            Categories.Products = unitOfWork.Product.GetAllWhere(i => i.PriceTotal != null, include: "Categories");
            return View(Categories);
        }
    }
}
