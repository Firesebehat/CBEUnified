using System;

namespace eTaxAPI.Model
{
    public class UserProfileModel
    {
        public string FullName { get; set; }
        public string[] Roles { get; set; }
        public DateTime LastLoginDate { get; set; }
        public DateTime LastPasswordChangeDate { get; set; }
        public int PasswordExpiryRemainingDays { get; set; }
        public int MinPasswordLength { get; set; }
        public int MinNumbersLength { get; set; }
        public int MinUpperCaseLength { get; set; }
        public int MaxPasswordLength { get; set; }
        public int MinSpecialCharactersLength { get; set; }
        public string MobileNo { get; set; }
        public string IMEINumber { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        //public bool ForcePasswordChange { get; set; }
    }
    public class UnifiedUserProfileModel
    {
        public string token_type { get; set; }
        public string access_token { get; set; }
        public string scope { get; set; }
        public DateTime expires_in { get; set; }
        public DateTime consented_on { get; set; }

    }
}