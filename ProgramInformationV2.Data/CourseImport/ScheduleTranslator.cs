using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Data.CourseImport {

    public static class ScheduleTranslator {
        private static readonly Dictionary<string, Terms> _termTranslation = new() { { "spring", Terms.Spring }, { "summer", Terms.Summer }, { "fall", Terms.Fall } };

        public static Course Translate(ScheduleCourse scheduleCourse, string source, bool includeSections) {
            var course = new Course {
                Source = source,
                CourseNumber = scheduleCourse.CourseNumber,
                Rubric = scheduleCourse.Rubric,
                CourseTitle = scheduleCourse.Title,
                Description = scheduleCourse.Description,
                ExternalUrl = $"https://courses.illinois.edu/schedule/terms/{scheduleCourse.Rubric}/{scheduleCourse.CourseNumber}",
                IsActive = true
            };
            if (includeSections) {
                course.Sections = [.. scheduleCourse.Sections.Select(s => new Section {
                    Term = _termTranslation.TryGetValue(s.Term, out var value) ? value : Terms.None,
                    SectionCode = s.SectionNumber,
                    CreditHours = s.CreditHours,
                    Description = s.SectionText + " " + s.SectionNotes,
                    BeginDate = DateTime.TryParse(s.Start?.Replace("Z", ""), out _) ? DateTime.Parse(s.Start?.Replace("Z", "") ?? "") : default,
                    EndDate = DateTime.TryParse(s.End?.Replace("Z", ""), out _) ? DateTime.Parse(s.End?.Replace("Z", "") ?? "") : default,
                    Source = source,
                    CRN = s.Crn,
                    Time = string.IsNullOrEmpty(s.StartTime) || string.IsNullOrEmpty(s.EndTime) ? "" : s.StartTime + " - " + s.EndTime,
                    Building = s.Building,
                    Room = s.RoomNumber,
                    DaysOfWeekList = Convert(s.DaysOfTheWeek),
                    FormatType = s.Type == "Online" ? FormatType.Online : FormatType.On__Campus,
                    FacultyNameList = s.Instructors == null ? [] : [.. s.Instructors.Select(i => new SectionFaculty { Name = i })],
                    IsActive = true
                })];
            }
            return course;
        }

        private static List<DayOfWeek> Convert(string s) {
            var returnValue = new List<DayOfWeek>();
            if (!string.IsNullOrEmpty(s)) {
                if (s.Contains('M')) {
                    returnValue.Add(DayOfWeek.Monday);
                }
                if (s.Contains('T')) {
                    returnValue.Add(DayOfWeek.Tuesday);
                }
                if (s.Contains('W')) {
                    returnValue.Add(DayOfWeek.Wednesday);
                }
                if (s.Contains('R')) {
                    returnValue.Add(DayOfWeek.Thursday);
                }
                if (s.Contains('F')) {
                    returnValue.Add(DayOfWeek.Friday);
                }
            }
            return returnValue;
        }
    }
}