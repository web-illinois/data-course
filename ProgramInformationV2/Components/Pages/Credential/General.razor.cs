using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.FieldList;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Components.Pages.Credential {

    public partial class General {
        private string _id = "";

        public Search.Models.Credential CredentialItem { get; set; } = default!;
        public IEnumerable<FieldItem> FieldItems { get; set; } = default!;

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        [Inject]
        protected FieldManager FieldManager { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected ProgramGetter ProgramGetter { get; set; } = default!;

        [Inject]
        protected ProgramSetter ProgramSetter { get; set; } = default!;

        public async Task Save() {
            Layout.RemoveDirty();
            _ = await ProgramSetter.SetCredential(CredentialItem);
            if (string.IsNullOrEmpty(_id)) {
                _id = CredentialItem.Id;
                await Layout.SetCacheId(_id);
            }
        }

        protected override async Task OnInitializedAsync() {
            var title = "New Credential";
            if (string.IsNullOrWhiteSpace(_id)) {
                var sourceCode = await Layout.CheckSource();
                _id = await Layout.GetCachedId();
                if (!string.IsNullOrWhiteSpace(_id)) {
                    CredentialItem = await ProgramGetter.GetCredential(_id);
                    title = CredentialItem.Title;
                } else {
                    CredentialItem = new Search.Models.Credential() {
                        Source = sourceCode,
                        ProgramId = await Layout.GetCachedParentId()
                    };
                }
                FieldItems = await FieldManager.GetMergedFieldItems(sourceCode, new CredentialGroup(), FieldType.General);
            }
            Layout.SetSidebar(SidebarEnum.Credential, title);
            await base.OnInitializedAsync();
        }
    }
}