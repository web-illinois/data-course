using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.FieldList;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Models;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Components.Pages.Course {
    public partial class NotesList {
        public ProgramInformationV2.Search.Models.Course CourseItem { get; set; } = default!;

        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        public List<Note> Notes { get; set; } = default!;
        public List<string> NoteTemplateTitles { get; set; } = default!;
        public string QuickLinkUrl { get; set; } = "";

        public string Instructions { get; set; } = default!;
        public bool UseItem { get; set; }
        [Inject]
        protected NoteTemplateHelper NoteTemplateHelper { get; set; } = default!;
        [Inject]
        protected FieldManager FieldManager { get; set; } = default!;
        [Inject]
        protected CourseGetter CourseGetter { get; set; } = default!;

        [Inject]
        protected CourseSetter CourseSetter { get; set; } = default!;


        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected SourceHelper SourceHelper { get; set; } = default!;

        public async Task Save() {
            Layout.RemoveDirty();

            await Layout.Log(CategoryType.Course, FieldType.NotesList, CourseItem);
            _ = await CourseSetter.SetCourse(CourseItem);
            await Layout.AddMessage("Course saved successfully.");
        }

        protected override async Task OnInitializedAsync() {
            var sourceCode = await Layout.CheckSource();
            var id = await Layout.GetCachedId();
            if (string.IsNullOrWhiteSpace(id)) {
                NavigationManager.NavigateTo("/");
            }
            CourseItem = await CourseGetter.GetCourse(id);
            var sidebar = await SourceHelper.DoesSourceUseItem(sourceCode, CategoryType.Section) ? SidebarEnum.CourseWithSection : SidebarEnum.Course;
            Layout.SetSidebar(sidebar, CourseItem.Title);
            QuickLinkUrl = await Layout.GetCachedQuickLink();
            Notes = CourseItem.NoteList?.ToList() ?? [];
            NoteTemplateTitles = [.. (await NoteTemplateHelper.GetNoteTemplatesAsync(sourceCode, CategoryType.Course)).Select(nt => nt.Title).Distinct().OrderBy(s => s)];
            var fieldItems = await FieldManager.GetMergedFieldItems(sourceCode, new CourseGroup(), FieldType.NotesList);
            Instructions = fieldItems.FirstOrDefault()?.Description ?? "";
            UseItem = fieldItems.FirstOrDefault()?.ShowItem ?? true;
        }
        public async Task SetDirty() {
            Layout.SetDirty();
        }
    }
}
