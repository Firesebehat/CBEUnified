using System;

namespace eTaxAPI.Model
{
    public class PaymentList
    {
        public int PaymentOrderNo { get; set; }
        public string CashierName { get; set; }
        public string ReceiptNo { get; set; }
        public string ServiceApplicationGuid { get; set; }
        public string CustomerName { get; set; }
        public string Tin { get; set; }
        public decimal PaidAmount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string ChequeNo { get; set; }
        public decimal TotalPayment { get; set; }
    }
}