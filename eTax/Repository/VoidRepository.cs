using System;
using eTaxAPI.Business;
using eTaxAPI.Model;
using eTaxAPI.DataAccess;

namespace eTaxAPI.Repository
{
    public class VoidRepository
    {
        public static Result SaveVoid(string strReceiptNo)
        {
            var r = new Result();
            try
            {
                var objorderBusiness = new OrdersBussiness();
                var objorder = new Orders();
                objorder = objorderBusiness.GetPaymentByReceiptNo(strReceiptNo);
                if (objorder == null)
                {
                    r.IsSaved = false;
                    r.ErrorMessage = "Order not found for Recipit No :'" + strReceiptNo + "'";
                    return r;
                }
               
                if (objorder.SiteID != AccessRight.GetLocation())
                {
                    r.IsSaved = false;
                    r.ErrorMessage = "Sorry, You are not allowed to void transaction prepared by another site";
                    return r;
                }
                if (objorder.IsVoid == true)
                {
                    r.IsSaved = false;
                    r.ErrorMessage = "Sorry, The transaction is already void";
                    return r;
                }
                //}

                r.IsSaved = objorderBusiness.VoidPayment(strReceiptNo, objorder.OrderId.ToString());
                r.ErrorMessage = r.IsSaved ? string.Empty : "Record was not saved";
                return r;
            }
            catch (Exception ex)
            {
                r.IsSaved = false;
                r.ErrorMessage = ex.Message;
                //throw new Exception("Error in SavePayment - " + ex.Message);
                return r;
            }
        }

        public static Result VoidPayment(int intOrderId)
        {
            var r = new Result();
            try
            {
                var objorderBusiness = new OrdersBussiness();
                var objorder = new Orders();
                objorder = objorderBusiness.GetOrderByOrderNo(intOrderId);

                if (objorder == null)
                {
                    r.IsSaved = false;
                    r.ErrorMessage = "Order not found!!!";
                    return r;
                }
                //else
                //{
                //    if (objorder.Paid == true)
                //    {
                //        r.IsSaved = false;
                //        r.ErrorMessage = "Sorry, You are not allowed to void already paid transactions!";
                //        return r;
                //    }

                if (objorder.SiteID != AccessRight.GetLocation())
                {
                    r.IsSaved = false;
                    r.ErrorMessage = "Sorry, You are not allowed to void transaction prepared by another site";
                    return r;
                }

                if (objorder.IsVoid == true)
                {
                    r.IsSaved = false;
                    r.ErrorMessage = "Sorry, The transaction is already void";
                    return r;
                }
                //}

                r.IsSaved = objorderBusiness.VoidPaymentByOrderId(intOrderId);
                r.ErrorMessage = r.IsSaved ? string.Empty : "Record was not saved";
                return r;
            }
            catch (Exception ex)
            {
                r.IsSaved = false;
                r.ErrorMessage = ex.Message;
                //throw new Exception("Error in SavePayment - " + ex.Message);
                return r;
            }
        }
    }
}