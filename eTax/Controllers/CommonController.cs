using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using eTaxAPI.ActionFilters;
using eTaxAPI.DataAccess;

namespace eTaxAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    [AuthorizationRequired]
    [eTaxAPIExceptionFilter]
    public class CommonController : ApiController
    {
        [HttpGet]
        [Route("Api/Common/GetSystemDate")]
        public HttpResponseMessage GetSystemDate()
        {
            var objCommon = new CPayment();
            var accident = objCommon.GetTodayDate();

            var response = Request.CreateResponse(HttpStatusCode.OK, accident);
            return response;
        }
    }
}