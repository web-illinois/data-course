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

    public partial class RelatedLinks {
        private LinkList _linkList = default!;

        public Search.Models.Credential CredentialItem { get; set; } = new Search.Models.Credential();

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        [Inject]
        protected CredentialGetter CredentialGetter { get; set; } = default!;

        [Inject]
        protected FieldManager FieldManager { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected ProgramSetter ProgramSetter { get; set; } = default!;

        [Inject]
        protected SourceHelper SourceHelper { get; set; } = default!;

        public async Task Save() {
            Layout.RemoveDirty();
            CredentialItem.LinkList = _linkList.GetSavedLinks();
            _ = await ProgramSetter.SetCredential(CredentialItem);
            await Layout.Log(CategoryType.Credential, FieldType.RelatedLinks, CredentialItem);
            await Layout.AddMessage("Credential saved successfully.");
        }

        protected override async Task OnInitializedAsync() {
            var sourceCode = await Layout.CheckSource();
            var id = await Layout.GetCachedId();
            if (string.IsNullOrWhiteSpace(id)) {
                NavigationManager.NavigateTo("/");
            }
            CredentialItem = await CredentialGetter.GetCredential(id);
            Layout.SetSidebar(SidebarEnum.Credential, CredentialItem.TitlePlusCredential);
            await base.OnInitializedAsync();
        }
    }
}