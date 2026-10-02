using System.Xml.Linq;

namespace ProgramInformationV2.Data.CourseImport {

    public class ScheduleSection {

        public ScheduleSection(XElement xml, string term) {
            Crn = xml.Attribute("id")?.Value ?? "";
            Term = term;
            SectionNumber = xml.Descendants("sectionNumber").FirstOrDefault()?.Value?.Trim() ?? "";
            SectionTitle = xml.Descendants("sectionTitle").FirstOrDefault()?.Value?.Trim() ?? "";
            StatusCode = xml.Descendants("statusCode").FirstOrDefault()?.Value?.Trim() ?? "";
            CreditHours = xml.Descendants("creditHours").FirstOrDefault()?.Value?.Trim() ?? "";
            SectionText = xml.Descendants("sectionText").FirstOrDefault()?.Value?.Trim() ?? "";
            SectionNotes = xml.Descendants("sectionNotes").FirstOrDefault()?.Value?.Trim() ?? "";
            SectionStatusCode = xml.Descendants("sectionStatusCode").FirstOrDefault()?.Value?.Trim() ?? "";
            Start = xml.Descendants("startDate").FirstOrDefault()?.Value?.Trim() ?? "";
            End = xml.Descendants("endDate").FirstOrDefault()?.Value?.Trim() ?? "";
            var meeting = xml.Descendants("meetings").FirstOrDefault()?.Elements().FirstOrDefault();
            if (meeting != null) {
                Type = meeting.Descendants("type").FirstOrDefault()?.Value?.Trim() ?? "";
                StartTime = meeting.Descendants("start").FirstOrDefault()?.Value?.Trim() ?? "";
                EndTime = meeting.Descendants("end").FirstOrDefault()?.Value?.Trim() ?? "";
                DaysOfTheWeek = meeting.Descendants("daysOfTheWeek").FirstOrDefault()?.Value?.Trim() ?? "";
                RoomNumber = meeting.Descendants("roomNumber").FirstOrDefault()?.Value?.Trim() ?? "";
                Building = meeting.Descendants("buildingName").FirstOrDefault()?.Value?.Trim() ?? "";
                Instructors = meeting.Descendants("instructors").FirstOrDefault()?.Descendants().Select(x => x.Value?.Trim() ?? "").ToList() ?? [];
            }
        }

        public string Building { get; set; } = "";
        public string CreditHours { get; set; } = "";
        public string Crn { get; set; } = "";
        public string DaysOfTheWeek { get; set; } = "";
        public string End { get; set; } = "";
        public string EndTime { get; set; } = "";
        public List<string> Instructors { get; set; } = default!;
        public string RoomNumber { get; set; } = "";
        public string SectionNotes { get; set; } = "";
        public string SectionNumber { get; set; } = "";
        public string SectionStatusCode { get; set; } = "";
        public string SectionText { get; set; } = "";
        public string Start { get; set; } = "";
        public string StartTime { get; set; } = "";
        public string StatusCode { get; set; } = "";
        public string Term { get; set; } = "";
        public string SectionTitle { get; set; } = "";
        public string Type { get; set; } = "";
    }
}