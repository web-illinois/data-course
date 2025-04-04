using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ProgramInformationV2.Function.Helper;
using ProgramInformationV2.Search.Getters;

namespace ProgramInformationV2.Function {

    public class GetPrograms(ProgramGetter programGetter, ILogger<GetPrograms> logger) {
        private readonly ILogger<GetPrograms> _logger = logger;
        private readonly ProgramGetter _programGetter = programGetter;

        [Function("ProgramFragment")]
        [OpenApiOperation(operationId: "ProgramFragment", tags: "Get Program Information", Description = "Get a program by the fragment supplied in the data entry area. This includes all credentials.")]
        [OpenApiParameter(name: "source", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **source** parameter given to you, can use 'test' to test.")]
        [OpenApiParameter(name: "fragment", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The fragment. If multiple programs have the same fragment, this will return the first one it finds.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(Program), Description = "The program and all credentials")]
        public async Task<HttpResponseData> Fragment([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req) {
            _logger.LogInformation("Called ProgramFragment.");
            var requestHelper = RequestHelperFactory.Create();
            requestHelper.Initialize(req);
            var source = requestHelper.GetRequest(req, "source");
            var fragment = requestHelper.GetRequest(req, "fragment");
            requestHelper.Validate();
            var returnItem = await _programGetter.GetProgram(source, fragment);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(returnItem);
            return response;
        }

        [Function("Program")]
        [OpenApiOperation(operationId: "Program", tags: "Get Program Information", Description = "Get a program. This includes all credentials.")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The id of the program (this includes the source).")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(Program), Description = "The program and all credentials")]
        public async Task<HttpResponseData> Id([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req) {
            _logger.LogInformation("Called Program.");
            var requestHelper = RequestHelperFactory.Create();
            requestHelper.Initialize(req);
            var id = requestHelper.GetRequest(req, "id");
            requestHelper.Validate();
            var returnItem = await _programGetter.GetProgram(id);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(returnItem);
            return response;
        }

        [Function("ProgramSearch")]
        [OpenApiOperation(operationId: "ProgramSearch", tags: "Get Program Information", Description = "Search for programs. This includes just active programs.")]
        [OpenApiParameter(name: "source", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **source** parameter given to you, can use 'test' to test.")]
        [OpenApiParameter(name: "tags", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "A list of tags. You can separate the tags by the characters '[-]'.")]
        [OpenApiParameter(name: "tags2", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "A list of tags. You can separate the tags by the characters '[-]'. Having multiple tags options allows you to vary the AND and OR options for the tags.")]
        [OpenApiParameter(name: "tags3", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "A list of tags. You can separate the tags by the characters '[-]'. Having multiple tags options allows you to vary the AND and OR options for the tags.")]
        [OpenApiParameter(name: "skills", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "A list of skills the course will give you. You can separate the skills by the characters '[-]'.")]
        [OpenApiParameter(name: "departments", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "A list of departments the program is in. You can separate the departments by the characters '[-]'.")]
        [OpenApiParameter(name: "q", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "A full text search string -- it will search the title and description for the search querystring.")]
        [OpenApiParameter(name: "formats", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "Either 'On-Campus', 'Online', 'Off-Campus', or 'Hybrid'. Can choose multiple by separating them with the characters '[-]'")]
        [OpenApiParameter(name: "credentials", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "The credential type (BS, MS, EdM, PhD, Certificate, etc.) Can choose multiple by separating them with the characters '[-]'")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(IEnumerable<Program>), Description = "All programs that meet the search criteria. If you filter by credentials, it will filter the credential list for each program.")]
        public async Task<HttpResponseData> Search([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req) {
            _logger.LogInformation("Called ProgramSearch.");
            var requestHelper = RequestHelperFactory.Create();
            requestHelper.Initialize(req);
            var source = requestHelper.GetRequest(req, "source");
            var tags = requestHelper.GetArray(req, "tags");
            var tags2 = requestHelper.GetArray(req, "tags2");
            var tags3 = requestHelper.GetArray(req, "tags3");
            var skills = requestHelper.GetArray(req, "skills");
            var query = requestHelper.GetRequest(req, "q", false);
            var departments = requestHelper.GetArray(req, "departments");
            var formats = requestHelper.GetArray(req, "formats");
            var credentials = requestHelper.GetArray(req, "credentials");
            requestHelper.Validate();
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(await _programGetter.GetPrograms(source, query, tags, tags2, tags3, skills, departments, formats, credentials));
            return response;
        }

        [Function("ProgramSuggest")]
        [OpenApiOperation(operationId: "ProgramSuggest", tags: "Get Program Information", Description = "Program suggest for look-ahead searchs and autocomplete.")]
        [OpenApiParameter(name: "source", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **source** parameter given to you, can use 'test' to test.")]
        [OpenApiParameter(name: "q", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "A full text search string for autocomplete.")]
        [OpenApiParameter(name: "take", In = ParameterLocation.Query, Required = false, Type = typeof(int), Description = "How many suggestions do you want? Defaults to 10.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(IEnumerable<string>), Description = "An array of strings")]
        public async Task<HttpResponseData> Suggest([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req) {
            _logger.LogInformation("Called ProgramSuggest.");
            var requestHelper = RequestHelperFactory.Create();
            requestHelper.Initialize(req);
            var source = requestHelper.GetRequest(req, "source");
            var search = requestHelper.GetRequest(req, "q");
            var take = requestHelper.GetInteger(req, "take", 10);
            requestHelper.Validate();
            var response = req.CreateResponse(HttpStatusCode.OK);
            // await response.WriteAsJsonAsync(await _programGetter.GetSuggestions(source, search, take));
            return response;
        }
    }
}