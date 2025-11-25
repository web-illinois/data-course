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
                ExternalUrl = $"https://courses.illinois.edu/schedule/terms/{scheduleCourse.Rubric}/{scheduleCourse.CourseNumber}",
                IsActive = true,
                CreditHours = scheduleCourse.CreditHours,
                Description = string.Empty,
                Information = string.Empty,
                ScheduleInformation = string.Empty,
                Prerequisite = string.Empty
            };
            course = course.AddDescription(scheduleCourse.Description);

            if (includeSections) {
                course.Sections = [.. scheduleCourse.Sections.Select(s => new Section {
                    Term = _termTranslation.TryGetValue(s.Term, out var value) ? value : Terms.None,
                    SectionCode = s.SectionNumber,
                    CreditHours = string.IsNullOrWhiteSpace(s.CreditHours) ? course.CreditHours.Replace(" hours.", "") : scheduleCourse.CreditHours.Replace(" hours.", ""),
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
                    IsActive = true,
                    Type = s.Type
                })];
            }
            return course;
        }

        private static string AddColon(string a, string b) => string.IsNullOrWhiteSpace(a) ? b : $"{a}: {b}";

        private static Course AddDescription(this Course course, string description) {
            if (!string.IsNullOrWhiteSpace(description)) {
                var array = description.Split(':');
                for (var i = 0; i < array.Length; i++) {
                    course = course.ParseDescription(array, i);
                }
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

        private static Course ParseDescription(this Course course, string[] s, int i) {
            var text = s[i].RemoveLastText("Course Information").RemoveLastText("Class Schedule Information").RemoveLastText("Prerequisite").RemoveLastText("Information").Trim();
            var isProcessed = false;
            if (!string.IsNullOrWhiteSpace(text)) {
                while (!isProcessed) {
                    if (i == 0) {
                        course.Description = AddColon(course.Description, text);
                        isProcessed = true;
                    } else if (s[i - 1].EndsWith("Class Schedule Information")) {
                        course.ScheduleInformation = AddColon(course.ScheduleInformation, text);
                        isProcessed = true;
                    } else if (s[i - 1].EndsWith("Information") || s[i - 1].EndsWith("Course Information")) {
                        course.Information = AddColon(course.Information, text);
                        isProcessed = true;
                    } else if (s[i - 1].EndsWith("Prerequisite")) {
                        course.Prerequisite = AddColon(course.Prerequisite, text);
                        isProcessed = true;
                    }
                    i -= 1;
                }
            }
            return course;
        }

        private static string RemoveLastText(this string s, string removedText) => s.EndsWith(removedText) ? s.Remove(s.LastIndexOf(removedText, StringComparison.Ordinal), removedText.Length) : s;
    }
}