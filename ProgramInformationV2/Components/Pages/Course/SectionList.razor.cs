using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.FieldList;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;

namespace ProgramInformationV2.Components.Pages.Course {

    public partial class SectionList {
        private bool? _useSections;
        public ProgramInformationV2.Search.Models.Course CourseItem { get; set; } = new ProgramInformationV2.Search.Models.Course();
        public string SectionId { get; set; } = "";

        [Inject]
        protected CourseGetter CourseGetter { get; set; } = default!;

        [Inject]
        protected FieldManager FieldManager { get; set; } = default!;

        [CascadingParameter]
        protected SidebarLayout Layout { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected SourceHelper SourceHelper { get; set; } = default!;

        protected async Task CreateSection() {
            await Layout.SetCacheParentId(CourseItem.Id);
            NavigationManager.NavigateTo($"/section/general");
        }

        protected async Task DeleteSection() {
            //TODO Delete section logic here
        }

        protected async Task EditSection() {
            await Layout.SetCacheId(SectionId);
            NavigationManager.NavigateTo($"/section/general");
        }

        protected override async Task OnInitializedAsync() {
            var sourceCode = await Layout.CheckSource();
            _useSections = await SourceHelper.DoesSourceUseItem(sourceCode, CategoryType.Section);
            var id = await Layout.GetCachedId();
            CourseItem = await CourseGetter.GetCourse(id);
            Layout.SetSidebar(SidebarEnum.Course, CourseItem.Title);
            await base.OnInitializedAsync();
        }
    }
}