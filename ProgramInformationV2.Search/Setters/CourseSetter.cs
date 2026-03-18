using OpenSearch.Client;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Models;
using ProgramInformationV2.Search.NoteTemplates;

namespace ProgramInformationV2.Search.Setters {

    public class CourseSetter(OpenSearchClient? openSearchClient, CourseGetter? courseGetter, INoteTemplateConvert noteTemplateConvert) {
        private readonly CourseGetter _courseGetter = courseGetter ?? default!;
        private readonly INoteTemplateConvert _noteTemplateConvert = noteTemplateConvert ?? default!;
        private readonly OpenSearchClient _openSearchClient = openSearchClient ?? default!;

        public async Task<string> DeleteCourse(string id) {
            var response = await _openSearchClient.DeleteAsync<Course>(id, d => d.Index(UrlTypes.Courses.ConvertToUrlString()));
            return response.IsValid ? $"Course {id} deleted" : "error";
        }

        public async Task<string> DeleteSection(string id) {
            var course = await _courseGetter.GetCourseBySection(id);
            if (course == null) {
                return "error";
            }
            _ = course.Sections.RemoveAll(c => c.Id == id);
            var response = await _openSearchClient.IndexAsync(course, i => i.Index(UrlTypes.Courses.ConvertToUrlString()));
            return response.IsValid ? $"Section {id} deleted" : "error";
        }

        public async Task<string> SetCourse(Course course) {
            course.Prepare();
            foreach (var note in course.NoteList) {
                note.DescriptionHtml = _noteTemplateConvert.ConvertToHtml(note.Description);
            }
            var response = await _openSearchClient.IndexAsync(course, i => i.Index(UrlTypes.Courses.ConvertToUrlString()));
            return response.IsValid ? course.Id : "";
        }

        public async Task<string> SetSection(Section section) {
            section.Prepare();
            var course = await _courseGetter.GetCourse(section.CourseId);
            _ = course.Sections.RemoveAll(s => s.Id == section.Id);
            course.Sections.Add(section);
            course.Prepare();
            var response = await _openSearchClient.IndexAsync(course, i => i.Index(UrlTypes.Courses.ConvertToUrlString()));
            return response.IsValid ? section.Id : "";
        }
    }
}