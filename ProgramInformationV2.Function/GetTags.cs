using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Function.Helper;

namespace ProgramInformationV2.Function {

    public class GetTags(FilterHelper filterHelper, ILogger<GetPrograms> logger) {
        private readonly FilterHelper _filterHelper = filterHelper;
        private readonly ILogger<GetPrograms> _logger = logger;

        [Function("Tags")]
        public async Task<HttpResponseData> Tags([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req) {
            var requestHelper = RequestHelperFactory.Create();
            requestHelper.Initialize(req);
            var source = requestHelper.GetRequest(req, "source");
            requestHelper.Validate();

            var returnItem = await _filterHelper.GetAllFilters(source);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(returnItem);
            return response;
        }
    }
}