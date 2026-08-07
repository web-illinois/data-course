using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using ProgramInformationV2.Data.CourseImport;
using ProgramInformationV2.Data.DataHelpers;
using System.Net;

namespace ProgramInformationV2.Function;

public class LoadCourseFromSchedule {
    private readonly CourseImportHelper _courseImportHelper;
    private readonly CourseImportManager _courseImportManager;
    private readonly ILogger<LoadCourseFromSchedule> _logger;

    public LoadCourseFromSchedule(CourseImportManager courseImportManager, CourseImportHelper courseImportHelper, ILogger<LoadCourseFromSchedule> logger) {
        _logger = logger;
        _courseImportManager = courseImportManager;
        _courseImportHelper = courseImportHelper;
    }

    [Function("LoadCourseFromSchedule")]
    [OpenApiOperation(operationId: "LoadCourseFromSchedule", tags: "Load Course From Schedule", Description = "Load Course that was scheduled using the UI. This will only do one at a time because of the load it causes on the courses.illinois.edu/cisapp process. Recommend only running this once every 10 seconds.")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "Response describing what was done")]
    public async Task<HttpResponseData> ScheduledLoad([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req) {
        _logger.LogInformation("Called LoadCourseFromSchedule.");
        var item = await _courseImportHelper.GetLatestPending();
        var response = req.CreateResponse(HttpStatusCode.OK);
        if (item == null) {
            await response.WriteStringAsync("Nothing pending");
            return response;
        }
        var returnValue = await _courseImportManager.ImportCourse(item.Rubric, item.CourseNumber, item.Log, true, true, false);
        await response.WriteAsJsonAsync(returnValue);
        _ = await _courseImportHelper.UpdatePending(item, returnValue);
        return response;
    }
}