using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Data.CourseImport {

    public class CourseImportManager(CourseGetter courseGetter, CourseSetter courseSetter, CourseImportHelper courseImportHelper, FacultyNameCourseHelper facultyNameCourseHelper) {
        private readonly CourseGetter _courseGetter = courseGetter;
        private readonly CourseImportHelper _courseImportHelper = courseImportHelper;
        private readonly CourseSetter _courseSetter = courseSetter;
        private readonly FacultyNameCourseHelper _facultyNameCourseHelper = facultyNameCourseHelper;

        public async Task<string> ImportCourse(string rubric, string courseNumber, string source, string url, bool includeSections, bool overwrite) {
            var itemGroups = XmlImporter.GetAllCoursesBySemester(rubric, courseNumber);
            var itemGroup = itemGroups.FirstOrDefault()?.Select(ig => ig);
            if (itemGroup == null) {
                return $"Course {rubric} {courseNumber} not found in system";
            }
            var scheduledCourse = XmlImporter.GetCourse(itemGroup ?? []);
            if (string.IsNullOrWhiteSpace(scheduledCourse.Title)) {
                return $"Course {rubric} {courseNumber} not found in system";
            }
            var course = ScheduleTranslator.Translate(scheduledCourse, source, includeSections);
            course = await _facultyNameCourseHelper.AddFaculty(course);
            course.Url = url.Replace("{rubric}", course.Rubric, StringComparison.OrdinalIgnoreCase).Replace("{coursenumber}", course.CourseNumber, StringComparison.OrdinalIgnoreCase);
            if (overwrite) {
                _ = await _courseSetter.SetCourse(course);
                _ = await _courseImportHelper.LoadComplete(rubric, courseNumber, url, overwrite, includeSections, source);
                return $"Course Imported: {rubric} {courseNumber} into {source}.";
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
                _ = await _courseImportHelper.LoadComplete(rubric, courseNumber, url, overwrite, includeSections, source);
                return $"Course Imported: {rubric} {courseNumber} into {source}. Number of sections added: {numberSectionsAdded}.";
            } else {
                return $"No new sections for {rubric} {courseNumber} -- import skipped.";
            }
        }
    }
}