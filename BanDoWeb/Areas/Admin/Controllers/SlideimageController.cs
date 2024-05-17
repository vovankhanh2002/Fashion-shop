using Microsoft.AspNetCore.Mvc;
using Project.DataAccess.Repository;
using Project.DataAccess.Repository.IRepository;
using System.Collections.Generic;
using BanDoWeb.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace BanDoWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Employee")]

    public class SlideimageController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private readonly INotyfService notyfService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public SlideimageController(IUnitOfWork unitOfWork, INotyfService notyfService, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            this.notyfService = notyfService;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Load()
        {
            return Json(new { data = _unitOfWork.Slideimage.GetAll(include: "Product") });
        }
        public IActionResult Create(int? id)
        {
            ViewBag.idProductImage = id;
            return View();
        }
        [HttpPost]
        public IActionResult Create(List<IFormFile> Image, int id)
        {
            var pathRoot = _webHostEnvironment.WebRootPath;
            var slide = _unitOfWork.Slideimage.GetAllWhere(i => i.ProductId == id);
            if(slide == null || slide.Count() < 4)
            {
                if (Image != null && Image.Count > 0)
                {
                    foreach (var image in Image)
                    {
                        var fileName = Guid.NewGuid().ToString();

                        if (image.Length > 0)
                        {
                            if(_unitOfWork.Slideimage.GetAllWhere(i => i.ProductId == id).Count() < 4)
                            {
                                var upload = Path.Combine(pathRoot, @"Content\assets\img\SideImage\");
                                var extention = Path.Combine(image.FileName);
                                using (var fileTream = new FileStream(Path.Combine(upload, fileName + extention), FileMode.Create))
                                {
                                    image.CopyTo(fileTream);
                                }
                                var slideimage = new SlidedImage()
                                {
                                    ProductId = id,
                                    Image = fileName + extention,
                                    Product = _unitOfWork.Product.GetById(i => i.Id == id)
                                };
                                _unitOfWork.Slideimage.Add(slideimage);
                                _unitOfWork.Save();
                            }
                            else
                            {
                                notyfService.Success("Bạn không thể tạo thêm vì tối đa chỉ có 4 ảnh");
                                return RedirectToAction("Index");
                            }
                        }
                    }
                    notyfService.Success("Bạn đã tạo mới thành công");
                    return RedirectToAction("Index");
                }
                notyfService.Error("Bạn đã tạo mới không thành công");
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            return View(_unitOfWork.Slideimage.GetById(i => i.Id == id));
        }
        [HttpPost]
        public IActionResult Edit(SlidedImage slidedImage, IFormFile Image)
        {
            var pathRoot = _webHostEnvironment.WebRootPath;
            var fileName = Guid.NewGuid().ToString();
            var Slideimage = _unitOfWork.Slideimage.GetById(i => i.Id == slidedImage.Id);
            if (slidedImage == null)
            {
                return View(slidedImage);
            }
            else
            {
                if (Image != null)
                {
                    var upload = Path.Combine(pathRoot, @"Content\assets\img\SideImage\");
                    var extention = Path.Combine(Image.FileName);
                    if (_unitOfWork.Slideimage.GetById(i => i.Id == slidedImage.Id).Image != null)
                    {
                        var file = Path.Combine(pathRoot + @"\Content\assets\img\SideImage\" + _unitOfWork.Slideimage.GetById(i => i.Id == slidedImage.Id).Image);
                        if (System.IO.File.Exists(file))
                        {
                            System.IO.File.Delete(file);
                        }
                    }
                    using (var fileTream = new FileStream(Path.Combine(upload, fileName + extention), FileMode.Create))
                    {
                        Image.CopyTo(fileTream);
                    }
                    Slideimage.Image = fileName + extention;
                }
            }
            Slideimage.ProductId = slidedImage.ProductId;
            notyfService.Success("Bạn đã cập nhật thành công");
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(int? id)
        {
            var pathRoot = _webHostEnvironment.WebRootPath;
            _unitOfWork.Slideimage.Delete(_unitOfWork.Slideimage.GetById(n => n.Id == id));
            _unitOfWork.Save();
            notyfService.Success("Bạn đã xóa thành công");
            var upload = Path.Combine(pathRoot, @"Content\assets\img\SideImage\");
            if (_unitOfWork.Slideimage.GetById(i => i.Id == id).Image != null)
            {
                var file = Path.Combine(pathRoot + @"\Content\assets\img\SideImage\" + _unitOfWork.Slideimage.GetById(i => i.Id == id).Image);
                if (System.IO.File.Exists(file))
                {
                    System.IO.File.Delete(file);
                }
            }
            return Json(new { success = true });
        }

    }
}
