using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Data.CanvasImport {
    public class CanvasImportManager(CanvasGenerator canvasGenerator) {
        private readonly CanvasGenerator _canvasGenerator = canvasGenerator;
        public async Task<Dictionary<string, string>> GetCourses(string s) {
            return (await _canvasGenerator.GetCourses(s)).OrderBy(d => d.Value).ToDictionary(d => d.Key, d => d.Value);
        }

        public async Task<Course> GetCourse(string id, string source) {
            var course = await _canvasGenerator.GetCourse(id);
            if (course == null || string.IsNullOrEmpty(course.Id)) {
                return new Course();
            }
            course.Source = source;
            course.Id = source + "-canvas_" + id;
            return course;
        }

    }
}
