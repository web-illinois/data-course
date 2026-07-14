using Amazon;
using Amazon.Runtime;
using OpenSearch.Client;
using OpenSearch.Net;
using OpenSearch.Net.Auth.AwsSigV4;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Search {

    public static class OpenSearchFactory {

        public static OpenSearchClient CreateClient(string? baseUrl, string? accessKey, string? secretKey, bool debug) {
            var client = new OpenSearchClient(GenerateConnection(baseUrl, accessKey, secretKey, debug));
            _ = client.ConnectionSettings.DefaultIndices.Add(typeof(Program), UrlTypes.Programs.ConvertToUrlString());
            _ = client.ConnectionSettings.DefaultIndices.Add(typeof(Course), UrlTypes.Courses.ConvertToUrlString());
            _ = client.ConnectionSettings.DefaultIndices.Add(typeof(RequirementSet), UrlTypes.RequirementSets.ConvertToUrlString());
            return client;
        }

        public static OpenSearchLowLevelClient CreateLowLevelClient(string? baseUrl, string? accessKey, string? secretKey, bool debug) => new(GenerateConnection(baseUrl, accessKey, secretKey, debug));

        public static string MapIndex(OpenSearchClient openSearchClient) {
            var returnValue = "Mapping: ";
            // NOTE: The following line is commented out to prevent deletion of indices. Uncomment if you want to delete existing indices before creating new ones.
            // returnValue += openSearchClient.Indices.Delete(UrlTypes.Programs.ConvertToUrlString()) + " " + openSearchClient.Indices.Delete(UrlTypes.Courses.ConvertToUrlString()) + " " + openSearchClient.Indices.Delete(UrlTypes.RequirementSets.ConvertToUrlString());
            // returnValue += openSearchClient.Indices.Delete(UrlTypes.Programs.ConvertToUrlString()) + " " + openSearchClient.Indices.Delete(UrlTypes.Courses.ConvertToUrlString());
            var indexPrograms = openSearchClient.Indices.Create(UrlTypes.Programs.ConvertToUrlString(), c => c.Map(m => m.AutoMap<Program>().Properties<Program>(p => p
                .Keyword(k => k.Name(f => f.Credentials.Select(f => f.FormatType)))
                .Keyword(k => k.Name(f => f.Credentials.Select(f => f.CredentialType)))
                .Keyword(k => k.Name(f => f.Credentials.Select(f => f.ProgramId)))
                .Keyword(k => k.Name(f => f.Credentials.Select(f => f.DepartmentList))))));
            returnValue += $"Program {(indexPrograms.IsValid ? "created" : "failed")} - {indexPrograms.DebugInformation}; ";
            var indexCourses = openSearchClient.Indices.Create(UrlTypes.Courses.ConvertToUrlString(), c => c.Map(m => m.AutoMap<Course>()));
            returnValue += $"Course {(indexCourses.IsValid ? "created" : "failed")} - {indexCourses.DebugInformation}; ";
            var indexRequirementSets = openSearchClient.Indices.Create(UrlTypes.RequirementSets.ConvertToUrlString(), c => c.Map(m => m.AutoMap<RequirementSet>()));
            returnValue += $"Req Set {(indexRequirementSets.IsValid ? "created" : "failed")}; - {indexRequirementSets.DebugInformation}; ";
            return returnValue;
        }

        private static ConnectionSettings GenerateConnection(string? baseUrl, string? accessKey, string? secretKey, bool debug) {
            var nodeAddress = new Uri(baseUrl ?? "");
            var connection = string.IsNullOrWhiteSpace(accessKey) || string.IsNullOrWhiteSpace(secretKey) ? null : new AwsSigV4HttpConnection(new BasicAWSCredentials(accessKey, secretKey), RegionEndpoint.USEast2);
            var config = new ConnectionSettings(nodeAddress, connection);
            if (debug) {
                _ = config.DisableDirectStreaming(true);
            }
            return config;
        }
    }
}