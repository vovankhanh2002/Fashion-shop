using BanDoWeb.Model.Models;
using CodeMegaVNPay.Models;
using Microsoft.AspNetCore.Http;

namespace CodeMegaVNPay.Services;
public interface IVnPayService
{
    string CreatePaymentUrl(OderHeader model, HttpContext context);
    PaymentResponseModel PaymentExecute(IQueryCollection collections);
}