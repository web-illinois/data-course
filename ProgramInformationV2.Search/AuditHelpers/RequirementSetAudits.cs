using OpenSearch.Client;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Search.AuditHelpers {

    public class RequirementSetAudits(OpenSearchClient? openSearchClient) {
        private readonly OpenSearchClient _openSearchClient = openSearchClient ?? default!;

        public async Task<List<GenericItem>> GetAllRequirementPublicSets(string source) {
            var response = await _openSearchClient.SearchAsync<RequirementSet>(s => s.Index(UrlTypes.RequirementSets.ConvertToUrlString())
                    .Size(1000)
                    .Query(q => q
                    .Bool(b => b
                    .Filter(f => f.Term(m => m.Field(fld => fld.Source).Value(source)))
                    .Must(f => f.Term(m => m.Field(fld => fld.CredentialId).Value(""))))));
            if (_openSearchClient.ConnectionSettings.DisableDirectStreaming) {
                Console.WriteLine(response.DebugInformation);
            }
            return response.IsValid ? response.Documents.Where(r => r.CredentialId == "").Select(r => new GenericItem { Id = r.Id, Title = GenerateString(r), IsActive = r.IsActive, Order = r.Order }).OrderBy(g => g.Title).ToList() : [];
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

        private static string GenerateString(RequirementSet r) {
            return r.Title + (string.IsNullOrWhiteSpace(r.Description) ? "" : "<br />" + r.Description) + "<br />" + string.Join(" / ", r.CourseRequirements.Select(cr => cr.Title + ": " + cr.Description));
        }
    }
}