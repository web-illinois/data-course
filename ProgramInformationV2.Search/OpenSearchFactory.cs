using Amazon;
using Amazon.Runtime;
using OpenSearch.Client;
using OpenSearch.Net;
using OpenSearch.Net.Auth.AwsSigV4;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Search {

    public static class OpenSearchFactory {

        private const string TempIndex = "pcr2_tempindex";

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
            // NOTE: change the 'forceIndexCreation' to true if you are changing the index -- this will greatly increase the load time.
            returnValue += ReloadIndex(openSearchClient, UrlTypes.Programs, false);
            returnValue += ReloadIndex(openSearchClient, UrlTypes.Courses, false);
            returnValue += ReloadIndex(openSearchClient, UrlTypes.RequirementSets, false);
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

        private static string ReloadIndex(OpenSearchClient openSearchClient, UrlTypes url, bool forceIndexCreation) {
            if (!forceIndexCreation) {
                return CreateIndex(openSearchClient, url, false);
            }
            var indexName = url.ConvertToUrlString();
            var returnValue = $"Reloading {indexName}: ";
            returnValue += CreateIndex(openSearchClient, url, true);
            var reindexResponse = openSearchClient.ReindexOnServer(r => r
                .Source(s => s.Index(indexName))
                .Destination(d => d.Index(TempIndex))
                .WaitForCompletion(true)
            );
            if (!reindexResponse.IsValid) {
                throw new Exception(reindexResponse.DebugInformation);
            }
            var deleteResponse = openSearchClient.Indices.Delete(indexName);
            returnValue += $"Delete {(deleteResponse.IsValid ? "succeeded" : "failed")} - {deleteResponse.DebugInformation}; ";
            returnValue += CreateIndex(openSearchClient, url, false);
            var movebackResponse = openSearchClient.ReindexOnServer(r => r
                .Source(s => s.Index(TempIndex))
                .Destination(d => d.Index(indexName))
                .WaitForCompletion(true)
            );
            if (!movebackResponse.IsValid) {
                throw new Exception(movebackResponse.DebugInformation);
            }
            var deleteTempResponse = openSearchClient.Indices.Delete(TempIndex);
            returnValue += $"Delete Temp {(deleteTempResponse.IsValid ? "succeeded" : "failed")} - {deleteTempResponse.DebugInformation}; ";
            return returnValue;
        }

        private static string CreateIndex(OpenSearchClient openSearchClient, UrlTypes url, bool temp) {
            return url switch {
                UrlTypes.Programs => CreateProgramIndex(openSearchClient, temp),
                UrlTypes.Courses => CreateCourseIndex(openSearchClient, temp),
                UrlTypes.RequirementSets => CreateRequirementSetIndex(openSearchClient, temp),
                _ => string.Empty,
            };
        }

        private static string CreateProgramIndex(OpenSearchClient openSearchClient, bool temp) {
            var indexName = temp ? TempIndex : UrlTypes.Programs.ConvertToUrlString();
            var response = openSearchClient.Indices.Create(indexName, c => c.Map(m => m.AutoMap<Program>().Properties<Program>(p => p
                .Keyword(k => k.Name(f => f.Credentials.Select(f => f.FormatType)))
                .Keyword(k => k.Name(f => f.Credentials.Select(f => f.CredentialType)))
                .Keyword(k => k.Name(f => f.Credentials.Select(f => f.ProgramId)))
                .Keyword(k => k.Name(f => f.Credentials.Select(f => f.DepartmentList))))));
            return $"Create Program {(response.IsValid ? "succeeded" : "failed")} - {response.DebugInformation}; ";
        }

        private static string CreateCourseIndex(OpenSearchClient openSearchClient, bool temp) {
            var indexName = temp ? TempIndex : UrlTypes.Courses.ConvertToUrlString();
            var response = openSearchClient.Indices.Create(indexName, c => c.Map(m => m.AutoMap<Course>()));
            return $"Create Course {(response.IsValid ? "succeeded" : "failed")} - {response.DebugInformation}; ";
        }

        private static string CreateRequirementSetIndex(OpenSearchClient openSearchClient, bool temp) {
            var indexName = temp ? TempIndex : UrlTypes.RequirementSets.ConvertToUrlString();
            var response = openSearchClient.Indices.Create(indexName, c => c.Map(m => m.AutoMap<RequirementSet>()));
            return $"Create RequirementSet {(response.IsValid ? "succeeded" : "failed")} - {response.DebugInformation}; ";
        }
    }
}