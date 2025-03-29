using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.FieldList;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Components.Pages.Section {

    public partial class LocationTime {
        public IEnumerable<FieldItem> FieldItems { get; set; } = default!;

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        public Search.Models.Section SectionItem { get; set; } = new Search.Models.Section();

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
            _ = await CourseSetter.SetSection(SectionItem);
            await Layout.Log(CategoryType.Section, FieldType.Location_Time, SectionItem);
            await Layout.AddMessage("Section saved successfully.");
        }

        protected override async Task OnInitializedAsync() {
            var sourceCode = await Layout.CheckSource();
            var id = await Layout.GetCachedId();
            if (string.IsNullOrWhiteSpace(id)) {
                NavigationManager.NavigateTo("/");
            }
            SectionItem = await CourseGetter.GetSection(id);
            FieldItems = await FieldManager.GetMergedFieldItems(sourceCode, new SectionGroup(), FieldType.Location_Time);
            await Layout.SetSidebar(SidebarEnum.Section, SectionItem.Title);
            await base.OnInitializedAsync();
        }
    }
}