using System;

namespace eTaxAPI.Model
{
    public class PaymentSite
    {
        public string OrgGuid { get; set; }

        public string Code { get; set; }

        public string SiteName { get; set; }

        public string SiteNameAmh { get; set; }

        public string SiteNameSort { get; set; }

        public string ParentAdministrationCode { get; set; }

        public string OrgType { get; set; }

        public bool IsActive { get; set; }

        public string FileNumber { get; set; }

        public string Telephone { get; set; }

        public string POBox { get; set; }

        public string EMail { get; set; }

        public byte[] Logo { get; set; }

        public string UserName { get; set; }

        public DateTime EventDatetime { get; set; }

        public string UpdatedUsername { get; set; }

        public DateTime UpdatedEventDatetime { get; set; }
        public string WeredaCode { get; set; }
        public string ZoneCode { get; set; }
        public string ParentCode { get; set; }
    }
}