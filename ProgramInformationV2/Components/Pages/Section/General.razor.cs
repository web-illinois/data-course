using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.FieldList;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Components.Pages.Section {

    public partial class General {
        public IEnumerable<FieldItem> FieldItems { get; set; } = default!;

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        public Search.Models.Section SectionItem { get; set; } = default!;

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
            await Layout.SetCacheId(SectionItem.Id);
            await Layout.SetSidebar(SidebarEnum.Section, SectionItem.Title);
            await Layout.Log(CategoryType.Section, FieldType.General, SectionItem);
            await Layout.AddMessage("Section saved successfully.");
        }

        protected override async Task OnInitializedAsync() {
            var sourceCode = await Layout.CheckSource();
            var id = await Layout.GetCachedId();
            if (!string.IsNullOrWhiteSpace(id)) {
                SectionItem = await CourseGetter.GetSection(id);
                await Layout.SetSidebar(SidebarEnum.Section, SectionItem.Title);
            } else {
                SectionItem = new Search.Models.Section() {
                    Source = sourceCode,
                    CourseId = await Layout.GetCachedParentId()
                };
                await Layout.SetSidebar(SidebarEnum.Section, "New Section", true);
            }
            FieldItems = await FieldManager.GetMergedFieldItems(sourceCode, new SectionGroup(), FieldType.General);
            await base.OnInitializedAsync();
        }
    }
}