using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Web;
using System.Web.Http.Results;
using eTaxAPI.Business;
using eTaxAPI.DataAccess;
using eTaxAPI.Model;

namespace eTaxAPI.Repository
{
    public class PaymentRepository
    {
        public static Result SavePaymentEx(int intPaymentOrderNo, PaymentEx pmt)
        {
            //Location = Config.GetMobileLocation(),
            var r = new Result();
            //to-do use transaction 
            // pmt.CashierName,

            if (intPaymentOrderNo != pmt.InvoiceNo)
            {
                r.ErrorCode = (int)HttpStatusCode.Redirect;
                r.ErrorMessage = "Order number in path param is different form request body";
                return r;
            }

            if (string.IsNullOrEmpty(pmt.ReceiptNo))
            {
                r.ErrorCode = (int)HttpStatusCode.RequestedRangeNotSatisfiable;
                r.ErrorMessage = "Receipt Number can not be empty";
                return r;
            }
            if (string.IsNullOrEmpty(pmt.BankCode))
                pmt.BankCode = string.Empty;
            if (string.IsNullOrEmpty(pmt.BankCode))
                pmt.BankCode = string.Empty;
            if (string.IsNullOrEmpty(pmt.CheckNo))
                pmt.CheckNo = string.Empty;
            if (string.IsNullOrEmpty(pmt.PaymentMethod))
                pmt.PaymentMethod = string.Empty;
            //if (pmt.LocationCode.Equals(null))
            //    pmt.LocationCode = 0;
            //string date = "2022-08-06";
            try
            {
                var objOrdersBusiness = new OrdersBussiness();
                var order = new Orders
                {
                    OrderId = intPaymentOrderNo,
                    InvoiceNo = intPaymentOrderNo,
                    IsPaid = true,
                    CashierUserName = GetCurrentUser(),
                    CheckNo = pmt.CheckNo ?? string.Empty,
                    ReceiptNo = pmt.ReceiptNo,
                    //Location = Convert.ToInt32(pmt.BankCode),
                    SiteID = AccessRight.GetLocation(),
                    PaymentDate = DateTime.Today,
                    BankCode = pmt.BankCode,
                    BankName = pmt.BankCode,
                    PaymentMethod = pmt.PaymentMethod
                };
                if (objOrdersBusiness.CheckPaymentIsDoneBythisPaymentOrderNo(pmt.InvoiceNo))
                {
                    r.ErrorCode = (int)HttpStatusCode.ExpectationFailed;
                    r.ErrorMessage = "The Payment is already made by this payment order number!";
                    r.IsSaved = false;
                    return r;
                }
                if (objOrdersBusiness.CheckReceiptNoExist(pmt.ReceiptNo))
                {
                    r.ErrorCode = (int)HttpStatusCode.UpgradeRequired;
                    r.ErrorMessage = "The Receipt Number has already been used!";
                    r.IsSaved = false;
                    return r;
                }
                if (!objOrdersBusiness.isValidOrderNo(pmt.InvoiceNo))
                {
                    r.ErrorCode = (int)HttpStatusCode.BadGateway;
                    r.ErrorMessage = "Error! Make Sure the Invoice Number is Valid!";
                    r.IsSaved = false;
                    return r;
                }
                if (objOrdersBusiness.isViodOrderNo(pmt.InvoiceNo))
                {
                    r.ErrorCode = (int)HttpStatusCode.ExpectationFailed;
                    r.ErrorMessage = "Sorry, The transaction is already void";
                    r.IsSaved = false;
                    return r;
                }

                var TaxPayerInfo = objOrdersBusiness.GetTaxPayerInfo(pmt.InvoiceNo);
                order.Tin = TaxPayerInfo.Item1;
                order.TellNo = TaxPayerInfo.Item2;
                order.TotalPayment = TaxPayerInfo.Item3;
                //order.

                if (objOrdersBusiness.UpdateEx(order))
                {
                    r.ErrorCode = 200;
                    r.ErrorMessage = string.Empty;
                    r.IsSaved = true;
                    return r;
                }
                else
                {
                    r.ErrorCode = (int)HttpStatusCode.InternalServerError;
                    r.ErrorMessage = "Error! Make Sure the Internet connection is availble";
                    r.IsSaved = false;
                    return r;
                }
            }
            catch (Exception ex)
            {
                r.ErrorMessage = ex.Message;
                r.IsSaved = false;
                return r;
            }
        }
        public static UnifiedResult SaveUnifiedPaymentEx(int intPaymentOrderNo, UnifiedPaymentEx pmt)
        {
            //Location = Config.GetMobileLocation(),
            var r = new UnifiedResult();
            //to-do use transaction 
            // pmt.CashierName,

            if (intPaymentOrderNo != pmt.Bill_Id)
            {
                r.Response_Code = (int)HttpStatusCode.Redirect;
                r.Response_Description = "Order number in path param is different form request body";
                return r;
            }

            if (pmt.End_To_End_Txn_Id != HttpContext.Current.Session["AuthToken"].ToString())
            {
                r.Response_Description = "ClientId Is not correct";
                r.Response_Code = 400;
                r.Status = false;
                return r;
            }

            //if (string.IsNullOrEmpty(pmt.ReceiptNo))
            //{
            //    r.Response_Code = (int)HttpStatusCode.RequestedRangeNotSatisfiable;
            //    r.Response_Description = "Receipt Number can not be empty";
            //    return r;
            //}
            if (string.IsNullOrEmpty(pmt.Bank_Code))
                pmt.Bank_Code = string.Empty;
            if (string.IsNullOrEmpty(pmt.Bank_Code))
                pmt.Bank_Code = string.Empty;
            if (string.IsNullOrEmpty(pmt.Cheque_No))
                pmt.Cheque_No = string.Empty;
            if (string.IsNullOrEmpty(pmt.Payment_Method))
                pmt.Payment_Method = string.Empty;


            //if (pmt.LocationCode.Equals(null))
            //    pmt.LocationCode = 0;
            //string date = "2022-08-06";
            try
            {
                var objOrdersBusiness = new OrdersBussiness();
                var order = new Orders
                {
                    OrderId = intPaymentOrderNo,
                    InvoiceNo = intPaymentOrderNo,
                    IsPaid = true,
                    CashierUserName = GetCurrentUser(),
                    CheckNo = pmt.Cheque_No ?? string.Empty,
                    ReceiptNo = pmt.Bill_Id.ToString(),
                    //Location = Convert.ToInt32(pmt.BankCode),
                    SiteID = AccessRight.GetLocation(),
                    PaymentDate = DateTime.Today,
                    BankCode = pmt.Bank_Code,
                    BankName = pmt.Bank_Code,
                    PaymentMethod = pmt.Payment_Method
                };
                if (objOrdersBusiness.CheckPaymentIsDoneBythisPaymentOrderNo(pmt.Bill_Id))
                {
                    r.Response_Code = (int)HttpStatusCode.ExpectationFailed;
                    r.Response_Description = "The Payment is already made by this payment order number!";
                    r.Status = false;
                    return r;
                }
                //if (objOrdersBusiness.CheckReceiptNoExist(pmt.ReceiptNo))
                //{
                //    r.Response_Code = (int)HttpStatusCode.UpgradeRequired;
                //    r.Response_Description = "The Receipt Number has already been used!";
                //    r.Status = false;
                //    return r;
                //}
                if (!objOrdersBusiness.isValidOrderNo(pmt.Bill_Id))
                {
                    r.Response_Code = (int)HttpStatusCode.BadGateway;
                    r.Response_Description = "Error! Make Sure the Invoice Number is Valid!";
                    r.Status = false;
                    return r;
                }
                if (objOrdersBusiness.isViodOrderNo(pmt.Bill_Id))
                {
                    r.Response_Code = (int)HttpStatusCode.ExpectationFailed;
                    r.Response_Description = "Sorry, The transaction is already void";
                    r.Status = false;
                    return r;
                }

                var TaxPayerInfo = objOrdersBusiness.GetTaxPayerInfo(pmt.Bill_Id);
                order.Tin = TaxPayerInfo.Item1;
                order.TellNo = TaxPayerInfo.Item2;
                order.TotalPayment = TaxPayerInfo.Item3;
                //order.

                if (objOrdersBusiness.UpdateEx(order))
                {
                    r.Response_Code = 200;
                    r.Response_Description = "Payment is done";
                    r.Status = true;
                    r.Cbe_Txn_Ref = pmt.Cbe_Txn_Ref;
                    r.Destination_Api_Name = pmt.Destination_Api_Name;
                    r.End_To_End_Txn_Id = pmt.End_To_End_Txn_Id;
                    r.Destination_txn_Ref = "Destination_txn_Ref";
                    return r;
                }
                else
                {
                    r.Response_Code = (int)HttpStatusCode.InternalServerError;
                    r.Response_Description = "Error! Make Sure the Internet connection is availble";
                    r.Status = false;
                    return r;
                }
            }
            catch (Exception ex)
            {
                r.Response_Description = ex.Message;
                r.Status = false;
                return r;
            }
        }
        private static string GetCurrentUser()
        {
            try
            {
                var strUser = (HttpContext.Current.Session["CurrentUser"].ToString());
                return !string.IsNullOrEmpty(strUser) ? strUser : string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }



        //public static PaymentList GetPaymentByReceiptNo(string strReceiptNo)
        //{
        //    var objOrd = new OrdersBussiness();
        //    try
        //    {
        //        return objOrd.GetPaymentByReceiptNo(strReceiptNo);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        public static List<PaymentList> GetPayments(DateTime PaymentDateFrom, DateTime PaymentDateTo, bool IsVoid, string UserID)
        {
            try
            {
                var objOrd = new OrdersBussiness();

                return objOrd.GetPayments(PaymentDateFrom, PaymentDateTo, IsVoid, UserID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}