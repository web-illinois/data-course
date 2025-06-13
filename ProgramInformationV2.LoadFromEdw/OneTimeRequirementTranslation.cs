using Newtonsoft.Json;
using ProgramInformationV2.Search;
using ProgramInformationV2.Search.Getters;
using Models = ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.LoadFromEdw {

    internal static class OneTimeRequirementTranslation {

        internal static async Task TranslateRequirementSets(string path, string requirementSetFile, string courseProgramFile, string searchUrl, string searchKey, string searchSecret) {
            var badRequirementSets = new List<string>();
            var badCourses = new List<string>();

            //pull in requirement set
            using var reader = new StreamReader(path + requirementSetFile);
            var items = JsonConvert.DeserializeObject<List<dynamic>>(reader.ReadToEnd()) ?? [];
            var requirementSets = new List<Models.RequirementSet>();
            foreach (var item in items) {
                Console.WriteLine($"Requirement Set: {item.title}");
                requirementSets.Add(new Models.RequirementSet {
                    Id = item.id,
                    Title = item.displayedtitle,
                    InternalTitleOverride = item.title,
                    Source = item.source,
                    IsActive = true,
                    Fragment = item.fragment ?? item.url ?? "",
                    IsReused = true,
                    Description = item.description,
                    MaximumCreditHours = item.maximumcredithours ?? 0,
                    MinimumCreditHours = item.minimumcredithours ?? 0,
                    Order = item.order ?? 0
                });
            }

            var openSearchClient = OpenSearchFactory.CreateClient(searchUrl, searchKey, searchSecret, false);
            var courseGetter = new CourseGetter(openSearchClient);

            //pull in course program set and add to requirements -- changed to call raw file from Amazon OpenSearch
            using var readerCourseProgram = new StreamReader(path + courseProgramFile);
            var itemCourseProgramRoot = JsonConvert.DeserializeObject<dynamic>(readerCourseProgram.ReadToEnd());
            if (itemCourseProgramRoot == null) {
                Console.WriteLine("No course program data found");
                return;
            }
            var itemsCoursePrograms = (Newtonsoft.Json.Linq.JArray) itemCourseProgramRoot.hits.hits;
            foreach (var itemMain in itemsCoursePrograms) {
                var item = ((dynamic) itemMain)._source;
                var requirement = requirementSets.FirstOrDefault(r => r.Id == item.parentid.ToString());
                if (requirement != null) {
                    string courseId = (item.source.ToString().Trim() + "-" + item.course.rubric.ToString().Trim() + "-" + item.course.coursenumber.ToString().Trim()).ToString();
                    var course = await courseGetter.GetCourse(courseId, true);
                    if (!string.IsNullOrEmpty(course.Id)) {
                        requirement.CourseRequirements.Add(new Models.CourseRequirement {
                            IsActive = true,
                            Source = item.source,
                            Description = item.course.notes,
                            ParentId = requirement.Id,
                            CourseId = courseId,
                            Title = course.Title,
                            Url = $"https://education.illinois.edu/course/{item.course.rubric.ToString().Trim()}/{item.course.coursenumber.ToString().Trim()}",
                        });
                        Console.WriteLine($"Course {courseId} added to Requirement Set {requirement.InternalTitle}");
                    } else {
                        badCourses.Add($"Course {courseId} not found for requirement set {requirement.InternalTitle}");
                    }
                } else {
                    badRequirementSets.Add($"Requirement Set for course program ID {item.id} not found");
                }
            }

            // save file
            requirementSets.ForEach(p => p.Prepare());
            using var writer = new StreamWriter(path + "new-" + requirementSetFile);
            var results = JsonConvert.SerializeObject(requirementSets, Formatting.Indented);
            writer.Write(results);

            foreach (var badCourse in badCourses.OrderBy(s => s)) {
                Console.WriteLine(badCourse);
            }

            foreach (var badRequirementSet in badRequirementSets.OrderBy(s => s)) {
                Console.WriteLine(badRequirementSet);
            }
        }
    }
}