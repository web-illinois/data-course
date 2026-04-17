using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.FieldList;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Data.Versioning;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Components.Pages.Program {

    public partial class Technical {
        public bool IsTest { get; set; }
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

        [Inject]
        protected VersionManager VersionManager { get; set; } = default!;


        public async Task Delete() {
            Layout.RemoveDirty();
            _ = await ProgramSetter.DeleteProgram(ProgramItem.Id);
            await Layout.Log(CategoryType.Program, FieldType.Technical, ProgramItem, "Deletion");
            NavigationManager.NavigateTo("/programs");
        }

        public async Task Save() {
            Layout.RemoveDirty();
            _ = await ProgramSetter.SetProgram(ProgramItem);
            await Layout.Log(CategoryType.Program, FieldType.Technical, ProgramItem);
            await Layout.AddMessage("Program saved successfully.");
        }

        public async Task CopyFromProduction() {
            var productionProgramItem = await ProgramGetter.GetProgram(ProgramItem.Id.Replace("!", ""));
            if (productionProgramItem == null || productionProgramItem.Id != ProgramItem.Id.Replace("!", "")) {
                await Layout.AddMessage("No corresponding program found in production.");
                return;
            }
            ProgramItem = await VersionManager.CopyProgramFromProduction(productionProgramItem);
            await Layout.Log(CategoryType.Program, FieldType.Technical, ProgramItem, "Copied from Production");
            await Layout.AddMessage("Program copied from production successfully. You can start editing immediately.");
        }

        public async Task TransferToProduction() {
            var newProgram = await VersionManager.TransferProgramToProduction(ProgramItem);
            await Layout.Log(CategoryType.Program, FieldType.Technical, newProgram, "Transferred to Production");
            await Layout.AddMessage("Program transferred to production successfully.");
        }

        protected override async Task OnInitializedAsync() {
            var sourceCode = await Layout.CheckSource();
            IsTest = sourceCode.EndsWith('!');
            var id = await Layout.GetCachedId();
            ProgramItem = await ProgramGetter.GetProgram(id);
            FieldItems = await FieldManager.GetMergedFieldItems(sourceCode, new ProgramGroup(), FieldType.Technical);
            var sidebar = await SourceHelper.DoesSourceUseItem(sourceCode, CategoryType.Credential) ? SidebarEnum.ProgramWithCredential : SidebarEnum.Program;
            Layout.SetSidebar(sidebar, ProgramItem.Title);
            await base.OnInitializedAsync();
        }
    }
}