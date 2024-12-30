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
using eTaxAPI.Controllers;
using eTaxAPI.Filters;
using eTaxAPI.Model;
using eTaxAPI.Service;
using eTaxAPI.Utilities;

namespace Penality.Controllers
{
    [AllowAnonymous]
    [eTaxAPIExceptionFilter]
    //[ApiAuthenticationFilter]
    [EnableCors("*", "*", "*")]
    public class AuthenticateUnifiedExController : ApiController
    {
        [HttpPost]
        [POST("get/token")]
        //[Route("Api/AuthenticateUnified/{client_id}/{client_secret}/{grant_type?}/{scope?}")]
        [Route("Api/AuthenticateUnified/{client_id}/{client_secret}/{grant_type}")]
        public HttpResponseMessage AuthenticateUnified(string client_id, string client_secret, string grant_type, string scope = "unified_outgoing")
        {
            //if (Thread.CurrentPrincipal != null && Thread.CurrentPrincipal.Identity.IsAuthenticated)
            //{
            //    var basicAuthenticationIdentity = Thread.CurrentPrincipal.Identity as BasicAuthenticationIdentity;
            //    if (basicAuthenticationIdentity != null)
            //    {
            //        var userName = basicAuthenticationIdentity.UserName;
            return GetUnifiedAuthToken(client_id, client_secret, grant_type);
            //    }
            //}
            //return null;
        }


        /// <summary>
        ///     Returns auth token for the validated user.
        /// </summary>
        /// <param name="ClientId"></param>
        /// <param name="ClientSecret"></param>
        /// <returns></returns>
        private HttpResponseMessage GetUnifiedAuthToken(string ClientId, string ClientSecret, string grant_type)
        {
            var result = new ActionUnifiedResult();
            HttpResponseMessage response;
            if (ClientId != string.Empty || ClientSecret != string.Empty || grant_type != string.Empty)
            {
                if (ClientId != ConfigurationManager.AppSettings["ClinetId"])
                {
                    result.Response_Description = "ClientId Is not correct";
                    result.Response_Code = 401;
                    result.Status = "Failure";
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                if (ClientSecret != ConfigurationManager.AppSettings["ClientSecret"])
                {
                    result.Response_Description = "Client Secret Is not correct";
                    result.Response_Code = 401;
                    result.Status = "Failure";
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                //if (grant_type != "client_credintials")
                //{
                //    result.Response_Description = "client_credintials is not correct";
                //    result.Response_Code = 401;
                //    result.Status = "Failure";
                //    return Request.CreateResponse(HttpStatusCode.OK, result);
                //}
            }
            try
            {
                var token = TokenService.GenerateToken(ClientId);
                var objUser = new UnifiedUserProfileModel
                {
                    token_type = "Access Token",
                    access_token = token.AuthToken,
                    scope = "Unified_Outgoing",
                    expires_in = token.ExpiresOn,
                    consented_on = token.IssuedOn
                };
                HttpContext.Current.Session["CurrentUser"] = ClientId;
                HttpContext.Current.Session["AuthToken"] = token.AuthToken;
                //HttpContext.Current.Session["LocationCode"] = objUser.LocationCode;
                //HttpContext.Current.Session["LocationName"] = objUser.LocationName;
                response = Request.CreateResponse(HttpStatusCode.OK, objUser);
                response.Headers.Add("Token", token.AuthToken);
                response.Headers.Add("TokenExpiry", ConfigurationManager.AppSettings["AuthTokenExpiry"]);
                response.Headers.Add("Access-Control-Expose-Headers", "Token,TokenExpiry");
                return response;
            }
            catch (Exception ex)
            {
                result.Response_Description = ex.Message;
                result.Response_Code = 400;
                result.Status = "Failure";
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
        }
        //private HttpResponseMessage GetAuthToken(string userName)
        //{
        //    ActionResult result = null;
        //    HttpResponseMessage response;
        //    try
        //    {
        //        var token = TokenService.GenerateToken(userName);
        //        //var response = Request.CreateResponse(HttpStatusCode.OK, "Authorized");

        //        var objUser = new UserProfileModel
        //        {
        //            FullName = userName,
        //            LastLoginDate = Helper.GetLastPasswordChangeDate(userName),
        //            Roles = Roles.GetRolesForUser(userName),
        //            PasswordExpiryRemainingDays = Helper.GetPasswordExpiryRemainingDays(userName),
        //            //ForcePasswordChange = Utilities.Helper.ForcePasswordChange(userName),
        //            MinNumbersLength = Helper.MinNumbersLength(),
        //            LastPasswordChangeDate = Helper.GetLastPasswordChangeDate(userName),
        //            MinPasswordLength = Helper.MinPasswordLength(),
        //            MinUpperCaseLength = Helper.MinUpperCaseLength(),
        //            MaxPasswordLength = Helper.MaxPasswordLength(),
        //            MinSpecialCharactersLength = Helper.MinSpecialCharactersLength(),
        //            MobileNo = "",
        //            IMEINumber = "",
        //            LocationCode = Helper.GetLocationCode(userName),
        //            LocationName = Helper.GetLocationName(userName)
        //        };
        //        HttpContext.Current.Session["CurrentUser"] = userName;
        //        HttpContext.Current.Session["LocationCode"] = objUser.LocationCode;
        //        HttpContext.Current.Session["LocationName"] = objUser.LocationName;
        //        response = Request.CreateResponse(HttpStatusCode.OK, objUser);
        //        response.Headers.Add("Token", token.AuthToken);
        //        response.Headers.Add("TokenExpiry", ConfigurationManager.AppSettings["AuthTokenExpiry"]);
        //        response.Headers.Add("Access-Control-Expose-Headers", "Token,TokenExpiry");
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        result.ErrorMessage = ex.Message;
        //        result.ErrorCode = 500;
        //        result.IsSuccess = false;
        //        return Request.CreateResponse(HttpStatusCode.BadRequest, result);
        //    }
        //}
    }
}