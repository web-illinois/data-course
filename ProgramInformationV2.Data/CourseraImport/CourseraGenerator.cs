using System.Text;
using System.Text.Json;

namespace ProgramInformationV2.Data.CourseraImport {
    public class CourseraGenerator {
        public async Task<CourseraCourse> GetCourse(string id) {
            var url = "https://www.coursera.org/graphql-gateway?opname=Search";
            var payload = """
            [{
                "operationName": "Search",
                "variables": {
                    "requests": [
                        {
                            "entityType": "PRODUCTS",
                            "limit": 10000,
                            "enableAutoAppliedFilters": false,
                            "requestOrigin": {
                                "pageType": "EQP",
                                "segmentType": "CONSUMER"
                            },
                            "facetFilters":[["partners:University of Illinois Urbana-Champaign"],["productTypeDescription:Courses"]],
                            "maxValuesPerFacet":2000,
                            "cursor": "0",
                            "query": ""
                        }
                    ]
                },
                "query": "query Search($requests: [Search_Request!]!) {\n  SearchResult {\n    search(requests: $requests) {\n      ...SearchResult\n      __typename\n    }\n    __typename\n  }\n}\n\nfragment SearchResult on Search_Result {\n  elements {\n    ...SearchHit\n    __typename\n  }\n  }\n\nfragment SearchHit on Search_Hit {\n  ...SearchArticleHit\n  ...SearchProductHit\n  ...SearchSuggestionHit\n  __typename\n}\n\nfragment SearchArticleHit on Search_ArticleHit {\n  aeName\n  careerField\n  category\n  createdByName\n  firstPublishedAt\n  id\n  internalContentEpic\n  internalProductLine\n  internalTargetKw\n  introduction\n  islocalized\n  lastPublishedAt\n  localizedCountryCd\n  localizedLanguageCd\n  name\n  subcategory\n  topics\n  url\n  skill: skills\n  __typename\n}\n\nfragment SearchProductHit on Search_ProductHit {\n  avgProductRating\n  cobrandingEnabled\n  completions\n  duration\n  id\n  imageUrl\n  isCourseFree\n  isCreditEligible\n  isNewContent\n  isPartOfCourseraPlus\n  name\n  numProductRatings\n  parentCourseName\n  parentLessonName\n  partnerLogos\n  partners\n  productCard {\n    ...SearchProductCard\n    __typename\n  }\n  productDifficultyLevel\n  productDuration\n  productType\n  skills\n  url\n  videosInLesson\n  translatedName\n  translatedSkills\n  translatedParentCourseName\n  translatedParentLessonName\n  tagline\n  fullyTranslatedLanguages\n  subtitlesOnlyLanguages\n  __typename\n}\n\nfragment SearchSuggestionHit on Search_SuggestionHit {\n  id\n  name\n  score\n  __typename\n}\n\nfragment SearchProductCard on ProductCard_ProductCard {\n  id\n  canonicalType\n  marketingProductType\n  badges\n  productTypeAttributes {\n    ... on ProductCard_Specialization {\n      ...SearchProductCardSpecialization\n      __typename\n    }\n    ... on ProductCard_Course {\n      ...SearchProductCardCourse\n      __typename\n    }\n    ... on ProductCard_Clip {\n      ...SearchProductCardClip\n      __typename\n    }\n    ... on ProductCard_Degree {\n      ...SearchProductCardDegree\n      __typename\n    }\n    __typename\n  }\n  __typename\n}\n\nfragment SearchProductCardSpecialization on ProductCard_Specialization {\n  isPathwayContent\n  __typename\n}\n\nfragment SearchProductCardCourse on ProductCard_Course {\n  isPathwayContent\n  rating\n  reviewCount\n  __typename\n}\n\nfragment SearchProductCardClip on ProductCard_Clip {\n  canonical {\n    id\n    __typename\n  }\n  __typename\n}\n\nfragment SearchProductCardDegree on ProductCard_Degree {\n  canonical {\n    id\n    __typename\n  }\n  __typename\n}\n"
            }]
            """;

            using var request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Content = new StringContent(payload, Encoding.UTF8, "application/json");
            request.Headers.Add("apollographql-client-name", "seo-entity-page");
            request.Headers.Add("apollographql-client-version", "3741b28900f73cb04ed39fe2210b2b9774b1d446");
            request.Headers.Add("operation-name", "Search");

            using var httpClient = new HttpClient();
            var response = await httpClient.SendAsync(request);
            using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var json = doc.RootElement.EnumerateArray().First();
            var items = json.GetProperty("data").GetProperty("SearchResult").GetProperty("search").EnumerateArray().First();
            var element = items.GetProperty("elements");
            var item = element.EnumerateArray().FirstOrDefault(e => e.GetProperty("id").ToString() == id);
            if (item.GetProperty("id").ToString() == id) {
                return new CourseraCourse() {
                    Id = item.GetProperty("id").ToString(),
                    Title = item.GetProperty("name").ToString(),
                    Url = item.GetProperty("url").ToString(),
                    ImageUrl = item.GetProperty("imageUrl").ToString(),
                    Skills = [.. item.GetProperty("skills").EnumerateArray().Select(s => s.ToString())],
                    IsCourseFree = item.GetProperty("isCourseFree").GetBoolean(),
                    IsCreditEligible = item.GetProperty("isCreditEligible").GetBoolean(),
                };
            }
            return new CourseraCourse();
        }

