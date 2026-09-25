namespace ProgramInformationV2.Data.Agent {
    public class TagCreator(string? url, string? apiKey) : BaseAgentQuery(url, apiKey) {
        public async Task<IEnumerable<string>> CreateTag(string input, IEnumerable<string> tagList) {
            SystemPrompt = $"You are a helpful AI assistant that analyzes a university program or course and assigns relevant tags. Respond with a semi-colon delimited string containing a list of tags. Limit yourself to 3 tags. Tags should always be one of the following: {string.Join(", ", tagList)}";
            UserPrompt = "Analyze this text and assign relevant tags.";
            var response = await GetResponseAsync(input);
            return response.Split(';', StringSplitOptions.RemoveEmptyEntries);
        }
    }
}
