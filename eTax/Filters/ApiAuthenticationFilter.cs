using System.Threading;
using System.Web;
using System.Web.Http.Controllers;
using eTaxAPI.DataAccess;

namespace eTaxAPI.Filters
{
    public class ApiAuthenticationFilter : GenericBasicAuthenticationFilter
    {
        public ApiAuthenticationFilter()
        {
        }

        public ApiAuthenticationFilter(bool isActive)
            : base(isActive)
        {
        }

        protected override bool OnAuthorizeUser(string username, string password, HttpActionContext actionContext)
        {
            var provider = actionContext.ControllerContext.Configuration;

            if (provider != null)
                if (User.Authenticate(username, password))
                {
                    var basicAuthenticationIdentity = Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
                    if (basicAuthenticationIdentity != null)
                    {
                        HttpContext.Current.Session["CurrentUser"] = username;
                        return true;
                    }
                         
                }
            return false;
        }
    }
}