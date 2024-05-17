using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using System.Diagnostics;

namespace BanDoWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly INotyfService _notyfService;
        private readonly IToastNotification _toastNotification;

        public HomeController(INotyfService notyfService, IToastNotification toastNotification)
        {
            _notyfService = notyfService;
            _toastNotification = toastNotification;
        }
        public IActionResult Index()
        {
            if(TempData["toastLogin"] != null)
            {
                _notyfService.Success("Login success wellcome to website.");
            }
            else if(TempData["toastLogout"] !=  null)
            {
                _notyfService.Success("Logout success.");
            }
            return View();
        }
     }
}