using System.Net;
using System.Xml.Linq;

namespace ProgramInformationV2.Data.CourseImport {

    public static class XmlImporter {
        private static readonly string _scheduleUrl = "https://courses.illinois.edu/cisapp/explorer/schedule/";
        private static readonly string[] _semesters = ["fall", "summer", "spring"];
        private static readonly int _yearsLookBack = 5;
        private static readonly int _yearsLookForward = 1;

        public static IEnumerable<IGrouping<string, CourseUrl>> GetAllCoursesBySemester(string rubric, string courseNumber) {
            var returnValue = new List<CourseUrl>();

            var urls = new List<Tuple<string, string, int>>();
            foreach (var year in BuildYearRange()) {
                foreach (var semester in _semesters) {
                    urls.Add(new Tuple<string, string, int>($"{_scheduleUrl}{year}/{semester}/{rubric.ToUpperInvariant()}.xml", semester, year));
                }
            }
            foreach (var url in urls) {
                try {
                    var document = XDocument.Load(url.Item1);
                    foreach (var node in document.Descendants("course")) {
                        var courseUrl = new CourseUrl { CourseNumber = node.Attributes("id").First().Value, Rubric = rubric.ToUpperInvariant(), Semester = url.Item2, Url = node.Attributes("href").First().Value, Year = url.Item3 };
                        if (courseUrl != null && (string.IsNullOrEmpty(courseNumber) || courseUrl.CourseNumber == courseNumber)) {
                            returnValue.Add(courseUrl);
                        }
                    }
                    Console.WriteLine($"Found {url.Item1}");
                } catch (WebException) {
                    Console.WriteLine($"Web Exception for {url.Item1}");
                    // do nothing -- assume a 404 error or 502 error, which means the course does not exist
                } catch (HttpRequestException) {
                    Console.WriteLine($"Http Request Exception for {url.Item1}");
                    // do nothing -- assume a 404 error or 502 error, which means the course does not exist
                }
            }
            return returnValue.GroupBy(i => i.ToString()).OrderBy(ig => ig.Key);
        }

        public static ScheduleCourse GetCourse(IEnumerable<CourseUrl> urls) {
            var course = new ScheduleCourse();

            foreach (var url in urls.OrderByDescending(ig => ig.Year).ThenByDescending(ig => ig.Semester)) {
                try {
                    _ = course.AddXml(XDocument.Load(url.Url + "?mode=detail"), url.Rubric, url.CourseNumber, url.Semester);
                } catch (WebException) {
                    // do nothing -- assume a 404 error or 502 error, which means the course does not exist
                } catch (HttpRequestException) {
                    // do nothing -- assume a 404 error or 502 error, which means the course does not exist
                }
                Thread.Sleep(500);
            }
            return course;
        }

        private static IEnumerable<int> BuildYearRange() => Enumerable.Range(DateTime.Now.Year - _yearsLookBack, _yearsLookBack + _yearsLookForward + 1).Reverse();
    }
}