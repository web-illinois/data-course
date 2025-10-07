using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.AuditHelpers;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Components.Pages.Audit {

    public partial class RequirementSetsWithInvalidCourses {

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        [Inject]
        public RequirementSetAudits RequirementSetAudits { get; set; } = default!;

        public List<GenericItem> RequirementSetList { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        protected async Task Edit(string requirementId) {
            await Layout.SetCacheId(requirementId);
            NavigationManager.NavigateTo("/requirementset/courses", true);
        }

        protected override async Task OnInitializedAsync() {
            await base.OnInitializedAsync();
            Layout.SetSidebar(SidebarEnum.Audit, "Audit");
            RequirementSetList = await RequirementSetAudits.GetAllRequirementSetsWithInvalidCourses(await Layout.CheckSource());
        }
    }
}