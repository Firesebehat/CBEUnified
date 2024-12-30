using System;
using System.Web.Security;

namespace eTaxAPI.DataAccess
{
    public class User
    {
        public static bool Authenticate(string strUser, string strPassword)
        {
            try
            {
                var b = Membership.ValidateUser(strUser, strPassword);
                return b;
            }
            catch (Exception ex)
            {
                var s = ex.Message;
                return false;
                //throw;
            }
        }
    }
}