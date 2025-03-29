using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Controls;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.FieldList;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Components.Pages.Course {

    public partial class Link {
        private ImageControl _imageCourseImage = default!;

        public ProgramInformationV2.Search.Models.Course CourseItem { get; set; } = default!;
        public IEnumerable<FieldItem> FieldItems { get; set; } = default!;

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        [Inject]
        protected CourseGetter CourseGetter { get; set; } = default!;

        [Inject]
        protected CourseSetter CourseSetter { get; set; } = default!;

        [Inject]
        protected FieldManager FieldManager { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        public async Task Save() {
            Layout.RemoveDirty();
            if (_imageCourseImage != null) {
                _ = await _imageCourseImage.SaveFileToPermanent();
            }
            _ = await CourseSetter.SetCourse(CourseItem);
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
            FieldItems = await FieldManager.GetMergedFieldItems(sourceCode, new CourseGroup(), FieldType.Link);
            await Layout.SetSidebar(SidebarEnum.Course, CourseItem.Title);
            await base.OnInitializedAsync();
        }
    }
}