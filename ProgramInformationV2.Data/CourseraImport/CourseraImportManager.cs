using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Data.CourseraImport {
    public class CourseraImportManager(CourseraGenerator courseraGenerator) {
        private readonly CourseraGenerator _courseraGenerator = courseraGenerator;

        public async Task<Dictionary<string, string>> GetCourses(string s) {
            return (await _courseraGenerator.GetCourses()).Where(c => c.Value.ToLowerInvariant().Contains(s.ToLowerInvariant()) || s == "").OrderBy(d => d.Value).ToDictionary(d => d.Key, d => d.Value);
        }

        public async Task<Course> GetCourse(string source, string id) {
            var courseraCourse = await _courseraGenerator.GetCourse(id);
            var course = new Course {
                Source = source,
                Title = courseraCourse.Title,
                Url = "https://www.coursera.com" + courseraCourse.Url,
                Id = source + "-" + courseraCourse.Id,
                PlatformType = PlatformTypes.Coursera,
                ImageUrl = courseraCourse.ImageUrl,
                SkillList = courseraCourse.Skills,
                IsActive = true,
                CourseTitle = courseraCourse.Title,
                Sections = [
                    new Section {
                        BeginDate = DateTime.MinValue,
                        EndDate = DateTime.MaxValue,
                        IsActive = true,
                        Term = Terms.Ongoing,
                        FormatType = FormatType.Online,
                        SectionCode = "Coursera Information"
                    }
                ]
            };
            course.CleanHtmlFields();
            course.SetId();
            return course;
        }

    }
}
