using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Controls;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Components.Pages.Course {

    public partial class Search {
        private bool _isRestricted = false;
        private string[] _restrictedIds = [];
        private SearchGenericItem _searchGenericItem = default!;

        private string _sourceCode = "";
        private bool? _useCourses;

        public List<GenericItem> CourseList { get; set; } = default!;

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        [Inject]
        protected CourseGetter CourseGetter { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected SecurityHelper SecurityHelper { get; set; } = default!;

        [Inject]
        protected SourceHelper SourceHelper { get; set; } = default!;

        protected async Task ChooseCourse() {
            if (!string.IsNullOrWhiteSpace(_searchGenericItem.SelectedItemId)) {
                await Layout.SetCacheId(_searchGenericItem.SelectedItemId);
                NavigationManager.NavigateTo("/course/general");
            }
        }

        protected async Task GetCourses() {
            CourseList = await CourseGetter.GetAllCoursesBySource(_sourceCode, _searchGenericItem == null ? "" : _searchGenericItem.SearchItem);
            if (_isRestricted) {
                CourseList = [.. CourseList.Where(pl => _restrictedIds.Contains(pl.Id))];
            }
            StateHasChanged();
        }

        protected async Task ImportCourse() {
            await Layout.ClearCacheId();
            NavigationManager.NavigateTo("/courses/import");
        }

        protected override async Task OnInitializedAsync() {
            Layout.SetSidebar(SidebarEnum.Courses, "Courses");
            _sourceCode = await Layout.CheckSource();
            (_isRestricted, _restrictedIds) = await SecurityHelper.GetRestrictions(await Layout.GetNetId(), _sourceCode);
            _useCourses = await SourceHelper.DoesSourceUseItem(_sourceCode, Data.DataModels.CategoryType.Course);
            await GetCourses();
            await base.OnInitializedAsync();
        }

        protected async Task SetNewCourse() {
            await Layout.ClearCacheId();
            NavigationManager.NavigateTo("/course/general");
        }
    }
}