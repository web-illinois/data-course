namespace ProgramInformationV2.Search.Models {

    public class Course : BasePublicObject {

        public Course() {
            AssociatedCourses = null;
            Sections = [];
            Identifier = new();
            IsActive = true;
            CreatedOn = DateTime.Now;
            LastUpdated = DateTime.Now;
        }

        public IEnumerable<CourseIdentifier>? AssociatedCourses { get; set; }

        public string Cost { get; set; } = "";

        public string CourseNumber => Identifier.CourseNumber;

        public string CourseTitle { get; set; } = "";

        public string CreditHours { get; set; } = "";

        public string Details { get; set; } = "";

        public string EnrollmentDate { get; set; } = "";

        public string ExternalDetails { get; set; } = "";

        public string ExternalUrl { get; set; } = "";

        public string Faculty { get; set; } = "";

        public IEnumerable<string> FacultyNetId { get; set; } = [];

        public IEnumerable<string> Formats => FormatValues.Select(t => t.ConvertToSingleString());

        public IEnumerable<FormatType> FormatValues { get; set; } = [];

        public CourseIdentifier Identifier { get; set; }

        public string ImageAltText { get; set; } = "";

        public string ImageUrl { get; set; } = "";

        public string Information { get; set; } = "";

        public bool IsCurrent => Sections.Any(s => s.IsCurrent);

        public bool IsUpcoming => Sections.Any(s => s.IsUpcoming);

        public string Length { get; set; } = "";

        public int MaximumCreditHours { get; set; }

        public int MinimumCreditHours { get; set; }

        public string Prerequisite { get; set; } = "";

        public string Rubric => Identifier.Rubric;

        public string ScheduleInformation { get; set; } = "";

        public List<Section> Sections { get; set; }

        public string SummaryText { get; set; } = "";

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
            ProcessLists();
            Sections?.ForEach(s => s.CleanHtmlFields());
            if (Sections?.Count > 0) {
                Faculty = string.Join("; ", Sections.SelectMany(s => s.FacultyNameList).OrderBy(s => s.Name).Select(s => s.ToString()).Distinct());
                FacultyNetId = Sections.SelectMany(s => s.FacultyNameList).OrderBy(s => s.NetId).Select(s => s.NetId).Distinct();
                TermValues = Sections.Select(s => s.Term).Distinct().OrderBy(s => s).ToList();
                FormatValues = Sections.Select(s => s.FormatType).Distinct().OrderBy(s => s).ToList();
                if (Sections.All(s => s.CreditHours != null && s.CreditHours.All(char.IsDigit))) {
                    MinimumCreditHours = Sections.Where(s => s.CreditHours != null).Min(s => int.Parse(s.CreditHours));
                    MaximumCreditHours = Sections.Where(s => s.CreditHours != null).Max(s => int.Parse(s.CreditHours));
                    CreditHours = MinimumCreditHours == MaximumCreditHours ? MinimumCreditHours.ToString() : $"{MinimumCreditHours} - {MaximumCreditHours}";
                }
            }
        }

        public void FilterBySection(string sectionCode) {
            if (Sections != null) {
                Sections = Sections.Where(s => s.SectionCode == sectionCode).OrderByDescending(s => s.BeginDate).ThenBy(s => s.SectionCode).ToList();
                if (Sections.Any(s => !string.IsNullOrWhiteSpace(s.Title)))
                    Title = Sections.First(s => !string.IsNullOrWhiteSpace(s.Title)).Title;
                if (Sections.Any(s => !string.IsNullOrWhiteSpace(s.Description)))
                    Description = Sections.First(s => !string.IsNullOrWhiteSpace(s.Description)).Description;
            }
        }

        public Course PrepareForJson() {
            Sections = [.. Sections.OrderBy(s => s.BeginDate).ThenBy(s => s.EndDate).ThenBy(s => s.SectionCode)];
            return this;
        }

        public override void SetId() {
            base.SetId();
            Title = string.IsNullOrWhiteSpace(Rubric) || string.IsNullOrWhiteSpace(CourseNumber) ? CourseTitle : $"{Identifier.Rubric} {Identifier.CourseNumber}: {CourseTitle}";
            Sections.ForEach(s => { s.Source = Source; s.CourseId = Id; s.SetId(); });
        }
    }
}