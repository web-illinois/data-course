using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.CourseImport;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.PageList;

namespace ProgramInformationV2.Components.Pages.Course {

    public partial class Import {
        private string _sourceCode = "";
        private bool? _useCourses;
        private bool? _useSections;

        public string CourseNumber { get; set; } = "";
        public bool IncludeSections { get; set; } = false;

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        public bool Overwrite { get; set; } = false;
        public string Rubric { get; set; } = "";

        [Inject]
        protected CourseImportManager CourseImportManager { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected SourceHelper SourceHelper { get; set; } = default!;

        protected override async Task OnInitializedAsync() {
            Layout.SetSidebar(SidebarEnum.Courses, "Courses");
            _sourceCode = await Layout.CheckSource();
            _useCourses = await SourceHelper.DoesSourceUseItem(_sourceCode, Data.DataModels.CategoryType.Course);
            _useSections = await SourceHelper.DoesSourceUseItem(_sourceCode, Data.DataModels.CategoryType.Section);
            await base.OnInitializedAsync();
        }

        protected async Task SendImport() {
            if (string.IsNullOrWhiteSpace(Rubric) || string.IsNullOrWhiteSpace(CourseNumber)) {
                await Layout.AddMessage("Need to fill out a rubric and course number for the import to start");
            } else {
                var urlTemplate = await SourceHelper.GetUrlTemplateFromSource(_sourceCode);
                await Layout.AddMessage(await CourseImportManager.ImportCourse(Rubric, CourseNumber, _sourceCode, urlTemplate, IncludeSections, Overwrite));
            }
        }
    }
}