using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Controls;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.FieldList;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Helpers;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Components.Pages.Course {

    public partial class Link {
        private ImageControl _imageCourseImage = default!;
        private string _oldUrl = "";
        public ProgramInformationV2.Search.Models.Course CourseItem { get; set; } = default!;
        public IEnumerable<FieldItem> FieldItems { get; set; } = default!;

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        public string QuickLinkUrl { get; set; } = "";

        [Inject]
        protected BulkEditor BulkEditor { get; set; } = default!;

        [Inject]
        protected CourseGetter CourseGetter { get; set; } = default!;

        [Inject]
        protected CourseSetter CourseSetter { get; set; } = default!;

        [Inject]
        protected FieldManager FieldManager { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected SourceHelper SourceHelper { get; set; } = default!;

        public async Task Save() {
            Layout.RemoveDirty();
            CourseItem.SetFullUrl(await Layout.GetBaseUrl());
            if (_imageCourseImage != null) {
                _ = await _imageCourseImage.SaveFileToPermanent();
            }
            _ = await CourseSetter.SetCourse(CourseItem);
            if (_oldUrl != CourseItem.Url) {
                _ = await BulkEditor.UpdateRequirementSets(CourseItem.Id, CourseItem.Title, CourseItem.Url);
                _oldUrl = CourseItem.Url;
            }
            await Layout.Log(CategoryType.Course, FieldType.Link, CourseItem);
            await Layout.AddMessage("Course saved successfully.");
        }

        protected override async Task OnInitializedAsync() {
            var sourceCode = await Layout.CheckSource();
            var id = await Layout.GetCachedId();
            if (string.IsNullOrWhiteSpace(id)) {
                NavigationManager.NavigateTo("/");
            }
            CourseItem = await CourseGetter.GetCourse(id);
            _oldUrl = CourseItem.Url;
            FieldItems = await FieldManager.GetMergedFieldItems(sourceCode, new CourseGroup(), FieldType.Link);
            var sidebar = await SourceHelper.DoesSourceUseItem(sourceCode, CategoryType.Section) ? SidebarEnum.CourseWithSection : SidebarEnum.Course;
            QuickLinkUrl = await Layout.GetCachedQuickLink();
            Layout.SetSidebar(sidebar, CourseItem.Title);
            await base.OnInitializedAsync();
        }
    }
}