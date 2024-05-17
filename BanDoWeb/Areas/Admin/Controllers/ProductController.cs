using Microsoft.AspNetCore.Mvc;
using Project.DataAccess.Repository;
using Project.DataAccess.Repository.IRepository;
using System.Collections.Generic;
using BanDoWeb.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using AspNetCoreHero.ToastNotification.Abstractions;
using BanDoWeb.Areas.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace BanDoWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Employee")]
    public class ProductController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly INotyfService notyfService;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, INotyfService notyfService, IHubContext<SignalsServer> hubContext)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            this.notyfService = notyfService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult getCoutViewsProd()
        {
            var lstProduct = _unitOfWork.Product.GetAll(include: "Categories");
            var count = 0;
            if(lstProduct != null)
            {
                foreach (var item in lstProduct)
                {
                    count += (int)item.Views;
                }
            }
            return Ok(count);
        }

        [HttpGet]
        public IActionResult Load(string? strSearch)
        {
            if (strSearch != null)
            {
                return Json(new { data = _unitOfWork.Product.GetAll(include: "Categories").Where(i => i.Title.Contains(strSearch)) });
            }
            if(strSearch != null)
            {
                return Json(new { data = _unitOfWork.Product.GetAll(include: "Categories").Where(i => i.CategoryId == int.Parse(strSearch)) });
            }
            return Json(new { data = _unitOfWork.Product.GetAll(include: "Categories") });
        }
        public IActionResult Create()
        {
            var productVM = new ProductVM();
            productVM.Product = new Product();
            productVM.ProductListItems = _unitOfWork.Category.GetAll().Select(i =>
                new SelectListItem
                {
                    Text = i.NameCategori,
                    Value = i.Id.ToString()
                }
            );
            ViewBag.productVM = productVM.ProductListItems;
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product, IFormFile ImageUrl)
        {
            var pathRoot = _webHostEnvironment.WebRootPath;
            var fileName = Guid.NewGuid().ToString();
            if (!ModelState.IsValid)
            {
                var productVM = new ProductVM();
                productVM.Product = new Product();
                productVM.ProductListItems = _unitOfWork.Category.GetAll().Select(i =>
                    new SelectListItem
                    {
                        Text = i.NameCategori,
                        Value = i.Id.ToString()
                    }
                );
                ViewBag.productVM = productVM.ProductListItems;
                return View(product);
            }
            else
            {
                if (ImageUrl != null)
                {
                    var upload = Path.Combine(pathRoot, @"Content\assets\img\product\");
                    var extention = Path.Combine(ImageUrl.FileName);
                    using (var fileTream = new FileStream(Path.Combine(upload, fileName + extention), FileMode.Create))
                    {
                        ImageUrl.CopyTo(fileTream);
                    }
                    product.ImageUrl = fileName + extention;
                    product.Views = 0;
                }
                if(product.Active == null)
                {
                    product.Active = false;
                }
                _unitOfWork.Product.Add(product);
                _unitOfWork.Save();
            }
            notyfService.Success("You have create success.");
            return RedirectToAction("Index");
           
        }
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            var product = new ProductVM();
            product.Product = _unitOfWork.Product.GetById(n => n.Id == id);
            product.ProductListItems = _unitOfWork.Category.GetAll().Select(i =>
                new SelectListItem
                {
                    Text = i.NameCategori,
                    Value = i.Id.ToString()
                }
            );
            ViewBag.Product = product.ProductListItems;
            return View(product);

        }
        [HttpPost]
        public IActionResult Edit(Product product, IFormFile ImageUrl)
        {
            var pathRoot = _webHostEnvironment.WebRootPath;
            var fileName = Guid.NewGuid().ToString();
            var ObjProduct = _unitOfWork.Product.GetById(i => i.Id == product.Id);
            if (product == null)
            {
                var productVM = new ProductVM();
                productVM.Product = new Product();
                productVM.ProductListItems = _unitOfWork.Category.GetAll().Select(i =>
                    new SelectListItem
                    {
                        Text = i.NameCategori,
                        Value = i.Id.ToString()
                    }
                );
                ViewBag.productVM = productVM.ProductListItems;
                return View(product);
            }
            else
            {
                if (ImageUrl != null)
                {
                    var upload = Path.Combine(pathRoot, @"Content\assets\img\product\");
                    var extention = Path.Combine(ImageUrl.FileName);
                    if(_unitOfWork.Product.GetById(i => i.Id == product.Id).ImageUrl != null)
                    {
                        var file = Path.Combine(pathRoot + @"\Content\assets\img\product\" + _unitOfWork.Product.GetById(i => i.Id == product.Id).ImageUrl);
                        if (System.IO.File.Exists(file))
                        {
                            System.IO.File.Delete(file);
                        }
                    }
                    using (var fileTream = new FileStream(Path.Combine(upload, fileName + extention), FileMode.Create))
                    {
                        ImageUrl.CopyTo(fileTream);
                    }
                    ObjProduct.ImageUrl = fileName + extention;
                }
            }
            ObjProduct.Title = product.Title;
            ObjProduct.Description = product.Description;
            ObjProduct.Price = product.Price;
            ObjProduct.OriginalPrice = product.OriginalPrice;
            ObjProduct.Warehouse = product.Warehouse;
            ObjProduct.PriceTotal = product.PriceTotal;
            ObjProduct.CategoryId = product.CategoryId;
            ObjProduct.Size = product.Size;
            ObjProduct.Color = product.Color;
            ObjProduct.Active = product.Active;
            _unitOfWork.Product.Update(ObjProduct);
            _unitOfWork.Save();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var pathRoot = _webHostEnvironment.WebRootPath;
            var product = _unitOfWork.Product.GetById(n => n.Id == id);
            if (product.ImageUrl != null)
            {
                var file = Path.Combine(pathRoot + @"\Content\assets\img\product\" + product.ImageUrl);
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                }
            }
            _unitOfWork.Product.Delete(product);
            _unitOfWork.Save();
            return Json(new { success = true });
        }

    }
}
