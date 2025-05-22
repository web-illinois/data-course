using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
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
        public bool UsePrograms { get; set; }

        [Inject]
        protected CredentialGetter CredentialGetter { get; set; } = default!;

        [Inject]
        protected IJSRuntime JsRuntime { get; set; } = default!;

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

        public async Task BackToProgram() {
            await Layout.SetCacheId(CredentialItem?.ProgramId ?? "");
            NavigationManager.NavigateTo("/program/credentiallist", true);
        }

        public async Task Save() {
            Layout.RemoveDirty();
            CredentialItem.RequirementSetIds = ChosenRequirementSetList.Select(r => r.Id);
            _ = await ProgramSetter.SetCredential(CredentialItem);
            var sourceCode = await Layout.CheckSource();
            UsePrograms = await SourceHelper.DoesSourceUseItem(sourceCode, CategoryType.Program);
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

        protected void Down(int i) {
            Layout.SetDirty();
            ChosenRequirementSetList.MoveItemDown(ChosenRequirementSetList[i]);
        }

        protected async Task Edit(string requirementId) {
            if (Layout.IsDirty) {
                if (!await JsRuntime.InvokeAsync<bool>("confirm", $"You have unsaved changes. Are you sure?")) {
                    return;
                }
            }
            Layout.RemoveDirty();
            await SetQuickCacheLink();
            await Layout.SetCacheId(requirementId);
            NavigationManager.NavigateTo("/requirementset/technical", true);
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
            CredentialItem = await CredentialGetter.GetCredential(id);
            var requirementSets = await RequirementSetGetter.GetRequirementSetsChosen(CredentialItem.RequirementSetIds);
            ChosenRequirementSetList = [];
            foreach (var reqId in CredentialItem.RequirementSetIds) {
                if (requirementSets.Any(r => r.Id == reqId)) {
                    ChosenRequirementSetList.Add(requirementSets.First(r => r.Id == reqId));
                }
            }
            Layout.SetSidebar(SidebarEnum.Credential, CredentialItem.TitlePlusCredential);
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

        private async Task SetQuickCacheLink() => await Layout.SetCacheQuickLink("/credential/courselist", CredentialItem.Id);
    }
}