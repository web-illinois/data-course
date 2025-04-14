using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Function.Helper;
using ProgramInformationV2.Search.JsonThinModels;

namespace ProgramInformationV2.Function {

    public class GetTags(FilterHelper filterHelper, ILogger<GetPrograms> logger) {
        private readonly FilterHelper _filterHelper = filterHelper;
        private readonly ILogger<GetPrograms> _logger = logger;

        [Function("Check")]
        [OpenApiOperation(operationId: "Check", tags: "Check Information", Description = "Get check information.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(List<TagList>), Description = "Standard response")]
        public async Task<HttpResponseData> Check([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req) {
            _logger.LogInformation("Called Check.");
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync("Check");
            return response;
        }

        [Function("Tags")]
        [OpenApiOperation(operationId: "Tags", tags: "Get Tag Information", Description = "Get tag information.")]
        [OpenApiParameter(name: "source", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **source** parameter given to you, can use 'test' to test.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(List<TagList>), Description = "The tags associated with the source")]
        public async Task<HttpResponseData> Tags([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req) {
            _logger.LogInformation("Called Tags.");
            var requestHelper = RequestHelperFactory.Create();
            requestHelper.Initialize(req);
            var source = requestHelper.GetRequest(req, "source");
            requestHelper.Validate();
            var returnItem = await _filterHelper.GetFilterListForExport(source);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(returnItem);
            return response;
        }
    }
}