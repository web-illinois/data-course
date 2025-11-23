using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.PageList;

namespace ProgramInformationV2.Components.Pages.Audit {

    public partial class CourseImportLog {

        [Inject]
        public CourseImportHelper CourseImportHelper { get; set; } = default!;

        public DateTime? LastItemImported { get; set; }

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        public IEnumerable<CourseImportEntry> LogItems { get; set; } = [];
        public int NumberItemsPending { get; set; }

        protected override async Task OnInitializedAsync() {
            await base.OnInitializedAsync();
            Layout.SetSidebar(SidebarEnum.Audit, "Audit");
            LogItems = await CourseImportHelper.GetLog(await Layout.CheckSource());
            LastItemImported = await CourseImportHelper.GetLastItemUpdated();
            NumberItemsPending = await CourseImportHelper.NumberItemsPending();
        }
    }
}