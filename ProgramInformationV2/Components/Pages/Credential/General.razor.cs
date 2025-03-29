using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.FieldList;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Components.Pages.Credential {

    public partial class General {
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
            await Layout.SetCacheId(CredentialItem.Id);
            await Layout.SetSidebar(SidebarEnum.Credential, CredentialItem.Title);
            await Layout.Log(CategoryType.Credential, FieldType.General, CredentialItem);
            await Layout.AddMessage("Credential saved successfully.");
        }

        protected override async Task OnInitializedAsync() {
            var sourceCode = await Layout.CheckSource();
            var id = await Layout.GetCachedId();
            if (!string.IsNullOrWhiteSpace(id)) {
                CredentialItem = await ProgramGetter.GetCredential(id);
                await Layout.SetSidebar(SidebarEnum.Credential, CredentialItem.Title);
            } else {
                CredentialItem = new Search.Models.Credential() {
                    Source = sourceCode,
                    ProgramId = await Layout.GetCachedParentId()
                };
                await Layout.SetSidebar(SidebarEnum.Credential, "New Credential", true);
            }
            FieldItems = await FieldManager.GetMergedFieldItems(sourceCode, new CredentialGroup(), FieldType.General);
            await base.OnInitializedAsync();
        }
    }
}