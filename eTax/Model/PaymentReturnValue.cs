using System;
using System.Net;

namespace eTaxAPI.Model
{
    public class PaymentReturnValue
    {
        public int OrderId { get; set; }
        public DateTime OrderedDate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsVoid { get; set; }
        public bool IsPaid { get; set; }
        public string CustomerName { get; set; }
        public string ServiceTaken { get; set; }
        public string Tin { get; set; }
        public string AccountNumber { get; set; }

        public int ErrorCode { get; set; }
        public int NoOfDays { get; set; }
        public string ErrorMessage { get; set; }

    }

    public class PaymentReturnValueEtSwitch : PaymentReturnValue
    {
        public string ETSRequestID { get; set; }
    }
    public class UnifiedPaymentReturnValue
    {
        //Unified Columns
        public int Bill_Id { get; set; }
        public decimal Penalty_Amount { get; set; }
        public decimal Bill_Amount { get; set; }
        public decimal Total_Amount { get; set; }

        public string First_Name { get; set; }
        public string Last_Name { get; set; }
        public string Full_Name { get; set; }
        public string Payment_Reason { get; set; }
        public string Tin_Number { get; set; }
        public string Credit_Acct_Number { get; set; }
        public DateTime Timestamp { get; set; }
        public string Status { get; set; }
        public string Transaction_Type { get; set; }
        public int Response_Code { get; set; }
        public string Response_Description { get; set; }


        public string Destination_Api_Name { get; set; }
        public string End_To_End_Txn_Id { get; set; }
        //public string DEST_UserName { get; set; }
        //public string DEST_Password { get; set; }


        //public int ErrorCode { get; set; }
        ////public int NoOfDays { get; set; }
        //////public string ErrorMessage { get; set; }
        ////public DateTime OrderedDate { get; set; }
        ////public bool IsPaid { get; set; }
        ////public bool IsVoid { get; set; }
    }
}