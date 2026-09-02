using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Controls;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Components.Pages.RequirementSet {

    public partial class Search {
        private SearchGenericItem _searchGenericItem = default!;
        private string _sourceCode = "";
        private bool? _useCredentials;
        private bool? _useRequirementSets;

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        public List<GenericItem> RequirementSetList { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected RequirementSetGetter RequirementSetGetter { get; set; } = default!;

        [Inject]
        protected SourceHelper SourceHelper { get; set; } = default!;

        protected async Task ChooseRequirementSet() {
            if (!string.IsNullOrWhiteSpace(_searchGenericItem.SelectedItemId)) {
                await Layout.SetCacheId(_searchGenericItem.SelectedItemId);
                NavigationManager.NavigateTo("/requirementset/general");
            }
        }

        protected async Task GetRequirementSet() {
            RequirementSetList = await RequirementSetGetter.GetAllRequirementSetsBySource(_sourceCode, _searchGenericItem == null ? "" : _searchGenericItem.SearchItem);
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync() {
            Layout.SetSidebar(SidebarEnum.RequirementSets, "Requirement Sets");
            _sourceCode = await Layout.CheckSource();
            _useRequirementSets = await SourceHelper.DoesSourceUseItem(_sourceCode, CategoryType.RequirementSet);
            _useCredentials = await SourceHelper.DoesSourceUseItem(_sourceCode, CategoryType.Credential);
            if (await SourceHelper.GetStartWithSearchFromSource(_sourceCode)) {
                await GetRequirementSet();
            } else {
                RequirementSetList = [];
            }
            await base.OnInitializedAsync();
        }

        protected async Task SetNewRequirementSet() {
            await Layout.ClearCacheId();
            NavigationManager.NavigateTo("/requirementset/general");
        }
    }
}