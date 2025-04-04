using OpenSearch.Client;

namespace ProgramInformationV2.Search.Models {

    public class Section : BasePublicObject {

        public Section() {
            FacultyNameList = [];
            DaysOfWeekList = [];
            IsActive = true;
            BeginDate = DateTime.Now.AddYears(-5);
            EndDate = DateTime.Now.AddYears(5);
            CreatedOn = DateTime.Now;
            LastUpdated = DateTime.Now;
        }

        public string AlternateTitle { get; set; } = "";

        public DateTime? BeginDate { get; set; }

        public string Building { get; set; } = "";

        [Keyword]
        public string CourseId { get; set; } = "";

        public string CreditHours { get; set; } = "";

        public string CRN { get; set; } = "";

        public string DateString => BeginDate.HasValue && EndDate.HasValue && BeginDate.Value.AddYears(2) > EndDate.Value && BeginDate.Value.Year > 2000 ? $"{BeginDate.Value.ToShortDateString()} - {EndDate.Value.ToShortDateString()}" : string.Empty;

        public List<DayOfWeek> DaysOfWeekList { get; set; } = default!;

        [Keyword]
        public string DaysOfWeekString => DaysOfWeekList.ConvertDaysToString();

        [Keyword]
        public override string EditLink => _editLink + "section/" + Id;

        public DateTime? EndDate { get; set; }

        public List<SectionFaculty> FacultyNameList { get; set; } = default!;

        public FormatType FormatType { get; set; }

        public IEnumerable<string> FormatTypeString => FormatType.ConvertFormatList();

        public string Information { get; set; } = "";

        public bool IsCurrent => BeginDate <= DateTime.Today && EndDate >= DateTime.Today;

        public bool IsShownInProfile { get; set; }

        public bool IsUpcoming => BeginDate > DateTime.Today;

        public string Location => string.IsNullOrWhiteSpace(Room) ? Building : $"{Room} {Building}";

        public string Room { get; set; } = "";

        public string SectionCode { get; set; } = "";

        public string SemesterYear { get; set; } = "";
        public Terms Term { get; set; }
        public string Time { get; set; } = "";
        public int TimeNumeric { get; set; }
        public string Type { get; set; } = "";

        public override void CleanHtmlFields() {
            Information = CleanHtml(Information);
            Description = CleanHtml(Description);
            FacultyNameList = [.. FacultyNameList.OrderBy(s => s.Name)];
        }

        public void ManageDayOfWeek(DayOfWeek day, bool isAdded) {
            if (isAdded && !DaysOfWeekList.Contains(day))
                DaysOfWeekList.Add(day);
            else if (!isAdded)
                DaysOfWeekList.Remove(day);
        }

        public void ManageFacultyList(string name, string netid, string url, bool isAdded) {
            if (isAdded && (!string.IsNullOrWhiteSpace(name) || !string.IsNullOrWhiteSpace(netid)))
                FacultyNameList.Add(new SectionFaculty { Name = name, NetId = netid, Url = url });
            else if (!isAdded && !string.IsNullOrWhiteSpace(name))
                _ = FacultyNameList.RemoveAll(fnl => fnl.Name == name);
            else if (!isAdded && !string.IsNullOrWhiteSpace(netid))
                _ = FacultyNameList.RemoveAll(fnl => fnl.NetId == netid);
        }

        public override void SetId() {
            base.SetId();
            Title = string.IsNullOrWhiteSpace(DateString) || string.IsNullOrWhiteSpace(SectionCode) ? AlternateTitle :
                (string.IsNullOrWhiteSpace(AlternateTitle) ? $"{DateString}: {SectionCode}" : $"{AlternateTitle} ({DateString}: {SectionCode})");
        }
    }
}