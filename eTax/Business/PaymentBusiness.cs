using System;
using eTaxAPI.DataAccess;
using eTaxAPI.Model;

namespace eTaxAPI.Business
{
    public class PaymentBusiness
    {
        public DateTime GetTodayDate()
        {
            var objPaymentDataAcess = new CPayment();
            return objPaymentDataAcess.GetTodayDate();
        }

        public PaymentReturnValue GetPaymentDetailByInvoiceNoEx(int OrderNo)
        {
            var objPaymentInfoDataAccess = new CtblOrder();
            return objPaymentInfoDataAccess.GetOrderByInvoiceNo(OrderNo);
        }

        public UnifiedPaymentReturnValue GetUnifiedPaymentDetailByBillNoEx(int OrderNo)
        {
            var objPaymentInfoDataAccess = new CtblOrder();
            return objPaymentInfoDataAccess.GetUnifiedOrderByBillNo(OrderNo);
        }
        public UnifiedPaymentReturnValue GetUnifiedPaymentDetailByBillNoExForDars(int OrderNo)
        {
            var objPaymentInfoDataAccess = new CtblOrder();
            return objPaymentInfoDataAccess.GetOrderByInvoiceNoCBE(OrderNo);
        }

        public PaymentReturnValue GetPaymentDetailByInvoiceNo(int OrderNo, string ETSRequestID)
        {
            var objPaymentInfoDataAccess = new CtblOrder();
            return objPaymentInfoDataAccess.GetOrderByInvoiceNoForEtSwitch(OrderNo, ETSRequestID);
        }
    }
}