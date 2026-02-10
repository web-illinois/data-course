using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Helpers;
using ProgramInformationV2.Search.Helpers;

namespace ProgramInformationV2.Components.Pages.Configuration {

    public partial class RequestDeletion {
        private string _sourceCode = "";

        [Inject]
        public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = default!;

        [Inject]
        public BulkEditor BulkEditor { get; set; } = default!;

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        [Inject]
        public SourceHelper SourceHelper { get; set; } = default!;

        protected async Task DeleteItems() {
            await Layout.AddMessage(await BulkEditor.DeleteAllItems(_sourceCode));
        }

        protected async Task DeleteSource() {
            var message = await BulkEditor.DeleteAllItems(_sourceCode) + " " + await SourceHelper.DeleteSource(_sourceCode, await UserHelper.GetUser(AuthenticationStateProvider));
            await Layout.AddMessage(message);
        }

        protected override async Task OnInitializedAsync() {
            _sourceCode = await Layout.CheckSource();
            Layout.SetSidebar(SidebarEnum.Configuration, "Configuration");
            base.OnInitialized();
        }
    }
}