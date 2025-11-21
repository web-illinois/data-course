using OpenSearch.Client;
using ProgramInformationV2.Search.JsonThinModels;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Search.Getters {

    public class CourseGetter(OpenSearchClient? openSearchClient) : BaseGetter<Course>(openSearchClient) {

        public async Task<List<GenericItem>> GetAllCoursesBySource(string source, string search) {
            var response = await _openSearchClient.SearchAsync<Course>(s => s.Index(UrlTypes.Courses.ConvertToUrlString())
                .Size(10000)
                .Sort(srt => srt.Ascending(f => f.TitleSortKeyword))
                .Query(q => q
                .Bool(b => b
                .Filter(f => f.Term(m => m.Field(fld => fld.Source).Value(source)))
                .Must(m => string.IsNullOrWhiteSpace(search) ? m.MatchAll() : m.Match(m => m.Field(fld => fld.Title).Query(search).Operator(Operator.And))))));
            LogDebug(response);
            return response.IsValid ? [.. response.Documents.Select(r => r.GetGenericItem()).OrderBy(g => g.Title)] : [];
        }

        public async Task<Course> GetCourse(string id, string section = "", bool activeOnly = false) {
            if (string.IsNullOrWhiteSpace(id)) {
                return new();
            }
            var response = await _openSearchClient.GetAsync<Course>(id);
            LogDebug(response);
            if (response.IsValid && !string.IsNullOrWhiteSpace(section)) {
                var returnValue = response.Source;
                _ = returnValue.Sections.RemoveAll(a => a.SectionCode != section);
                return returnValue;
            }
            return response.IsValid ? response.Source : new Course();
        }

        public async Task<Course> GetCourse(string source, string fragment) {
            var response = await _openSearchClient.SearchAsync<Course>(s => s.Index(UrlTypes.Courses.ConvertToUrlString())
                    .Query(q => q
                    .Bool(b => b
                    .Filter(f => f.Term(m => m.Field(fld => fld.Source).Value(source)),
                            f => f.Term(m => m.Field(fld => fld.IsActive).Value(true)),
                            f => f.Term(m => m.Field(fld => fld.Fragment).Value(fragment))))));
            LogDebug(response);
            return response.IsValid ? response.Documents?.FirstOrDefault() ?? new() : new();
        }

        public async Task<Course> GetCourseBySection(string sectionId) {
            if (string.IsNullOrWhiteSpace(sectionId)) {
                return new();
            }
            var response = await _openSearchClient.SearchAsync<Course>(s => s.Index(UrlTypes.Courses.ConvertToUrlString()).Query(q => q.Match(m => m.Field(fld => fld.Sections.Select(ft => ft.Id)).Query(sectionId))));
            LogDebug(response);
            return response.IsValid ? response.Documents.FirstOrDefault() ?? new Course() : new Course();
        }

        public async Task<SearchObject<Course>> GetCourses(string source, string search, IEnumerable<string> tags, IEnumerable<string> tags2, IEnumerable<string> tags3, IEnumerable<string> skills, IEnumerable<string> departments, IEnumerable<string> formats, string rubric, IEnumerable<string> terms, bool isUpcoming, bool isCurrent, int take, int skip) {
            var response = await _openSearchClient.SearchAsync<Course>(s => s.Index(UrlTypes.Courses.ConvertToUrlString())
                    .Skip(skip)
                    .Size(take)
                    .Query(q => q
                    .Bool(b => b
                    .Filter(f => f.Term(m => m.Field(fld => fld.Source).Value(source)),
                        f => f.Term(m => m.Field(fld => fld.IsActive).Value(true)),
                        f => tags.Any() ? f.Terms(m => m.Field(fld => fld.TagList).Terms(tags)) : f.MatchAll(),
                        f => tags2.Any() ? f.Terms(m => m.Field(fld => fld.TagList).Terms(tags2)) : f.MatchAll(),
                        f => tags3.Any() ? f.Terms(m => m.Field(fld => fld.TagList).Terms(tags3)) : f.MatchAll(),
                        f => departments.Any() ? f.Terms(m => m.Field(fld => fld.DepartmentList).Terms(departments)) : f.MatchAll(),
                        f => skills.Any() ? f.Terms(m => m.Field(fld => fld.SkillList).Terms(skills)) : f.MatchAll(),
                        f => formats.Any() ? f.Terms(m => m.Field(fld => fld.Formats).Terms(formats)) : f.MatchAll(),
                        f => rubric.Any() ? f.Term(m => m.Field(fld => fld.Rubric).Value(rubric)) : f.MatchAll(),
                        f => terms.Any() ? f.Terms(m => m.Field(fld => fld.Terms).Terms(terms)) : f.MatchAll(),
                        f => isUpcoming ? f.Term(m => m.Field(fld => fld.IsUpcoming).Value(true)) : f.MatchAll(),
                        f => isCurrent ? f.Term(m => m.Field(fld => fld.IsCurrent).Value(true)) : f.MatchAll())
                    .Must(m => !string.IsNullOrWhiteSpace(search) ? m.Match(m => m.Field(fld => fld.Title).Query(search)) : m.MatchAll())))
                    .Sort(srt => srt.Ascending(f => f.TitleSortKeyword))
                    .Suggest(a => a.Phrase("didyoumean", p => p.Text(search).Field(fld => fld.Title))));
            LogDebug(response);

            List<Course> documents = response.IsValid ? [.. response.Documents] : [];
            return new SearchObject<Course>() {
                Error = !response.IsValid ? response.ServerError.Error.ToString() : "",
                DidYouMean = response.Suggest["didyoumean"].FirstOrDefault()?.Options?.FirstOrDefault()?.Text ?? "",
                Total = (int) response.Total,
                Items = documents
            };
        }

        public async Task<List<Course>> GetCoursesByFaculty(string netid) {
            var response = await _openSearchClient.SearchAsync<Course>(s => s.Index(UrlTypes.Courses.ConvertToUrlString())
                    .Query(q => q
                    .Bool(b => b
                    .Filter(f => f.Term(m => m.Field(fld => fld.Sections.Select(s => s.FacultyNameList.Select(fnl => fnl.NetId))).Value(netid)),
                            f => f.Term(m => m.Field(fld => fld.IsActive).Value(true))))));
            LogDebug(response);
            return response.IsValid ? response.Documents.ToList() ?? [] : [];
        }

        public async Task<Section> GetSection(string sectionId) {
            if (string.IsNullOrWhiteSpace(sectionId)) {
                return new();
            }
            var course = await GetCourseBySection(sectionId);
            return course.Sections?.SingleOrDefault(c => c.Id == sectionId) ?? new Section();
        }
    }
}