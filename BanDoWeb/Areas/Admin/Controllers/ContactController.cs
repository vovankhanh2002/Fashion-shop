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

    public class ContactController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private readonly INotyfService notyfService;

        public ContactController(IUnitOfWork unitOfWork, INotyfService notyfService)
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
            return Json(new { data = _unitOfWork.Contact.GetAll() });
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Contact contact)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (!ModelState.IsValid)
            {
                return View(contact);
            }
            contact.ApplicationUserId = claim.Value;
            contact.ApplicationUser = _unitOfWork.ApplicationUser.GetById(i => i.Id == claim.Value);
            _unitOfWork.Contact.Add(contact);
            _unitOfWork.Save();
            notyfService.Success("Bạn đã tạo thành công");
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            return View(_unitOfWork.Contact.GetById(i => i.Id == id));
        }
        [HttpPost]
        public IActionResult Edit(Contact contact)
        {
            _unitOfWork.Contact.UpdateContact(contact);
            _unitOfWork.Save();
            notyfService.Success("You have update success.");
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(int? id)
        {
            _unitOfWork.Contact.Delete(_unitOfWork.Contact.GetById(n => n.Id == id));
            _unitOfWork.Save();
            notyfService.Success("You have delete success.");
            return Json(new { success = true });
        }

    }
}
