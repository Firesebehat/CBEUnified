using System;

namespace CUSTORCommon.Ethiopic
{
    public class EthiopicDateTime
    {
        private static int Day;
        private static int Month;
        private static int Year;
        private static string EthTime;
        public static string ErrorMessage;

        private static bool IsValidRange(int value, int intMin, int intMax)
        {
            if (intMax == -1)
            {
                if (value >= intMin)
                    return true;
            }
            else
            {
                if (value >= intMin && value <= intMax)
                    return true;
            }
            return false;
        }

        public static bool IsETLeapYear(int intEthYear)
        {
            if (Math.Abs(1999 - intEthYear) % 4 == 0)
                return true;
            return false;
        }

        public static bool IsValidEthiopicDate(int intEthDay, int intEthMonth, int intEthYear)
        {
            try
            {
                if (!IsValidRange(intEthDay, 1, 30))
                {
                    ErrorMessage = "ቀን ከ1 እስከ 30 ውስጥ መሆን አለበት";
                    return false;
                }
                if (!IsValidRange(intEthMonth, 1, 13))
                {
                    ErrorMessage = "ወር ከ1 እስከ 13 ውስጥ መሆን አለበት";
                    return false;
                }
                if (!IsValidRange(intEthYear, 1900, 9991))
                {
                    ErrorMessage = "ወር ከ1 እስከ 13 ውስጥ መሆን አለበት";
                    return false;
                }
                if (IsETLeapYear(intEthYear))
                {
                    if (intEthMonth == 13)
                        if (!IsValidRange(intEthDay, 1, 6))
                        {
                            ErrorMessage = "ቀን ከ1 እስከ 6 ውስጥ መሆን አለበት";
                            return false;
                        }
                }
                else
                {
                    if (intEthMonth == 13)
                        if (!IsValidRange(intEthDay, 1, 5))
                        {
                            ErrorMessage = "ቀን ከ1 እስከ 5 ውስጥ መሆን አለበት";
                            return false;
                        }
                }
                if (!IsETLeapYear(intEthYear))
                    if (!IsValidRange(intEthMonth, 1, 12))
                    {
                        ErrorMessage = "ወር ከ1 እስከ 12 ውስጥ መሆን አለበት";
                        return false;
                    }

                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = "Invalid Date - " + ex.Message;
                return false;
            }
        }

        public static bool IsValidEthiopicDate2(int intEthDay, int intEthMonth, int intEthYear)
        {
            if (IsValidRange(intEthDay, 1, 30))
            {
                if (IsValidRange(intEthMonth, 1, 13))
                {
                    if (IsValidRange(intEthYear, 1900, 9991))
                    {
                        if (IsETLeapYear(intEthYear))
                        {
                            if (intEthMonth == 13)
                                if (IsValidRange(intEthDay, 1, 6))
                                {
                                }
                                else
                                {
                                    ErrorMessage = "ቀን ከ1 እስከ 6 ውስጥ መሆን አለበት";
                                    return false;
                                }
                        }
                        else
                        {
                            if (intEthMonth == 13)
                                if (IsValidRange(intEthDay, 1, 5))
                                {
                                }
                                else
                                {
                                    ErrorMessage = "ቀን ከ1 እስከ 5 ውስጥ መሆን አለበት";
                                    return false;
                                }
                        }
                    }
                    else
                    {
                        ErrorMessage = "አመት ከ1900 እስከ 9991 ውስጥ መሆን አለበት";
                        return false;
                    }
                }
                else
                {
                    ErrorMessage = "ወር ከ1 እስከ 13 ውስጥ መሆን አለበት";
                    return false;
                }
            }
            else
            {
                ErrorMessage = "ቀን ከ1 እስከ 30 ውስጥ መሆን አለበት";
                return false;
            }

            return true;
        }

        public static bool IsValidGregorianDate(int intGCDay, int intGCMonth, int intGCYear)
        {
            if (IsValidRange(intGCYear, 1900, 9998))
            {
                if (IsValidRange(intGCMonth, 1, 12))
                {
                    if (IsValidRange(intGCDay, 1, DateTime.DaysInMonth(intGCYear, intGCMonth)))
                        return true;
                    ErrorMessage = "ቀን ከ1 እስከ " + DateTime.DaysInMonth(intGCYear, intGCMonth) + " ውስጥ መሆን አለበት";
                    return false;
                }
                ErrorMessage = "ወር ከ1 እስከ 12 ውስጥ መሆን አለበት";
                return false;
            }
            ErrorMessage = "የአውሮፓውያን አመት ከ1900 እስከ 9998 ውስጥ መሆን አለበት";
            return false;
        }

