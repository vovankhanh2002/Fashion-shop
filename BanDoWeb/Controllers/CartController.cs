using AspNetCoreHero.ToastNotification.Abstractions;
using BanDoWeb.Areas.Hubs;
using BanDoWeb.Model.Models;
using CodeMegaVNPay.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using Project.DataAccess.Repository.IRepository;
using System.Security.Claims;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using X.PagedList;

namespace BanDoWeb.Controllers
{
    [BindProperties]
    public class CartController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly INotyfService notyfService;
        private readonly IHubContext<SignalsServer> _hubContext;
        private readonly IVnPayService _vnPayService;
        private TwilioSetting twilioSetting { get; set; }
        public ShoppingCartVM shoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork,
            INotyfService notyfService,
            IHubContext<SignalsServer> hubContext,
            IOptions<TwilioSetting> _twilioSetting,
            IVnPayService vnPayService)
        {
            this.unitOfWork = unitOfWork;
            this.notyfService = notyfService;
            _hubContext = hubContext;
            twilioSetting = _twilioSetting.Value;
            _vnPayService = vnPayService;
        }
        public IActionResult Index(int id)
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddToCart(int id,int quantity, List<string> lstColor, List<string> lstSize)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if(claim == null)
            {
                return Json(new { success = false });
            }
            var product = unitOfWork.Product.GetById(i => i.Id == id);
            if (lstColor.Count <=0 || lstSize.Count <=0 || quantity > product.Warehouse)
            {
                return Json(new { bassQuantity = true});
            }
            else
            {
                ShoppingCart cartOjb = unitOfWork.ShoppingCart.GetById(i => i.ApplicationUserId == claim.Value && i.ProductId == id && i.Color == lstColor[0] && i.Size == lstSize[0]);
                ShoppingCart cart = new ShoppingCart();
                cart.ApplicationUserId = claim.Value;
                cart.ApplicationUser = unitOfWork.ApplicationUser.GetById(i => i.Id == claim.Value);
                cart.ProductId = id;
                cart.Product = product;
                cart.Color = lstColor[0];
                cart.Size = lstSize[0];
                cart.Count = quantity;
                if (cartOjb != null)
                {
                    unitOfWork.ShoppingCart.InCount(cartOjb, quantity);
                }
                else
                {
                    unitOfWork.ShoppingCart.Add(cart);
                }
                unitOfWork.Save();
                notyfService.Success("Add to cart product success");
                
            }
            return Json(new { data= unitOfWork.ShoppingCart.GetAll().Count(), success = true });
        } 
        public IActionResult ShowCart()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if(claim != null)
            {
                shoppingCartVM = new ShoppingCartVM()
                {
                    LstshoppingCarts = unitOfWork.ShoppingCart.GetAllWhere(u => u.ApplicationUserId == claim.Value, include: "Product")
                };
            }
            else
            {
                return RedirectPermanent("~/Identity/Account/Login?returnUrl=/cart/showcart" );
            }
            var sum = 0;
            foreach (var item in shoppingCartVM.LstshoppingCarts)
            {
                sum += item.Count * (int)item.Product.Price;
            }
            ViewBag.Sum = sum;
            ViewBag.SumTotal = sum + 10;
            return View(shoppingCartVM);
        }
        [HttpPost]
        public IActionResult DeleteCart(int id)
        {
            unitOfWork.ShoppingCart.Delete(unitOfWork.ShoppingCart.GetById(i => i.Id == id));
            unitOfWork.Save();
            return Json(new {success = true});
        }
        [HttpPost]
        public IActionResult UpdateCart(int id, int quantity)
        {
            var cartUpdate = unitOfWork.ShoppingCart.GetById(i => i.Id == id);
            cartUpdate.Count = quantity;
            unitOfWork.ShoppingCart.Update(cartUpdate);
            unitOfWork.Save();
            return Json(new {data= unitOfWork.ShoppingCart.GetAll(), success = true });
        }
        public IActionResult CheckOut()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if(claim != null)
            {
                shoppingCartVM = new ShoppingCartVM()
                {
                    LstshoppingCarts = unitOfWork.ShoppingCart.GetAllWhere(u => u.ApplicationUserId == claim.Value, include: "Product"),
                    OrderHeader = new()

                };
                shoppingCartVM.OrderHeader.ApplicationUsers = unitOfWork.ApplicationUser.GetById(i => i.Id == claim.Value);
                shoppingCartVM.OrderHeader.FirstName = shoppingCartVM.OrderHeader.ApplicationUsers.Name;
                shoppingCartVM.OrderHeader.State = shoppingCartVM.OrderHeader.ApplicationUsers.State;
                shoppingCartVM.OrderHeader.City = shoppingCartVM.OrderHeader.ApplicationUsers.City;
                shoppingCartVM.OrderHeader.PostalCode = shoppingCartVM.OrderHeader.ApplicationUsers.PostalCode;
                shoppingCartVM.OrderHeader.StreetAddress = shoppingCartVM.OrderHeader.ApplicationUsers.StreetAddress;
                shoppingCartVM.OrderHeader.Email = shoppingCartVM.OrderHeader.ApplicationUsers.Email;
            }
            else
            {
                return RedirectPermanent("~/Identity/Account/Login?returnUrl=/cart/CheckOut");
            }
            //shoppingCartVM.OrderHeader.PhoneNumber = int.Parse(shoppingCartVM.OrderHeader.ApplicationUsers.PhoneNumber);
            
            foreach (var item in shoppingCartVM.LstshoppingCarts)
            {
                shoppingCartVM.CartTotal += (item.Count * (int)item.Product.Price);
                shoppingCartVM.OrderHeader.OderTotal += item.Count * (int)item.Product.Price;
            }

            return View(shoppingCartVM);
        }
        [ActionName("CheckOut")]
        [HttpPost]
        public async Task<IActionResult> CheckOutOder(string payment)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCartVM.OrderHeader.ApplicationUsers = unitOfWork.ApplicationUser.GetById(i => i.Id == claim.Value);
            shoppingCartVM.LstshoppingCarts = unitOfWork.ShoppingCart.GetAllWhere(u => u.ApplicationUserId == claim.Value, include: "Product");
            shoppingCartVM.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            shoppingCartVM.OrderHeader.OderStatus = SD.StatusPending;
            shoppingCartVM.OrderHeader.ApplicationUserId = claim.Value;
            shoppingCartVM.OrderHeader.OderDate = DateTime.Now;
            shoppingCartVM.OrderHeader.PaymentDate = DateTime.Now;
            shoppingCartVM.OrderHeader.PaymentOutDate = DateTime.Now.AddDays(14);
            shoppingCartVM.OrderHeader.ShippingDate = DateTime.Now;
            shoppingCartVM.OrderHeader.PaymentIntenId = payment;
            shoppingCartVM.OrderHeader.Tracking = "null";
            if (shoppingCartVM.OrderHeader.FirstName == null
                || shoppingCartVM.OrderHeader.LastName == null
                || shoppingCartVM.OrderHeader.Email == null
                || shoppingCartVM.OrderHeader.PhoneNumber == null
                || shoppingCartVM.OrderHeader.StreetAddress == null
                || shoppingCartVM.OrderHeader.City == null
                || shoppingCartVM.OrderHeader.State == null)
            {
                return View(shoppingCartVM);
            }
            if (payment == null)
            {
                ViewBag.error = "payment";
                return View(shoppingCartVM);
            }
            foreach (var item in shoppingCartVM.LstshoppingCarts)
            {
                shoppingCartVM.CartTotal += (item.Count * (int)item.Product.Price);
            }
            shoppingCartVM.OrderHeader.OderTotal = shoppingCartVM.CartTotal;
            shoppingCartVM.OrderHeader.OderTotal += 10000;
            unitOfWork.OderHeader.Add(shoppingCartVM.OrderHeader);
            unitOfWork.Save();
            await _hubContext.Clients.All.SendAsync("LoadOrderHeader");
            foreach (var item in shoppingCartVM.LstshoppingCarts)
            {
                OderDetail oderDetail = new OderDetail();
                oderDetail.ProductId = item.ProductId;
                oderDetail.Product = item.Product;
                oderDetail.OderHeaderId = shoppingCartVM.OrderHeader.Id;
                oderDetail.OderHeaders = shoppingCartVM.OrderHeader;
                oderDetail.Price = item.Product.Price;
                oderDetail.Color = item.Color;
                oderDetail.Count = item.Count;
                oderDetail.Size = item.Size;
                oderDetail.Tittle = item.Product.Title;
                unitOfWork.OderDetail.Add(oderDetail);
                var product = unitOfWork.Product.GetById(i => i.Id == item.ProductId);
                product.Warehouse -= item.Count;
                unitOfWork.Product.Update(product);
                unitOfWork.Save();
                await _hubContext.Clients.All.SendAsync("LoadOrderHeader");

            }
            unitOfWork.ShoppingCart.DeleteRange(shoppingCartVM.LstshoppingCarts);
            unitOfWork.Save();
            OderHeader oderHeaderComfirm = unitOfWork.OderHeader.GetById(i => i.Id == shoppingCartVM.OrderHeader.Id);
            var oderDetailComfirm = unitOfWork.OderDetail.GetAllWhere(i => i.OderHeaderId == shoppingCartVM.OrderHeader.Id);
            string bodyMessage = "";
            foreach (var item in oderDetailComfirm)
            {
                bodyMessage += " - " + item.Tittle;
                bodyMessage += " - " + item.Count;
                bodyMessage += " - " + item.Size;
                bodyMessage += " - " + item.Price;
                bodyMessage += " - " + item.Color;
            }
            if (payment == "2")
            {
                
                var url = _vnPayService.CreatePaymentUrl(oderHeaderComfirm, HttpContext);
                //TwilioClient.Init(twilioSetting.AccountSid, twilioSetting.AuthoToken);
                //try
                //{
                    //var message = MessageResource.Create(
                        //body: "Oder placed on Van Khanh and Tue Tam. Your oder ID:" + shoppingCartVM.OrderHeader.Id + bodyMessage + "Payment with Bank VNPay.",
                        //from: new Twilio.Types.PhoneNumber(twilioSetting.PhoneNumber),
                        //to: new Twilio.Types.PhoneNumber("+84" + oderHeaderComfirm.PhoneNumber.ToString())
                        //);
                //}
                //catch
                //{

                //}
                return Redirect(url); 
            }else if(payment == "3")
            {
                //TwilioClient.Init(twilioSetting.AccountSid, twilioSetting.AuthoToken);
                //try
                //{
                    //var message = MessageResource.Create(
                        //body: "Oder placed on Van Khanh and Tue Tam. Your oder ID:" + shoppingCartVM.OrderHeader.Id + bodyMessage + "Payment on delivery.",
                        //from: new Twilio.Types.PhoneNumber(twilioSetting.PhoneNumber),
                        //to: new Twilio.Types.PhoneNumber("+84" + oderHeaderComfirm.PhoneNumber.ToString())
                        //);
                    //Console.WriteLine(message.Sid);
                //}
                //catch
                //{

                //}
            }
            return RedirectToAction("OderSuccess", new { id = shoppingCartVM.OrderHeader.Id });
        }

        public IActionResult OderSuccess(int id)
        {
            var oderHeader = unitOfWork.OderHeader.GetById(i => i.Id == id);
            ViewBag.OderTotalSucc = oderHeader.OderTotal;
            ViewBag.TrackOrder = oderHeader.OderStatus;
            ViewBag.OrderNo = oderHeader.Id;
            return View(unitOfWork.OderDetail.GetAllWhere(i => i.OderHeaderId == id));
        }

        public IActionResult PurchaseOrder(int? page)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            int pageSize = 9;
            int pageNumber = page == null || page <= 0 ? 1 : page.Value;
            return View(unitOfWork.OderDetail.GetAll(include: "OderHeaders,Product").Where(i => i.OderHeaders.ApplicationUserId == claim.Value).ToPagedList(pageNumber, pageSize));
        }

        public IActionResult DetailOrder(int id)
        {
            var oderheaderVM = new OderheaderVM();
            oderheaderVM.oderHeader = unitOfWork.OderHeader.GetById(i => i.Id == id);
            oderheaderVM.oderDetails = unitOfWork.OderDetail.GetAll(include: "OderHeaders,Product").Where(i => i.OderHeaderId == id);
            return View(oderheaderVM);
        }



        public IActionResult Cancelled(int id)
        {
            var oderHeader = unitOfWork.OderHeader.GetById(i => i.Id == id);
            if (oderHeader.OderStatus == "Pending" || oderHeader.OderStatus == "Approved")
            {
                oderHeader.OderStatus = "Cancelled";
                unitOfWork.OderHeader.updateOderHeader(oderHeader);
                unitOfWork.Save();
                notyfService.Success("Đã hủy đơn hàng thành công.");
                return RedirectToAction("PurchaseOrder");
            }
            else
            {
                notyfService.Error("Đơn hàng này không thể hủy.");
                return RedirectToAction("PurchaseOrder");
            }
        }
        
    }
}
