using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.PageList;

namespace ProgramInformationV2.Components.Pages.Configuration {
    public partial class Settings {
        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        public string BaseUrl { get; set; } = "";

        public bool InitiateSearch { get; set; } = false;

        [Inject]
        protected SourceHelper SourceHelper { get; set; } = default!;

        protected override async Task OnInitializedAsync() {
            await base.OnInitializedAsync();
            var source = await Layout.CheckSource();
            var sourceItem = await SourceHelper.GetSource(source);
            BaseUrl = sourceItem.BaseUrl;
            InitiateSearch = sourceItem.StartWithSearch;
            Layout.SetSidebar(SidebarEnum.Configuration, "Configuration");
        }
        protected async Task Save() {
            var source = await Layout.CheckSource();
            var sourceItem = await SourceHelper.GetSource(source);
            sourceItem.BaseUrl = BaseUrl;
            sourceItem.StartWithSearch = InitiateSearch;
            await SourceHelper.SaveSource(sourceItem);
            Layout.RemoveDirty();
            await Layout.AddMessage($"Settings have been saved.");
        }
    }
}
