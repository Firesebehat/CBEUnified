using System;
using System.Configuration;
using System.Web.Profile;
using System.Web.Security;
using CUSTORCommon.Ethiopic;

namespace eTaxAPI.Utilities
{
    public class Helper
    {
        public enum DateRangeType
        {
            eToday,
            eYesterday,
            eThisMonth,
            eThisYear
        }

        private static string GetThisMonthRange()
        {
            try
            {
                var thisMonth = EthiopicDateTime.GetEthiopicMonth(DateTime.Today.Day, DateTime.Today.Month,
                    DateTime.Today.Year);
                var thisYear = EthiopicDateTime.GetEthiopicYear(DateTime.Today.Day, DateTime.Today.Month,
                    DateTime.Today.Year);
                var dtFrom = EthiopicDateTime.GetGregorianDate(1, thisMonth, thisYear);

                var dtTo = thisMonth < 13
                    ? EthiopicDateTime.GetGregorianDate(30, thisMonth, thisYear)
                    : EthiopicDateTime.GetGregorianDate(EthiopicDateTime.IsETLeapYear(thisYear) ? 6 : 5, thisMonth,
                        thisYear);

                return "between '" + dtFrom.ToString("yyyy-MM-dd") + "'" + " and '" + dtTo.ToString("yyyy-MM-dd") + "'";
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private static string GetThisYearRange()
        {
            try
            {
                var thisMonth = EthiopicDateTime.GetEthiopicMonth(DateTime.Today.Day, DateTime.Today.Month,
                    DateTime.Today.Year);
                var thisYear = EthiopicDateTime.GetEthiopicYear(DateTime.Today.Day, DateTime.Today.Month,
                    DateTime.Today.Year);
                var dtFrom = EthiopicDateTime.GetGregorianDate(1, 1, thisYear);

                var dtTo = thisMonth < 13
                    ? EthiopicDateTime.GetGregorianDate(30, thisMonth, thisYear)
                    : EthiopicDateTime.GetGregorianDate(EthiopicDateTime.IsETLeapYear(thisYear) ? 6 : 5, thisMonth,
                        thisYear);

                return "between '" + dtFrom.ToString("yyyy-MM-dd") + "'" + " and '" + dtTo.ToString("yyyy-MM-dd") + "'";
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        public static string GetDateRange(DateRangeType dateRange)
        {
            switch (dateRange)
            {
                case DateRangeType.eToday:
                    {
                        var dtToday = DateTime.Today;
                        return "='" + dtToday.ToString("yyyy-MM-dd") + "'";
                    }
                case DateRangeType.eYesterday:
                    var dtYesterday = DateTime.Today.AddDays(-1);
                    return "='" + dtYesterday.ToString("yyyy-MM-dd") + "'";
                case DateRangeType.eThisMonth:
                    return GetThisMonthRange();
                case DateRangeType.eThisYear:
                    return GetThisYearRange();
                default:
                    return "='" + DateTime.Today.ToString("yyyy-MM-dd") + "'";
            }
        }

        public static DateTime GetLastPasswordChangeDate(string strUserName)
        {
            try
            {
                var usr = Membership.GetUser(strUserName);
                return usr.LastPasswordChangedDate;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public static int GetPasswordExpiryRemainingDays(string strUserName)
        {
            int intExpiryDays;
            try
            {
                intExpiryDays = Convert.ToInt32(ConfigurationManager.AppSettings["PasswordExpiryDuration"]);
            }
            catch
            {
                intExpiryDays = 15;
            }
            try
            {
                var dt = GetLastPasswordChangeDate(strUserName);
                var dblDays = (DateTime.Today.Date - dt.Date).Days;
                return intExpiryDays - dblDays;
            }
            catch
            {
                return intExpiryDays;
            }
        }

        public static string GetFullname(string strUserName)
        {
            var p = ProfileBase.Create(strUserName);
            var staff = p.GetProfileGroup("Staff");
            return (string)staff.GetPropertyValue("FullName");
            //return "";
        }

        public static string GetMobileNo(string strUserName)
        {
            try
            {
                var p = ProfileBase.Create(strUserName);
                var staff = p.GetProfileGroup("Staff");
                //return (string)staff.GetPropertyValue("MobileNo");
                return "";
            }
            catch
            {
                return string.Empty;
            }
        }
        
        public static string GetLocationName(string strUserName)
        {
            try
            {
                var p = ProfileBase.Create(strUserName);
                var org = p.GetProfileGroup("Organization");
                var orgName = (string)org.GetPropertyValue("NameEng");
                if (!string.IsNullOrEmpty(orgName))
                    return Convert.ToString(orgName);
                return string.Empty;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }


        public static string GetLocationCode(string strUserName)
        {
            try
            {
                var p = ProfileBase.Create(strUserName);

                var org = p.GetProfileGroup("MOTBranches");
                var orgName = (string)org.GetPropertyValue("TaxCenterId");
                if (!string.IsNullOrEmpty(orgName))
                    return Convert.ToString(orgName);
                return string.Empty;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        public static int MinPasswordLength()
        {
            try
            {
                //redundent
                var passwordSetting = CPasswordManager.GetPasswordSetting();
                return passwordSetting.MinLength;
            }
            catch
            {
                return 0;
            }
        }

        public static int MaxPasswordLength()
        {
            try
            {
                //redundent
                var passwordSetting = CPasswordManager.GetPasswordSetting();
                return passwordSetting.MaxLength;
            }
            catch
            {
                return 0;
            }
        }

        public static int MinNumbersLength()
        {
            try
            {
                //redundent
                var passwordSetting = CPasswordManager.GetPasswordSetting();
                return passwordSetting.NumsLength;
            }
            catch
            {
                return 0;
            }
        }

        public static int MinUpperCaseLength()
        {
            try
            {
                //redundent
                var passwordSetting = CPasswordManager.GetPasswordSetting();
                return passwordSetting.UpperLength;
            }
            catch
            {
                return 0;
            }
        }

        public static int MinSpecialCharactersLength()
        {
            try
            {
                //redundent
                var passwordSetting = CPasswordManager.GetPasswordSetting();
                return passwordSetting.SpecialLength;
            }
            catch
            {
                return 0;
            }
        }

        public static bool ForcePasswordChange(string strUserName)
        {
            const string strDefaultPassword = "Ch@ngeP@55w0rd";
            try
            {
                var usr = Membership.GetUser(strUserName);
                var strPwd = usr.GetPassword();
                return strPwd == strDefaultPassword;
            }
            catch
            {
                return false;
            }
        }
    }
}