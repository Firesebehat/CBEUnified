using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Security;
using eTaxAPI.ActionFilters;
using eTaxAPI.DataAccess;
using eTaxAPI.Model;
using eTaxAPI.Utilities;

namespace eTaxAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    [AuthorizationRequired]
    [eTaxAPIExceptionFilter]
    public class UserProfileController : ApiController
    {
        [HttpGet]
        [Route("Api/UserProfile/{userName}")]
        public HttpResponseMessage Get(string userName)
        {

            HttpResponseMessage response;
            if (userName.Length == 0)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, "User Name can not be empty.");
                return response;
            }

            var objUser = new UserProfileModel
            {
                FullName = Helper.GetFullname(userName),
                LastLoginDate = Helper.GetLastPasswordChangeDate(userName),
                Roles = Roles.GetRolesForUser(userName),
                PasswordExpiryRemainingDays = Helper.GetPasswordExpiryRemainingDays(userName),
                //ForcePasswordChange = Utilities.Helper.ForcePasswordChange(userName),
                MinNumbersLength = Helper.MinNumbersLength(),
                LastPasswordChangeDate = Helper.GetLastPasswordChangeDate(userName),
                MinPasswordLength = Helper.MinPasswordLength(),
                MinUpperCaseLength = Helper.MinUpperCaseLength(),
                MaxPasswordLength = Helper.MaxPasswordLength(),
                MinSpecialCharactersLength = Helper.MinSpecialCharactersLength()
            };
            response = Request.CreateResponse(HttpStatusCode.OK, objUser);
            return response;
        }
    }
}