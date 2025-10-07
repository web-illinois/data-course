using OpenSearch.Client;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Search.AuditHelpers {

    public class CourseAudits(OpenSearchClient? openSearchClient) {
        private readonly OpenSearchClient _openSearchClient = openSearchClient ?? default!;

        public async Task<List<GenericItemWithChildren>> GetAllCoursesByRequirementSets(string source) {
            var requirementSets = await _openSearchClient.SearchAsync<RequirementSet>(s => s.Index(UrlTypes.RequirementSets.ConvertToUrlString())
                    .Size(1000)
                    .Query(q => q
                    .Bool(b => b
                    .Filter(f => f.Term(m => m.Field(fld => fld.Source).Value(source)), f => f.Term(m => m.Field(fld => fld.IsActive).Value(true))))));

            if (_openSearchClient.ConnectionSettings.DisableDirectStreaming) {
                Console.WriteLine(requirementSets.DebugInformation);
            }

            var courses = await _openSearchClient.SearchAsync<Course>(s => s.Index(UrlTypes.Courses.ConvertToUrlString())
                    .Size(10000)
                    .Source(s => s.Includes(f => f.Fields("title", "id", "isActive")))
                    .Query(q => q
                    .Bool(b => b
                    .Filter(f => f.Term(m => m.Field(fld => fld.Source).Value(source))))));

            if (_openSearchClient.ConnectionSettings.DisableDirectStreaming) {
                Console.WriteLine(courses.DebugInformation);
            }

            var programs = await _openSearchClient.SearchAsync<Program>(s => s.Index(UrlTypes.Programs.ConvertToUrlString())
                .Size(1000)
                .Query(q => q
                .Bool(b => b
                .Filter(f => f.Term(m => m.Field(fld => fld.Source).Value(source)), f => f.Term(m => m.Field(fld => fld.IsActive).Value(true))))));

            if (_openSearchClient.ConnectionSettings.DisableDirectStreaming) {
                Console.WriteLine(programs.DebugInformation);
            }

            if (!requirementSets.IsValid || !courses.IsValid || !programs.IsValid) {
                return [];
            }

            var credentials = programs.Documents.SelectMany(p => p.Credentials).Where(c => c.IsActive).ToList();
            var returnValue = new List<GenericItemWithChildren>();

            foreach (var requirement in requirementSets.Documents) {
                if (requirement.CourseRequirements != null) {
                    foreach (var course in requirement.CourseRequirements) {
                        var courseDetail = courses.Documents.FirstOrDefault(c => c.Id == course.CourseId);
                        if (courseDetail != null) {
                            foreach (var credential in credentials.Where(c => c.RequirementSetIds.Contains(requirement.Id))) {
                                if (returnValue.Select(r => r.Id).Contains(courseDetail.Id)) {
                                    if (!returnValue.First(r => r.Id == courseDetail.Id).Children.Select(child => child.Id).Contains(requirement.Id)) {
                                        returnValue.First(r => r.Id == courseDetail.Id).Children.Add(new GenericItem { Id = requirement.Id, Title = credential.Title + ": " + requirement.InternalTitle });
                                    }
                                } else {
                                    var newItem = new GenericItemWithChildren {
                                        Id = courseDetail.Id,
                                        Title = courseDetail.Title,
                                        IsActive = courseDetail.IsActive,
                                        Order = courseDetail.Order,
                                        Children = [new GenericItem { Id = requirement.Id, Title = credential.Title + ": " + requirement.InternalTitle }]
                                    };
                                    returnValue.Add(newItem);
                                }
                            }
                        }
                    }
                }
            }

            return [.. returnValue.OrderBy(g => g.Title)];
        }
    }
}