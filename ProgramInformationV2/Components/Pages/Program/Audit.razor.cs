using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.FieldList;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;

namespace ProgramInformationV2.Components.Pages.Program {

    public partial class Audit {

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        [Inject]
        public LogHelper LogHelper { get; set; } = default!;

        public IEnumerable<Log> LogItems { get; set; } = [];
        public Search.Models.Program ProgramItem { get; set; } = default!;

        [Inject]
        protected FieldManager FieldManager { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected ProgramGetter ProgramGetter { get; set; } = default!;

        [Inject]
        protected SourceHelper SourceHelper { get; set; } = default!;

        protected override async Task OnInitializedAsync() {
            var sourceCode = await Layout.CheckSource();
            var id = await Layout.GetCachedId();
            if (string.IsNullOrWhiteSpace(id)) {
                NavigationManager.NavigateTo("/");
            }
            ProgramItem = await ProgramGetter.GetProgram(id);
            var sidebar = await SourceHelper.DoesSourceUseItem(sourceCode, CategoryType.Credential) ? SidebarEnum.ProgramWithCredential : SidebarEnum.Program;

            LogItems = await LogHelper.GetLog(await Layout.CheckSource(), id);

            Layout.SetSidebar(sidebar, ProgramItem.Title);
            await base.OnInitializedAsync();
        }
    }
}