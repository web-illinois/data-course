using OpenSearch.Client;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Search.AuditHelpers {

    public class ProgramAudits(OpenSearchClient? openSearchClient) {
        private readonly OpenSearchClient _openSearchClient = openSearchClient ?? default!;

        public async Task<List<GenericItem>> GetAllProgramsAndCredentials(string source) {
            var response = await _openSearchClient.SearchAsync<Program>(s => s.Index(UrlTypes.Programs.ConvertToUrlString())
                    .Size(1000)
                    .Query(q => q
                    .Bool(b => b
                    .Filter(f => f.Term(m => m.Field(fld => fld.Source).Value(source))))));
            if (_openSearchClient.ConnectionSettings.DisableDirectStreaming) {
                Console.WriteLine(response.DebugInformation);
            }
            if (!response.IsValid) {
                return [];
            }
            var returnValue = new List<GenericItem>();
            foreach (var program in response.Documents.OrderBy(d => d.Title)) {
                returnValue.Add(new GenericItem {
                    Id = GetUrl(program.UrlFull, program.Url, program.Fragment),
                    Title = "Program: " + program.Title,
                    IsActive = program.IsActive
                });
                foreach (var credential in program.Credentials) {
                    returnValue.Add(new GenericItem {
                        Id = GetUrl(credential.UrlFull, credential.Url, credential.Fragment),
                        Title = "Credential: " + credential.InternalTitle,
                        IsActive = credential.IsActive
                    });
                }
            }
            return [.. returnValue];
        }

        private static string GetUrl(string urlFull, string url, string fragment) {
            return string.IsNullOrWhiteSpace(urlFull) ? string.IsNullOrWhiteSpace(fragment) ? url : fragment : urlFull;
        }
    }
}