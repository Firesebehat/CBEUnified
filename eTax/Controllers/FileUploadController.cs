using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using eTaxAPI.ActionFilters;
using eTaxAPI.Infrastructure;

namespace eTaxAPI.Controllers
{
    [EnableCors("*", "*", "*")]
    [AuthorizationRequired]
    [eTaxAPIExceptionFilter]
    public class FileUploadController : ApiController
    {
        [MimeMultipart]
        public async Task<FileUploadResult> Post()
        {
            var uploadPath = HttpContext.Current.Server.MapPath("~/Uploads");
            var multipartFormDataStreamProvider = new UploadMultipartFormProvider(uploadPath);

            // Read the MIME multipart asynchronously 
            await Request.Content.ReadAsMultipartAsync(multipartFormDataStreamProvider);

            var _localFileName = multipartFormDataStreamProvider
                .FileData.Select(multiPartData => multiPartData.LocalFileName).FirstOrDefault();


            // Create response
            return new FileUploadResult
            {
                LocalFilePath = _localFileName,
                FileName = Path.GetFileName(_localFileName),
                FileLength = new FileInfo(_localFileName).Length
            };
        }
    }
}