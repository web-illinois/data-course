using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.CanvasImport;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Components.Pages.Course {
    public partial class CanvasImport {
        private string _sourceCode = "";

        private bool? _useCourses = true;

        public string SearchTerm { get; set; } = "";

        public string ListOfCanvasCourseId { get; set; } = "";

        public string ListOfCanvasCourseIdSelected { get; set; } = "";

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        [Inject]
        protected CourseSetter CourseSetter { get; set; } = default!;

        [Inject]
        protected CanvasImportManager CanvasImportManager { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected SourceHelper SourceHelper { get; set; } = default!;

        public Dictionary<string, string> ListOfCanvasCourses { get; set; } = default!;

        public Dictionary<string, string> ListOfCanvasCoursesSelected { get; set; } = default!;

        protected override async Task OnInitializedAsync() {
            Layout.SetSidebar(SidebarEnum.Courses, "Courses");
            _sourceCode = await Layout.CheckSource();
            _useCourses = await SourceHelper.DoesSourceUseItem(_sourceCode, Data.DataModels.CategoryType.Course);
            ListOfCanvasCoursesSelected = new Dictionary<string, string>();
            await Search();
            await base.OnInitializedAsync();
        }


        protected async Task<bool> Transfer() {
            if (ListOfCanvasCourses.ContainsKey(ListOfCanvasCourseId)) {
                ListOfCanvasCoursesSelected.TryAdd(ListOfCanvasCourseId, ListOfCanvasCourses[ListOfCanvasCourseId]);
                ListOfCanvasCourses.Remove(ListOfCanvasCourseId);
                ListOfCanvasCourseId = ListOfCanvasCourses.FirstOrDefault().Key ?? "";
                ListOfCanvasCoursesSelected = ListOfCanvasCoursesSelected.OrderBy(c => c.Value).ToDictionary();
            }

            StateHasChanged();
            return true;
        }

        protected async Task<bool> Remove() {
            if (ListOfCanvasCoursesSelected.ContainsKey(ListOfCanvasCourseIdSelected)) {
                ListOfCanvasCourses.TryAdd(ListOfCanvasCourseIdSelected, ListOfCanvasCoursesSelected[ListOfCanvasCourseIdSelected]);
                ListOfCanvasCoursesSelected.Remove(ListOfCanvasCourseIdSelected);
                ListOfCanvasCourseIdSelected = ListOfCanvasCoursesSelected.FirstOrDefault().Key ?? "";
                ListOfCanvasCourses = ListOfCanvasCourses.OrderBy(c => c.Value).ToDictionary();
            }

            StateHasChanged();
            return true;
        }

        protected async Task<bool> TransferAll() {
            foreach (var item in ListOfCanvasCourses) {
                ListOfCanvasCoursesSelected.TryAdd(item.Key, item.Value);
            }
            ListOfCanvasCoursesSelected = ListOfCanvasCoursesSelected.OrderBy(c => c.Value).ToDictionary();
            ListOfCanvasCourses.Clear();
            StateHasChanged();
            return true;
        }

        protected async Task<bool> RemoveAll() {
            foreach (var item in ListOfCanvasCoursesSelected) {
                ListOfCanvasCourses.TryAdd(item.Key, item.Value);
            }
            ListOfCanvasCourses = ListOfCanvasCourses.OrderBy(c => c.Value).ToDictionary();
            ListOfCanvasCoursesSelected.Clear();
            StateHasChanged();
            return true;
        }

        protected async Task Search() {
            ListOfCanvasCourses = await CanvasImportManager.GetCourses(SearchTerm);
            StateHasChanged();
        }

        protected async Task SendImport() {
            await Layout.AddMessage("Starting to add courses - please wait");
            var success = 0;
            var failedTitles = new List<string>();
            foreach (var course in ListOfCanvasCoursesSelected) {
                var newCourse = await CanvasImportManager.GetCourse(course.Key, _sourceCode);
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
