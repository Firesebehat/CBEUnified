using eTaxAPI.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eTaxAPI.Business
{
    public class PhoneInformation
    {
        public bool IsIMEINumberAllowed(string IMEINumber)
        {
            TelephoneDataAccess objTelphoneDataAccess = new TelephoneDataAccess();
            try
            {
                return objTelphoneDataAccess.IsIMEINumberAllowed(IMEINumber);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }
    }
}