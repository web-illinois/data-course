using OpenSearch.Net;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Search.Setters {

    public class BulkEditor(OpenSearchLowLevelClient? openSearchLowLevelClient) {
        private readonly OpenSearchLowLevelClient _openSearchLowLevelClient = openSearchLowLevelClient ?? default!;

        public async Task<string> DeleteAllItems(string source) {
            var responsePrograms = await _openSearchLowLevelClient.DeleteByQueryAsync<StringResponse>(UrlTypes.Programs.ConvertToUrlString(), GenerateGetJson(source));
            var responseCourses = await _openSearchLowLevelClient.DeleteByQueryAsync<StringResponse>(UrlTypes.Courses.ConvertToUrlString(), GenerateGetJson(source));
            var responseRequirementSets = await _openSearchLowLevelClient.DeleteByQueryAsync<StringResponse>(UrlTypes.RequirementSets.ConvertToUrlString(), GenerateGetJson(source));
            // return response.IsValid ? $"Course {id} deleted" : "error";
            return responsePrograms.Success && responseCourses.Success && responseRequirementSets.Success ? source + " deleted" : source + " not deleted";
        }

        private string GenerateGetJson(string sourceCode) => "{\"query\": { \"bool\": { \"must\": { \"match_all\": { } }, \"filter\": [ { \"bool\": { \"must\": [ { \"term\": { \"source\":  \"" + sourceCode + "\" } } ] } } ] } } }";
    }
}