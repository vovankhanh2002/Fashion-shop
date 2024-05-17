using BanDoWeb.Model.Models;
using Microsoft.AspNetCore.Mvc;
using Project.DataAccess.Repository.IRepository;
using X.PagedList;

namespace BanDoWeb.Controllers
{
    public class ShopController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ShopController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Index(int? page, int? id)
        {
            var lstProduct = _unitOfWork.Product.GetAll();
            int pageSize = 20;
            int pageNumber = page == null || page <= 0 ? 1 : page.Value;
            int totalPage = lstProduct.Count();
            var start = (pageNumber - pageSize) * pageSize;
            lstProduct = lstProduct.Skip(start).Take(start).ToList();
            ViewBag.Categoris = _unitOfWork.Category.GetAll();
            if (id != null)
            {
                var productCategory = _unitOfWork.Product.GetAll().Where(i => i.CategoryId == id).ToList();
                return View(productCategory.ToPagedList(pageNumber, pageSize));
            }
            return View(_unitOfWork.Product.GetAll().ToPagedList(pageNumber, pageSize));
            //return Json(new { data = lstProduct,currentpage = pageNumber,pagesize = pageSize, success = true});
        }
        [HttpPost]
        public IActionResult Filter(List<string>?lstCategori, List<string>?lstPrice, List<string>?lstColor, List<string>?lstSize, int? page)
        {
            int pageSize = 9;
            int pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var list = _unitOfWork.Product.GetAll();
            string strCategori = lstCategori[0];
            string strPrice = lstPrice[0];
            if (lstPrice.Count > 0 && lstColor.Count > 0 && lstSize.Count >0)
            {
                string strColor = lstColor[0];
                if (strPrice != "1000")
                {
                    string strSize = lstSize[0];
                    var categori = int.Parse(strCategori);
                    var minPrice = int.Parse(strPrice.Substring(0, strPrice.IndexOf("-")));
                    var maxPrice = int.Parse(strPrice.Substring(strPrice.IndexOf("-") + 1));
                    if(strCategori == "0")
                    {
                        var lstResultPrice = list.Where(i => i.Price >= minPrice && i.Price <= maxPrice && i.Color == strColor && i.Size == strSize).ToList();
                        return Json(new { data = lstResultPrice, success = true });
                    }
                    else
                    {
                        var lstResultPrice = list.Where(i => i.Price >= minPrice && i.Price <= maxPrice && i.CategoryId == categori && i.Color == strColor && i.Size == strSize).ToList();
                        return Json(new { data = lstResultPrice, success = true });
                    }
                    
                }
                else if (strCategori == "0" )
                {
                    string strSize = lstSize[0];
                    var categori = int.Parse(strCategori);

                    var lstResultPrice = list.Where(i => i.Color == strColor && i.Size == strSize).ToList();
                    return Json(new { data = lstResultPrice, success = true });
                }
                else
                {
                    string strSize = lstSize[0];
                    var categori = int.Parse(strCategori);
                    var lstResultPrice = list.Where(i => i.CategoryId == categori && i.Color == strColor && i.Size == strSize).ToList();
                    return Json(new { data = lstResultPrice, success = true });
                }
                
            }else if(lstColor.Count > 0 && lstSize.Count <=0)
            {
                string strColor = lstColor[0];
                if (strPrice != "1000")
                {
                    var categori = int.Parse(strCategori);
                    var minPrice = int.Parse(strPrice.Substring(0, strPrice.IndexOf("-")));
                    var maxPrice = int.Parse(strPrice.Substring(strPrice.IndexOf("-") + 1));
                    if (strCategori == "0")
                    {
                        var lstResultPrice = list.Where(i => i.Price >= minPrice && i.Price <= maxPrice && i.Color == strColor).ToList();
                        return Json(new { data = lstResultPrice, success = true });
                    }
                    else
                    {
                        var lstResultPrice = list.Where(i => i.Price >= minPrice && i.Price <= maxPrice && i.CategoryId == categori && i.Color == strColor).ToList();
                        return Json(new { data = lstResultPrice, success = true });
                    }
                    
                }
                else if (strCategori == "0")
                {
                    var categori = int.Parse(strCategori);
                    var lstResultPrice = list.Where(i => i.Color == strColor).ToList();
                    return Json(new { data = lstResultPrice, success = true });
                }
                else
                {
                    var categori = int.Parse(strCategori);
                    var lstResultPrice = list.Where(i => i.CategoryId == categori && i.Color == strColor).ToList();
                    return Json(new { data = lstResultPrice, success = true });
                }
            }
            else if(lstSize.Count > 0 && lstColor.Count <= 0)
            {
                if (strPrice != "1000")
                {
                    string strSize = lstSize[0];
                    var categori = int.Parse(strCategori);
                    var minPrice = int.Parse(strPrice.Substring(0, strPrice.IndexOf("-")));
                    var maxPrice = int.Parse(strPrice.Substring(strPrice.IndexOf("-") + 1));
                    if (strCategori == "0")
                    {
                        var lstResultPrice = list.Where(i => i.Price >= minPrice && i.Price <= maxPrice && i.Size == strSize).ToList();
                        return Json(new { data = lstResultPrice, success = true });
                    }
                    else
                    {
                        var lstResultPrice = list.Where(i => i.Price >= minPrice && i.Price <= maxPrice && i.CategoryId == categori && i.Size == strSize).ToList();
                        return Json(new { data = lstResultPrice, success = true });
                    }
                    
                }
                else if (strCategori == "0")
                {
                    string strSize = lstSize[0];
                    var categori = int.Parse(strCategori);
                    var lstResultPrice = list.Where(i => i.Size == strSize).ToList();
                    return Json(new { data = lstResultPrice, success = true });
                }
                else
                {
                    string strSize = lstSize[0];
                    var categori = int.Parse(strCategori);
                    var lstResultPrice = list.Where(i => i.CategoryId == categori && i.Size == strSize).ToList();
                    return Json(new { data = lstResultPrice, success = true });
                }
            }
            else if(strCategori == "0" && strPrice != "1000")
            {
                var categori = int.Parse(strCategori);
                var minPrice = int.Parse(strPrice.Substring(0, strPrice.IndexOf("-")));
                var maxPrice = int.Parse(strPrice.Substring(strPrice.IndexOf("-") + 1));
                var lstResultPrice = list.Where(i => i.Price >= minPrice && i.Price <= maxPrice).ToList();
                return Json(new { data = lstResultPrice, success = true });
            }
            else if(strCategori != "0" && strPrice != "1000")
            {
                var categori = int.Parse(strCategori);
                var minPrice = int.Parse(strPrice.Substring(0, strPrice.IndexOf("-")));
                var maxPrice = int.Parse(strPrice.Substring(strPrice.IndexOf("-") + 1));
                var lstResultPrice = list.Where(i => i.Price >= minPrice && i.Price <= maxPrice && i.CategoryId == categori).ToList();
                return Json(new { data = lstResultPrice, success = true });
            }
            else if(strCategori == "0" && strPrice == "1000")
            {
                return Json(new { data = list, success = true });
            }
            
            return Json(new { data = list.Where(i => i.CategoryId == int.Parse(strCategori)), success = true });
        }
        public IActionResult Latest(int? page)
        {
            int pageSize = 9;
            int pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var lstProdut = _unitOfWork.Product.GetAll().OrderByDescending(i => i.Price).ToPagedList(pageNumber, pageSize);
            ViewBag.latest = "Latest";
            return View(lstProdut);
        }
        [HttpGet]
        public IActionResult GetProductSuggestions(string searchTerm)
        {
            if(searchTerm != null)
            {
                var suggestions = _unitOfWork.Product.GetAll().Where(p => p.Title.Contains(searchTerm)).Take(5);
                return Json(suggestions);
            }
            return Json(null);

        }
    }
}
