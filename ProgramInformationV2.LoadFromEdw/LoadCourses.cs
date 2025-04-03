using ProgramInformationV2.Data.CourseImport;
using ProgramInformationV2.Search;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.LoadFromEdw {

    internal static class LoadCourses {

        internal static async Task Run(string rubricList, string source, string searchUrl) {
            Console.WriteLine($"Starting EDW Load: {DateTime.Now.ToLongTimeString()}");
            Console.WriteLine($"Search URL: {searchUrl}");
            Console.WriteLine($"Source: {source}");

            var openSearchClient = OpenSearchFactory.CreateClient(searchUrl, "", "", true);
            var courseGetter = new CourseGetter(openSearchClient);
            var courseLoader = new CourseSetter(openSearchClient, courseGetter);

            foreach (var rubric in rubricList.Split(',').Select(r => r.Trim().ToUpper())) {
                Console.WriteLine("----------------------------------");
                Console.WriteLine($"Rubric: {rubric}");
                var itemGroups = XmlImporter.GetAllCoursesBySemester(rubric, "");
                Console.WriteLine($"Rubric loaded - number of courses: {itemGroups.Count()}");

                foreach (var itemGroup in itemGroups) {
                    Console.WriteLine($"{itemGroup.Key} (number of semesters: {itemGroup.Count()}): {DateTime.Now.ToLongTimeString()}");
                    var scheduledCourse = XmlImporter.GetCourse(itemGroup);
                    if (string.IsNullOrWhiteSpace(scheduledCourse.Title)) {
                        Console.WriteLine($"Course {itemGroup.Key} not found in system");
                    } else {
                        var course = ScheduleTranslator.Translate(scheduledCourse, source, true);
                        var courseId = await courseLoader.SetCourse(course);
                        Console.WriteLine($"Course Imported: {courseId}.");
                    }
                }
            }
            Console.WriteLine($"Finishing load: {DateTime.Now.ToLongTimeString()}");
        }
    }
}