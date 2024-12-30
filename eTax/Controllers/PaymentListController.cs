using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using eTaxAPI.ActionFilters;
using eTaxAPI.Model;
using eTaxAPI.Repository;
using eTaxAPI.Controllers;

namespace eTaxAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    [AuthorizationRequired]
    [eTaxAPIExceptionFilter]
    public class PaymentListController : ApiController
    {
        [HttpGet]
        [Route("Api/PaymentList/{DateFrom}/{DateTo}/{IsVoid}/{BankCode}")]
        public IEnumerable<PaymentList> PaymentList(DateTime dateFrom, DateTime dateTo, bool isVoid, string BankCode)
        {
            return PaymentRepository.GetPayments(dateFrom, dateTo, isVoid, BankCode);
        }

        
    }
}