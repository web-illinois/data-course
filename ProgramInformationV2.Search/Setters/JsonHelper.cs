using Newtonsoft.Json;
using OpenSearch.Net;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Search.Setters {

    public class JsonHelper(OpenSearchLowLevelClient? openSearchLowLevelClient) {
        private OpenSearchLowLevelClient _openSearchLowLevelClient = openSearchLowLevelClient ?? default!;

        public async Task<string> GetJson(string sourceCode, UrlTypes urltype) {
            var body = await _openSearchLowLevelClient.SearchAsync<StringResponse>(urltype.ConvertToUrlString(), GenerateGetJson(sourceCode));
            dynamic? json = JsonConvert.DeserializeObject(body.Body ?? "");
            if (json == null || json?.hits == null || json?.hits.hits == null) {
                return "error";
            }
            return JsonConvert.SerializeObject(json?.hits.hits);
        }

        public async Task<string> LoadJson(string sourceCode, UrlTypes urltype, string jsonString) {
            var json = JsonConvert.DeserializeObject<IEnumerable<dynamic>>(jsonString ?? "");
            var success = 0;
            var failureIds = new List<string>();
            if (json == null) {
                return "error";
            }
            foreach (var jsonItem in json) {
                var body = JsonConvert.SerializeObject(jsonItem._source);
                string id = jsonItem._id.ToString();
                if (id.StartsWith(sourceCode)) {
                    var response = await _openSearchLowLevelClient.IndexAsync<StringResponse>(urltype.ConvertToUrlString(), id, body);
                    if (response.Success) {
                        success++;
                    } else {
                        failureIds.Add(id);
                    }
                }
            }
            return failureIds.Count == 0 ? $"Loaded {success} items." : $"Failed to load items: {string.Join("; ", failureIds)}";
        }

        private string GenerateGetJson(string sourceCode) => "{\"query\":{\"match\":{\"source\":{\"query\":\"" + sourceCode + "\"}}}}";
    }
}