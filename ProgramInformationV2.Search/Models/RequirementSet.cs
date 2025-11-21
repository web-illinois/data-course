using System.Text.Json;
using OpenSearch.Client;

namespace ProgramInformationV2.Search.Models {

    public class RequirementSet : BaseObject {

        public RequirementSet() {
            CreatedOn = DateTime.Now;
            LastUpdated = DateTime.Now;
            CourseRequirements = [];
            IsReused = false;
        }

        public List<CourseRequirement> CourseRequirements { get; set; } = default!;

        [Keyword]
        public string CredentialId { get; set; } = "";

        public string Description { get; set; } = "";

        [Keyword]
        public override string EditLink => _editLink + "requirementset/" + Id;

        public override string InternalTitle => string.IsNullOrWhiteSpace(InternalTitleOverride) ? Title : $"{Title} ({InternalTitleOverride})";

        public string InternalTitleOverride { get; set; } = "";

        public bool IsReused { get; set; }

        public int MaximumCreditHours { get; set; }
        public int MinimumCreditHours { get; set; }

        public override void CleanHtmlFields() {
            Description = CleanHtml(Description);
            if (CourseRequirements != null && CourseRequirements?.Count() > 0) {
                CourseRequirements = [.. CourseRequirements.OrderBy(s => s.Title)];
            }
        }

        public override void SetId() {
            base.SetId();
            CourseRequirements.ForEach(c => { c.Source = Source; c.ParentId = Id; c.SetId(); });
        }

        public override string ToString() => JsonSerializer.Serialize(this);
    }
}