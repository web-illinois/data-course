using OpenSearch.Client;

namespace ProgramInformationV2.Search.Models {

    public class Program : BaseTaggableObject {

        public Program() {
            Credentials = [];
            SkillList = [];
            TagList = [];
            DepartmentList = [];
            IsActive = true;
            LastUpdated = DateTime.Now;
            CreatedOn = DateTime.Now;
        }

        public string AlternateUrl { get; set; } = "";

        public string AlternateUrlTitle { get; set; } = "";

        public IEnumerable<string> CredentialFragmentList => Credentials.Where(c => c.IsActive).Select(c => c.Fragment);

        [Keyword]
        public IEnumerable<string> CredentialIdList => Credentials.Select(c => c.Id);

        public List<Credential> Credentials { get; set; } = default!;

        [Keyword]
        public IEnumerable<string> CredentialsFullList => Credentials.Where(c => c.IsActive).Distinct().Select(c => c.CredentialType.ConvertToSingleString());

        [Keyword]
        public IEnumerable<string> CredentialsHybridList => Credentials.Where(c => (c.FormatType & FormatType.Hybrid) == FormatType.Hybrid && c.IsActive).Distinct().Select(c => c.CredentialType.ConvertToSingleString());

        [Keyword]
        public IEnumerable<string> CredentialsOffCampusList => Credentials.Where(c => (c.FormatType & FormatType.Off__Campus) == FormatType.Off__Campus && c.IsActive).Distinct().Select(c => c.CredentialType.ConvertToSingleString());

        [Keyword]
        public IEnumerable<string> CredentialsOnCampusList => Credentials.Where(c => (c.FormatType & FormatType.On__Campus) == FormatType.On__Campus && c.IsActive).Distinct().Select(c => c.CredentialType.ConvertToSingleString());

        [Keyword]
        public IEnumerable<string> CredentialsOnlineList => Credentials.Where(c => (c.FormatType & FormatType.Online) == FormatType.Online && c.IsActive).Distinct().Select(c => c.CredentialType.ConvertToSingleString());

        public string DetailImageAltText { get; set; } = "";

        public string DetailImageUrl { get; set; } = "";

        [Keyword]
        public override string EditLink => _editLink + "program/" + Id;

        [Keyword]
        public IEnumerable<string> Formats => Credentials.Where(c => c.CredentialType != CredentialType.None && c.IsActive).Select(c => c.FormatType.ConvertToSingleString()).Distinct();

        public string ProgramGroupDescription { get; set; } = "";

        public string ProgramGroupTitle { get; set; } = "";

        public string ProgramGroupUrl { get; set; } = "";

        public IEnumerable<string> RequirementSetIds => Credentials.SelectMany(c => c.RequirementSetIds).Distinct();

        public string SummaryImageAltText { get; set; } = "";

        public string SummaryImageUrl { get; set; } = "";

        public string SummaryText { get; set; } = "";

        public string VideoUrl { get; set; } = "";

        public string WhoShouldApply { get; set; } = "";

        public override void CleanHtmlFields() {
            SummaryText = CleanHtml(SummaryText);
            WhoShouldApply = CleanHtml(WhoShouldApply);
            Description = CleanHtml(Description);
            VideoUrl = ConvertVideoToEmbed(VideoUrl);
            ProcessLists();
            if (Credentials != null && Credentials?.Count > 0) {
                Credentials = Credentials.OrderBy(c => c.CredentialType).ThenBy(c => c.Title).ToList();
                DepartmentList = DepartmentList.Union(Credentials.Where(c => c.DepartmentList != null).SelectMany(c => c.DepartmentList)).Distinct();
                SkillList = SkillList.Union(Credentials.Where(c => c.SkillList != null).SelectMany(c => c.SkillList)).Distinct();
                TagList = TagList.Union(Credentials.Where(c => c.TagList != null).SelectMany(c => c.TagList)).Distinct();
                Credentials?.ForEach(c => {
                    c.ProgramId = Id;
                    c.ProgramTitle = Title;
                    c.CleanHtmlFields();
                });
            }
        }

        public override void SetFragment() {
            base.SetFragment();
            Credentials.ForEach(c => c.SetFragment());
        }

        public override void SetId() {
            base.SetId();
            Credentials.ForEach(c => { c.Source = Source; c.ProgramId = Id; c.ProgramTitle = Title; c.SetId(); });
        }
    }
}