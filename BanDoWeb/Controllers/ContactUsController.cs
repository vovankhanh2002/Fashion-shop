using AspNetCoreHero.ToastNotification.Abstractions;
using BanDoWeb.Model.Models;
using Microsoft.AspNetCore.Mvc;
using Project.DataAccess.Repository.IRepository;
using System.Security.Claims;

namespace BanDoWeb.Controllers
{
    public class ContactUsController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private readonly INotyfService notyfService;

        public ContactUsController(IUnitOfWork unitOfWork, INotyfService notyfService)
        {
            _unitOfWork = unitOfWork;
            this.notyfService = notyfService;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Create(Contact contact)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            contact.ApplicationUserId = claim.Value;
            if (contact.Name == null && contact.Email == null && contact.Subject == null && contact.Message == null)
            {
                return View(contact);
            }
            if(claim.Value != null)
            {
                var contactus = new Contact();
                contactus.Name = contact.Name;
                contactus.Email = contact.Email;
                contactus.Subject = contact.Subject;
                contactus.Message = contact.Message;
                contactus.ApplicationUserId = contact.ApplicationUserId;
                contactus.ApplicationUser = _unitOfWork.ApplicationUser.GetById(i => i.Id == claim.Value);
                _unitOfWork.Contact.Add(contactus);
                _unitOfWork.Save();
                notyfService.Success("Bạn đã gửi tin nhắn thành công.");
                return RedirectToAction("Index");
            }
            return RedirectPermanent("~/Identity/Account/Login?returnUrl=/ContactUs/Index");
        }

    }
}
