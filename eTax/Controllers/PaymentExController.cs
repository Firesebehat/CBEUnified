using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Results;
using eTaxAPI.ActionFilters;
using eTaxAPI.Business;
using eTaxAPI.DataAccess;
using eTaxAPI.Model;
using eTaxAPI.Repository;

namespace eTaxAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    [AuthorizationRequired]
    [eTaxAPIExceptionFilter]
    public class PaymentExController : ApiController
    {
        [HttpPost]
        [Route("Api/UnifiedPaymentEx")]
        public UnifiedResult UnifiedPaymentEx([FromBody] UnifiedPaymentEx pmt)
        {
            //if (!AccessRight.CanAccessPaymentResource())
            //{
            //    var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            //    {
            //        Content = new StringContent("User has no right to access this resource"),
            //        ReasonPhrase = "User has no right to access this resource"
            //    };
            //    throw new HttpResponseException(msg);
            //}
            var UnifiedResult = new UnifiedResult();
            if (Convert.ToBoolean(ConfigurationManager.AppSettings["AddisEnabled"]) || Convert.ToBoolean(ConfigurationManager.AppSettings["DireDawaEnabled"]) || Convert.ToBoolean(ConfigurationManager.AppSettings["SomaliEnabled"]))
            {
                UnifiedResult = PaymentRepository.SaveUnifiedPaymentEx(pmt);
            }
            else if (Convert.ToBoolean(ConfigurationManager.AppSettings["DarsEnabled"]) || Convert.ToBoolean(ConfigurationManager.AppSettings["MotriEnabled"]))
            {
                UnifiedResult = PaymentRepository.SaveUnifiedPaymentExForDars(pmt);
            }
            else
            {
                UnifiedResult = PaymentRepository.SaveUnifiedPaymentEx(pmt);
            }
            //if (UnifiedResult.Status != "Success")
            //{
            //    //HttpRequestMessage Request = null;

            //    //result.Response_Description = "AuthToken Is not correct";
            //    //result.Response_Code = 200;
            //    //result.Status = "Failure";
            //    //return Request.CreateResponse(HttpStatusCode.OK, result);

            //    var UnifiedResult = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            //    {
            //        Content = new StringContent(UnifiedResult.Response_Description),
            //        ReasonPhrase = UnifiedResult.Response_Description,
            //        StatusCode = (HttpStatusCode)UnifiedResult.Response_Code
            //    };

            //    return msg;

            //}
            return UnifiedResult;
        }


        /// <summary>
        ///     Gets a penalty record by payment order number (Bill_Id) including Business Registration name
        /// </summary>
        /// <param name="Bill_Id" - a Unique Payment Order Number for a penalty record.></param>
        /// <returns>A JSON Object representing an offense record  </returns>
        /// 
        [HttpPost]
        //[Route("Api/GetUnifiedPaymentDetailByBillNoEx/{Bill_Id}/{End_To_End_Txn_Id}/{Destination_Api_Name='PaymentAPI'}/{DEST_UserName?}/{DEST_Password?}")]
        [Route("Api/GetUnifiedPaymentDetailByBillNoEx")]
        //public HttpResponseMessage GetUnifiedPaymentDetailByBillNoEx(int Bill_Id, string End_To_End_Txn_Id, string Destination_Api_Name = null, string DEST_UserName = null, string DEST_Password = null)
        public HttpResponseMessage GetUnifiedPaymentDetailByBillNoEx([FromBody] UnifiedPaymentEx pmt)
        {
            //string result.Response_Description = "";
            //int result.Response_Code  = 200;
            UnifiedResult result = new UnifiedResult();

            try
            {
                //Result r = new Result();
                try
                {
                    int OrderNo = Convert.ToInt32(pmt.Bill_Id);
                }
                catch
                {
                    var msg = new HttpResponseMessage();
                    result.Status = "Failure";
                    result.Response_Description = "Character is not allowed in Invoice number";
                    result.Response_Code = 401;
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                if (ConfigurationManager.AppSettings["ClinetId"] != HttpContext.Current.Session["CurrentUser"].ToString())
                {
                    result.Response_Description = "ClinetId Is not correct";
                    result.Response_Code = 401;
                    result.Status = "Failure";
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }

                //if (pmt.End_To_End_Txn_Id != HttpContext.Current.Session["AuthToken"].ToString())
                //{
                //    result.Response_Description = "AuthToken Is not correct";
                //    result.Response_Code = 200;
                //    result.Status = "Failure";
                //    return Request.CreateResponse(HttpStatusCode.OK, result);
                //}
                var objPayment = new PaymentBusiness();
                var paymentDetail = new UnifiedPaymentReturnValue();

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["AddisEnabled"]) || Convert.ToBoolean(ConfigurationManager.AppSettings["DireDawaEnabled"]) || Convert.ToBoolean(ConfigurationManager.AppSettings["SomaliEnabled"]))
                {
                    paymentDetail = objPayment.GetUnifiedPaymentDetailByBillNoEx(Convert.ToInt32(pmt.Bill_Id));
                }
                else if (Convert.ToBoolean(ConfigurationManager.AppSettings["DarsEnabled"]) || Convert.ToBoolean(ConfigurationManager.AppSettings["MotriEnabled"]))
                {
                    paymentDetail = objPayment.GetUnifiedPaymentDetailByBillNoExForDars(Convert.ToInt32(pmt.Bill_Id));
                }
                else
                {
                    paymentDetail = objPayment.GetUnifiedPaymentDetailByBillNoEx(Convert.ToInt32(pmt.Bill_Id));
                }
                paymentDetail.End_To_End_Txn_Id = pmt.End_To_End_Txn_Id; //HttpContext.Current.Session["tokenValue"].ToString();

                //if (paymentDetail.Response_Code == 200)
                //{

                //    if (paymentDetail.IsVoid == true)
                //    {
                //        result.Response_Description = "Sorry, The Payment is already void!";
                //        result.Response_Code = (int)HttpStatusCode.Forbidden;
                //        //var msg = new HttpResponseMessage();
                //        return Request.CreateResponse(HttpStatusCode.Forbidden, result);
                //    }
                //    if (paymentDetail.IsPaid == true)
                //    {
                //        result.Response_Description = "The Payment is already made by this payment order number!";
                //        result.Response_Code = (int)HttpStatusCode.Forbidden;
                //        //var msg = new HttpResponseMessage();
                //        return Request.CreateResponse(HttpStatusCode.Forbidden, result);
                //    }
                //    if (paymentDetail.NoOfDays > 30)
                //    {
                //        result.Response_Description = "Sorry, The Payment is already Passed 30 days,Please back to Revenue Office!!";
                //        result.Response_Code = (int)HttpStatusCode.Forbidden;
                //        //var msg = new HttpResponseMessage();
                //        return Request.CreateResponse(HttpStatusCode.Forbidden, result);
                //    }
                //}
                if (paymentDetail.Response_Code == 101)
                {
                    result.Status = "Failure";
                    result.Response_Description = "Invoice No. doesn't exist";
                    result.Response_Code = (int)HttpStatusCode.OK;
                    var msg = new HttpResponseMessage();
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                var response = Request.CreateResponse(HttpStatusCode.OK, paymentDetail);
                return response;
            }
            catch
            {
                result.Response_Description = "Payment Not Done";
                result.Response_Code = 501;
                result.Status = "Failure";
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
        }



    }
}