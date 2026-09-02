using ProgramInformationV2.Search.Models;
using System.Text.Json;

namespace ProgramInformationV2.Data.CanvasImport {
    public class CanvasGenerator(string? apiKey) {

        private readonly string _apiKey = apiKey ?? throw new ArgumentNullException(nameof(apiKey));

        public async Task<Dictionary<string, string>> GetCourses(string search) {
            var page = 1;
            var count = 100;
            var returnValue = new Dictionary<string, string>();
            while (page <= 10) {
                var url = search.Length > 1 ? $"https://illinois.catalog.instructure.com/api/v1/courses?is_enrollable=true&title={search}&per_page={count}&page={page}" : $"https://illinois.catalog.instructure.com/api/v1/courses?is_enrollable=true&per_page={count}&page={page}";
                using var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
                using var httpClient = new HttpClient();
                var response = await httpClient.SendAsync(request);
                using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
                var element = doc.RootElement.GetProperty("courses");
                if (element.GetArrayLength() == 0) {
                    break;
                }
                foreach (var item in element.EnumerateArray()) {
                    returnValue.Add(item.GetProperty("id").ToString(), item.GetProperty("title").ToString());
                }
                page++;
            }
            return returnValue;
        }

        public async Task<Course> GetCourse(string id) {
            var url = $"https://illinois.catalog.instructure.com/api/v1/courses/{id}";
            using var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);

            using var httpClient = new HttpClient();
            var response = await httpClient.SendAsync(request);
            using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var element = doc.RootElement.GetProperty("course");
            return new Course {
                Id = element.GetProperty("id").ToString(),
                Title = element.GetProperty("title").ToString(),
                CourseTitle = element.GetProperty("title").ToString(),
                Details = element.GetProperty("description").ToString(),
                Cost = element.GetProperty("enrollment_fee").ToString(),
                SummaryText = element.GetProperty("short_description").ToString(),
                Url = element.GetProperty("listing_url").ToString(),
                ImageUrl = element.GetProperty("listing_image").ToString(),
                ImageAltText = element.GetProperty("image_alt_text").ToString(),
                CreditHours = element.GetProperty("credits").ToString(),
                PlatformType = PlatformTypes.Canvas,
                IsActive = true,
                Sections = [
                    new Section {
                        BeginDate = DateTime.MinValue,
                        EndDate = DateTime.MaxValue,
                        IsActive = true,
                        Term = Terms.Ongoing,
                        FormatType = FormatType.Online,
                        SectionCode = "Canvas Information"
                    }
                ]

            };
        }
    }
}
