using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Controls;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.FieldList;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Models;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Components.Pages.Credential {

    public partial class Technical {
        private SearchGenericItem _searchGenericItem = default!;
        public List<GenericItem> ProgramList { get; set; } = default!;

        public string ProgramTitle { get; set; } = "";

        public Search.Models.Credential CredentialItem { get; set; } = new Search.Models.Credential();
        public IEnumerable<FieldItem> FieldItems { get; set; } = default!;

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        public bool UsePrograms { get; set; }

        [Inject]
        protected CredentialGetter CredentialGetter { get; set; } = default!;

        [Inject]
        protected FieldManager FieldManager { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected ProgramGetter ProgramGetter { get; set; } = default!;

        [Inject]
        protected ProgramSetter ProgramSetter { get; set; } = default!;

        [Inject]
        protected SourceHelper SourceHelper { get; set; } = default!;

        public async Task BackToProgram() {
            await Layout.SetCacheId(CredentialItem?.ProgramId ?? "");
            NavigationManager.NavigateTo("/program/credentiallist", true);
        }

        public async Task Delete() {
            Layout.RemoveDirty();
            _ = await ProgramSetter.DeleteCredential(CredentialItem.Id);
            await Layout.Log(CategoryType.Credential, FieldType.Technical, CredentialItem, "Deletion");
            NavigationManager.NavigateTo("/credentials");
        }

        public async Task Save() {
            Layout.RemoveDirty();
            _ = await ProgramSetter.SetCredential(CredentialItem);
            await Layout.Log(CategoryType.Credential, FieldType.Technical, CredentialItem);
            await Layout.AddMessage("Credential saved successfully.");
        }

        protected async Task ChooseProgram() {
            if (!string.IsNullOrWhiteSpace(_searchGenericItem.SelectedItemId)) {
                await ProgramSetter.TransferCredentialToAnotherProgram(CredentialItem, _searchGenericItem.SelectedItemId);
                await Layout.AddMessage("Credential transferred.");
                ProgramTitle = ProgramList.FirstOrDefault(x => x.Id == _searchGenericItem.SelectedItemId)?.Title ?? "Unknown Program";
            }
        }

        protected async Task GetPrograms() {
            var sourceCode = await Layout.CheckSource();
            ProgramList = await ProgramGetter.GetAllProgramsBySource(sourceCode, _searchGenericItem == null ? "" : _searchGenericItem.SearchItem, _searchGenericItem == null ? "" : _searchGenericItem.Department ?? "");
            StateHasChanged();
        }

        protected override async Task OnInitializedAsync() {
            var sourceCode = await Layout.CheckSource();
            var id = await Layout.GetCachedId();
            if (string.IsNullOrWhiteSpace(id)) {
                NavigationManager.NavigateTo("/");
            }
            CredentialItem = await CredentialGetter.GetCredential(id);
            UsePrograms = await SourceHelper.DoesSourceUseItem(sourceCode, CategoryType.Program);
            ProgramList = await ProgramGetter.GetAllProgramsBySource(sourceCode, _searchGenericItem == null ? "" : _searchGenericItem.SearchItem, _searchGenericItem == null ? "" : _searchGenericItem.Department ?? "");
            ProgramTitle = ProgramList.FirstOrDefault(x => x.Id == CredentialItem.ProgramId)?.Title ?? "Unknown Program";
            FieldItems = await FieldManager.GetMergedFieldItems(sourceCode, new CredentialGroup(), FieldType.Technical);
            Layout.SetSidebar(SidebarEnum.Credential, CredentialItem.TitlePlusCredential);
            await base.OnInitializedAsync();
        }
    }
}