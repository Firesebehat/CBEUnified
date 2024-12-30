namespace eTaxAPI.Model
{
    public class Result
    {
        public bool IsSaved { get; set; }
        public string ErrorMessage { get; set; }
        public int ErrorCode { get; set; }


    }
    public class UnifiedResult
    {
        //public bool IsSaved { get; set; }
        //public string ErrorMessage { get; set; }
        //public int ErrorCode { get; set; }

        public string Status { get; set; }
        public int Response_Code { get; set; }
        public string Response_Description { get; set; }

        public string Destination_Api_Name { get; set; }
        public string End_To_End_Txn_Id { get; set; }
        public string Cbe_Txn_Ref { get; set; }
        public string Destination_txn_Ref { get; set; }
    }

    public class ActionResult
    {
        public bool IsSuccess { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
    }
    public class ActionUnifiedResult
    {
        public string Status { get; set; }
        public int Response_Code { get; set; }
        public string Response_Description { get; set; }

        //public string Destination_Api_Name { get; set; }
        //public string End_To_End_Txn_Id { get; set; }
        //public string Cbe_Txn_Ref { get; set; }
        //public string Destination_txn_Ref { get; set; }
    }
}