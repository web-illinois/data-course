using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Data.CourseImport {

    public class CourseImportManager(CourseGetter courseGetter, CourseSetter courseSetter, CourseImportHelper courseImportHelper, FacultyNameCourseHelper facultyNameCourseHelper, SourceHelper sourceHelper) {
        private readonly CourseGetter _courseGetter = courseGetter;
        private readonly CourseImportHelper _courseImportHelper = courseImportHelper;
        private readonly CourseSetter _courseSetter = courseSetter;
        private readonly FacultyNameCourseHelper _facultyNameCourseHelper = facultyNameCourseHelper;
        private readonly SourceHelper _sourceHelper = sourceHelper;

        public async Task<string> ImportCourse(string rubric, string courseNumber, string source, bool includeSections, bool overwrite, bool createLog = true) {
            var url = await _sourceHelper.GetUrlTemplateFromSource(source);
            var log = "";
            var itemGroups = XmlImporter.GetAllCoursesBySemester(rubric, courseNumber);
            if (itemGroups == null) {
                return $"Course {rubric} {courseNumber} not found in system";
            }
            var scheduledCourse = XmlImporter.GetCourse(itemGroups ?? []);
            if (string.IsNullOrWhiteSpace(scheduledCourse.Title)) {
                return $"Course {rubric} {courseNumber} not found in system";
            }
            var course = ScheduleTranslator.Translate(scheduledCourse, source, includeSections);
            course = await _facultyNameCourseHelper.AddFaculty(course);
            course.Url = url.Replace("{rubric}", course.Rubric, StringComparison.OrdinalIgnoreCase).Replace("{coursenumber}", course.CourseNumber, StringComparison.OrdinalIgnoreCase);
            if (overwrite) {
                _ = await _courseSetter.SetCourse(course);
                log = $"Course Imported: {rubric} {courseNumber} into {source} - Number of sections added: {course.Sections.Count}.";
                if (createLog) {
                    _ = await _courseImportHelper.LoadComplete(rubric, courseNumber, url, overwrite, includeSections, source, log);
                }
                return log;
            }
            var originalCourse = await _courseGetter.GetCourse($"{source}-{rubric}-{courseNumber}");
            var numberSectionsAdded = 0;
            foreach (var section in course.Sections) {
                if (!originalCourse.Sections.Any(s => s.SectionCode == section.SectionCode && s.Term == section.Term && s.SemesterYear == section.SemesterYear)) {
                    originalCourse.Sections.Add(section);
                    numberSectionsAdded++;
                }
            }
            if (numberSectionsAdded > 0) {
                _ = await _courseSetter.SetCourse(originalCourse);
                log = $"Course Imported: {rubric} {courseNumber} into {source}. Number of sections added: {numberSectionsAdded}.";
                if (createLog) {
                    _ = await _courseImportHelper.LoadComplete(rubric, courseNumber, url, overwrite, includeSections, source, log);
                }
            } else {
                log = $"No new sections for {rubric} {courseNumber} -- import skipped.";
            }
            return log;
        }

        public async Task<string> ImportRubric(string rubric, string source) {
            var courseNumbers = XmlImporter.GetCourses(rubric);
            foreach (var courseNumber in courseNumbers) {
                _ = await _courseImportHelper.LoadPending(rubric.ToUpperInvariant(), courseNumber, source);
            }
            return $"{courseNumbers.Count()} courses from {rubric} scheduled to be imported -- all sections will be added and information will be overwritten.";
        }
    }
}