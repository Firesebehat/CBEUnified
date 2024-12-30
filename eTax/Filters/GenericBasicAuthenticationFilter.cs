using System;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace eTaxAPI.Filters
{
    /// <summary>
    ///     Generic basic Authentication filter.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class GenericBasicAuthenticationFilter : AuthorizationFilterAttribute
    {
        private readonly bool _isActive = true;


        public GenericBasicAuthenticationFilter()
        {
        }


        public GenericBasicAuthenticationFilter(bool isActive)
        {
            _isActive = isActive;
        }


        public override void OnAuthorization(HttpActionContext filterContext)
        {
            if (!_isActive) return;
            var identity = FetchAuthHeader(filterContext);
            if (identity == null)
            {
                ChallengeAuthRequest(filterContext);
                return;
            }
            var genericPrincipal = new GenericPrincipal(identity, null);
            Thread.CurrentPrincipal = genericPrincipal;
            if (!OnAuthorizeUser(identity.Name, identity.Password, filterContext))
            {
                ChallengeAuthRequest(filterContext);
                return;
            }
            base.OnAuthorization(filterContext);
        }


        protected virtual bool OnAuthorizeUser(string username, string password, HttpActionContext filterContext)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return false;
            return true;
        }

        protected virtual BasicAuthenticationIdentity FetchAuthHeader(HttpActionContext filterContext)
        {
            string authHeaderValue = null;
            var authRequest = filterContext.Request.Headers.Authorization;
            if (authRequest != null && authRequest.Scheme == "Basic")
                authHeaderValue = authRequest.Parameter;
            if (string.IsNullOrEmpty(authHeaderValue))
                return null;
            authHeaderValue = Encoding.Default.GetString(Convert.FromBase64String(authHeaderValue));
            var credentials = authHeaderValue.Split(':');
            return credentials.Length < 2 ? null : new BasicAuthenticationIdentity(credentials[0], credentials[1]);
        }

        private static void ChallengeAuthRequest(HttpActionContext filterContext)
        {
            var host = filterContext.Request.RequestUri.DnsSafeHost;
            filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            filterContext.Response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", host));
        }
    }
}