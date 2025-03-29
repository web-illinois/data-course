using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.FieldList;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Setters;
using FieldType = ProgramInformationV2.Data.DataModels.FieldType;

namespace ProgramInformationV2.Components.Pages.Course {

    public partial class General {
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
            await Layout.SetCacheId(CourseItem.Id);
            await Layout.SetSidebar(SidebarEnum.Course, CourseItem.Title);
            await Layout.Log(CategoryType.Course, FieldType.General, CourseItem);
            await Layout.AddMessage("Course saved successfully.");
        }

        protected override async Task OnInitializedAsync() {
            var sourceCode = await Layout.CheckSource();
            var id = await Layout.GetCachedId();
            if (!string.IsNullOrWhiteSpace(id)) {
                CourseItem = await CourseGetter.GetCourse(id);
                await Layout.SetSidebar(SidebarEnum.Course, CourseItem.Title);
            } else {
                CourseItem = new ProgramInformationV2.Search.Models.Course {
                    Source = sourceCode
                };
                await Layout.SetSidebar(SidebarEnum.Course, "New Course", true);
            }
            FieldItems = await FieldManager.GetMergedFieldItems(sourceCode, new CourseGroup(), FieldType.General);
            await base.OnInitializedAsync();
        }
    }
}