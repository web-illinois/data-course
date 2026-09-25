using ProgramInformationV2.Data.Agent;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Data.CanvasImport {
    public class CanvasImportManager(CanvasGenerator canvasGenerator, FullCourse fullCourse, FilterHelper filterHelper, SourceHelper sourceHelper) {
        private readonly CanvasGenerator _canvasGenerator = canvasGenerator;
        private readonly FullCourse _fullCourse = fullCourse;
        private readonly FilterHelper _filterHelper = filterHelper;
        private readonly SourceHelper _sourceHelper = sourceHelper;

        public async Task<Dictionary<string, string>> GetCourses(string s) {
            return (await _canvasGenerator.GetCourses(s)).OrderBy(d => d.Value).ToDictionary(d => d.Key, d => d.Value);
        }

        public async Task<Course> GetCourse(string id, string source) {
            var useAi = await _sourceHelper.UseAiFromSource(source);
            var course = await _canvasGenerator.GetCourse(id);
            var skills = await _filterHelper.GetFilters(source, DataModels.TagType.Skill);
            var skillList = skills.TagSources != null ? skills.TagSources.Select(a => a.Title) : [];
            if (course == null || string.IsNullOrEmpty(course.Id)) {
                return new Course();
            }
            if (useAi && _fullCourse.IsValid()) {
                var aiObject = await _fullCourse.GenerateInformation(course.Title, course.Url, skillList);
                if (string.IsNullOrWhiteSpace(course.SummaryText)) {
                    course.SummaryText = aiObject.Summary;
                }
                if (string.IsNullOrWhiteSpace(course.Description)) {
                    course.Details = aiObject.Description;
                }
                if (string.IsNullOrWhiteSpace(course.Details)) {
                    course.Details = aiObject.Details;
                }
                if (course.SkillList == null || !course.SkillList.Any()) {
                    course.SkillList = aiObject.SkillList;
                }
            }
            course.Source = source;
            course.Id = source + "-canvas_" + id;
            return course;
        }

    }
}
