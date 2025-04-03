using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ProgramInformationV2.Function
{
    public class GetCredentials
    {
        private readonly ILogger<GetCredentials> _logger;

        public GetCredentials(ILogger<GetCredentials> logger)
        {
            _logger = logger;
        }

        [Function("GetCredentials")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
