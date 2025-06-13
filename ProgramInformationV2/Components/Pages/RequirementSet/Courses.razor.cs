using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ProgramInformationV2.Components.Controls;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Models;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Components.Pages.RequirementSet {

    public partial class Courses {
        private SearchGenericItem _searchGenericItem = default!;

        private string _sourceCode = "";
        private bool? _useCourses;
        public List<GenericItem> CourseList { get; set; } = default!;

        public List<CourseRequirement> CourseRequirements { get; set; } = default!;

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        [SupplyParameterFromQuery]
        [Parameter]
        public bool? Quicklink { get; set; }

        public string QuickLinkUrl { get; set; } = "";
        public ProgramInformationV2.Search.Models.RequirementSet RequirementSetItem { get; set; } = default!;

        [Inject]
        protected CourseGetter CourseGetter { get; set; } = default!;

        [Inject]
        protected IJSRuntime JsRuntime { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected RequirementSetGetter RequirementSetGetter { get; set; } = default!;

        [Inject]
        protected RequirementSetSetter RequirementSetSetter { get; set; } = default!;

        [Inject]
        protected SourceHelper SourceHelper { get; set; } = default!;

        public async Task EditCourse(string courseId) {
            if (Layout.IsDirty) {
                if (!await JsRuntime.InvokeAsync<bool>("confirm", $"You have unsaved changes. Are you sure?")) {
                    return;
                }
            }
            Layout.RemoveDirty();
            await SetQuickCacheLink();
            await Layout.SetCacheId(courseId);
            NavigationManager.NavigateTo("/course/general", true);
        }

        public void RemoveCourse(int i) {
            CourseRequirements.RemoveAt(i);
            Layout.SetDirty();
            StateHasChanged();
        }

        public async Task Save() {
            Layout.RemoveDirty();
            RequirementSetItem.CourseRequirements = [.. CourseRequirements.OrderBy(cr => cr.Title)];
            _ = await RequirementSetSetter.SetRequirementSet(RequirementSetItem);
            await Layout.Log(CategoryType.RequirementSet, FieldType.CourseList, RequirementSetItem);
            await Layout.AddMessage("Requirement Set saved successfully.");
        }

        protected async Task ChooseCourse() {
            if (!string.IsNullOrWhiteSpace(_searchGenericItem.SelectedItemId)) {
                var course = await CourseGetter.GetCourse(_searchGenericItem.SelectedItemId);
                CourseRequirements.Add(new CourseRequirement {
                    CourseId = course.Id,
                    Title = course.Title,
                    ParentId = RequirementSetItem.Id,
                    Url = course.Url,
                    IsActive = true,
                    CreatedOn = DateTime.Now,
                    LastUpdated = DateTime.Now,
                    Source = _sourceCode,
                });
                StateHasChanged();
                Layout.SetDirty();
            }
        }

        protected async Task CreateNewCourse() {
            await SetQuickCacheLink();
            NavigationManager.NavigateTo("/course/general", true);
        }

        protected async Task GetCourses() {
            CourseList = await CourseGetter.GetAllCoursesBySource(_sourceCode, _searchGenericItem == null ? "" : _searchGenericItem.SearchItem);
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync() {
            if (Quicklink.HasValue && Quicklink.Value) {
                await Layout.ReplaceCacheIdWithQuickLink();
            }
            var id = await Layout.GetCachedId();
            if (string.IsNullOrWhiteSpace(id)) {
                NavigationManager.NavigateTo("/");
            }
            _sourceCode = await Layout.CheckSource();
            RequirementSetItem = await RequirementSetGetter.GetRequirementSet(id);
            CourseRequirements = [.. RequirementSetItem.CourseRequirements];
            _useCourses = await SourceHelper.DoesSourceUseItem(_sourceCode, CategoryType.Course);
            await GetCourses();
            Layout.SetSidebar(SidebarEnum.RequirementSet, RequirementSetItem.InternalTitle);
            QuickLinkUrl = await Layout.GetCachedQuickLink();
            await base.OnInitializedAsync();
        }

        private async Task SetQuickCacheLink() => await Layout.SetCacheQuickLink("/requirementset/courses", RequirementSetItem.Id);
    }
}