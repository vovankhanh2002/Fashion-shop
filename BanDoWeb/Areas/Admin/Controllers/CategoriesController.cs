using AspNetCoreHero.ToastNotification.Abstractions;
using BanDoWeb.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.DataAccess.Repository.IRepository;
using StackExchange.Redis;
using System.IO;

namespace BanDoWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class CategoriesController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly INotyfService _notyfService;

        public CategoriesController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, INotyfService notyfService)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _notyfService = notyfService;
        }
        [Authorize(Roles = "Employee")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Load(string? strSearch)
        {
            if (strSearch != null)
            {
                return Json(new { data = _unitOfWork.Category.GetAll().Where(i => i.NameCategori.Contains(strSearch)) });
            }
            return Json(new { data = _unitOfWork.Category.GetAll() });
        }
        [Authorize(Roles = "Employee")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "Employee")]
        [HttpPost]
        public IActionResult Create(Categories categories,IFormFile ImageUrl)
        {
            var pathRoot = _webHostEnvironment.WebRootPath;
            var fileName = Guid.NewGuid().ToString();
            if(categories.NameCategori == null)
            {
                return RedirectToAction("Create");
            }
            else
            {
                if(ImageUrl != null)
                {
                    var upload = Path.Combine(pathRoot, @"Content\assets\img\categories\");
                    var extention = Path.Combine(ImageUrl.FileName);
                    using (var fileTream = new FileStream(Path.Combine(upload, fileName + extention), FileMode.Create))
                    {
                        ImageUrl.CopyTo(fileTream);
                    }
                    categories.ImageUrl = fileName + extention;
                }
                _unitOfWork.Category.Add(categories);
                _unitOfWork.Save();
            }
            _notyfService.Success("You have create success.");
            return RedirectToAction("Index");

        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var ojbCategories = _unitOfWork.Category.GetById(n => n.Id == id);
            return View(ojbCategories);
        }
        [HttpPost]
        public IActionResult Edit(Categories categories, IFormFile ImageUrl)
        {
            var pathRoot = _webHostEnvironment.WebRootPath;
            var fileName = Guid.NewGuid().ToString();
            var categoriId = _unitOfWork.Category.GetById(i => i.Id == categories.Id);
            if (categories.NameCategori == null)
            {
                return RedirectToAction("Create");
            }
            else
            {
                if (ImageUrl != null)
                {
                    var upload = Path.Combine(pathRoot, @"Content\assets\img\categories\");
                    var extention = Path.Combine(ImageUrl.FileName);
                    var file = Path.Combine(pathRoot + @"\Content\assets\img\categories\" + categoriId.ImageUrl);
                    if (System.IO.File.Exists(file))
                    {
                        System.IO.File.Delete(file);
                    }
                    using (var fileTream = new FileStream(Path.Combine(upload,  fileName + extention), FileMode.Create))
                    {
                        ImageUrl.CopyTo(fileTream);
                    }
                    categoriId.NameCategori = categories.NameCategori;
                    categoriId.ImageUrl = fileName + extention;
                }
                else if (ImageUrl == null)
                {
                    categoriId.NameCategori = categories.NameCategori;
                    categoriId.ImageUrl = categoriId.ImageUrl;
                }
                _unitOfWork.Category.updateCategory(categoriId);
                _unitOfWork.Save();
            }
            _notyfService.Success("You have update success.");

            return RedirectToAction("Index");

        }
        [HttpPost]
        public IActionResult Delete(int? id)
        {
            var pathRoot = _webHostEnvironment.WebRootPath;
            var ojbCategories = _unitOfWork.Category.GetById(n => n.Id == id);
            if (ojbCategories.ImageUrl != null)
            {
                var file = Path.Combine(pathRoot + @"\Content\assets\img\categories\" + ojbCategories.ImageUrl);
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                }
            }
            _unitOfWork.Category.Delete(ojbCategories);
            _unitOfWork.Save();
            _notyfService.Success("You have delete success.");
            return Json(new { success = true });
        }

    }
}

