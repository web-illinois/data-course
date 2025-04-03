using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ProgramInformationV2.Function
{
    public class GetRequirementSets
    {
        private readonly ILogger<GetRequirementSets> _logger;

        public GetRequirementSets(ILogger<GetRequirementSets> logger)
        {
            _logger = logger;
        }

        [Function("GetRequirementSets")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
