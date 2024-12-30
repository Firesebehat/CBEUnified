namespace eTaxAPI.Model
{
    public class TaxPayer
    {
        public string FirstName { get; set; }
        public string FatherName { get; set; }
        public string GrandName { get; set; }

        public string FirstNameEnglish { get; set; }
        public string FatherNameEnglish { get; set; }
        public string GrandNameEnglish { get; set; }

        public string DriverID { get; set; }
        public string LicenseArea { get; set; }
        public string LicenseNumber { get; set; }      
        public string LicenseGrade { get; set; }
        public string LicenseRegion { get; set; }
        public bool IsSuspended { get; set; }
        public string Telephone { get; set; }
    }
}