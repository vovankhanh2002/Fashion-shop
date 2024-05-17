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
    public class OderHeaderController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private IWebHostEnvironment _webHostEnvironment;
        private readonly INotyfService _notyfService;
        private readonly IHubContext<SignalsServer> _hubContext;
        private OderheaderVM oderheaderVM { get; set; }

        public OderHeaderController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, INotyfService notyfService, IHubContext<SignalsServer> hubContext )
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _notyfService = notyfService;
            _hubContext = hubContext;
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
                return Json(new { data = _unitOfWork.OderHeader.GetAll().Where(i => i.LastName.Contains(strSearch)) });
            }
            return Json(new { data = _unitOfWork.OderHeader.GetAll() });
        }
        [HttpGet]
        public IActionResult getOrderHeader()
        {
            var res = _unitOfWork.OderHeader.GetAll();
            return Ok(res);
        }
        [HttpGet]
        public IActionResult getOrderDetail()
        {
            var res = _unitOfWork.OderDetail.GetAll();
            return Ok(res);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            _unitOfWork.OderHeader.Delete(_unitOfWork.OderHeader.GetById(n => n.Id == id));
            _unitOfWork.OderDetail.DeleteRange(_unitOfWork.OderDetail.GetAll().Where(i => i.Id == id));
            _unitOfWork.Save();
            await _hubContext.Clients.All.SendAsync("LoadOrderHeader");
            return Json(new { success = true});
        }
        public IActionResult getByIdDetail(int id)
        {
            var oderheaders = _unitOfWork.OderHeader.GetById(i => i.Id == id);
            var oderdetail = _unitOfWork.OderDetail.GetAllWhere(i => i.OderHeaderId == id, include: "Product");
            oderheaderVM = new OderheaderVM();
            oderheaderVM.OderheaderListItems = null;
            oderheaderVM.oderHeader = oderheaders;
            oderheaderVM.oderDetails = oderdetail;
            return View(oderheaderVM);
        }
        [HttpPost]
        public IActionResult getByIdDetailSuccess(int id)
        {
            return Json(new { success = true, data = id });
        }

        public IActionResult Cancelled(int id)
        {
            var oderHeader = _unitOfWork.OderHeader.GetById(i => i.Id == id);
            if(oderHeader.OderStatus != SD.StatusPending || oderHeader.OderStatus != SD.StatusApproved || oderHeader.OderStatus != SD.StatusSuccess)
            {
                oderHeader.OderStatus = SD.StatusCancelled;
                _unitOfWork.OderHeader.updateOderHeader(oderHeader);
                _unitOfWork.Save();
                _notyfService.Success("Đã hủy đơn hàng thành công.");
                return RedirectToAction("getByIdDetail", new {id = id});
            }
            else
            {
                _notyfService.Error("Đơn hàng này không thể hủy.");
                return RedirectToAction("getByIdDetail", new { id = id });
            }
        }
        public IActionResult Approved(int id)
        {
            var oderHeader = _unitOfWork.OderHeader.GetById(i => i.Id == id);
                oderHeader.OderStatus = SD.StatusApproved;
                _unitOfWork.OderHeader.updateOderHeader(oderHeader);
                _unitOfWork.Save();
            _notyfService.Success("Đơn hàng đã hoàn tất chuẩn bị gửi.");
            return RedirectToAction("getByIdDetail", new { id = id });
        }
        public IActionResult Processing(int id)
        {
            var oderHeader = _unitOfWork.OderHeader.GetById(i => i.Id == id);
                oderHeader.OderStatus = SD.StatusInProcess;
                _unitOfWork.OderHeader.updateOderHeader(oderHeader);
                _unitOfWork.Save();
            _notyfService.Success("Đơn hàng đã gửi đi.");
            return RedirectToAction("getByIdDetail", new { id = id });
        }
        public IActionResult Shipped(int id)
        {
            var oderHeader = _unitOfWork.OderHeader.GetById(i => i.Id == id);
            oderHeader.OderStatus = SD.StatusShipped;
            _unitOfWork.OderHeader.updateOderHeader(oderHeader);
            _unitOfWork.Save();
            _notyfService.Success("Đơn hàng đang giao đến.");
            return RedirectToAction("getByIdDetail", new { id = id });
        }
        public IActionResult Success(int id)
        {
            var oderHeader = _unitOfWork.OderHeader.GetById(i => i.Id == id);
            oderHeader.OderStatus = SD.StatusSuccess;
            _unitOfWork.OderHeader.updateOderHeader(oderHeader);
            _unitOfWork.Save();
            _notyfService.Success("Đã giao hàng.");
            return RedirectToAction("getByIdDetail", new { id = id });
        }
        public IActionResult Rejected(int id)
        {
            var oderHeader = _unitOfWork.OderHeader.GetById(i => i.Id == id);
            if (oderHeader.PaymentStatus != SD.StatusApproved)
            {
                oderHeader.PaymentStatus = SD.PaymentStatusRejected;
                _unitOfWork.OderHeader.updateOderHeader(oderHeader);
                _unitOfWork.Save();
                _notyfService.Success("Đã hủy đơn hàng thành công.");
                return RedirectToAction("getByIdDetail", new { id = id });
            }
            else
            {
                _notyfService.Error("Đơn hàng này đã thanh toán không thể hủy.");
                return RedirectToAction("getByIdDetail", new { id = id });
            }
        }
        public IActionResult PaymentApproved(int id)
        {
            var oderHeader = _unitOfWork.OderHeader.GetById(i => i.Id == id);
            oderHeader.PaymentStatus = "Approved";
            _unitOfWork.OderHeader.updateOderHeader(oderHeader);
            _unitOfWork.Save();
            _notyfService.Success("Đơn hàng đã được thanh toán.");
            return RedirectToAction("getByIdDetail", new { id = id });
        }
        public IActionResult PaymentStatusDelayPayment(int id)
        {
            var oderHeader = _unitOfWork.OderHeader.GetById(i => i.Id == id);
            oderHeader.PaymentStatus = "PaymentStatusDelayPayment";
            _unitOfWork.OderHeader.updateOderHeader(oderHeader);
            _unitOfWork.Save();
            _notyfService.Success("Đơn hàng này đang trễ thanh toán.");
            return RedirectToAction("getByIdDetail", new { id = id });
        }
    }
}

