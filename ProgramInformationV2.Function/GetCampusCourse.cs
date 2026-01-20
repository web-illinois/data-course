using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ProgramInformationV2.Data.CourseImport;
using ProgramInformationV2.Function.Helper;
using ProgramInformationV2.Search.Models;
using System.Net;

namespace ProgramInformationV2.Function;

public class GetCampusCourse(ILogger<GetCampusCourse> logger) {
    private readonly ILogger<GetCampusCourse> _logger = logger;

    [Function("CampusCourse")]
    [OpenApiOperation(operationId: "CampusCourse", tags: "Get Course Information", Description = "Get a course from the campus tools. This is using the same course object, but the source and many other fields not supported by the campus tools will be blank. Note that this is doing a lot of calls to see if the course exists in a particular semester, so it may take some time to run as the only way to check is to validate timeout.")]
    [OpenApiParameter(name: "rubric", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The rubric.")]
    [OpenApiParameter(name: "coursenumber", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The course number.")]
    [OpenApiParameter(name: "singlesemester", In = ParameterLocation.Query, Required = true, Type = typeof(bool), Description = "If set to true, then only query latest semester -- used if you just need the description. Defaults to false.")]
    [OpenApiParameter(name: "courseonly", In = ParameterLocation.Query, Required = true, Type = typeof(bool), Description = "If set to true, then do not include section information. Defaults to false.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(List<Course>), Description = "The list of courses")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequestData req) {
        _logger.LogInformation("Called CampusCourse.");
        var requestHelper = RequestHelperFactory.Create();
        requestHelper.Initialize(req);
        var rubric = requestHelper.GetRequest(req, "rubric");
        var coursenumber = requestHelper.GetRequest(req, "coursenumber");
        var singlesemester = requestHelper.GetBoolean(req, "singlesemester");
        var courseonly = requestHelper.GetBoolean(req, "courseonly");
        requestHelper.Validate();

        var itemGroups = XmlImporter.GetAllCoursesBySemester(rubric, coursenumber, singlesemester);
        var scheduledCourse = XmlImporter.GetCourse(itemGroups ?? []);
        var course = ScheduleTranslator.Translate(scheduledCourse, "", !courseonly);
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(course);
        return response;
    }
}