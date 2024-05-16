using BanDoWeb.Model.Models;
using CodeMegaVNPay.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BanDoWeb.Access.Repository.IRepository
{
    internal interface IPayPalService
    {
        string CreatePaymentUrl(OderHeader model, HttpContext context);
        PaymentResponseModel PaymentExecute(IQueryCollection collections);
    }
}
