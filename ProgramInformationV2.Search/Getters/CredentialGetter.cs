using OpenSearch.Client;
using ProgramInformationV2.Search.JsonThinModels;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Search.Getters {

    public class CredentialGetter(OpenSearchClient? openSearchClient, ProgramGetter programGetter, RequirementSetGetter requirementSetGetter) : BaseGetter<Program>(openSearchClient) {
        private readonly ProgramGetter _programGetter = programGetter;
        private readonly RequirementSetGetter _requirementSetGetter = requirementSetGetter;

        public async Task<List<GenericItem>> GetAllCredentialsByRequirementId(string requirementId) {
            var response =
                await _openSearchClient.SearchAsync<Program>(s => s.Index(UrlTypes.Programs.ConvertToUrlString())
                    .Size(100)
                    .Query(q => q
                    .Bool(b => b
                    .Filter(f => f.Term(t => t.Field(fld => fld.Credentials.Select(c => c.RequirementSetIds)).Value(requirementId))))));
            LogDebug(response);
            return response.IsValid ? [.. response.Documents.SelectMany(c => c.Credentials).Where(c => c.RequirementSetIds.Contains(requirementId)).Select(r => r.GetGenericItem()).OrderBy(g => g.Title)] : [];
        }

        public async Task<List<GenericItem>> GetAllCredentialsBySource(string source, string search) {
            var response = await _openSearchClient.SearchAsync<Program>(s => s.Index(UrlTypes.Programs.ConvertToUrlString())
                .Size(1000)
                .Query(q => q
                .Bool(b => b
                .Filter(f => f.Term(m => m.Field(fld => fld.Source).Value(source)))
                .Must(m => string.IsNullOrWhiteSpace(search) ? m.MatchAll() : m.Match(m => m.Field(fld => fld.Credentials.Select(ft => ft.Title)).Query(search).Operator(Operator.And))))));
            LogDebug(response);
            return response.IsValid ? [.. response.Documents.SelectMany(c => c.Credentials).Select(r => r.GetGenericItem()).OrderBy(g => g.Title)] : [];
        }

        public async Task<Credential> GetCredential(string credentialId) => (await _programGetter.GetProgramByCredential(credentialId)).Credentials?.SingleOrDefault(c => c.Id == credentialId) ?? new();

        public async Task<SearchObject<Credential>> GetCredentials(string source, string search, IEnumerable<string> tags, IEnumerable<string> tags2, IEnumerable<string> tags3, IEnumerable<string> skills, IEnumerable<string> departments, IEnumerable<string> formats, IEnumerable<string> credentials) {
            var response = await _programGetter.GetPrograms(source, search, tags, tags2, tags3, skills, departments, formats, credentials);
            var credentialList = response.Items.SelectMany(p => p.Credentials).Where(c => c.IsActive).OrderBy(c => c.Title).ToList();
            if (tags.Any()) {
                credentialList = credentialList.Where(c => c.TagList.Any(t => tags.Contains(t))).ToList();
            }
            if (tags2.Any()) {
                credentialList = credentialList.Where(c => c.TagList.Any(t => tags2.Contains(t))).ToList();
            }
            if (tags3.Any()) {
                credentialList = credentialList.Where(c => c.TagList.Any(t => tags3.Contains(t))).ToList();
            }
            if (skills.Any()) {
                credentialList = credentialList.Where(c => c.SkillList.Any(s => skills.Contains(s))).ToList();
            }
            if (departments.Any()) {
                credentialList = credentialList.Where(c => c.DepartmentList.Any(d => departments.Contains(d))).ToList();
            }
            if (credentials.Any()) {
                credentialList = credentialList.Where(c => credentials.Contains(c.CredentialTypeString)).ToList();
            }

            return new SearchObject<Credential>() {
                Error = response.Error,
                DidYouMean = response.DidYouMean,
                Total = credentialList.Count,
                Items = string.IsNullOrWhiteSpace(search) ? [.. credentialList.OrderBy(c => c.Title)] : [.. credentialList]
            };
        }

        public async Task<CredentialWithRequirementSets> GetCredentialWithRequirementSet(string credentialId) {
            var program = await _programGetter.GetProgramByCredential(credentialId);
            var credential = program.Credentials?.SingleOrDefault(c => c.Id == credentialId) ?? new();

            return await AddRequirementSets(credential, program?.Credentials ?? []);
        }

        public async Task<CredentialWithRequirementSets> GetCredentialWithRequirementSet(string source, string fragment) {
            var response = await _openSearchClient.SearchAsync<Program>(s => s.Index(UrlTypes.Programs.ConvertToUrlString())
                    .Query(q => q
                    .Bool(b => b
                    .Filter(f => f.Term(m => m.Field(fld => fld.Source).Value(source)),
                            f => f.Term(m => m.Field(fld => fld.IsActive).Value(true)),
                            f => f.Term(m => m.Field(fld => fld.Credentials.Select(c => c.Fragment)).Value(fragment))))));
            LogDebug(response);
            var program = response.IsValid ? response.Documents?.FirstOrDefault() : new();
            var credential = program?.Credentials.FirstOrDefault(c => c.Fragment == fragment) ?? new();
            return await AddRequirementSets(credential, program?.Credentials ?? []);
        }

        private async Task<CredentialWithRequirementSets> AddRequirementSets(Credential credential, List<Credential> otherCredentials) {
            if (!credential.IsActive) {
                return new();
            }

            var requirementSets = await _requirementSetGetter.GetRequirementSets(credential.RequirementSetIds);
            var returnValue = new CredentialWithRequirementSets {
                Credential = credential,
                RequirementSets = []
            };
            returnValue.OtherCredentials = [.. otherCredentials.Where(oc => oc.Id != credential.Id && oc.IsActive).OrderBy(oc => oc.CredentialType).Select(oc => new CredentialOption {
                Title = oc.Title, Url = oc.Url, UrlFull = oc.UrlFull, CredentialType = oc.CredentialType, Id = oc.Id, FormatType = oc.FormatType
            })];
            foreach (var reqId in credential.RequirementSetIds) {
                if (requirementSets.Any(r => r.Id == reqId)) {
                    returnValue.RequirementSets.Add(requirementSets.First(r => r.Id == reqId));
                }
            }
            return returnValue;
        }
    }
}