        public static string GetEthiopicDate(int intGCDay, int intGCMonth, int intGCYear)
        {
            SetEThiopicDate(intGCDay, intGCMonth, intGCYear);
            return Convert.ToString(Day) + "/" + Convert.ToString(Month) + "/" + Convert.ToString(Year);
        }

        public static string GetGregorianStringFormatedDate(int intEthDay, int intEthMonth, int intEthYear)
        {
            SetGCDate(intEthDay, intEthMonth, intEthYear);
            return Convert.ToString(Day) + "/" + Convert.ToString(Month) + "/" + Convert.ToString(Year);
        }

        public static DateTime GetGregorianDate(int intEthDay, int intEthMonth, int intEthYear)
        {
            SetGCDate(intEthDay, intEthMonth, intEthYear);
            return new DateTime(Year, Month, Day);
        }

        public static int GetGregorianDay(int intEthDay, int intEthMonth, int intEthYear)
        {
            SetGCDate(intEthDay, intEthMonth, intEthYear);
            return Day;
        }

        public static int GetGregorianMonth(int intEthDay, int intEthMonth, int intEthYear)
        {
            SetGCDate(intEthDay, intEthMonth, intEthYear);
            return Month;
        }

        public static int GetGregorianYear(int intEthDay, int intEthMonth, int intEthYear)
        {
            SetGCDate(intEthDay, intEthMonth, intEthYear);
            return Year;
        }

        public static int GetEthiopicDay(int intGCDay, int intGCMonth, int intGCYear)
        {
            SetEThiopicDate(intGCDay, intGCMonth, intGCYear);
            return Day;
        }

        public static int GetEthiopicMonth(int intGCDay, int intGCMonth, int intGCYear)
        {
            SetEThiopicDate(intGCDay, intGCMonth, intGCYear);
            return Month;
        }

        public static int GetEthiopicYear(int intGCDay, int intGCMonth, int intGCYear)
        {
            SetEThiopicDate(intGCDay, intGCMonth, intGCYear);
            return Year;
        }

        private static void SetGCDate(int intEthDay, int intEthMonth, int intEthYear)
        {
            var intDayDiff = 0;
            var intGCDay = 0;
            var intGCMonth = 0;
            var intGCYear = 0;
            var intAdd = 0;
            var intMax = 0;

            if (DateTime.IsLeapYear(intEthYear + 8))
                intAdd = 1;
            switch (intEthMonth)
            {
                case 1:
                    intGCMonth = 9;
                    intDayDiff = 10 + intAdd;
                    intMax = 30;
                    break;
                case 2:
                    intGCMonth = 10;
                    intDayDiff = 10 + intAdd;
                    intMax = 31;
                    break;
                case 3:
                    intGCMonth = 11;
                    intDayDiff = 9 + intAdd;
                    intMax = 30;
                    break;
                case 4:
                    intGCMonth = 12;
                    intDayDiff = 9 + intAdd;
                    intMax = 31;
                    break;
                case 5:
                    intGCMonth = 1;
                    intDayDiff = 8 + intAdd;
                    intMax = 31;
                    break;
                case 6:
                    intGCMonth = 2;
                    intDayDiff = 7 + intAdd;
                    if (intAdd > 0)
                        intMax = 29;
                    else
                        intMax = 28;
                    break;
                case 7:
                    intGCMonth = 3;
                    intDayDiff = 9;
                    intMax = 31;
                    break;
                case 8:
                    intGCMonth = 4;
                    intDayDiff = 8;
                    intMax = 30;
                    break;
                case 9:
                    intGCMonth = 5;
                    intDayDiff = 8;
                    intMax = 31;
                    break;
                case 10:
                    intGCMonth = 6;
                    intDayDiff = 7;
                    intMax = 30;
                    break;
                case 11:
                    intGCMonth = 7;
                    intDayDiff = 7;
                    intMax = 31;
                    break;
                case 12:
                    intGCMonth = 8;
                    intDayDiff = 6;
                    intMax = 31;
                    break;
                case 13:
                    intGCMonth = 9;
                    intDayDiff = 5;
                    intMax = 30;
                    break;
            }

            intGCDay = intEthDay + intDayDiff;
            if (intGCDay > intMax)
            {
                intGCDay = intGCDay - intMax;
                intGCMonth = intGCMonth + 1;
                if (intGCMonth == 13)
                    intGCMonth = 1;
            }

            intGCYear = GetGCYear(intEthMonth, intEthYear, intGCMonth);
            Year = intGCYear;
            Month = intGCMonth;
            Day = intGCDay;
        }

