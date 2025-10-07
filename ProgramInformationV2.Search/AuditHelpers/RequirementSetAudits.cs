using OpenSearch.Client;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Search.AuditHelpers {

    public class RequirementSetAudits(OpenSearchClient? openSearchClient) {
        private readonly OpenSearchClient _openSearchClient = openSearchClient ?? default!;

        public async Task<List<GenericItemWithChildren>> GetAllRequirementPublicSets(string source) {
            var response = await _openSearchClient.SearchAsync<RequirementSet>(s => s.Index(UrlTypes.RequirementSets.ConvertToUrlString())
                    .Size(1000)
                    .Query(q => q
                    .Bool(b => b
                    .Filter(f => f.Term(m => m.Field(fld => fld.Source).Value(source)))
                    .Must(f => f.Term(m => m.Field(fld => fld.CredentialId).Value(""))))));
            if (_openSearchClient.ConnectionSettings.DisableDirectStreaming) {
                Console.WriteLine(response.DebugInformation);
            }
            return response.IsValid ? [.. response.Documents.Where(r => r.CredentialId == "").Select(r => new GenericItemWithChildren { Id = r.Id, Title = r.Title + (string.IsNullOrWhiteSpace(r.Description) ? "" : "<br />" + r.Description), IsActive = r.IsActive, Order = r.Order, Children = [.. r.CourseRequirements.Select(cr => new GenericItem { Title = cr.Title, Id = cr.CourseId })] }).OrderBy(g => g.Title)] : [];
        }

        public async Task<List<GenericItem>> GetAllRequirementSetsWithInvalidCourses(string source) {
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

            if (!requirementSets.IsValid || !courses.IsValid) {
                return [];
            }

            var returnValue = new List<GenericItem>();

            foreach (var requirement in requirementSets.Documents) {
                if (requirement.CourseRequirements != null) {
                    foreach (var course in requirement.CourseRequirements) {
                        var courseDetail = courses.Documents.FirstOrDefault(c => c.Id == course.CourseId);
                        if (courseDetail == null) {
                            returnValue.Add(new GenericItem { Id = requirement.Id, Title = requirement.InternalTitle + ": " + course.Title, IsActive = requirement.IsActive, Order = requirement.Order });
                        }
                    }
                }
            }

            return [.. returnValue.OrderBy(g => g.Title)];
        }

        public async Task<List<GenericItem>> GetMissingRequirementSets(string source) {
            var credentialList = await _openSearchClient.SearchAsync<Program>(s => s.Index(UrlTypes.Programs.ConvertToUrlString())
                    .Size(1000)
                    .Query(q => q
                    .Bool(b => b
                    .Filter(f => f.Term(m => m.Field(fld => fld.Source).Value(source))))));

            if (_openSearchClient.ConnectionSettings.DisableDirectStreaming) {
                Console.WriteLine(credentialList.DebugInformation);
            }

            var requirementSetIds = credentialList.Documents.SelectMany(c => c.Credentials).SelectMany(c => c.RequirementSetIds).Distinct().ToList();

            var response = await _openSearchClient.SearchAsync<RequirementSet>(s => s.Index(UrlTypes.RequirementSets.ConvertToUrlString())
                    .Size(1000)
                    .Query(q => q
                    .Bool(b => b
                    .Filter(f => f.Term(m => m.Field(fld => fld.Source).Value(source)))
                    .MustNot(f => f.Terms(m => m.Field(fld => fld.Id).Terms(requirementSetIds))))));
            if (_openSearchClient.ConnectionSettings.DisableDirectStreaming) {
                Console.WriteLine(response.DebugInformation);
            }
            return response.IsValid ? response.Documents.Select(r => r.GetGenericItem()).OrderBy(g => g.Title).ToList() : [];
        }

        public async Task<List<GenericItem>> GetPublicRequirementSetsThatCanBeMadePrivate(string source) {
            var credentialList = await _openSearchClient.SearchAsync<Program>(s => s.Index(UrlTypes.Programs.ConvertToUrlString())
                    .Size(1000)
                    .Query(q => q
                    .Bool(b => b
                    .Filter(f => f.Term(m => m.Field(fld => fld.Source).Value(source))))));

            if (_openSearchClient.ConnectionSettings.DisableDirectStreaming) {
                Console.WriteLine(credentialList.DebugInformation);
            }

            var requirementSetIds = credentialList.Documents.SelectMany(c => c.Credentials).SelectMany(c => c.RequirementSetIds).GroupBy(x => x).Where(g => g.Count() == 1).Select(g => g.Key).ToList();

            var response = await _openSearchClient.SearchAsync<RequirementSet>(s => s.Index(UrlTypes.RequirementSets.ConvertToUrlString())
                    .Size(1000)
                    .Query(q => q
                    .Bool(b => b
                    .Filter(f => f.Term(m => m.Field(fld => fld.Source).Value(source)))
                    .Must(f => f.Terms(m => m.Field(fld => fld.Id).Terms(requirementSetIds)),
                        f => f.Term(m => m.Field(fld => fld.CredentialId).Value(""))))));
            if (_openSearchClient.ConnectionSettings.DisableDirectStreaming) {
                Console.WriteLine(response.DebugInformation);
            }
            return response.IsValid ? response.Documents.Where(r => r.CredentialId == "").Select(r => r.GetGenericItem()).OrderBy(g => g.Title).ToList() : [];
        }
    }
}