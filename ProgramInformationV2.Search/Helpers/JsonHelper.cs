using Newtonsoft.Json;
using OpenSearch.Client;
using OpenSearch.Net;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Search.Helpers {

    public class JsonHelper(OpenSearchLowLevelClient? openSearchLowLevelClient, OpenSearchClient? openSearchClient) {
        private readonly OpenSearchClient _openSearchClient = openSearchClient ?? default!;
        private readonly OpenSearchLowLevelClient _openSearchLowLevelClient = openSearchLowLevelClient ?? default!;

        public async Task<string> GetJson(string sourceCode, UrlTypes urltype, int page) {
            var body = await _openSearchLowLevelClient.SearchAsync<StringResponse>(urltype.ConvertToUrlString(), GenerateGetJson(sourceCode, page));
            dynamic? json = JsonConvert.DeserializeObject(body.Body ?? "");
            return json == null || json?.hits == null || json?.hits.hits == null ? "error" : (string) JsonConvert.SerializeObject(json?.hits.hits);
        }

        public async Task<string> GetJsonFull(string sourceCode, UrlTypes urltype) {
            var body = await _openSearchLowLevelClient.SearchAsync<StringResponse>(urltype.ConvertToUrlString(), GenerateGetJson(sourceCode));
            dynamic? json = JsonConvert.DeserializeObject(body.Body ?? "");
            return json == null || json?.hits == null || json?.hits.hits == null ? "error" : (string) JsonConvert.SerializeObject(json?.hits.hits);
        }

        public async Task<string> LoadJson(string sourceCode, UrlTypes urltype, string jsonString) {
            var json = JsonConvert.DeserializeObject<IEnumerable<dynamic>>(jsonString ?? "");
            var success = 0;
            var failureIds = new List<string>();
            var useRawJsonItems = false;
            if (json == null) {
                return "error";
            }
            foreach (var jsonItem in json) {
                if (jsonItem._source == null) {
                    useRawJsonItems = true;
                    try {
                        if (urltype == UrlTypes.Programs) {
                            Program stronglyTypedProgram = JsonConvert.DeserializeObject<Program>(jsonItem.ToString());
                            stronglyTypedProgram.Prepare();
                            var response = await _openSearchClient.IndexAsync(stronglyTypedProgram, i => i.Index(urltype.ConvertToUrlString()));
                            if (response.IsValid) {
                                success++;
                            } else {
                                failureIds.Add("unknown program ID");
                            }
                        } else if (urltype == UrlTypes.Courses) {
                            Course stronglyTypedCourse = JsonConvert.DeserializeObject<Course>(jsonItem.ToString());
                            stronglyTypedCourse.Prepare();
                            var response = await _openSearchClient.IndexAsync(stronglyTypedCourse, i => i.Index(urltype.ConvertToUrlString()));
                            if (response.IsValid) {
                                success++;
                            } else {
                                failureIds.Add("unknown course ID");
                            }
                        } else if (urltype == UrlTypes.RequirementSets) {
                            RequirementSet stronglyTypedRequirementSet = JsonConvert.DeserializeObject<RequirementSet>(jsonItem.ToString());
                            stronglyTypedRequirementSet.Prepare();
                            var response = await _openSearchClient.IndexAsync(stronglyTypedRequirementSet, i => i.Index(urltype.ConvertToUrlString()));
                            if (response.IsValid) {
                                success++;
                            } else {
                                failureIds.Add("unknown requirement set ID");
                            }
                        } else {
                            failureIds.Add("unknown item from old system");
                        }
                    } catch (Exception e) {
                        failureIds.Add($"Error processing item: {e.Message}");
                    }
                } else {
                    var body = JsonConvert.SerializeObject(jsonItem._source);
                    string id = jsonItem._id.ToString();
                    if (id.StartsWith(sourceCode + "-")) {
                        var response = await _openSearchLowLevelClient.IndexAsync<StringResponse>(urltype.ConvertToUrlString(), id, body);
                        if (response.Success) {
                            success++;
                        } else {
                            failureIds.Add(id);
                        }
                    }
                }
            }
            if (failureIds.Count > 5) {
                return $"Loaded {success} items. Failed to load {failureIds.Count} items. {(useRawJsonItems ? "Used raw JSON." : "")}";
            }
            if (failureIds.Count > 0) {
                return $"Loaded {success} items. Failed to load items: {string.Join("; ", failureIds)}. {(useRawJsonItems ? "Used raw JSON." : "")}";
            }
            return $"Loaded {success} items. {(useRawJsonItems ? "Used raw JSON." : "")}";
        }

        private static string GenerateGetJson(string sourceCode, int skip) => "{ \"from\": " + (skip * 50) + ", \"size\": 50, \"query\":{\"match\":{\"source\":{\"query\":\"" + sourceCode + "\"}}}}";

        private static string GenerateGetJson(string sourceCode) => "{ \"size\": 10000, \"query\":{\"match\":{\"source\":{\"query\":\"" + sourceCode + "\"}}}}";
    }
}