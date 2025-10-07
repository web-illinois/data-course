using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.AuditHelpers;
using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Components.Pages.Audit {

    public partial class CourseByRequirementSet {

        [Inject]
        public CourseAudits CourseAudits { get; set; } = default!;

        public List<GenericItemWithChildren> CourseList { get; set; } = default!;

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        protected async Task Edit(string courseId) {
            await Layout.SetCacheId(courseId);
            NavigationManager.NavigateTo("/course/technical", true);
        }

        protected async Task EditRequirement(string courseId) {
            await Layout.SetCacheId(courseId);
            NavigationManager.NavigateTo("/requirementset/courses", true);
        }

        protected override async Task OnInitializedAsync() {
            await base.OnInitializedAsync();
            Layout.SetSidebar(SidebarEnum.Audit, "Audit");
            CourseList = await CourseAudits.GetAllCoursesByRequirementSets(await Layout.CheckSource());
        }
    }
}