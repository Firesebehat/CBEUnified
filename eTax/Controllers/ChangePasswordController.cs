using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using eTaxAPI.ActionFilters;
using eTaxAPI.Common;
using eTaxAPI.DataAccess;
using eTaxAPI.Model;

namespace eTaxAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    [AuthorizationRequired]
    [eTaxAPIExceptionFilter]
    public class ChangePasswordController : ApiController
    {
        public HttpResponseMessage Post([FromBody] ChangePasswordModel pwd)
        {
            
            var objPwd = new ChangePassword();
            var result = objPwd.DoChangePassword(pwd.UserName, pwd.OldPassword, pwd.NewPassword, pwd.IsNewOrReset);

            var response = Request.CreateResponse(HttpStatusCode.OK, result);
            return response;
        }
    }
}