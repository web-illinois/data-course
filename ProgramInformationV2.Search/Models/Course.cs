using OpenSearch.Client;

namespace ProgramInformationV2.Search.Models {

    public class Course : BaseTaggableObject {

        public Course() {
            AssociatedCourses = null;
            Sections = [];
            IsActive = true;
            CreatedOn = DateTime.Now;
            LastUpdated = DateTime.Now;
        }

        public IEnumerable<CourseRequirement>? AssociatedCourses { get; set; }

        public string Cost { get; set; } = "";

        [Keyword]
        public string CourseNumber { get; set; } = "";

        public string CourseTitle { get; set; } = "";

        public string CreditHours { get; set; } = "";

        public List<DayOfWeek> DaysOfWeekList { get; set; } = default!;

        [Keyword]
        public string DaysOfWeekString => DaysOfWeekList.ConvertDaysToString();

        public string DefaultBuilding { get; set; } = "";
        public string DefaultRoom { get; set; } = "";
        public string DefaultTime { get; set; } = "";
        public string Details { get; set; } = "";

        [Keyword]
        public override string EditLink => _editLink + "course/" + Id;

        public string EnrollmentDate { get; set; } = "";
        public string ExternalDetails { get; set; } = "";
        public string ExternalUrl { get; set; } = "";
        public string Faculty { get; set; } = "";
        public List<SectionFaculty> FacultyNameList { get; set; } = default!;

        [Keyword]
        public IEnumerable<string> Formats => FormatValues.Select(t => t.ConvertToSingleString());

        public IEnumerable<FormatType> FormatValues { get; set; } = [];
        public string ImageAltText { get; set; } = "";
        public string ImageUrl { get; set; } = "";
        public string Information { get; set; } = "";
        public bool IsCurrent => Sections.Any(s => s.IsCurrent);
        public bool IsUpcoming => Sections.Any(s => s.IsUpcoming);
        public string Length { get; set; } = "";
        public int MaximumCreditHours { get; set; }
        public int MinimumCreditHours { get; set; }
        public string Prerequisite { get; set; } = "";

        [Keyword]
        public string Rubric { get; set; } = "";

        public string ScheduleInformation { get; set; } = "";

        public List<Section> Sections { get; set; }

        public string SummaryText { get; set; } = "";

        public Terms Term { get; set; }

        [Keyword]
        public IEnumerable<string> Terms => TermValues.Select(t => t.ConvertToSingleString());

        public IEnumerable<Terms> TermValues { get; set; } = [];
        public string VideoUrl { get; set; } = "";
        internal override string CreateId => Id = string.IsNullOrWhiteSpace(Rubric) && string.IsNullOrWhiteSpace(CourseNumber) ? Source + "-" + Guid.NewGuid().ToString() : Source + "-" + Rubric + "-" + CourseNumber;

        public override void CleanHtmlFields() {
            SummaryText = CleanHtml(SummaryText);
            Details = CleanHtml(Details);
            Description = CleanHtml(Description);
            ExternalDetails = CleanHtml(ExternalDetails);
            VideoUrl = ConvertVideoToEmbed(VideoUrl);
            TermValues = [Term];
            ProcessLists();
            if (Sections != null && Sections?.Count > 0) {
                Sections = [.. Sections.OrderByDescending(s => s.BeginDate).OrderByDescending(s => s.EndDate).ThenBy(s => s.SectionCode)];
                FacultyNameList = [.. Sections.SelectMany(s => s.FacultyNameList).Distinct().OrderBy(s => s.Name)];
                TermValues = Sections.Select(s => s.Term).Distinct().OrderBy(s => s).ToList();
                FormatValues = Sections.Select(s => s.FormatType).Distinct().OrderBy(s => s).ToList();
                if (Sections.All(s => s.CreditHours != null && s.CreditHours != "" && s.CreditHours.All(char.IsDigit))) {
                    MinimumCreditHours = Sections.Where(s => s.CreditHours != null && s.CreditHours != "").Min(s => int.Parse(s.CreditHours));
                    MaximumCreditHours = Sections.Where(s => s.CreditHours != null && s.CreditHours != "").Max(s => int.Parse(s.CreditHours));
                    CreditHours = MinimumCreditHours == MaximumCreditHours ? MinimumCreditHours.ToString() : $"{MinimumCreditHours} - {MaximumCreditHours}";
                }
                Sections?.ForEach(s => s.CleanHtmlFields());
            }
            Faculty = FacultyNameList == null || FacultyNameList.Count == 0 ? "" : string.Join("; ", FacultyNameList.Distinct().OrderBy(s => s.Name).Select(s => s.ToString()));
        }

        public override void SetId() {
            base.SetId();
            Title = string.IsNullOrWhiteSpace(Rubric) || string.IsNullOrWhiteSpace(CourseNumber) ? CourseTitle : $"{Rubric} {CourseNumber}: {CourseTitle}";
            Sections.ForEach(s => { s.Source = Source; s.CourseId = Id; s.SetId(); });
        }
    }
}