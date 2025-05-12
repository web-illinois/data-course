using Newtonsoft.Json;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.LoadFromEdw {

    internal static class OneTimeCourseTranslation {

        internal static void TranslateCourses(string path, string file) {
            using var reader = new StreamReader(path + file);
            var items = JsonConvert.DeserializeObject<List<dynamic>>(reader.ReadToEnd()) ?? [];
            var courses = new List<Search.Models.Course>();
            foreach (var item in items) {
                Console.WriteLine($"Item: {item.title}");
                courses.Add(new Search.Models.Course {
                    Id = item.id,
                    Title = item.title,
                    CourseTitle = item.coursetitle ?? "",
                    Cost = item.cost ?? "",
                    CourseNumber = item.coursenumber ?? "",
                    Details = item.details ?? "",
                    EnrollmentDate = item.enrollmentdate ?? "",
                    ExternalDetails = item.externaldetails ?? "",
                    Faculty = item.faculty ?? "",
                    ImageAltText = item.imagealttext ?? "",
                    ImageUrl = item.imageurl ?? "",
                    Information = item.information ?? "",
                    ExternalUrl = item.externalurl ?? "",
                    Length = item.length ?? "",
                    MaximumCreditHours = item.maximumcredithours ?? 0,
                    MinimumCreditHours = item.minimumcredithours ?? 0,
                    Rubric = item.rubric ?? "",
                    ScheduleInformation = item.scheduleinformation ?? "",
                    Source = item.source,
                    IsActive = item.isactive ?? true, // Default to true if not specified
                    Fragment = item.fragment ?? item.url ?? "",
                    Description = item.description,
                    Url = item.url ?? "",
                    UrlFull = item.url ?? "",
                    LastUpdatedBy = item.lastupdatedby ?? "",
                    TagList = item.taglist != null ? item.taglist.ToObject<List<string>>() : [],
                    SkillList = item.skilllist != null ? item.skilllist.ToObject<List<string>>() : [],
                    DepartmentList = item.departmentlist != null ? item.departmentlist.ToObject<List<string>>() : [],
                    VideoUrl = item.videourl ?? "",
                    SummaryText = item.summarytext ?? "",
                    FacultyNameList = Array.Empty<SectionFaculty>().ToList()
                });
            }
            courses.ForEach(p => p.Prepare());
            using var writer = new StreamWriter(path + "new-" + file);
            var results = JsonConvert.SerializeObject(courses, Formatting.Indented);
            writer.Write(results);
        }
    }
}