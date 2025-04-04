using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ProgramInformationV2.Function.Helper;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.JsonThinModels;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Function {

    public class GetCredentials(CredentialGetter credentialGetter, ILogger<GetCredentials> logger) {
        private readonly CredentialGetter _credentialGetter = credentialGetter;
        private readonly ILogger<GetCredentials> _logger = logger;

        [Function("CredentialFragment")]
        [OpenApiOperation(operationId: "CredentialFragment", tags: "Get Credential Information", Description = "Get a credential by the fragment supplied in the data entry area. This includes all requirement sets associated with the credential. If the credential is not found or marked inactive, it will send a blank credential.")]
        [OpenApiParameter(name: "source", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **source** parameter given to you, can use 'test' to test.")]
        [OpenApiParameter(name: "fragment", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The fragment. If multiple credentials have the same fragment, this will return the first one it finds.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(CredentialWithRequirementSets), Description = "The credential and all requirement sets.")]
        public async Task<HttpResponseData> Fragment([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req) {
            _logger.LogInformation("Called CredentialFragment.");
            var requestHelper = RequestHelperFactory.Create();
            requestHelper.Initialize(req);
            var source = requestHelper.GetRequest(req, "source");
            var fragment = requestHelper.GetRequest(req, "fragment");
            requestHelper.Validate();
            var returnItem = await _credentialGetter.GetCredentialWithRequirementSet(source, fragment);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(returnItem);
            return response;
        }

        [Function("Credential")]
        [OpenApiOperation(operationId: "Credential", tags: "Get Credential Information", Description = "Get a credential by ID. This includes all requirement sets associated with the credential. If the credential is not found or marked inactive, it will send a blank credential.")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The ID of the credential (the source is included in the ID).")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(CredentialWithRequirementSets), Description = "The credential and all requirement sets.")]
        public async Task<HttpResponseData> Id([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req) {
            _logger.LogInformation("Called Credential.");
            var requestHelper = RequestHelperFactory.Create();
            requestHelper.Initialize(req);
            var id = requestHelper.GetRequest(req, "id");
            requestHelper.Validate();
            var returnItem = await _credentialGetter.GetCredentialWithRequirementSet(id);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(returnItem);
            return response;
        }

        [Function("CredentialSearch")]
        [OpenApiOperation(operationId: "CredentialSearch", tags: "Get Credential Information", Description = "Search for credentials. This includes just active programs.")]
        [OpenApiParameter(name: "source", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **source** parameter given to you, can use 'test' to test.")]
        [OpenApiParameter(name: "tags", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "A list of tags. You can separate the tags by the characters '[-]'.")]
        [OpenApiParameter(name: "tags2", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "A list of tags. You can separate the tags by the characters '[-]'. Having multiple tags options allows you to vary the AND and OR options for the tags.")]
        [OpenApiParameter(name: "tags3", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "A list of tags. You can separate the tags by the characters '[-]'. Having multiple tags options allows you to vary the AND and OR options for the tags.")]
        [OpenApiParameter(name: "skills", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "A list of skills the credential will give you. You can separate the skills by the characters '[-]'.")]
        [OpenApiParameter(name: "departments", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "A list of departments the credential is in. You can separate the departments by the characters '[-]'.")]
        [OpenApiParameter(name: "q", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "A full text search string -- it will search the title and description for the search querystring.")]
        [OpenApiParameter(name: "formats", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "Either 'On-Campus', 'Online', 'Off-Campus', or 'Hybrid'. Can choose multiple by separating them with the characters '[-]'")]
        [OpenApiParameter(name: "credentials", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "The credential type (BS, MS, EdM, PhD, Certificate, etc.) Can choose multiple by separating them with the characters '[-]'")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(IEnumerable<Credential>), Description = "All credentials that meet the search criteria.")]
        public async Task<HttpResponseData> Search([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req) {
            _logger.LogInformation("Called CredentialSearch.");
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
            await response.WriteAsJsonAsync(await _credentialGetter.GetCredentials(source, query, tags, tags2, tags3, skills, departments, formats, credentials));
            return response;
        }

        [Function("CredentialSuggest")]
        [OpenApiOperation(operationId: "CredentialSuggest", tags: "Get Credential Information", Description = "Credential suggest for look-ahead searchs and autocomplete.")]
        [OpenApiParameter(name: "source", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **source** parameter given to you, can use 'test' to test.")]
        [OpenApiParameter(name: "q", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "A full text search string for autocomplete.")]
        [OpenApiParameter(name: "take", In = ParameterLocation.Query, Required = false, Type = typeof(int), Description = "How many suggestions do you want? Defaults to 10.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(IEnumerable<string>), Description = "An array of strings")]
        public async Task<HttpResponseData> Suggest([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req) {
            _logger.LogInformation("Called CredentialSuggest.");
            var requestHelper = RequestHelperFactory.Create();
            requestHelper.Initialize(req);
            var source = requestHelper.GetRequest(req, "source");
            var search = requestHelper.GetRequest(req, "q");
            var take = requestHelper.GetInteger(req, "take", 10);
            requestHelper.Validate();
            var response = req.CreateResponse(HttpStatusCode.OK);
            // await response.WriteAsJsonAsync(await _credentialGetter.GetSuggestions(source, search, take));
            return response;
        }
    }
}