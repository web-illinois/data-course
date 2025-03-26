using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.FieldList;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Setters;
using FieldType = ProgramInformationV2.Data.DataModels.FieldType;

namespace ProgramInformationV2.Components.Pages.Course {

    public partial class General {
        private string _id = "";

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
            _ = await CourseSetter.SetCourse(CourseItem);
            if (string.IsNullOrEmpty(_id)) {
                _id = CourseItem.Id;
                Layout.SetSidebar(SidebarEnum.Course, CourseItem.Title);
                await Layout.SetCacheId(_id);
            }
        }

        protected override async Task OnInitializedAsync() {
            if (string.IsNullOrWhiteSpace(_id)) {
                var sourceCode = await Layout.CheckSource();
                _id = await Layout.GetCachedId();
                if (!string.IsNullOrWhiteSpace(_id)) {
                    CourseItem = await CourseGetter.GetCourse(_id);
                    Layout.SetSidebar(SidebarEnum.Course, CourseItem.Title);
                } else {
                    CourseItem = new ProgramInformationV2.Search.Models.Course {
                        Source = sourceCode
                    };
                }
                FieldItems = await FieldManager.GetMergedFieldItems(sourceCode, new CourseGroup(), FieldType.General);
            }
            await base.OnInitializedAsync();
        }
    }
}