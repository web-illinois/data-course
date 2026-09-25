using ProgramInformationV2.Data.Agent;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Data.CourseraImport {
    public class CourseraImportManager(CourseraGenerator courseraGenerator, FullCourse fullCourse, FilterHelper filterHelper, SourceHelper sourceHelper) {
        private readonly CourseraGenerator _courseraGenerator = courseraGenerator;
        private readonly FullCourse _fullCourse = fullCourse;
        private readonly FilterHelper _filterHelper = filterHelper;
        private readonly SourceHelper _sourceHelper = sourceHelper;

        public async Task<Dictionary<string, string>> GetCourses(string s) {
            return (await _courseraGenerator.GetCourses()).Where(c => c.Value.ToLowerInvariant().Contains(s.ToLowerInvariant()) || s == "").OrderBy(d => d.Value).ToDictionary(d => d.Key, d => d.Value);
        }

        public async Task<Course> GetCourse(string source, string id) {
            var useAi = await _sourceHelper.UseAiFromSource(source);
            var courseraCourse = await _courseraGenerator.GetCourse(id);
            var course = new Course {
                Source = source,
                Title = courseraCourse.Title,
                Url = "https://www.coursera.org" + courseraCourse.Url,
                Id = source + "-" + courseraCourse.Id,
                PlatformType = PlatformTypes.Coursera,
                ImageUrl = courseraCourse.ImageUrl,
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
            if (useAi && _fullCourse.IsValid()) {
                var skills = await _filterHelper.GetFilters(source, DataModels.TagType.Skill);
                var skillList = skills.TagSources != null ? skills.TagSources.Select(a => a.Title) : [];
                var aiObject = await _fullCourse.GenerateInformation(course.Title, course.Url, skillList);
                if (string.IsNullOrWhiteSpace(course.SummaryText)) {
                    course.SummaryText = aiObject.Summary;
                }
                if (string.IsNullOrWhiteSpace(course.Details)) {
                    course.Details = aiObject.Details;
                }
                if (string.IsNullOrWhiteSpace(course.Description)) {
                    course.Description = aiObject.Description;
                }
                if (course.SkillList == null || !course.SkillList.Any()) {
                    course.SkillList = aiObject.SkillList;
                }
            }
            course.CleanHtmlFields();
            course.SetId();
            return course;
        }

    }
}
