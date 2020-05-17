using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using PopHistoryFunction.EntityFramework;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PopHistoryFunction
{
    public class PsaSetIds
    {
        private PopHistoryFunctionContext _context { get; set; }

        public PsaSetIds(PopHistoryFunctionContext context)
        {
            _context = context;
        }

        [FunctionName("PsaSetIds")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                log.LogInformation("PsaSetIds Started");
                var psaSetIds = _context.PsaSet.Select(x => x.Id).ToList();
                log.LogInformation("PsaSetIds Ended");
                return new OkObjectResult(psaSetIds);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "PsaSetIds Caught Exception");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [FunctionName("HelloWorld")]
        public async Task<IActionResult> Run2(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            return new OkObjectResult("Hello World");
        }
    }
}
