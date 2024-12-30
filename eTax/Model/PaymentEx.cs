using System;

namespace eTaxAPI.Model
{
    public class PaymentEx
    {
        public int InvoiceNo { get; set; }
        public string ReceiptNo { get; set; }
        public string CheckNo { get; set; }
        public string BankCode { get; set; }
        public string PaymentMethod { get; set; }
    }

    public class UnifiedPaymentEx
    {
        public int Bill_Id { get; set; }
        //public decimal Amount { get; set; }
        //public decimal Currency { get; set; }
        //public string First_Name { get; set; }
        //public string Father_Name { get; set; }
        //public string Full_Name { get; set; }
        //public string Tin_Number { get; set; }
        //public string Credit_Acct_Number { get; set; }
        //public DateTime Timestamp { get; set; }
        //public int Status { get; set; }
        //public string Phone_No { get; set; }
        //public string Cheque_No { get; set; }
        //public string Bank_Code { get; set; }
        //public string Payment_Method { get; set; }
        //public string DEST_UserName { get; set; }
        //public string DEST_Password { get; set; }

        public string Destination_Api_Name { get; set; }
        public string End_To_End_Txn_Id { get; set; }
        public string Cbe_Txn_Ref { get; set; }
    }
}
