using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Components.Pages.ProgramCredentials {
    public partial class TransferCourse {
        private string _sourceCode = "";
        private bool? _useCredentials;

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        [Inject]
        protected CourseGetter CourseGetter { get; set; } = default!;

        [Inject]
        protected ProgramSetter ProgramSetter { get; set; } = default!;

        [Inject]
        protected SecurityHelper SecurityHelper { get; set; } = default!;

        [Inject]
        protected SourceHelper SourceHelper { get; set; } = default!;

        public string SearchTerm { get; set; } = "";
        public string ListOfCourseId { get; set; } = "";

        public string ListOfCourseIdSelected { get; set; } = "";

        public Dictionary<string, string> ListOfCourses { get; set; } = default!;

        public Dictionary<string, string> ListOfCoursesSelected { get; set; } = default!;


        protected override async Task OnInitializedAsync() {
            Layout.SetSidebar(SidebarEnum.ProgramCredential, "Programs and Credentials");
            _sourceCode = await Layout.CheckSource();
            _useCredentials = await SourceHelper.DoesSourceUseItem(_sourceCode, CategoryType.Credential);
            ListOfCoursesSelected = new Dictionary<string, string>();
            if (await SourceHelper.GetStartWithSearchFromSource(_sourceCode)) {
                await Search();
            } else {
                ListOfCourses = new Dictionary<string, string>();
            }
            await base.OnInitializedAsync();
        }

        protected async Task<bool> Transfer() {
            if (ListOfCourses.ContainsKey(ListOfCourseId)) {
                ListOfCoursesSelected.TryAdd(ListOfCourseId, ListOfCourses[ListOfCourseId]);
                ListOfCourses.Remove(ListOfCourseId);
                ListOfCourseId = ListOfCourses.FirstOrDefault().Key ?? "";
                ListOfCoursesSelected = ListOfCoursesSelected.OrderBy(c => c.Value).ToDictionary();
            }
            StateHasChanged();
            return true;
        }

        protected async Task<bool> Remove() {
            if (ListOfCoursesSelected.ContainsKey(ListOfCourseIdSelected)) {
                ListOfCourses.TryAdd(ListOfCourseIdSelected, ListOfCoursesSelected[ListOfCourseIdSelected]);
                ListOfCoursesSelected.Remove(ListOfCourseIdSelected);
                ListOfCourseIdSelected = ListOfCoursesSelected.FirstOrDefault().Key ?? "";
                ListOfCourses = ListOfCourses.OrderBy(c => c.Value).ToDictionary();
            }
            StateHasChanged();
            return true;
        }

        protected async Task<bool> TransferAll() {
            foreach (var item in ListOfCourses) {
                ListOfCoursesSelected.TryAdd(item.Key, item.Value);
            }
            ListOfCoursesSelected = ListOfCoursesSelected.OrderBy(c => c.Value).ToDictionary();
            ListOfCourses.Clear();
            StateHasChanged();
            return true;
        }

        protected async Task<bool> RemoveAll() {
            foreach (var item in ListOfCoursesSelected) {
                ListOfCourses.TryAdd(item.Key, item.Value);
            }
            ListOfCourses = ListOfCourses.OrderBy(c => c.Value).ToDictionary();
            ListOfCoursesSelected.Clear();
            StateHasChanged();
            return true;
        }

        protected async Task Search() {
            var courses = await CourseGetter.GetAllCoursesBySource(_sourceCode, SearchTerm);
            ListOfCourses = courses.ToDictionary(c => c.Id, c => c.Title);
            StateHasChanged();
        }

        protected async Task<bool> SendImport() {
            await Layout.AddMessage("Starting to transfer courses - please wait");
            var success = 0;
            var failedTitles = new List<string>();

            foreach (var item in ListOfCoursesSelected) {
                var course = await CourseGetter.GetCourse(item.Key);
                if (await ProgramSetter.SetCredential(course.ConvertToCredential()) != "") {
                    success++;
                } else {
                    failedTitles.Add(course.Title);
                }
            }
            await Layout.AddMessage($"Total courses added: {success}. Failed items: {(failedTitles.Count == 0 ? "none" : string.Join(", ", failedTitles))}");
            return true;
        }

    }
}
