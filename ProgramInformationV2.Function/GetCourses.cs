using System.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ProgramInformationV2.Function.Helper;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Function {

    public class GetCourses(CourseGetter courseGetter, ILogger<GetCourses> logger) {
        private readonly CourseGetter _courseGetter = courseGetter;
        private readonly ILogger<GetCourses> _logger = logger;

        [Function("CourseByFaculty")]
        [OpenApiOperation(operationId: "CourseByFaculty", tags: "Get Course Information", Description = "Get all active courses from a specific faculty.")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiParameter(name: "netid", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The netid of the faculty.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(List<Course>), Description = "The list of courses")]
        public async Task<HttpResponseData> ByFaculty([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req) {
            _logger.LogInformation("Called CourseByFaculty.");
            var requestHelper = RequestHelperFactory.Create();
            requestHelper.Initialize(req);
            var netid = requestHelper.GetRequest(req, "netid");
            requestHelper.Validate();
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(await _courseGetter.GetCoursesByFaculty(netid));
            return response;
        }

        [Function("CourseFragment")]
        [OpenApiOperation(operationId: "CourseFragment", tags: "Get Course Information", Description = "Get a course by the fragment supplied in the data entry area. This includes all sections.")]
        [OpenApiParameter(name: "source", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **source** parameter given to you, can use 'test' to test.")]
        [OpenApiParameter(name: "fragment", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The fragment. If multiple courses have the same fragment, this will return the first one it finds.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(Course), Description = "The course and all sections")]
        public async Task<HttpResponseData> Fragment([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req) {
            _logger.LogInformation("Called CourseFragment.");
            var requestHelper = RequestHelperFactory.Create();
            requestHelper.Initialize(req);
            var source = requestHelper.GetRequest(req, "source");
            var fragment = requestHelper.GetRequest(req, "fragment");
            requestHelper.Validate();
            var returnItem = (await _courseGetter.GetCourse(source, fragment));
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(returnItem);
            return response;
        }

        [Function("Course")]
        [OpenApiOperation(operationId: "Course", tags: "Get Course Information", Description = "Get a course. This includes all sections.")]
        [OpenApiParameter(name: "id", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The id of the course (this includes the source).")]
        [OpenApiParameter(name: "section", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "Optional section ID")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(Course), Description = "The course and all sections")]
        public async Task<HttpResponseData> Id([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req) {
            _logger.LogInformation("Called Course.");
            var requestHelper = RequestHelperFactory.Create();
            requestHelper.Initialize(req);
            var id = requestHelper.GetRequest(req, "id");
            var section = requestHelper.GetRequest(req, "section", false);
            requestHelper.Validate();
            var returnItem = await _courseGetter.GetCourse(id, section, true);
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(returnItem);
            return response;
        }

        [Function("CourseSearch")]
        [OpenApiOperation(operationId: "CourseSearch", tags: "Get Course Information", Description = "Search for courses. This includes just active courses.")]
        [OpenApiParameter(name: "source", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **source** parameter given to you, can use 'test' to test.")]
        [OpenApiParameter(name: "tags", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "A list of tags. You can separate the tags by the characters '[-]'.")]
        [OpenApiParameter(name: "tags2", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "A list of tags. You can separate the tags by the characters '[-]'. Having multiple tags options allows you to vary the AND and OR options for the tags.")]
        [OpenApiParameter(name: "tags3", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "A list of tags. You can separate the tags by the characters '[-]'. Having multiple tags options allows you to vary the AND and OR options for the tags.")]
        [OpenApiParameter(name: "skills", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "A list of skills the course will give you. You can separate the skills by the characters '[-]'.")]
        [OpenApiParameter(name: "departments", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "A list of departments the course is in. You can separate the departments by the characters '[-]'.")]
        [OpenApiParameter(name: "q", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "A full text search string -- it will search the title and description for the search querystring.")]
        [OpenApiParameter(name: "period", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "Either 'upcoming' (future courses), 'current' (courses that are going on now), or 'open' (courses that are going on now or in the future).")]
        [OpenApiParameter(name: "formats", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "Either 'On-Campus', 'Online', 'Off-Campus', or 'Hybrid'. Can choose multiple by separating them with the characters '[-]'")]
        [OpenApiParameter(name: "rubric", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "The course rubric.")]
        [OpenApiParameter(name: "terms", In = ParameterLocation.Query, Required = false, Type = typeof(string), Description = "Either 'Fall', 'Spring', 'Summer', or 'Winter'. Can choose multiple by separating them with the characters '[-]'")]
        [OpenApiParameter(name: "take", In = ParameterLocation.Query, Required = false, Type = typeof(int), Description = "How many courses do you want? Defaults to 1000.")]
        [OpenApiParameter(name: "skip", In = ParameterLocation.Query, Required = false, Type = typeof(int), Description = "A skip value to help with pagination. Defaults to 0.")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(IEnumerable<Program>), Description = "All programs that meet the search criteria. If you filter by credentials, it will filter the credential list for each program.")]
        public async Task<HttpResponseData> Search([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequestData req) {
            _logger.LogInformation("Called CourseSearch.");
            var requestHelper = RequestHelperFactory.Create();
            requestHelper.Initialize(req);
            var source = requestHelper.GetRequest(req, "source");
            var tags = requestHelper.GetArray(req, "tags");
            var tags2 = requestHelper.GetArray(req, "tags2");
            var tags3 = requestHelper.GetArray(req, "tags3");
            var skills = requestHelper.GetArray(req, "skills");
            var departments = requestHelper.GetArray(req, "departments");
            var query = requestHelper.GetRequest(req, "q", false);
            var formats = requestHelper.GetArray(req, "formats");
            var rubric = requestHelper.GetRequest(req, "rubric", false);
            var terms = requestHelper.GetArray(req, "terms");
            var take = requestHelper.GetInteger(req, "take", 1000);
            var skip = requestHelper.GetInteger(req, "skip");
            var period = requestHelper.GetRequest(req, "period", false);

            var isUpcoming = period.Equals("upcoming") || period.Equals("open");
            var isCurrent = period.Equals("current") || period.Equals("open");

            requestHelper.Validate();
            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(await _courseGetter.GetCourses(source, query, tags, tags2, tags3, skills, departments, formats, rubric, terms, isUpcoming, isCurrent, take, skip));
            return response;
        }

        [Function("CourseSuggest")]
        [OpenApiOperation(operationId: "CourseSuggest", tags: "Get Course Information", Description = "Course suggest for look-ahead searchs and autocomplete.")]
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
            //await response.WriteAsJsonAsync(await _courseGetter.GetSuggestions(source, search, take));
            return response;
        }
    }
}