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

    public class ReviewsController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private readonly INotyfService notyfService;

        public ReviewsController(IUnitOfWork unitOfWork, INotyfService notyfService)
        {
            _unitOfWork = unitOfWork;
            this.notyfService = notyfService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Load()
        {
            return Json(new { data = _unitOfWork.Navbar.GetAll(include: "ApplicationUser") });
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Reviews reviews)
        {
            
            if (!ModelState.IsValid)
            {
                return View(reviews);
            }
            _unitOfWork.Review.Add(reviews);
            _unitOfWork.Save();
            notyfService.Success("Bạn đã tạo thành công");
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            return View(_unitOfWork.Review.GetById(i => i.Id == id));
        }
        [HttpPost]
        public IActionResult Edit(Reviews reviews)
        {
            _unitOfWork.Review.UpdateReviews(reviews);
            _unitOfWork.Save();
            notyfService.Success("Bạn đã cập nhật thành công");
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(int? id)
        {
            _unitOfWork.Review.Delete(_unitOfWork.Review.GetById(n => n.Id == id));
            _unitOfWork.Save();
            notyfService.Success("Bạn đã xóa thành công");
            return Json(new { success = true });
        }

    }
}