        private static void SetEThiopicDate(int intGCDay, int intGCMonth, int intGCYear)
        {
            int intDayDiff, intPagumen;
            var intECDay = 0;
            var intECMonth = 0;
            var intECYear = 0;


            //Get The Starting Month
            if (intGCMonth > 8)
                intECMonth = intGCMonth - 8;
            else
                intECMonth = intGCMonth + 4;

            //Get no of days for Pagumen
            if (DateTime.IsLeapYear(intGCYear + 1))
                intPagumen = 6;
            else
                intPagumen = 5;
            //Get Date Difference
            intDayDiff = GetDateDifference(intGCMonth, intGCYear);

            if (intGCMonth == 10 || intGCMonth == 11 || intGCMonth == 12)
            {
                if (DateTime.IsLeapYear(intGCYear + 1))
                    intDayDiff += 1;
                intECDay = intGCDay - intDayDiff;
                if (intECDay <= 0)
                {
                    intECDay += 30;
                    intECMonth -= 1;
                }
            }
            else if (intGCMonth == 1 || intGCMonth == 2)
            {
                if (DateTime.IsLeapYear(intGCYear))
                    intDayDiff += 1;
                intECDay = intGCDay - intDayDiff;
                if (intECDay <= 0)
                {
                    intECDay += 30;
                    intECMonth -= 1;
                }
            }
            else if (intGCMonth == 9)
            {
                if (DateTime.IsLeapYear(intGCYear + 1))
                {
                    intDayDiff = 11;
                    intPagumen = 6;
                }
                else
                {
                    intDayDiff = 10;
                    intPagumen = 5;
                }
                intECDay = intGCDay - intDayDiff;
                if (intECDay <= 0)
                {
                    intECDay = intECDay + intPagumen;
                    if (intECDay <= 0)
                    {
                        intECDay += 30;
                        intECMonth = 12;
                    }
                    else
                    {
                        intECMonth = 13;
                    }
                }
            }

            else if (intGCMonth == 3 || intGCMonth == 4 || intGCMonth == 5 || intGCMonth == 6 ||
                     intGCMonth == 7 || intGCMonth == 8)
            {
                intECDay = intGCDay - intDayDiff;
                if (intECDay <= 0)
                {
                    intECDay += 30;
                    intECMonth -= 1;
                }
            }

            //Ethiopian Year
            intECYear = GetETYear(intGCMonth, intGCYear, intECMonth);

            Year = intECYear;
            Month = intECMonth;
            Day = intECDay;
        }

        private static int GetDateDifference(int intGCMonth, int intGCYear)
        {
            var intDayDiff = 0;
            switch (intGCMonth)
            {
                case 8:
                    intDayDiff = 6;
                    break;
                case 2:
                case 6:
                case 7:
                    intDayDiff = 7;
                    break;
                case 1:
                case 4:
                case 5:
                    intDayDiff = 8;
                    break;
                case 3:
                case 11:
                case 12:
                    intDayDiff = 9;
                    break;
                case 10:
                    intDayDiff = 10;
                    break;
                case 9:
                    if (DateTime.IsLeapYear(intGCYear + 1))
                        intDayDiff = 11;
                    else
                        intDayDiff = 10;
                    break;
            }
            return intDayDiff;
        }

        private static int GetGCYear(int intECMonth, int intECYear, int intGCMonth)
        {
            int intYearTemp;
            switch (intGCMonth)
            {
                case 9:
                case 10:
                case 11:
                case 12:
                    intYearTemp = intECYear + 7;
                    if (intGCMonth == 9 && (intECMonth == 12 || intECMonth == 13))
                        intYearTemp = intYearTemp + 1;
                    break;
                default:
                    intYearTemp = intECYear + 8;
                    break;
            }
            return intYearTemp;
        }

        private static int GetETYear(int intGCMonth, int intGCYear, int intECMonth)
        {
            int intYearTemp;
            switch (intGCMonth)
            {
                case 9:
                case 10:
                case 11:
                case 12:
                    intYearTemp = intGCYear - 7;
                    if (intGCMonth == 9 && (intECMonth == 12 || intECMonth == 13))
                        intYearTemp = intYearTemp - 1;
                    break;
                default:
                    intYearTemp = intGCYear - 8;
                    break;
            }
            return intYearTemp;
        }


