using System.Text.Json;
using OpenSearch.Client;

namespace ProgramInformationV2.Search.Models {

    public class Credential : BaseTaggableObject {

        public Credential() {
            IsActive = true;
            RequirementSetIds = [];
            CreatedOn = DateTime.Now;
            LastUpdated = DateTime.Now;
        }

        public string Cost { get; set; } = "";

        public CredentialType CredentialType { get; set; }

        [Keyword]
        public string CredentialTypeString => CredentialType.ConvertToSingleString();

        public string DisclaimerText { get; set; } = "";

        [Keyword]
        public override string EditLink => _editLink + "credential/" + Id;

        public string Enrollment { get; set; } = "";

        public string ExternalUrl { get; set; } = "";

        public FormatType FormatType { get; set; }

        [Keyword]
        public IEnumerable<string> FormatTypeString => FormatType.ConvertFormatList();

        public string HourText { get; set; } = "";

        public string ImageAltText { get; set; } = "";

        public string ImageUrl { get; set; } = "";

        public override string InternalTitle => string.IsNullOrWhiteSpace(ProgramTitle) || ProgramTitle == Title ? $"{Title} ({CredentialTypeString})" : $"{Title} ({ProgramTitle} / {CredentialTypeString})";
        public bool IsTranscriptable { get; set; }

        public string Length { get; set; } = "";

        public string MajorTitle { get; set; } = "";

        public string MinorTitle { get; set; } = "";

        public string Notes { get; set; } = "";

        [Keyword]
        public string ProgramId { get; set; } = "";

        public string ProgramTitle { get; set; } = "";

        [Keyword]
        public IEnumerable<string> RequirementSetIds { get; set; }

        public string SummaryText { get; set; } = "";

        public string SummaryTitle { get; set; } = "";

        public string TitlePlusCredential => string.IsNullOrWhiteSpace(Title) ? "" : $"{Title} ({CredentialTypeString})";

        public string TranscriptableName { get; set; } = "";

        public override void CleanHtmlFields() {
            SummaryText = CleanHtml(SummaryText);
            Description = CleanHtml(Description);
            Notes = CleanHtml(Notes);
            ProcessLists();
        }

        public override GenericItem GetGenericItem() => new() { Id = Id, IsActive = IsActive, Order = Order, Title = InternalTitle, ParentId = ProgramId };

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}