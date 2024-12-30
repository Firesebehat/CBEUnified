using System;
using System.Web;
using System.Web.Profile;
using System.Web.Security;

namespace eTaxAPI.DataAccess
{
    public class AccessRight
    {
        public static bool CanAccessResource()
        {
            string strCurrentUser;
            try
            {
                strCurrentUser = HttpContext.Current.Session["CurrentUser"].ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            if (!Roles.IsUserInRole(strCurrentUser, "Tax Assesment Expert") &&
                !Roles.IsUserInRole(strCurrentUser, "Cashier"))
                return false;

            return true;
        }

        public static bool CanAccessPaymentResource()
        {
            string strCurrentUser;
            try
            {
                strCurrentUser = HttpContext.Current.Session["CurrentUser"].ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            if (!Roles.IsUserInRole(strCurrentUser, "Cashier"))
                return false;

            return true;
        }


        public static bool CanAccesseTaxRegistrationResource()
        {
            string strCurrentUser;
            try
            {
                strCurrentUser = HttpContext.Current.Session["CurrentUser"].ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            if (!Roles.IsUserInRole(strCurrentUser, "Tax Assesment Expert"))
                return false;
            return true;
        }



        public static bool CanAccessTransactionResourceForVoid()
        {
            string strCurrentUser;
            try
            {
                strCurrentUser = HttpContext.Current.Session["CurrentUser"].ToString();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

            if (!Roles.IsUserInRole(strCurrentUser, "Cashier"))
                return false;

            return true;
        }


        public static string GetLocationName()
        {
            try
            {
                var strCurrentUser = HttpContext.Current.Session["CurrentUser"].ToString();
                var p = ProfileBase.Create(strCurrentUser);
                var org = p.GetProfileGroup("Organization");
                var orgName = (string)org.GetPropertyValue("NameEng");
                if (!string.IsNullOrEmpty(orgName))
                    return orgName;
                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
        

        public static string GetLocation()
        {
            try
            {
                var strCurrentUser = HttpContext.Current.Session["CurrentUser"].ToString();
                var p = ProfileBase.Create(strCurrentUser);
                //var staff = ((ProfileGroupBase)(p.GetProfileGroup("Staff")));
                //string strFullname = (string)staff.GetPropertyValue("FullName");
                var org = p.GetProfileGroup("Organization");
                var orgCode = (string)org.GetPropertyValue("Code");
                if (!string.IsNullOrEmpty(orgCode))
                    return Convert.ToString(orgCode);
                return "";
            }
            catch
            {
                return "";
            }
        }
    }
}