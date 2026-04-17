using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using ProgramInformationV2.Search.NoteTemplates;
using System.Net;

namespace ProgramInformationV2.Function;

public class NoteTemplate(NoteTemplateSingleton noteTemplateSingleton, ILogger<NoteTemplate> logger) {
    private readonly NoteTemplateSingleton _noteTemplateSingleton = noteTemplateSingleton;
    private readonly ILogger<NoteTemplate> _logger = logger;

    [Function("RefreshNoteTemplates")]
    [OpenApiOperation(operationId: "RefreshNoteTemplates", tags: "Note Templates", Description = "Refresh the NoteTemplate information.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "Standard response")]
    public async Task<HttpResponseData> Refresh([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req) {
        _logger.LogInformation("Called Note Template Refresh.");
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(_noteTemplateSingleton.ResetNoteTemplate());
        return response;
    }

    [Function("GetAllNoteTemplates")]
    [OpenApiOperation(operationId: "GetAllNoteTemplates", tags: "Note Templates", Description = "Get the NoteTemplate information, used for debugging. This will automatically refresh the note template information regardless of ")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "Standard response")]
    public async Task<HttpResponseData> GetAll([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req) {
        _logger.LogInformation("Called Note Template GetAll.");
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(await _noteTemplateSingleton.GetNoteTemplates());
        return response;
    }
}