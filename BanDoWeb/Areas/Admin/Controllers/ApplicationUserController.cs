using AspNetCoreHero.ToastNotification.Abstractions;
using BanDoWeb.Areas.Hubs;
using BanDoWeb.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using Project.DataAccess.Repository.IRepository;
using StackExchange.Redis;
using System.IO;
using System.Security.Claims;

namespace BanDoWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    [BindProperties]
    public class ApplicationUserController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly INotyfService _notyfService;

        public ApplicationUserController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, INotyfService notyfService, IHubContext<SignalsServer> hubContext)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _notyfService = notyfService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Load()
        {
            return Json(new { data = _unitOfWork.ApplicationUser.GetAll() });
        }
        [HttpGet]
        public IActionResult getCountApplication()
        {
            return Ok(_unitOfWork.ApplicationUser.GetAll().Count());
        }

        public IActionResult Edit(string? id)
        {
            var a = _unitOfWork.ApplicationUser.GetById(i => i.Id == id);
            return View(a);
        }
        [ActionName("Edit")]
        [HttpPost]
        public IActionResult Update()
        {
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Employee")]
        public IActionResult Delete(string? id)
        {
            _unitOfWork.ApplicationUser.Delete(_unitOfWork.ApplicationUser.GetById(n => n.Id == id));
            _unitOfWork.Save();
            _notyfService.Success("You have delete success.");
            return RedirectToAction("Index");
        }

    }
}

