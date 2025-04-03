using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.FieldList;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Models;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Components.Pages.RequirementSet {

    public partial class Technical {
        public IEnumerable<GenericItem> CredentialsUsingRequirementSet = default!;
        public IEnumerable<FieldItem> FieldItems { get; set; } = default!;

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        public ProgramInformationV2.Search.Models.RequirementSet RequirementSetItem { get; set; } = default!;

        [Inject]
        protected CredentialGetter CredentialGetter { get; set; } = default!;

        [Inject]
        protected FieldManager FieldManager { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected ProgramGetter ProgramGetter { get; set; } = default!;

        [Inject]
        protected RequirementSetGetter RequirementSetGetter { get; set; } = default!;

        [Inject]
        protected RequirementSetSetter RequirementSetSetter { get; set; } = default!;

        public async Task Delete() {
            Layout.RemoveDirty();
            _ = await RequirementSetSetter.DeleteRequirementSet(RequirementSetItem.Id);
            await Layout.Log(CategoryType.RequirementSet, FieldType.Technical, RequirementSetItem, "Deletion");
            NavigationManager.NavigateTo("/requirementsets");
        }

        public async Task Save() {
            Layout.RemoveDirty();
            if (RequirementSetItem.IsReused) {
                RequirementSetItem.CredentialId = "";
            } else {
                RequirementSetItem.CredentialId = CredentialsUsingRequirementSet.FirstOrDefault()?.Id ?? "";
            }
            _ = await RequirementSetSetter.SetRequirementSet(RequirementSetItem);
            await Layout.Log(CategoryType.RequirementSet, FieldType.Technical, RequirementSetItem);
            await Layout.AddMessage("Requirement Set saved successfully.");
        }

        protected override async Task OnInitializedAsync() {
            var sourceCode = await Layout.CheckSource();
            var id = await Layout.GetCachedId();
            if (string.IsNullOrWhiteSpace(id)) {
                NavigationManager.NavigateTo("/");
            }
            RequirementSetItem = await RequirementSetGetter.GetRequirementSet(id);
            CredentialsUsingRequirementSet = await CredentialGetter.GetAllCredentialsByRequirementId(id);
            await Layout.SetSidebar(SidebarEnum.RequirementSet, RequirementSetItem.InternalTitle);
            FieldItems = await FieldManager.GetMergedFieldItems(sourceCode, new RequirementSetGroup(), FieldType.General);
            await base.OnInitializedAsync();
        }
    }
}