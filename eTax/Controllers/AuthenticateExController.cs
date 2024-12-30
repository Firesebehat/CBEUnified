using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Security;
using AttributeRouting.Web.Http;
using eTaxAPI.Business;
using eTaxAPI.Filters;
using eTaxAPI.Model;
using eTaxAPI.Service;
using eTaxAPI.Utilities;

namespace Penality.Controllers
{
    [AllowAnonymous]
    [ApiAuthenticationFilter]
    [EnableCors("*", "*", "*")]
    public class AuthenticateExController : ApiController
    {
        [HttpPost]
        [POST("get/token")]
        [Route("Api/AuthenticateEx")]
        public HttpResponseMessage AuthenticateEx()
        {
            if (Thread.CurrentPrincipal != null && Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                var basicAuthenticationIdentity = Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
                if (basicAuthenticationIdentity != null)
                {
                    var userName = basicAuthenticationIdentity.UserName;
                    return GetAuthToken(userName);
                }
            }
            return null;
        }
         
        private HttpResponseMessage GetAuthToken(string userName)
        {
            ActionResult result = null;
            HttpResponseMessage response;
            try
            {
                var token = TokenService.GenerateToken(userName);
                //var response = Request.CreateResponse(HttpStatusCode.OK, "Authorized");

                var objUser = new UserProfileModel
                {
                    FullName = userName,
                    LastLoginDate = Helper.GetLastPasswordChangeDate(userName),
                    Roles = Roles.GetRolesForUser(userName),
                    PasswordExpiryRemainingDays = Helper.GetPasswordExpiryRemainingDays(userName),
                    //ForcePasswordChange = Utilities.Helper.ForcePasswordChange(userName),
                    MinNumbersLength = Helper.MinNumbersLength(),
                    LastPasswordChangeDate = Helper.GetLastPasswordChangeDate(userName),
                    MinPasswordLength = Helper.MinPasswordLength(),
                    MinUpperCaseLength = Helper.MinUpperCaseLength(),
                    MaxPasswordLength = Helper.MaxPasswordLength(),
                    MinSpecialCharactersLength = Helper.MinSpecialCharactersLength(),
                    MobileNo = "",
                    IMEINumber = "",
                    LocationCode = Helper.GetLocationCode(userName),
                    LocationName = Helper.GetLocationName(userName)
                };
                HttpContext.Current.Session["CurrentUser"] = userName;
                HttpContext.Current.Session["LocationCode"] = objUser.LocationCode;
                HttpContext.Current.Session["LocationName"] = objUser.LocationName;
                response = Request.CreateResponse(HttpStatusCode.OK, objUser);
                response.Headers.Add("Token", token.AuthToken);
                response.Headers.Add("TokenExpiry", ConfigurationManager.AppSettings["AuthTokenExpiry"]);
                response.Headers.Add("Access-Control-Expose-Headers", "Token,TokenExpiry");
                return response;
            }
            catch (Exception ex)
            {
                result.ErrorMessage = ex.Message;
                result.ErrorCode = 500;
                result.IsSuccess = false;
                return Request.CreateResponse(HttpStatusCode.BadRequest, result);
            }
        }
    }
}