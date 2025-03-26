using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.FieldList;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;

namespace ProgramInformationV2.Components.Pages.Program {

    public partial class CredentialList {
        private bool? _useCredentials;
        public string CredentialId { get; set; } = "";

        public Search.Models.Program ProgramItem { get; set; } = new Search.Models.Program();

        [Inject]
        protected FieldManager FieldManager { get; set; } = default!;

        [CascadingParameter]
        protected SidebarLayout Layout { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected ProgramGetter ProgramGetter { get; set; } = default!;

        [Inject]
        protected SourceHelper SourceHelper { get; set; } = default!;

        protected async Task CreateCredential() {
            await Layout.SetCacheParentId(ProgramItem.Id);
            NavigationManager.NavigateTo($"/credential/general");
        }

        protected async Task DeleteCredential() {
            //TODO Delete credential logic here
        }

        protected async Task EditCredential() {
            await Layout.SetCacheId(CredentialId);
            NavigationManager.NavigateTo($"/credential/general");
        }

        protected override async Task OnInitializedAsync() {
            var sourceCode = await Layout.CheckSource();
            _useCredentials = await SourceHelper.DoesSourceUseItem(sourceCode, CategoryType.Credential);
            var id = await Layout.GetCachedId();
            ProgramItem = await ProgramGetter.GetProgram(id);
            Layout.SetSidebar(SidebarEnum.Program, ProgramItem.Title);
            await base.OnInitializedAsync();
        }
    }
}