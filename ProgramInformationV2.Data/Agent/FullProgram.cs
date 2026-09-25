using System.Text.Json;

namespace ProgramInformationV2.Data.Agent {
    public class FullProgram(string? url, string? apiKey) : BaseAgentQuery(url, apiKey) {
        public async Task<GenericItem> GenerateInformation(string input, string url, IEnumerable<string> tagList) {
            SystemPrompt = $"You are a helpful AI assistant that analyzes a university program. Rewrite the program based on the title, the contents of the URL {url}, and the Illinois Brand Standard style guide at https://brand.illinois.edu/editorial-and-style-guide. Respond with a JSON object that has the following values: summary, description in HTML format, and tags. \n\n Summary is a one sentence description in plain text used in a card. Description is a three-sentence detailed explanation in HTML format. The language of the description should be friendly, helpful, and targeted for starting college students. Tags is a semi-colon delimited string containing a list of tags. Limit yourself to 3 tags. Tags should always be one of the following: {string.Join(", ", tagList)}";
            UserPrompt = $"Analyze this text and the contents of the URL {url} and generate a JSON file based on the system prompt.";
            var response = await GetResponseAsync(input);
            var jsonDoc = JsonDocument.Parse(response);
            var returnValue = new GenericItem {
                Title = input,
                Url = url
            };
            if (jsonDoc.RootElement.TryGetProperty("description", out var descriptionElement)) {
                returnValue.Description = descriptionElement.GetString() ?? "";
            }
            if (jsonDoc.RootElement.TryGetProperty("summary", out var summaryElement)) {
                returnValue.Summary = summaryElement.GetString() ?? "";
            }
            if (jsonDoc.RootElement.TryGetProperty("tags", out var tagsElement)) {
                returnValue.SkillList = tagsElement.GetString()?.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList() ?? [];
            }
            return returnValue;
        }
    }
}