        public async Task<Dictionary<string, string>> GetCourses() {
            var url = "https://www.coursera.org/graphql-gateway?opname=Search";
            var payload = """
            [{
                "operationName": "Search",
                "variables": {
                    "requests": [
                        {
                            "entityType": "PRODUCTS",
                            "limit": 10000,
                            "enableAutoAppliedFilters": false,
                            "requestOrigin": {
                                "pageType": "EQP",
                                "segmentType": "CONSUMER"
                            },
                            "facetFilters":[["partners:University of Illinois Urbana-Champaign"],["productTypeDescription:Courses"]],
                            "maxValuesPerFacet":2000,
                            "cursor": "0",
                            "query": ""
                        }
                    ]
                },
                "query": "query Search($requests: [Search_Request!]!) {\n  SearchResult {\n    search(requests: $requests) {\n      ...SearchResult\n      __typename\n    }\n    __typename\n  }\n}\n\nfragment SearchResult on Search_Result {\n  elements {\n    ...SearchHit\n    __typename\n  }\n  }\n\nfragment SearchHit on Search_Hit {\n  ...SearchArticleHit\n  ...SearchProductHit\n  ...SearchSuggestionHit\n  __typename\n}\n\nfragment SearchArticleHit on Search_ArticleHit {\n  aeName\n  careerField\n  category\n  createdByName\n  firstPublishedAt\n  id\n  internalContentEpic\n  internalProductLine\n  internalTargetKw\n  introduction\n  islocalized\n  lastPublishedAt\n  localizedCountryCd\n  localizedLanguageCd\n  name\n  subcategory\n  topics\n  url\n  skill: skills\n  __typename\n}\n\nfragment SearchProductHit on Search_ProductHit {\n  avgProductRating\n  cobrandingEnabled\n  completions\n  duration\n  id\n  imageUrl\n  isCourseFree\n  isCreditEligible\n  isNewContent\n  isPartOfCourseraPlus\n  name\n  numProductRatings\n  parentCourseName\n  parentLessonName\n  partnerLogos\n  partners\n  productCard {\n    ...SearchProductCard\n    __typename\n  }\n  productDifficultyLevel\n  productDuration\n  productType\n  skills\n  url\n  videosInLesson\n  translatedName\n  translatedSkills\n  translatedParentCourseName\n  translatedParentLessonName\n  tagline\n  fullyTranslatedLanguages\n  subtitlesOnlyLanguages\n  __typename\n}\n\nfragment SearchSuggestionHit on Search_SuggestionHit {\n  id\n  name\n  score\n  __typename\n}\n\nfragment SearchProductCard on ProductCard_ProductCard {\n  id\n  canonicalType\n  marketingProductType\n  badges\n  productTypeAttributes {\n    ... on ProductCard_Specialization {\n      ...SearchProductCardSpecialization\n      __typename\n    }\n    ... on ProductCard_Course {\n      ...SearchProductCardCourse\n      __typename\n    }\n    ... on ProductCard_Clip {\n      ...SearchProductCardClip\n      __typename\n    }\n    ... on ProductCard_Degree {\n      ...SearchProductCardDegree\n      __typename\n    }\n    __typename\n  }\n  __typename\n}\n\nfragment SearchProductCardSpecialization on ProductCard_Specialization {\n  isPathwayContent\n  __typename\n}\n\nfragment SearchProductCardCourse on ProductCard_Course {\n  isPathwayContent\n  rating\n  reviewCount\n  __typename\n}\n\nfragment SearchProductCardClip on ProductCard_Clip {\n  canonical {\n    id\n    __typename\n  }\n  __typename\n}\n\nfragment SearchProductCardDegree on ProductCard_Degree {\n  canonical {\n    id\n    __typename\n  }\n  __typename\n}\n"
            }]
            """;

            using var request = new HttpRequestMessage(HttpMethod.Post, url);

            request.Content = new StringContent(payload, Encoding.UTF8, "application/json");
            request.Headers.Add("apollographql-client-name", "seo-entity-page");
            request.Headers.Add("apollographql-client-version", "3741b28900f73cb04ed39fe2210b2b9774b1d446");
            request.Headers.Add("operation-name", "Search");

            using var httpClient = new HttpClient();
            var response = await httpClient.SendAsync(request);
            using var doc = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var json = doc.RootElement.EnumerateArray().First();
            var items = json.GetProperty("data").GetProperty("SearchResult").GetProperty("search").EnumerateArray().First();
            var element = items.GetProperty("elements");
            var returnValue = new Dictionary<string, string>();
            foreach (var item in element.EnumerateArray()) {
                returnValue.Add(item.GetProperty("id").ToString(), item.GetProperty("name").ToString());
            }
            return returnValue;
        }
    }
}
