using System.Text.Json;

namespace ProgramInformationV2.Data.Agent {
    public class FullCourse(string? url, string? apiKey) : BaseAgentQuery(url, apiKey) {
        public async Task<GenericItem> GenerateInformation(string input, string url, IEnumerable<string> tagList) {
            SystemPrompt =
                $"You are a helpful AI assistant that analyzes a university course. Rewrite the course based on the input and the Illinois Brand Standard style guide at https://brand.illinois.edu/editorial-and-style-guide. Respond with a JSON object that has the following values: summary, description, details, and tags. \n\n Summary is a one sentence description in plain text used in a card. Description is a two to three sentence description in text format. . Details is a three to five sentence detailed explanation in HTML format, which should have a <h2> summary tag and enclosed in a <p>, describing what the course will teach and who will benefit. Use unordered lists (<ul> and <li>) in the details when appropriate to help with scanning. The language of the description and details should be friendly, helpful, and targeted for starting college students. Tags is a semi-colon delimited string containing a list of tags appropriate to the course. Limit yourself to 3 tags. Tags should always be one of the following: {string.Join(", ", tagList)}";
            UserPrompt = string.IsNullOrWhiteSpace(url) ?
                $"Analyze this text and generate a JSON file based on the system prompt." :
                $"Analyze this title and the contents of the URL {url} and generate a JSON file based on the system prompt.";
            var response = await GetResponseAsync(input);
            var jsonDoc = JsonDocument.Parse(response);
            var returnValue = new GenericItem {
                Title = input,
                Url = url
            };
            if (jsonDoc.RootElement.TryGetProperty("summary", out var summaryElement)) {
                returnValue.Summary = summaryElement.GetString() ?? "";
            }
            if (jsonDoc.RootElement.TryGetProperty("description", out var descriptionElement)) {
                returnValue.Description = descriptionElement.GetString() ?? "";
            }
            if (jsonDoc.RootElement.TryGetProperty("details", out var detailsElement)) {
                returnValue.Details = detailsElement.GetString() ?? "";
            }
            if (jsonDoc.RootElement.TryGetProperty("tags", out var tagsElement)) {
                returnValue.SkillList = tagsElement.GetString()?.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList() ?? [];
            }
            return returnValue;
        }
    }
}
