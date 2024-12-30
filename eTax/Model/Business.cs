using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eTaxAPI.Model
{
    public class Business
    {
    }

    public class BusinessListViewModel
    {
        public string OwnerTIN { get; set; }
        public DateTime DateRegistered { get; set; }
        public int status { get; set; }
        public string LegalStatus { get; set; }
        public string LegalStatusDescription { get; set; }
        public string BusinessNameAmh { get; set; }
        public string BusnessName { get; set; }
        public string BusinessNameRegional { get; set; }
        public string TradeNameAmh { get; set; }
        public string TradesName { get; set; }
        public string TradeNameRegional { get; set; }
        public string LicenceNumber { get; set; }
        public List<BusinessAddress> Addresses { get; set; }
        public List<BusinessCatagory> BusinessCatagories { get; set; }
        public List<Manager> Managers { get; set; }
        public decimal Capital { get; set; }
    }

    public class Manager
    {
        public string NameAmh { get; set; }
        public string FatherNameAmh { get; set; }
        public string GrandFatherNameAmh { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string GrandFatherName { get; set; }
        public string NameRegional { get; set; }
        public string FatherNameRegional { get; set; }
        public string GrandFatherNameRegional { get; set; }
        public string TradeNameAmh { get; set; }
        public string TradesName { get; set; }
        public string TradeNameRegional { get; set; }
        public string Nationality { get; set; }
        public string NationalityDescsription { get; set; }
    }

    public class BusinessAddress
    {
        public string RegionID { get; set; }
        public string Zone { get; set; }
        public string City { get; set; }
        public string WoredaID { get; set; }
        public string KebeleID { get; set; }
        public string HouseNo { get; set; }
        public string Fax { get; set; }
        public string Email { get; set; }
        public string Pobox { get; set; }
        public string MobileNo { get; set; }
        public string IsMainOffice { get; set; }
    }

    public class BusinessCatagory
    {
        public string MajorDivision { get; set; }
        public string Division { get; set; }
        public string MajorGroup { get; set; }
        public string BGroup { get; set; }
        public string SubGroup { get; set; }
    }
}