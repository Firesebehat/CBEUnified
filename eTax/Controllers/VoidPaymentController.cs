using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using eTaxAPI.ActionFilters;
using eTaxAPI.DataAccess;
using eTaxAPI.Model;
using eTaxAPI.Repository;

namespace eTaxAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    [AuthorizationRequired]
    [eTaxAPIExceptionFilter]
    public class VoidPaymentController : ApiController
    {
        [Route("api/VoidPayment/VoidByReceiptNo/{ReceiptNo}")]
        [HttpPut]
        public bool VoidByReceiptNo(string receiptNo)
        {
            string ErrorMessage = "";
            int ErrorCode = 200;
            try
            {
                if (!AccessRight.CanAccessTransactionResourceForVoid())
                {
                    ErrorCode = 402;
                    ErrorMessage = "User has no right to access this resource";
                    var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new StringContent("User has no right to access this resource"),
                        ReasonPhrase = "User has no right to access this resource"
                    };
                    throw new HttpResponseException(msg);
                }

                Result r = VoidRepository.SaveVoid(receiptNo);
                if (r.IsSaved == true)
                    return true;
                else
                {
                    ErrorMessage = r.ErrorMessage;
                    ErrorCode = r.ErrorCode;
                    var msg = new HttpResponseMessage();
                    throw new HttpResponseException(msg);
                }
            }
            catch
            {
                var msg = new HttpResponseMessage
                {
                    Content = new StringContent(ErrorMessage),
                    ReasonPhrase = ErrorMessage,
                    StatusCode = (HttpStatusCode)ErrorCode
                };
                throw new HttpResponseException(msg);
            }
        }


        [Route("api/VoidPaymentByInvoiceNo/{InvoiceNo}")]
        [HttpPut]
        public bool VoidPayment(string InvoiceNo)
        {
            string ErrorMessage = "";
            int ErrorCode = 200;
            try
            {
                if (!AccessRight.CanAccessTransactionResourceForVoid())
                {
                    ErrorCode = 402;
                    ErrorMessage = "User has no right to access this resource";

                    var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new StringContent("User has no right to access this resource"),
                        ReasonPhrase = "User has no right to access this resource"
                    };
                    throw new HttpResponseException(msg);
                }

                //if (InvoiceNo.Length > 9)
                //{
                //    var msg = new HttpResponseMessage();
                //    ErrorMessage = "Invoice number should not exceed nine digits";
                //    ErrorCode = (int)HttpStatusCode.LengthRequired;
                //    throw new HttpResponseException(msg);
                //}
                int OrderNo = 0;
                try
                {
                    OrderNo = Convert.ToInt32(InvoiceNo);
                }
                catch
                {
                    var msg = new HttpResponseMessage();
                    //ErrorMessage = "Character is not allowed in Invoice number";
                    //ErrorCode = (int)HttpStatusCode.Ambiguous;
                    ErrorMessage = "Invoice No. doesn't exist";
                    ErrorCode = (int)HttpStatusCode.SwitchingProtocols;
                    throw new HttpResponseException(msg);
                }
                
                Result r = VoidRepository.VoidPayment(OrderNo);
                if (r.IsSaved == true)
                    return true;
                else
                {
                    ErrorMessage = r.ErrorMessage;
                    ErrorCode = r.ErrorCode;
                    var msg = new HttpResponseMessage();
                    throw new HttpResponseException(msg);
                }
            }
            catch
            {
                var msg = new HttpResponseMessage
                {
                    Content = new StringContent(ErrorMessage),
                    ReasonPhrase = ErrorMessage,
                    StatusCode = (HttpStatusCode)ErrorCode
                };
                throw new HttpResponseException(msg);
            }
        }
    }
}