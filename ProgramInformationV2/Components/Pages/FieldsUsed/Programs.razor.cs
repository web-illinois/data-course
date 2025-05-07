using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.FieldList;
using ProgramInformationV2.Data.PageList;

namespace ProgramInformationV2.Components.Pages.FieldsUsed {

    public partial class Programs {
        public Dictionary<FieldType, string> FieldGroupInstructions = default!;
        public IEnumerable<IGrouping<FieldType, FieldItem>> FieldItems = default!;

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        public string BaseUrl { get; set; } = "";

        [Inject]
        public FieldManager FieldManager { get; set; } = default!;

        public string Instructions { get; set; } = "";

        public bool IsUsed { get; set; }

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        [Inject]
        public SourceHelper SourceHelper { get; set; } = default!;

        public async Task SaveChanges() {
            Layout.RemoveDirty();
            var sourceCode = await Layout.CheckSource();
            await FieldManager.SaveFieldItems(sourceCode, CategoryType.Program, IsUsed, FieldItems.SelectMany(a => a));
            _ = await SourceHelper.SaveBaseUrl(sourceCode, BaseUrl);
            await Layout.AddMessage("Information saved");
        }

        public void SaveUsedChange(bool isUsed) {
            IsUsed = isUsed;
            Layout.SetDirty();
        }

        protected override async Task OnInitializedAsync() {
            var sourceCode = await Layout.CheckSource();
            var targetGroup = new ProgramGroup();
            Instructions = targetGroup.Instructions;
            FieldGroupInstructions = targetGroup.FieldTypeInstructions;
            (IsUsed, FieldItems) = await FieldManager.MergeFieldItems(targetGroup, sourceCode);
            Layout.SetSidebar(SidebarEnum.FieldsUsed, "Fields Used");
            BaseUrl = await SourceHelper.GetBaseUrlFromSource(sourceCode);
            await base.OnInitializedAsync();
        }
    }
}