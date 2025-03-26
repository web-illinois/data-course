using OpenSearch.Client;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Search.Setters {

    public class CourseSetter(OpenSearchClient? openSearchClient, CourseGetter? courseGetter) {
        private readonly CourseGetter _courseGetter = courseGetter ?? default!;
        private readonly OpenSearchClient _openSearchClient = openSearchClient ?? default!;

        public async Task<string> SetCourse(Course course) {
            course.Prepare();
            var response = await _openSearchClient.IndexAsync(course, i => i.Index(UrlTypes.Courses.ConvertToUrlString()));
            return response.IsValid ? course.Id : "";
        }
    }
}