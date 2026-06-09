using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.CourseraImport;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Components.Pages.Course {
    public partial class CourseraImport {
        private string _sourceCode = "";
        private bool? _useCourses = true;
        public string SearchTerm { get; set; } = "";
        public string ListOfCourseraCourseId { get; set; } = "";

        public string ListOfCourseraCourseIdSelected { get; set; } = "";
        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        [Inject]
        protected CourseSetter CourseSetter { get; set; } = default!;

        [Inject]
        protected CourseraImportManager CourseraImportManager { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected SourceHelper SourceHelper { get; set; } = default!;

        public Dictionary<string, string> ListOfCourseraCourses { get; set; } = default!;

        public Dictionary<string, string> ListOfCourseraCoursesSelected { get; set; } = default!;

        protected override async Task OnInitializedAsync() {
            Layout.SetSidebar(SidebarEnum.Courses, "Courses");
            _sourceCode = await Layout.CheckSource();
            _useCourses = await SourceHelper.DoesSourceUseItem(_sourceCode, Data.DataModels.CategoryType.Course);
            ListOfCourseraCoursesSelected = new Dictionary<string, string>();
            await Search();
            await base.OnInitializedAsync();
        }


        protected async Task<bool> Transfer() {
            if (ListOfCourseraCourses.ContainsKey(ListOfCourseraCourseId)) {
                ListOfCourseraCoursesSelected.TryAdd(ListOfCourseraCourseId, ListOfCourseraCourses[ListOfCourseraCourseId]);
                ListOfCourseraCourses.Remove(ListOfCourseraCourseId);
                ListOfCourseraCourseId = ListOfCourseraCourses.FirstOrDefault().Key ?? "";
                ListOfCourseraCoursesSelected = ListOfCourseraCoursesSelected.OrderBy(c => c.Value).ToDictionary();
            }
            StateHasChanged();
            return true;
        }

        protected async Task<bool> Remove() {
            if (ListOfCourseraCoursesSelected.ContainsKey(ListOfCourseraCourseIdSelected)) {
                ListOfCourseraCourses.TryAdd(ListOfCourseraCourseIdSelected, ListOfCourseraCoursesSelected[ListOfCourseraCourseIdSelected]);
                ListOfCourseraCoursesSelected.Remove(ListOfCourseraCourseIdSelected);
                ListOfCourseraCourseIdSelected = ListOfCourseraCoursesSelected.FirstOrDefault().Key ?? "";
                ListOfCourseraCourses = ListOfCourseraCourses.OrderBy(c => c.Value).ToDictionary();
            }
            StateHasChanged();
            return true;
        }

        protected async Task<bool> TransferAll() {
            foreach (var item in ListOfCourseraCourses) {
                ListOfCourseraCoursesSelected.TryAdd(item.Key, item.Value);
            }
            ListOfCourseraCoursesSelected = ListOfCourseraCoursesSelected.OrderBy(c => c.Value).ToDictionary();
            ListOfCourseraCourses.Clear();
            StateHasChanged();
            return true;
        }

        protected async Task<bool> RemoveAll() {
            foreach (var item in ListOfCourseraCoursesSelected) {
                ListOfCourseraCourses.TryAdd(item.Key, item.Value);
            }
            ListOfCourseraCourses = ListOfCourseraCourses.OrderBy(c => c.Value).ToDictionary();
            ListOfCourseraCoursesSelected.Clear();
            StateHasChanged();
            return true;
        }

        protected async Task Search() {
            ListOfCourseraCourses = await CourseraImportManager.GetCourses(SearchTerm);
            StateHasChanged();
        }

        protected async Task SendImport() {
            await Layout.AddMessage("Starting to add courses - please wait");
            var success = 0;
            var failedTitles = new List<string>();
            foreach (var course in ListOfCourseraCoursesSelected) {
                var newCourse = await CourseraImportManager.GetCourse(_sourceCode, course.Key);
                if (await CourseSetter.SetCourse(newCourse) != "") {
                    await Layout.AddMessage("Course added: " + newCourse.Title);
                    success++;
                } else {
                    failedTitles.Add(string.IsNullOrWhiteSpace(newCourse.Title) ? "unknown course" : newCourse.Title);
                }
            }
            await Layout.AddMessage($"Total courses added: {success}. Failed items: {(failedTitles.Count == 0 ? "none" : string.Join(", ", failedTitles))}");
        }
    }
}
