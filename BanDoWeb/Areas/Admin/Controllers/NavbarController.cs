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

    public class NavbarController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private readonly INotyfService notyfService;

        public NavbarController(IUnitOfWork unitOfWork, INotyfService notyfService)
        {
            _unitOfWork = unitOfWork;
            this.notyfService = notyfService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Load(string? strSearch)
        {
            if (strSearch != null)
            {
                return Json(new { data = _unitOfWork.Navbar.GetAll().Where(i => i.TitleNavBar.Contains(strSearch)) });
            }
            return Json(new { data = _unitOfWork.Navbar.GetAll() });
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Navbar navbar)
        {
            
            if (!ModelState.IsValid)
            {
                return View(navbar);
            }
            navbar.DateSetNavbar = DateTime.Now;
            _unitOfWork.Navbar.Add(navbar);
            _unitOfWork.Save();
            notyfService.Success("You have create success.");
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            return View(_unitOfWork.Navbar.GetById(i => i.Id == id));
        }
        [HttpPost]
        public IActionResult Edit(Navbar navbar)
        {
            _unitOfWork.Navbar.UpdateNavar(navbar);
            _unitOfWork.Save();
            notyfService.Success("You have update success.");
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(int? id)
        {
            _unitOfWork.Navbar.Delete(_unitOfWork.Navbar.GetById(n => n.Id == id));
            _unitOfWork.Save();
            notyfService.Success("You have delete success.");
            return Json(new { success = true });
        }

    }
}
