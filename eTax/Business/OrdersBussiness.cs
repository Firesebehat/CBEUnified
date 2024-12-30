using System;
using System.Collections.Generic;
using System.Data;
using eTaxAPI.DataAccess;
using eTaxAPI.Model;

namespace eTaxAPI.Business
{
    public class OrdersBussiness
    {
        public bool UpdateEx(Orders objOrders)
        {
            var objOrdersDataAcess = new CtblOrder();

            return objOrdersDataAcess.UpdateEx(objOrders);
        }
        public bool UpdateDarsEx(Orders objOrders)
        {
            var objOrdersDataAcess = new CtblOrder();

            return objOrdersDataAcess.UpdateDarsEx(objOrders);
        }
        public bool VoidPaymentByOrderId(int intID)
        {
            var objOrdersDataAcess = new CtblOrder();
            return objOrdersDataAcess.VoidPaymentByOrderId(intID);
        }
        public bool VoidPayment(string strReceiptNo, string strInvoiceNo)
        {
            var objOrdersDataAcess = new CtblOrder();
            return objOrdersDataAcess.VoidPayment(strReceiptNo, strInvoiceNo);
        }
        public Orders GetPaymentByReceiptNo(string strReceiptNo)
        {
            var objOrdersDataAcess = new CtblOrder();
            return objOrdersDataAcess.GetPaymentByReceiptNo(strReceiptNo);
        }
        public Orders GetOrderByOrderNo(int OrderNo)
        {
            var objOrdersDataAcess = new CtblOrder();
            return objOrdersDataAcess.GetOrderByOrderNo(OrderNo);
        }
        public bool CheckReceiptNoExist(string ReceiptNo)
        {
            var objOrdersDataAcess = new CtblOrder();
            return objOrdersDataAcess.CheckReceiptNoExist(ReceiptNo);
        }
        public bool CheckPaymentIsDoneBythisPaymentOrderNo(int OrderNo)
        {
            var objOrdersDataAcess = new CtblOrder();
            return objOrdersDataAcess.CheckPaymentIsDoneBythisPaymentOrderNo(OrderNo);
        }
        public bool isValidOrderNo(int OrderNo)
        {
            var objOrdersDataAcess = new CtblOrder();
            return objOrdersDataAcess.isValidOrderNo(OrderNo);
        }
        public bool isViodOrderNo(int OrderNo)
        {
            var objOrdersDataAcess = new CtblOrder();
            return objOrdersDataAcess.isViodOrderNo(OrderNo);
        }

        public Tuple<string, string, decimal> GetTaxPayerInfo(int OrderNo)
        {
            var objOrdersDataAcess = new CtblOrder();
            return objOrdersDataAcess.GetTaxPayerInfo(OrderNo);
        }
        public Orders GetServiceApplicationDetailByInvoiceNo(int OrderNo)
        {
            var objOrdersDataAcess = new CtblOrder();
            return objOrdersDataAcess.GetServiceApplicationDetailByInvoiceNo(OrderNo);
        }
        public Orders GetDetailByInvoiceNo(int OrderNo)
        {
            var objOrdersDataAcess = new CtblOrder();
            return objOrdersDataAcess.GetDetailByInvoiceNo(OrderNo);
        }
        //public List<PaymentList> GetPayments(DateTime PaymentDateFrom, DateTime PaymentDateTo)
        //{
        //    var objOrdersDataAcess = new CtblOrder();
        //    return objOrdersDataAcess.GetPayments(PaymentDateFrom, PaymentDateTo);
        //}
        public List<PaymentList> GetPayments(DateTime PaymentDateFrom, DateTime PaymentDateTo, bool IsVoid, string BankCode)
        {
            var objOrdersDataAcess = new CtblOrder();
            return objOrdersDataAcess.GetPayments(PaymentDateFrom, PaymentDateTo, IsVoid, BankCode);
        }
    }
}