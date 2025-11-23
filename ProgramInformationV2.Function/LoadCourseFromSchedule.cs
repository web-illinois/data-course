using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using ProgramInformationV2.Data.CourseImport;
using ProgramInformationV2.Data.DataHelpers;

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
    public async Task<HttpResponseData> ScheduledLoad([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req) {
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