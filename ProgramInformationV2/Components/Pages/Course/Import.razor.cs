using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.PageList;

namespace ProgramInformationV2.Components.Pages.Course {

    public partial class Import {
        private string _sourceCode = "";
        private bool? _useCourses;

        public string CourseNumber { get; set; } = "";
        public bool ImportTitleAndDescriptionOnly { get; set; } = false;
        public bool IncludeSections { get; set; } = false;

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        public string Rubric { get; set; } = "";
        public string UrlTemplate { get; set; } = "";

        [Inject]
        protected CourseImportHelper CourseImportHelper { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected SourceHelper SourceHelper { get; set; } = default!;

        protected override async Task OnInitializedAsync() {
            Layout.SetSidebar(SidebarEnum.Courses, "Courses");
            _sourceCode = await Layout.CheckSource();
            _useCourses = await SourceHelper.DoesSourceUseItem(_sourceCode, Data.DataModels.CategoryType.Course);
            await base.OnInitializedAsync();
        }

        protected async Task SendImport() {
            _ = await CourseImportHelper.Load(Rubric, CourseNumber, UrlTemplate, ImportTitleAndDescriptionOnly, IncludeSections, _sourceCode);
            await Layout.AddMessage(string.IsNullOrWhiteSpace(CourseNumber) ? $"Course import started for all '{Rubric}'" : $"Course import started for {Rubric} {CourseNumber}");
        }
    }
}