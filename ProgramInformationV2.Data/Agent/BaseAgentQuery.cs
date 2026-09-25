using Azure.AI.OpenAI;
using Microsoft.Extensions.AI;

namespace ProgramInformationV2.Data.Agent {
    public abstract class BaseAgentQuery(string? url, string? apiKey) {
        private readonly string _url = url ?? "";
        private readonly string _apiKey = apiKey ?? "";
        private const string _deploymentName = "gpt-chat-latest";

        protected string ErrorMessage { get; set; } = "";
        protected string SystemPrompt { get; set; } = "";
        protected string UserPrompt { get; set; } = "";

        protected string UserPlusItem(string s) => $"{UserPrompt.TrimEnd('.', ':', ' ')}:\n\n{s}";
        internal async Task<string> GetResponseAsync(string input) {
            if (!IsValid()) {
                ErrorMessage = "Invalid URL or API key.";
                return "";
            }
            ErrorMessage = "";
            try {
                var chatClient = Setup();
                var messages = GetMessages(input);
                var response = await chatClient.GetResponseAsync(messages);
                return response.Text ?? "";
            } catch (Exception e) {
                ErrorMessage = e.Message;
                return "";
            }
        }

        public bool IsValid() => !string.IsNullOrWhiteSpace(_url) && !string.IsNullOrWhiteSpace(_apiKey);

        public string GetErrorMessage() => ErrorMessage;

        private IChatClient Setup() {
            var openAIClient = new AzureOpenAIClient(new Uri(_url), new System.ClientModel.ApiKeyCredential(_apiKey));
            return openAIClient.GetChatClient(_deploymentName).AsIChatClient();
        }

        private List<ChatMessage> GetMessages(string s) {
            if (string.IsNullOrWhiteSpace(SystemPrompt) || string.IsNullOrWhiteSpace(UserPrompt)) {
                throw new InvalidOperationException("SystemPrompt and UserPrompt must be set before calling GetMessages.");
            }
            return
            [
                new (ChatRole.System, SystemPrompt),
                    new (ChatRole.User, UserPlusItem(s))
            ];
        }
    }
}
