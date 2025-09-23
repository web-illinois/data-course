using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Controls;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Components.Pages.ProgramCredentials {

    public partial class Credentials {
        private SearchGenericItem _searchGenericItem = default!;
        private string _sourceCode = "";
        private bool? _useCredentials;
        private bool? _usePrograms;

        public List<GenericItem> CredentialList { get; set; } = default!;

        public List<string> DepartmentList { get; set; } = default!;

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        [Inject]
        protected CredentialGetter CredentialGetter { get; set; } = default!;

        [Inject]
        protected FilterHelper FilterHelper { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected ProgramGetter ProgramGetter { get; set; } = default!;

        [Inject]
        protected SourceHelper SourceHelper { get; set; } = default!;

        protected async Task ChooseCredential() {
            if (!string.IsNullOrWhiteSpace(_searchGenericItem.SelectedItemId)) {
                await Layout.SetCacheId(_searchGenericItem.SelectedItemId);
                NavigationManager.NavigateTo("/credential/general");
            }
        }

        protected async Task GetCredentials() {
            CredentialList = await CredentialGetter.GetAllCredentialsBySource(_sourceCode, _searchGenericItem == null ? "" : _searchGenericItem.SearchItem, _searchGenericItem == null ? "" : _searchGenericItem.Department);
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync() {
            Layout.SetSidebar(SidebarEnum.ProgramCredential, "Programs and Credentials");
            _sourceCode = await Layout.CheckSource();
            _useCredentials = await SourceHelper.DoesSourceUseItem(_sourceCode, CategoryType.Credential);
            _usePrograms = await SourceHelper.DoesSourceUseItem(_sourceCode, CategoryType.Program);
            await GetCredentials();
            var (tagSources, _) = await FilterHelper.GetFilters(_sourceCode, TagType.Department);
            DepartmentList = [.. tagSources.Select(t => t.Title).OrderBy(t => t)];
            await base.OnInitializedAsync();
        }

        protected async Task SetNewCredential() {
            await Layout.ClearCacheId();
            NavigationManager.NavigateTo("/credential/general");
        }
    }
}