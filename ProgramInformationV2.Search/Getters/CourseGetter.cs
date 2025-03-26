using OpenSearch.Client;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Search.Getters {

    public class CourseGetter : BaseGetter<Course> {

        public CourseGetter(OpenSearchClient? openSearchClient) : base(openSearchClient) {
        }

        public async Task<List<GenericItem>> GetAllCoursesBySource(string source, string search) {
            var response = string.IsNullOrWhiteSpace(search) ?
                await _openSearchClient.SearchAsync<Course>(s => s.Index(UrlTypes.Courses.ConvertToUrlString()).Query(q => q.Match(m => m.Field(fld => fld.Source).Query(source)))) :
                await _openSearchClient.SearchAsync<Course>(s => s.Index(UrlTypes.Courses.ConvertToUrlString())
                    .Query(m => m.Match(m => m.Field(fld => fld.Source).Query(source)) && m.Match(m => m.Field(fld => fld.Title).Query(search))));
            LogDebug(response);
            return response.IsValid ? response.Documents.Select(r => r.GetGenericItem()).OrderBy(g => g.Title).ToList() : new List<GenericItem>();
        }

        public async Task<Course> GetCourse(string id) {
            var response = await _openSearchClient.GetAsync<Course>(id);
            LogDebug(response);
            return response.IsValid ? response.Source : new Course();
        }
    }
}