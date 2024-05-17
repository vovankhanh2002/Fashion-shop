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
    [Authorize(Roles = "Admin,Employee")]
    public class OderDetailsController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly INotyfService _notyfService;
        private readonly IHubContext<SignalsServer> hubContext;

        public OderDetailsController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, INotyfService notyfService, IHubContext<SignalsServer> hubContext)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _notyfService = notyfService;
            this.hubContext = hubContext;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult getCountOrderDetail()
        {
            var res = _unitOfWork.OderDetail.GetAll().Count();
            return Ok(res);
        }
        [HttpGet]
        public IActionResult Load(string? strSearch)
        {
            if (strSearch != null)
            {
                return Json(new { data = _unitOfWork.OderDetail.GetAll().Where(i => i.Product.Title.Contains(strSearch) | i.Product.Size.Contains(strSearch)) });
            }
            return Json(new { data = _unitOfWork.OderDetail.GetAll() });
        }

        
    }
}

