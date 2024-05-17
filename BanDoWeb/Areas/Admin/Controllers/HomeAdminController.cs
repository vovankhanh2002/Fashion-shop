using BanDoWeb.Model.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.DataAccess.Repository.IRepository;
using System.Security.Claims;

namespace BanDoWeb.Areas.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Employee")]

    public class HomeAdminController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public Dashboard dashboard { get; set; }
        public HomeAdminController(IUnitOfWork unitOfWork   )
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Home()
        {
            return View();
        }
        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            var userId = _unitOfWork.ApplicationUser.GetById(i => i.Id == claim.Value);
            return View(userId);
        }

        public IActionResult Dashboard()
        {
            dashboard = new Dashboard();
            dashboard.oderHeaders = _unitOfWork.OderHeader.GetAll().ToList();
            dashboard.orderDetail = _unitOfWork.OderDetail.GetAll().Count();
            return View(dashboard);
        }

        public IActionResult chartInfomation()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Statistics()
        {
            var query = from o in _unitOfWork.OderHeader.GetAll()
                        join od in _unitOfWork.OderDetail.GetAll()
                        on o.Id equals od.OderHeaderId
                        join p in _unitOfWork.Product.GetAll()
                        on od.ProductId equals p.Id
                        select new
                        {
                            OderDate = o.OderDate,
                            Quanlity = od.Count,
                            Price = od.Price,
                            Warehouse = p.Warehouse,
                            Totals = o.OderTotal,
                            OriginalPrice = p.OriginalPrice,
                            
                        };
            var result = query.GroupBy(i => new { i.OderDate.Value.Year, i.OderDate.Value.Month }).Select(g => new
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Total = g.Sum(g => g.Totals),
                Profit = g.Sum(g => g.Warehouse * g.OriginalPrice)
            }).OrderBy(r => r.Year)
            .ThenBy(r => r.Month)
            .ToList();
                         
            return Json(new {data = result, success = true});
        }
        [HttpGet]
        public IActionResult StatisticsUser()
        {
            var query = from o in _unitOfWork.ApplicationUser.GetAll()
                        select new
                        {
                            Date = o.Date,
                        };

            var result = query.GroupBy(i => new { i.Date.Value.Year, i.Date.Value.Month }).Select(g => new
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Count = g.Count()
            }).OrderBy(r => r.Year)
            .ThenBy(r => r.Month)
            .ToList();

            return Json(new { data = result, success = true });
        }
        [HttpGet]
        public IActionResult StatisticsUserNumberOfVs()
        {
            var query = from o in _unitOfWork.NumberOfVisits.GetAll()
                        select new
                        {
                            Date = o.DateTime,
                            Number = o.accessNumber
                        };

            var result = query.GroupBy(i => new { i.Date.Year, i.Date.Month }).Select(g => new
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Number = g.Sum(i => i.Number)
            }).OrderBy(r => r.Year)
            .ThenBy(r => r.Month)
            .ToList();
            return Json(new { data = result, success = true });
        }
        [HttpGet]
        public IActionResult StockChart()
        {
            var query = from o in _unitOfWork.Category.GetAll()
                        join i in _unitOfWork.Product.GetAll()
                        on o.Id equals i.CategoryId
                        select new
                        {
                            Name = o.NameCategori,
                            SumW = i.Warehouse,
                        };
            var result = query.GroupBy(i => new { i.Name }).Select(g => new
            {
                Name = g.Key.Name,
                Sumw = g.Sum(i => i.SumW)
            }).OrderBy(r => r.Name)
            .ToList();
            return Json(new { data = result, success = true });
        }
    }
}
