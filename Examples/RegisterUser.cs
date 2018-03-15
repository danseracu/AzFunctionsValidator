using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using AzureFunctionsValidator;
using Examples.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace Examples
{
    public static class RegisterUser
    {
        [FunctionName("RegisterUser")]
        [Validate(typeof(UserModel), "validationResult")]
        [CustomValidate(typeof(UserModel), "validationResult", typeof(CustomValidator))]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req, TraceWriter log,
            [IgnoreBind]List<ValidationResult> validationResult)
        {
            if (validationResult.Any())
                return req.CreateResponse(HttpStatusCode.BadRequest, validationResult);

            return req.CreateResponse(HttpStatusCode.OK);
        }
    }
}
