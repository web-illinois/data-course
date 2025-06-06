using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Search.JsonThinModels {

    public class CredentialOption {
        public CredentialType CredentialType { get; set; }

        public string CredentialTypeString => CredentialType.ConvertToSingleString();
        public FormatType FormatType { get; set; }
        public IEnumerable<string> FormatTypeString => FormatType.ConvertFormatList();
        public string Id { get; set; } = "";
        public string Title { get; set; } = "";
        public string Url { get; set; } = "";
        public string UrlFull { get; set; } = "";
    }
}