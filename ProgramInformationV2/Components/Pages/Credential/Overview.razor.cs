using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Controls;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.FieldList;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Components.Pages.Credential {

    public partial class Overview {
        private RichTextEditor _rteDescription = default!;

        private RichTextEditor _rteProgramNotes = default!;

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

        public async Task Save() {
            Layout.RemoveDirty();
            if (_rteDescription != null) {
                CredentialItem.Description = await _rteDescription.GetValue();
            }
            if (_rteProgramNotes != null) {
                CredentialItem.Notes = await _rteProgramNotes.GetValue();
            }
            _ = await ProgramSetter.SetCredential(CredentialItem);
            await Layout.Log(CategoryType.Credential, FieldType.Overview, CredentialItem);
            await Layout.AddMessage("Credential saved successfully.");
        }

        protected override async Task OnInitializedAsync() {
            var sourceCode = await Layout.CheckSource();
            var id = await Layout.GetCachedId();
            if (string.IsNullOrWhiteSpace(id)) {
                NavigationManager.NavigateTo("/");
            }
            CredentialItem = await CredentialGetter.GetCredential(id);
            _rteDescription.InitialValue = CredentialItem.Description;
            _rteProgramNotes.InitialValue = CredentialItem.Notes;
            UsePrograms = await SourceHelper.DoesSourceUseItem(sourceCode, CategoryType.Program);
            FieldItems = await FieldManager.GetMergedFieldItems(sourceCode, new CredentialGroup(), FieldType.Overview);
            Layout.SetSidebar(SidebarEnum.Credential, CredentialItem.TitlePlusCredential);
            await base.OnInitializedAsync();
        }
    }
}