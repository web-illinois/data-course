using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.FieldList;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Components.Pages.Program {

    public partial class Technical {
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

        public async Task Delete() {
            Layout.RemoveDirty();
            var result = await ProgramSetter.DeleteProgram(ProgramItem.Id);
            await Layout.Log(CategoryType.Program, FieldType.Technical, ProgramItem, "Deletion");
            await Layout.AddMessage(result);
        }

        public async Task Save() {
            Layout.RemoveDirty();
            _ = await ProgramSetter.SetProgram(ProgramItem);
            await Layout.Log(CategoryType.Program, FieldType.Technical, ProgramItem);
            await Layout.AddMessage("Program saved successfully.");
        }

        protected override async Task OnInitializedAsync() {
            var sourceCode = await Layout.CheckSource();
            var id = await Layout.GetCachedId();
            ProgramItem = await ProgramGetter.GetProgram(id);
            FieldItems = await FieldManager.GetMergedFieldItems(sourceCode, new ProgramGroup(), FieldType.Technical);
            await Layout.SetSidebar(SidebarEnum.Program, ProgramItem.Title);
            await base.OnInitializedAsync();
        }
    }
}