using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using eTaxAPI.Model;
using eTaxAPI.Models;
using eTaxAPI.Service;
using System;
using System.Configuration;

using System.Web.Http.Cors;
using System.Web.Security;
using AttributeRouting.Web.Http;
using eTaxAPI.Business;
using eTaxAPI.Controllers;
using eTaxAPI.Filters;

using eTaxAPI.Utilities;

namespace eTaxAPI.ActionFilters
{
    public class AuthorizationRequiredAttribute : ActionFilterAttribute
    {
        //private const string Token = "Token";
        private const string Token = "Authorization";

        public override void OnActionExecuting(HttpActionContext filterContext)
        {
            //var provider = new TokenService();
            UnifiedResult result = new UnifiedResult();

            if (filterContext.Request.Headers.Contains(Token))
            {
                var tokenValue = filterContext.Request.Headers.GetValues(Token).First();
                string strToken = tokenValue.Remove(0, 7);
                var strValidUserName = TokenService.ValidateToken(strToken);

                // Validate Token
                if (string.IsNullOrEmpty(strValidUserName))
                {
                    //var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                    //{
                    //    ReasonPhrase = "Invalid Request"
                    //};
                    filterContext.Response = filterContext.Request.CreateResponse(HttpStatusCode.OK,
                        new
                        {
                            Response_Description = "Invalid Request",
                            Status = "Failure",
                            Response_Code = "401"
                        });
                    //filterContext.Response = responseMessage;
                }
                else
                {
                    //var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
                    //{
                    //    ReasonPhrase = "Authorization Success"
                    //};

                    //filterContext.Response = responseMessage;
                    HttpContext.Current.Session["CurrentUser"] = strValidUserName;
                    HttpContext.Current.Session["tokenValue"] = tokenValue;
                }
            }
            else
            {
                //var msg = new HttpResponseMessage();
                //result.Status = "Failure";
                //result.Response_Description = "Invalid Request";
                //result.Response_Code = 401;
                ////return Request.CreateResponse(HttpStatusCode.OK, result);

                filterContext.Response = new HttpResponseMessage(HttpStatusCode.OK);
            }

            base.OnActionExecuting(filterContext);
            //}
        }
    }
}