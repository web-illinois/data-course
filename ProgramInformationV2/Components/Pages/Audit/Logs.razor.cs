using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.PageList;

namespace ProgramInformationV2.Components.Pages.Audit {

    public partial class Logs {

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        [Inject]
        public LogHelper LogHelper { get; set; } = default!;

        public IEnumerable<Log> LogItems { get; set; } = [];

        protected override async Task OnInitializedAsync() {
            await base.OnInitializedAsync();
            await Layout.SetSidebar(SidebarEnum.Audit, "Audit");
            LogItems = await LogHelper.GetLog(await Layout.CheckSource());
        }
    }
}