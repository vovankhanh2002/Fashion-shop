using Microsoft.AspNetCore.Mvc;
using Project.DataAccess.Repository;
using Project.DataAccess.Repository.IRepository;
using System.Collections.Generic;
using BanDoWeb.Model.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AspNetCoreHero.ToastNotification.Abstractions;
using System.Security.Claims;

namespace BanDoWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Employee")]

    public class NewsletterController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private readonly INotyfService notyfService;

        public NewsletterController(IUnitOfWork unitOfWork, INotyfService notyfService)
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
            return Json(new { data = _unitOfWork.Newsletter.GetAll(include: "ApplicationUser") });
        }
        [HttpPost]
        public IActionResult Create(string newsletter)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (!ModelState.IsValid)
            {
                return View(newsletter);
            }
            var Newsletters = _unitOfWork.Newsletter.GetById(i => i.ApplicationUserId == claim.Value);
            if (Newsletters == null)
            {
                var ObjNewsletter = new Newsletter();
                ObjNewsletter.ApplicationUserId = claim.Value;
                ObjNewsletter.Email = newsletter;
                ObjNewsletter.ApplicationUser = _unitOfWork.ApplicationUser.GetById(i => i.Id == claim.Value);
                _unitOfWork.Newsletter.Add(ObjNewsletter);
                _unitOfWork.Save();
                notyfService.Success("Bạn đã đăng ký bản tin thành công");
            }
            else
            {
                notyfService.Information("Tài khoản của bạn đã đăng ký bản tin");
            }
           
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(int? id)
        {
            _unitOfWork.Newsletter.Delete(_unitOfWork.Newsletter.GetById(n => n.Id == id));
            _unitOfWork.Save();
            notyfService.Success("Bạn đã xóa thành công");
            return Json(new { success = true });
        }

    }
}
