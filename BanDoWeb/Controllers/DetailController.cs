using BanDoWeb.Areas.Hubs;
using BanDoWeb.Model.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Project.DataAccess.Repository.IRepository;
using System.Security.Claims;

namespace BanDoWeb.Controllers
{
    public class DetailController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IHubContext<SignalsServer> hubContext;

        public DetailController(IUnitOfWork unitOfWork, IHubContext<SignalsServer> hubContext)
        {
            this.unitOfWork = unitOfWork;
            this.hubContext = hubContext;
        }
        public async Task<IActionResult> Index(int? id)
        {
            var detailVM = new DetailVM();
            var productDetails = unitOfWork.Product.GetById(i => i.Id == id);
            var sumStar = unitOfWork.Review.GetAllWhere(i => i.ProductId == productDetails.Id).Sum(i => i.Star);
            var countStar = unitOfWork.Review.GetAllWhere(i => i.ProductId == productDetails.Id).Count();
            if (productDetails != null)
            {
                productDetails.Views = productDetails.Views + 1;
                unitOfWork.Product.Update(productDetails);
                unitOfWork.Save();
            }
            await hubContext.Clients.All.SendAsync("LoadOrderHeader");
            detailVM.Product = unitOfWork.Product.GetById(i => i.Id == id, include: "Categories");
            detailVM.Reviews = unitOfWork.Review.GetAllWhere(i => i.ProductId == productDetails.Id);
            detailVM.SlidedImages = unitOfWork.Slideimage.GetAllWhere(i => i.ProductId == productDetails.Id);
            if(countStar > 0)
            {
                detailVM.Star = sumStar/countStar;
            }
            return View(detailVM);
        }
        [HttpPost]
        public IActionResult ReviewProduct(Reviews reviews)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            if(claim != null)
            {
                var objReview = new Reviews();
                objReview.Name = reviews.Name;
                objReview.Email = reviews.Email;
                objReview.MessageReview = reviews.MessageReview;
                objReview.Star = reviews.Star;
                objReview.ApplicationId = claim.Value;
                objReview.ApplicationUser = unitOfWork.ApplicationUser.GetById(i => i.Id == claim.Value);
                objReview.ProductId = reviews.ProductId;
                objReview.Product = unitOfWork.Product.GetById(i => i.Id == reviews.ProductId);
                objReview.DateTime = DateTime.Now;
                unitOfWork.Review.Add(objReview);
                unitOfWork.Save();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
