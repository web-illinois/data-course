using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.FieldList;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Setters;
using FieldType = ProgramInformationV2.Data.DataModels.FieldType;

namespace ProgramInformationV2.Components.Pages.Program {

    public partial class General {
        private SidebarEnum _sidebar = SidebarEnum.None;
        public IEnumerable<FieldItem> FieldItems { get; set; } = default!;

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        public Search.Models.Program ProgramItem { get; set; } = default!;

        [Inject]
        protected FieldManager FieldManager { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected ProgramGetter ProgramGetter { get; set; } = default!;

        [Inject]
        protected ProgramSetter ProgramSetter { get; set; } = default!;

        [Inject]
        protected SourceHelper SourceHelper { get; set; } = default!;

        public async Task Save() {
            Layout.RemoveDirty();
            _ = await ProgramSetter.SetProgram(ProgramItem);
            await Layout.SetCacheId(ProgramItem.Id);
            Layout.SetSidebar(_sidebar, ProgramItem.Title);
            await Layout.Log(CategoryType.Program, FieldType.General, ProgramItem);
            await Layout.AddMessage("Program saved successfully.");
        }

        protected override async Task OnInitializedAsync() {
            var sourceCode = await Layout.CheckSource();
            var id = await Layout.GetCachedId();
            _sidebar = await SourceHelper.DoesSourceUseItem(sourceCode, CategoryType.Credential) ? SidebarEnum.ProgramWithCredential : SidebarEnum.Program;

            if (!string.IsNullOrWhiteSpace(id)) {
                ProgramItem = await ProgramGetter.GetProgram(id);
                Layout.SetSidebar(_sidebar, ProgramItem.InternalTitle);
            } else {
                ProgramItem = new Search.Models.Program() {
                    Source = sourceCode
                };
                Layout.SetSidebar(SidebarEnum.Program, "New Program", true);
            }
            FieldItems = await FieldManager.GetMergedFieldItems(sourceCode, new ProgramGroup(), FieldType.General);

            await base.OnInitializedAsync();
        }
    }
}