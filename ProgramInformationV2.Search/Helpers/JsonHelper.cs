using Newtonsoft.Json;
using OpenSearch.Client;
using OpenSearch.Net;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Search.Helpers {

    public class JsonHelper(OpenSearchLowLevelClient? openSearchLowLevelClient, OpenSearchClient? openSearchClient) {
        private readonly OpenSearchClient _openSearchClient = openSearchClient ?? default!;
        private readonly OpenSearchLowLevelClient _openSearchLowLevelClient = openSearchLowLevelClient ?? default!;

        public async Task<string> GetJson(string sourceCode, UrlTypes urlType, int page) {
            var body = await _openSearchLowLevelClient.SearchAsync<StringResponse>(urlType.ConvertToUrlString(), GenerateGetJson(sourceCode, page));
            dynamic? json = JsonConvert.DeserializeObject(body.Body ?? "");
            return json == null || json?.hits == null || json?.hits.hits == null ? "error" : (string)JsonConvert.SerializeObject(json?.hits.hits);
        }

        public async Task<string> GetJsonFull(string sourceCode, UrlTypes urlType) {
            var body = await _openSearchLowLevelClient.SearchAsync<StringResponse>(urlType.ConvertToUrlString(), GenerateGetJson(sourceCode));
            dynamic? json = JsonConvert.DeserializeObject(body.Body ?? "");
            return json == null || json?.hits == null || json?.hits.hits == null ? "error" : (string)JsonConvert.SerializeObject(json?.hits.hits);
        }

        public async Task<string> LoadJson(string sourceCode, UrlTypes urlType, string jsonString) {
            var json = JsonConvert.DeserializeObject<IEnumerable<dynamic>>(jsonString ?? "");
            var success = 0;
            var useRawJsonItems = false;
            var failureIds = new List<string>();
            if (json == null) {
                return "error";
            }
            foreach (var jsonItem in json) {
                var body = jsonItem.ToString();
                if (jsonItem._source == null) {
                    useRawJsonItems = true;
                } else {
                    body = JsonConvert.SerializeObject(jsonItem._source);
                }
                try {
                    switch (urlType) {
                        case UrlTypes.Programs: {
                            Program stronglyTypedProgram = JsonConvert.DeserializeObject<Program>(body);
                            if (stronglyTypedProgram == null) {
                                return "error";
                            }
                            foreach (var credential in stronglyTypedProgram.Credentials) {
                                credential.Prepare();
                            }
                            stronglyTypedProgram.Prepare();
                            var response = await _openSearchClient.IndexAsync(stronglyTypedProgram, i => i.Index(urlType.ConvertToUrlString()));
                            if (response.IsValid) {
                                success++;
                            } else {
                                failureIds.Add("unknown program ID");
                            }
                            break;
                        }
                        case UrlTypes.Courses: {
                            Course stronglyTypedCourse = JsonConvert.DeserializeObject<Course>(body);
                            foreach (var credential in stronglyTypedCourse.Sections) {
                                credential.Prepare();
                            }
                            stronglyTypedCourse.Prepare();
                            var response = await _openSearchClient.IndexAsync(stronglyTypedCourse, i => i.Index(urlType.ConvertToUrlString()));
                            if (response.IsValid) {
                                success++;
                            } else {
                                failureIds.Add("unknown course ID");
                            }
                            break;
                        }
                        case UrlTypes.RequirementSets: {
                            RequirementSet stronglyTypedRequirementSet = JsonConvert.DeserializeObject<RequirementSet>(body);
                            stronglyTypedRequirementSet.Prepare();
                            var response = await _openSearchClient.IndexAsync(stronglyTypedRequirementSet, i => i.Index(urlType.ConvertToUrlString()));
                            if (response.IsValid) {
                                success++;
                            } else {
                                failureIds.Add("unknown requirement set ID");
                            }
                            break;
                        }
                        default:
                            failureIds.Add("unknown item from old system");
                            break;
                    }
                } catch (Exception e) {
                    failureIds.Add($"Error processing item: {e.Message}");
                }
            }
            return failureIds.Count > 0
                ? $"Loaded {success} items. Failed to load {(failureIds.Count < 5 ? string.Join("; ", failureIds) : failureIds.Count)} items. {(useRawJsonItems ? "Used raw JSON." : "")}"
                : $"Loaded {success} items. {(useRawJsonItems ? "Used raw JSON." : "")}";
        }

        private static string GenerateGetJson(string sourceCode, int skip) => "{ \"from\": " + (skip * 50) + ", \"size\": 50, \"sort\": [ { \"title.keyword\" : \"asc\" } ], \"query\":{\"match\":{\"source\":{\"query\":\"" + sourceCode + "\"}}}}";

        private static string GenerateGetJson(string sourceCode) => "{ \"size\": 10000, \"sort\": [ { \"title.keyword\" : \"asc\" } ], \"query\":{\"match\":{\"source\":{\"query\":\"" + sourceCode + "\"}}}}";
    }
}