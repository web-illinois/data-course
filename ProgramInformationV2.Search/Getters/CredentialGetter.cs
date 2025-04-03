using OpenSearch.Client;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Search.Getters {

    public class CredentialGetter(OpenSearchClient? openSearchClient, ProgramGetter programGetter) : BaseGetter<Program>(openSearchClient) {
        private ProgramGetter _programGetter = programGetter;

        public async Task<List<GenericItem>> GetAllCredentialsByRequirementId(string requirementId) {
            var response =
                await _openSearchClient.SearchAsync<Program>(s => s.Index(UrlTypes.Programs.ConvertToUrlString()).Query(q => q.Bool(b => b.Filter(f => f.Term(t => t.Field(fld => fld.Credentials.Select(c => c.RequirementSetIds)).Value(requirementId))))));
            LogDebug(response);
            return response.IsValid ? response.Documents.SelectMany(c => c.Credentials).Where(c => c.RequirementSetIds.Contains(requirementId)).Select(r => r.GetGenericItem()).OrderBy(g => g.Title).ToList() : new List<GenericItem>();
        }

        public async Task<List<GenericItem>> GetAllCredentialsBySource(string source, string search) {
            var response = string.IsNullOrWhiteSpace(search) ?
                await _openSearchClient.SearchAsync<Program>(s => s.Index(UrlTypes.Programs.ConvertToUrlString()).Query(q => q.Match(m => m.Field(fld => fld.Source).Query(source)))) :
                await _openSearchClient.SearchAsync<Program>(s => s.Index(UrlTypes.Programs.ConvertToUrlString())
                    .Query(m => m.Match(m => m.Field(fld => fld.Source).Query(source))
                        && (m.Match(m => m.Field(fld => fld.Title).Query(search)) || m.Match(m => m.Field(fld => fld.Credentials.Select(ft => ft.Title)).Query(search)))));
            LogDebug(response);
            return response.IsValid ? response.Documents.SelectMany(c => c.Credentials).Select(r => r.GetGenericItem()).OrderBy(g => g.Title).ToList() : new List<GenericItem>();
        }

        public async Task<Credential> GetCredential(string credentialId) => (await _programGetter.GetProgramByCredential(credentialId)).Credentials?.SingleOrDefault(c => c.Id == credentialId) ?? new Credential();
    }
}