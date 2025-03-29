using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Controls;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Models;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Components.Pages.Credential {

    public partial class CourseList {
        private SearchGenericItem _searchGenericItem = default!;

        private string _sourceCode = "";

        public List<GenericItem> ChosenRequirementSetList { get; set; } = default!;
        public Search.Models.Credential CredentialItem { get; set; } = new Search.Models.Credential();

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        [SupplyParameterFromQuery]
        [Parameter]
        public bool? Quicklink { get; set; }

        public List<GenericItem> RequirementSetList { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected ProgramGetter ProgramGetter { get; set; } = default!;

        [Inject]
        protected ProgramSetter ProgramSetter { get; set; } = default!;

        [Inject]
        protected RequirementSetGetter RequirementSetGetter { get; set; } = default!;

        [Inject]
        protected SourceHelper SourceHelper { get; set; } = default!;

        public async Task Save() {
            Layout.RemoveDirty();
            CredentialItem.RequirementSetIds = ChosenRequirementSetList.Select(r => r.Id);
            _ = await ProgramSetter.SetCredential(CredentialItem);
            await Layout.Log(CategoryType.Credential, FieldType.CourseList, CredentialItem);
            await Layout.AddMessage("Credential saved successfully.");
        }

        protected void ChooseRequirementSet() {
            if (!string.IsNullOrWhiteSpace(_searchGenericItem.SelectedItemId)) {
                ChosenRequirementSetList.Add(new GenericItem {
                    Id = _searchGenericItem.SelectedItemId,
                    Title = _searchGenericItem.SelectedItemTitle,
                    IsActive = true
                });
                Layout.SetDirty();
                StateHasChanged();
            }
        }

        protected async Task CreateNewPrivateRequirementSet() {
            await SetQuickCacheLink();
            await Layout.SetCacheParentId(CredentialItem.Id);
            NavigationManager.NavigateTo("/requirementset/general", true);
        }

        protected async Task CreateNewPublicRequirementSet() {
            await SetQuickCacheLink();
            NavigationManager.NavigateTo("/requirementset/general", true);
        }

        protected void Down(int i) {
            Layout.SetDirty();
            ChosenRequirementSetList.MoveItemDown(ChosenRequirementSetList[i]);
        }

        protected async Task Edit(string requirementId) {
            await SetQuickCacheLink();
            await Layout.SetCacheId(requirementId);
            NavigationManager.NavigateTo("/requirementset/general", true);
        }

        protected async Task GetRequirementSet() {
            RequirementSetList = await RequirementSetGetter.GetAllRequirementSetsBySourceIncludingPrivate(_sourceCode, _searchGenericItem == null ? "" : _searchGenericItem.SearchItem, CredentialItem.Id);
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync() {
            if (Quicklink.HasValue && Quicklink.Value) {
                await Layout.ReplaceCacheIdWithQuickLink();
            }
            _sourceCode = await Layout.CheckSource();
            var id = await Layout.GetCachedId();
            if (string.IsNullOrWhiteSpace(id)) {
                NavigationManager.NavigateTo("/");
            }
            CredentialItem = await ProgramGetter.GetCredential(id);
            ChosenRequirementSetList = await RequirementSetGetter.GetRequirementSetsChosen(CredentialItem.RequirementSetIds);
            await Layout.SetSidebar(SidebarEnum.Credential, CredentialItem.Title);
            await GetRequirementSet();
            await base.OnInitializedAsync();
        }

        protected void Remove(int i) {
            Layout.SetDirty();
            ChosenRequirementSetList.RemoveAt(i);
        }

        protected void Up(int i) {
            Layout.SetDirty();
            ChosenRequirementSetList.MoveItemUp(ChosenRequirementSetList[i]);
        }

        private async Task SetQuickCacheLink() => await Layout.SetCacheQuickLink("Back to credential " + CredentialItem.Title, "/credential/courselist", CredentialItem.Id);
    }
}