        public static string GetETHTime(string GCHour)
        {
            switch (GCHour)
            {
                case "00":
                    EthTime = "00";
                    break;
                case "01":
                    EthTime = "07";
                    break;
                case "02":
                    EthTime = "08";
                    break;
                case "03":
                    EthTime = "09";
                    break;
                case "04":
                    EthTime = "10";
                    break;
                case "05":
                    EthTime = "11";
                    break;
                case "06":
                    EthTime = "12";
                    break;
                case "07":
                    EthTime = "01";
                    break;
                case "08":
                    EthTime = "02";
                    break;
                case "09":
                    EthTime = "03";
                    break;
                case "10":
                    EthTime = "04";
                    break;
                case "11":
                    EthTime = "05";
                    break;
                case "12":
                    EthTime = "06";
                    break;
                case "13":
                    EthTime = "07";
                    break;
                case "14":
                    EthTime = "08";
                    break;
                case "15":
                    EthTime = "09";
                    break;
                case "16":
                    EthTime = "10";
                    break;
                case "17":
                    EthTime = "11";
                    break;
                case "18":
                    EthTime = "12";
                    break;
                case "19":
                    EthTime = "01";
                    break;
                case "20":
                    EthTime = "02";
                    break;
                case "21":
                    EthTime = "03";
                    break;
                case "22":
                    EthTime = "04";
                    break;
                case "23":
                    EthTime = "05";
                    break;
            }
            return EthTime;
        }

        public static string TranslateDateMonth(DateTime date)
        {
            string ethiopicDate;
            //To display date with the following format "month, day year"
            var splitedDate = date.ToString().Split('/', ' ');
            //Convert gregorian date to ethiopic date
            var reportedDate =
                GetEthiopicDate(Convert.ToInt32(splitedDate[1]), Convert.ToInt32(splitedDate[0]),
                    Convert.ToInt32(splitedDate[2])).Split('/');
            //Write ethiopic date by the following format "monthName day, year"
            var translatedMonth = "";
            switch (reportedDate[1])
            {
                case "1":
                    translatedMonth = "መስከረም";
                    break;
                case "2":
                    translatedMonth = "ጥቅምት";
                    break;
                case "3":
                    translatedMonth = "ህዳር";
                    break;
                case "4":
                    translatedMonth = "ታህሳስ";
                    break;
                case "5":
                    translatedMonth = "ጥር";
                    break;
                case "6":
                    translatedMonth = "የካቲት";
                    break;
                case "7":
                    translatedMonth = "መጋቢት";
                    break;
                case "8":
                    translatedMonth = "ሚያዚያ";
                    break;
                case "9":
                    translatedMonth = "ግንቦት";
                    break;
                case "10":
                    translatedMonth = "ሰኔ";
                    break;
                case "11":
                    translatedMonth = "ሐምሌ";
                    break;
                case "12":
                    translatedMonth = "ነሐሴ";
                    break;
                case "13":
                    translatedMonth = "ጳግሜ";
                    break;
            }

            return ethiopicDate = translatedMonth + " " + reportedDate[0] + " , " + reportedDate[2];
        }

        public static string TranslateEthiopicDateMonth(string ethiopicDate)
        {
            //To display date with the following format "month, day year"
            var splitedDate = ethiopicDate.Split('/', ' ');
            //Write ethiopic date by the following format "monthName day, year"
            var translatedMonth = "";
            switch (splitedDate[1])
            {
                case "1":
                    translatedMonth = "መስከረም";
                    break;
                case "2":
                    translatedMonth = "ጥቅምት";
                    break;
                case "3":
                    translatedMonth = "ህዳር";
                    break;
                case "4":
                    translatedMonth = "ታህሳስ";
                    break;
                case "5":
                    translatedMonth = "ጥር";
                    break;
                case "6":
                    translatedMonth = "የካቲት";
                    break;
                case "7":
                    translatedMonth = "መጋቢት";
                    break;
                case "8":
                    translatedMonth = "ሚያዚያ";
                    break;
                case "9":
                    translatedMonth = "ግንቦት";
                    break;
                case "10":
                    translatedMonth = "ሰኔ";
                    break;
                case "11":
                    translatedMonth = "ሐምሌ";
                    break;
                case "12":
                    translatedMonth = "ነሐሴ";
                    break;
                case "13":
                    translatedMonth = "ጳግሜ";
                    break;
            }

            return ethiopicDate = translatedMonth + " " + splitedDate[0] + " , " + splitedDate[2];
            ;
        }

        public static string GetEthiopicDate(DateTime dt)
        {
            try
            {
                if (dt.Day == 1 && dt.Month == 1 && dt.Year == 1753)
                    return string.Empty;
                return GetEthiopicDate(dt.Day, dt.Month, dt.Year);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}