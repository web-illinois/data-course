using OpenSearch.Client;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Search.Getters {

    public class RequirementSetGetter(OpenSearchClient? openSearchClient) : BaseGetter<RequirementSet>(openSearchClient) {

        public async Task<List<GenericItem>> GetAllRequirementSetsBySource(string source, string search) {
            var response = await _openSearchClient.SearchAsync<RequirementSet>(s => s.Index(UrlTypes.RequirementSets.ConvertToUrlString())
                    .Size(1000)
                    .Query(q => q
                    .Bool(b => b
                    .Filter(f => f.Term(m => m.Field(fld => fld.Source).Value(source)))
                    .Must(m => string.IsNullOrWhiteSpace(search) ? m.MatchAll() : m.Match(m => m.Field(fld => fld.Title).Query(search).Operator(Operator.And))))));
            LogDebug(response);
            return response.IsValid ? response.Documents.Select(r => r.GetGenericItem()).OrderBy(g => g.Title).ToList() : [];
        }

        public async Task<List<GenericItem>> GetAllRequirementSetsBySourceIncludingPrivate(string source, string search, string credentialId) {
            var response = await _openSearchClient.SearchAsync<RequirementSet>(s => s.Index(UrlTypes.RequirementSets.ConvertToUrlString())
                    .Size(1000)
                    .Query(q => q
                    .Bool(b => b
                    .Filter(f => f
                    .Bool(b2 => b2
                    .Should(s => s
                    .Term(m => m.Field(fld => fld.Source).Value(source)), m => m.Term(t => t.IsReused, true), m => m.Term(t => t.CredentialId, credentialId)).MinimumShouldMatch(MinimumShouldMatch.Fixed(2))
                    .Must(m => string.IsNullOrWhiteSpace(search) ? m.MatchAll() : m.Match(m => m.Field(fld => fld.Title).Query(search).Operator(Operator.And))))))));
            LogDebug(response);
            return response.IsValid ? response.Documents.Select(r => r.GetGenericItem()).OrderBy(g => g.Title).ToList() : [];
        }

        public async Task<RequirementSet> GetRequirementSet(string id) {
            if (string.IsNullOrWhiteSpace(id)) {
                return new();
            }
            var response = await _openSearchClient.GetAsync<RequirementSet>(id);
            LogDebug(response);
            return response.IsValid ? response.Source : new RequirementSet();
        }

        public async Task<List<RequirementSet>> GetRequirementSets(IEnumerable<string> ids) {
            if (ids == null || !ids.Any()) {
                return [];
            }
            var response = await _openSearchClient.SearchAsync<RequirementSet>(s => s.Index(UrlTypes.RequirementSets.ConvertToUrlString())
                .Size(50)
                .Query(q => q
                .Bool(b => b
                .Filter(f => f.Terms(m => m.Field(fld => fld.Id).Terms(ids))))));
            LogDebug(response);
            return response.IsValid ? [.. response.Documents.OrderBy(d => d.Title)] : [];
        }

        public async Task<List<GenericItem>> GetRequirementSetsChosen(IEnumerable<string> ids) => [.. (await GetRequirementSets(ids)).Select(r => r.GetGenericItem())];
    }
}