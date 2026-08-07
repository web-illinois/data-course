using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using System.Net;

namespace ProgramInformationV2.Function;

public class StatusFunction(ILogger<StatusFunction> logger) {
    private readonly ILogger<StatusFunction> _logger = logger;

    [Function("Check")]
    [OpenApiOperation(operationId: "Check", tags: "Health Check", Description = "Get health check information.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "Standard response")]
    public async Task<HttpResponseData> Check([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req) {
        _logger.LogInformation("Called Check.");
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync("Check");
        return response;
    }
}