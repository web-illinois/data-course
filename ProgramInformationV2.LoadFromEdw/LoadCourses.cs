using Microsoft.EntityFrameworkCore;
using ProgramInformationV2.Data.CourseImport;
using ProgramInformationV2.Data.DataContext;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Search;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.LoadFromEdw {

    internal static class LoadCourses {

        internal static async Task Run(string rubricList, string source, string searchUrl, string searchKey, string searchSecret, string urlTemplate, string fragment, string databaseConnectionString) {
            Console.WriteLine($"Starting EDW Load: {DateTime.Now.ToLongTimeString()}");
            Console.WriteLine($"Search URL: {searchUrl}");
            Console.WriteLine($"Source: {source}");

            var optionsBuilder = new DbContextOptionsBuilder<ProgramContext>();
            optionsBuilder.UseSqlServer(databaseConnectionString);

            var context = new ProgramContext(optionsBuilder.Options);

            var openSearchClient = OpenSearchFactory.CreateClient(searchUrl, searchKey, searchSecret, true);
            var courseGetter = new CourseGetter(openSearchClient);
            var courseLoader = new CourseSetter(openSearchClient, courseGetter);
            var facultyNameList = new Dictionary<string, FacultyName>();
            var facultyNameListNotFound = new List<string>();
            foreach (var rubric in rubricList.Split(',').Select(r => r.Trim().ToUpper())) {
                Console.WriteLine("----------------------------------");
                Console.WriteLine($"Rubric: {rubric}");
                var itemGroups = XmlImporter.GetAllCoursesBySemester(rubric, "");
                Console.WriteLine($"Rubric loaded - number of courses: {itemGroups.Count()}");
                var count = 1;

                foreach (var itemGroup in itemGroups) {
                    Console.WriteLine($"{count++}. {itemGroup.Key} (number of semesters: {itemGroup.Count()}): {DateTime.Now.ToLongTimeString()}");
                    var scheduledCourse = XmlImporter.GetCourse(itemGroup);
                    if (string.IsNullOrWhiteSpace(scheduledCourse.Title)) {
                        Console.WriteLine($"************** Course {itemGroup.Key} not found in system **************");
                    } else {
                        var course = ScheduleTranslator.Translate(scheduledCourse, source, true);
                        course.Url = urlTemplate.Replace("{rubric}", course.Rubric).Replace("{coursenumber}", course.CourseNumber);
                        course.Fragment = fragment.Replace("{rubric}", course.Rubric).Replace("{coursenumber}", course.CourseNumber);
                        foreach (var section in course.Sections) {
                            foreach (var faculty in section.FacultyNameList) {
                                if (!string.IsNullOrWhiteSpace(faculty.Name)) {
                                    if (facultyNameList.ContainsKey(faculty.Name)) {
                                        faculty.NetId = facultyNameList[faculty.Name].NetId;
                                        faculty.ShowInProfile = true;
                                        faculty.Url = facultyNameList[faculty.Name].ProfileUrl;
                                    }
                                    if (!facultyNameListNotFound.Contains(faculty.Name)) {
                                        var name = await context.FacultyNames.FirstOrDefaultAsync(fn => fn.Name == faculty.Name);
                                        if (name != null) {
                                            faculty.NetId = name.NetId;
                                            faculty.ShowInProfile = true;
                                            faculty.Url = name.ProfileUrl;
                                            facultyNameList[faculty.Name] = name;
                                        } else {
                                            facultyNameListNotFound.Add(faculty.Name);
                                        }
                                    }
                                }
                            }
                        }

                        course.Prepare();
                        var courseId = await courseLoader.SetCourse(course);
                        Console.WriteLine($"Course Imported: {courseId}.");
                    }
                }
            }
            Console.WriteLine($"Finishing load: {DateTime.Now.ToLongTimeString()}");
        }
    }
}