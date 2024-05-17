using AspNetCoreHero.ToastNotification.Abstractions;
using BanDoWeb.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.DataAccess.Repository.IRepository;

namespace BanDoWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Employee")]

    public class CompanyController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private readonly INotyfService notyfService;

        public CompanyController(IUnitOfWork unitOfWork, INotyfService notyfService)
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
                return Json(new { data = _unitOfWork.Company.GetAll().Where(i => i.Name.Contains(strSearch)) });
            }
            return Json(new { data = _unitOfWork.Company.GetAll() });
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Company company)
        {

            if (!ModelState.IsValid)
            {
                return View(company);
            }
            company.DateTime = DateTime.Now;
            _unitOfWork.Company.Add(company);
            _unitOfWork.Save();
            notyfService.Success("You have create success.");
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            return View(_unitOfWork.Company.GetById(i => i.Id == id));
        }
        [HttpPost]
        public IActionResult Edit(Company company)
        {
            company.DateTime = DateTime.Now;
            _unitOfWork.Company.Update(company);
            _unitOfWork.Save();
            notyfService.Success("You have update success.");
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(int? id)
        {
            _unitOfWork.Company.Delete(_unitOfWork.Company.GetById(n => n.Id == id));
            _unitOfWork.Save();
            notyfService.Success("You have delete success.");
            return Json(new { success = true });
        }


    }
}
