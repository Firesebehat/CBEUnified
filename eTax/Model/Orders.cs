using System;

namespace eTaxAPI.Model
{
    public class Orders
    {
        public int OrderId { get; set; }
        public DateTime OrderedDate { get; set; }
        public Guid ServiceGuid { get; set; }
        public Guid ServiceApplicationGuid { get; set; }
        public int InvoiceNo { get; set; }
        public string ReceiptNo { get; set; }
        public int TaxPayerId { get; set; }
        public bool IsPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public string CheckNo { get; set; }
        public int ServiceID { get; set; }
        public string LicenceNo { get; set; }
        public bool IsVoid { get; set; }
        public int PaymentReason { get; set; }
        public int AdditionalReason { get; set; }
        public string Description1 { get; set; }
        public bool IsOther { get; set; }
        public string Session { get; set; }
        public DateTime RenewalFrom { get; set; }
        public DateTime RenewalTo { get; set; }
        public bool PaidAll { get; set; }
        public string OrderedBy { get; set; }
        public string CashierUserName { get; set; }
        public string Remark { get; set; }
        public int NextService { get; set; }
        public bool Posted { get; set; }
        public string VoucherNo { get; set; }
        public DateTime PostedDate { get; set; }
        public bool IsFederal { get; set; }
        public string SiteID { get; set; }
        public string UserName { get; set; }
        public string UpdatedUsername { get; set; }
        public string Main_guid { get; set; }
        public string Parent_guid { get; set; }
        public string BankCode { get; set; }
        public string BankName { get; set; }
        public string PaymentMethod { get; set; }
        public string ModeOfCollection { get; set; }

        public string TellNo { get; set; }
        public string MobileNo { get; set; }
        public decimal TotalPayment { get; set; }
        public string Tin { get; set; }
        public string AccountNo { get; set; }
        public string ServiceName { get; set; }
        public string ServiceNameEnglish { get; set; }
        public decimal Amount { get; set; }

    }
}