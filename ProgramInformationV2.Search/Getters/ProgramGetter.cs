using OpenSearch.Client;
using ProgramInformationV2.Search.JsonThinModels;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Search.Getters {

    public class ProgramGetter(OpenSearchClient? openSearchClient) : BaseGetter<Program>(openSearchClient) {

        public async Task<List<GenericItem>> GetAllProgramsBySource(string source, string search, string department) {
            var response = await _openSearchClient.SearchAsync<Program>(s => s.Index(UrlTypes.Programs.ConvertToUrlString())
                    .Size(1000)
                    .Query(q => q
                    .Bool(b => b
                    .Filter(f => f.Term(m => m.Field(fld => fld.Source).Value(source)),
                        f => string.IsNullOrWhiteSpace(department) ? f.MatchAll() : f.Terms(m => m.Field(fld => fld.DepartmentList).Terms(department)))
                    .Must(m => string.IsNullOrWhiteSpace(search) ? m.MatchAll() : m.Match(m => m.Field(fld => fld.Title).Query(search).Operator(Operator.And))))));
            LogDebug(response);
            return response.IsValid ? [.. response.Documents.Select(r => r.GetGenericItem()).OrderBy(g => g.Title)] : [];
        }

        public async Task<Program> GetProgram(string id, bool activeOnly = false) {
            if (string.IsNullOrWhiteSpace(id)) {
                return new();
            }
            var response = await _openSearchClient.GetAsync<Program>(id);
            LogDebug(response);
            return !response.IsValid || response.Source == null
                ? new Program()
                : activeOnly && !response.Source.IsActive
                ? new Program()
                : response.Source;
        }

        public async Task<Program> GetProgram(string source, string fragment) {
            var response = await _openSearchClient.SearchAsync<Program>(s => s.Index(UrlTypes.Programs.ConvertToUrlString())
                    .Query(q => q
                    .Bool(b => b
                    .Filter(f => f.Term(m => m.Field(fld => fld.Source).Value(source)),
                            f => f.Term(m => m.Field(fld => fld.IsActive).Value(true)),
                            f => f.Term(m => m.Field(fld => fld.Fragment).Value(fragment))))));
            LogDebug(response);
            return response.IsValid ? response.Documents?.FirstOrDefault() ?? new() : new();
        }

        public async Task<Program> GetProgramByCredential(string credentialId) {
            if (string.IsNullOrWhiteSpace(credentialId)) {
                return new();
            }
            var response = await _openSearchClient.SearchAsync<Program>(s => s.Index(UrlTypes.Programs.ConvertToUrlString())
                    .Query(q => q
                    .Match(m => m.Field(fld => fld.CredentialIdList).Query(credentialId))));
            LogDebug(response);
            return response.IsValid ? response.Documents.FirstOrDefault() ?? new() : new();
        }

        public async Task<SearchObject<Program>> GetPrograms(string source, string search, IEnumerable<string> tags, IEnumerable<string> tags2, IEnumerable<string> tags3, IEnumerable<string> skills, IEnumerable<string> departments, IEnumerable<string> formats, IEnumerable<string> credentials, int take, int skip) {
            var response = await _openSearchClient.SearchAsync<Program>(s => s.Index(UrlTypes.Programs.ConvertToUrlString())
                    .Skip(skip)
                    .Size(take)
                    .Query(q => q
                    .Bool(b => b
                    .Filter(f => f.Term(m => m.Field(fld => fld.Source).Value(source)),
                        f => f.Term(m => m.Field(fld => fld.IsActive).Value(true)),
                        f => tags.Any() ? f.Terms(m => m.Field(fld => fld.TagList).Terms(tags)) : f.MatchAll(),
                        f => tags2.Any() ? f.Terms(m => m.Field(fld => fld.TagList).Terms(tags2)) : f.MatchAll(),
                        f => tags3.Any() ? f.Terms(m => m.Field(fld => fld.TagList).Terms(tags3)) : f.MatchAll(),
                        f => credentials.Any()
                             ? formats.Count() == 1 ?
                                (formats.Single() == "Online" ? f.Terms(m => m.Field(fld => fld.CredentialsOnlineList).Terms(credentials))
                                : formats.Single() == "On-Campus" ? f.Terms(m => m.Field(fld => fld.CredentialsOnCampusList).Terms(credentials))
                                : formats.Single() == "Off-Campus" ? f.Terms(m => m.Field(fld => fld.CredentialsOffCampusList).Terms(credentials))
                                : formats.Single() == "Hybrid" ? f.Terms(m => m.Field(fld => fld.CredentialsHybridList).Terms(credentials))
                                : f.Terms(m => m.Field(fld => fld.CredentialsFullList).Terms(credentials)))
                              : f.Terms(m => m.Field(fld => fld.CredentialsFullList).Terms(credentials)) : f.MatchAll(),
                        f => formats.Any() ? f.Terms(m => m.Field(fld => fld.Formats).Terms(formats)) : f.MatchAll(),
                        f => departments.Any() ? f.Terms(m => m.Field(fld => fld.DepartmentList).Terms(departments)) : f.MatchAll(),
                        f => skills.Any() ? f.Terms(m => m.Field(fld => fld.SkillList).Terms(skills)) : f.MatchAll())
                    .Must(m => !string.IsNullOrWhiteSpace(search) ? m.MultiMatch(m => m.Fields(fld => fld.Field("title^10").Field("summarytext^5").Field("description^2").Field("whoshouldapply")).Query(search)) : m.MatchAll())))
                    .Suggest(a => a.Phrase("didyoumean", p => p.Text(search).Field(fld => fld.Title))));
            LogDebug(response);

            List<Program> documents = response.IsValid ? (string.IsNullOrEmpty(search) ? [.. response.Documents.OrderBy(p => p.Title)] : [.. response.Documents]) : [];
            return new SearchObject<Program>() {
                Error = !response.IsValid ? response.ServerError.Error.ToString() : "",
                DidYouMean = response.Suggest["didyoumean"].FirstOrDefault()?.Options?.FirstOrDefault()?.Text ?? "",
                Total = (int)response.Total,
                Items = documents
            };
        }
    }
}