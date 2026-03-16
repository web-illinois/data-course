using Microsoft.AspNetCore.Components;
using ProgramInformationV2.Components.Layout;
using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Data.DataModels;
using ProgramInformationV2.Data.FieldList;
using ProgramInformationV2.Data.PageList;
using ProgramInformationV2.Search.Getters;
using ProgramInformationV2.Search.Models;
using ProgramInformationV2.Search.Setters;

namespace ProgramInformationV2.Components.Pages.Program {
    public partial class NoteList {
        [CascadingParameter]
        public SidebarLayout Layout { get; set; } = default!;

        public Search.Models.Program ProgramItem { get; set; } = new Search.Models.Program();

        [Inject]
        protected FieldManager FieldManager { get; set; } = default!;

        [Inject]
        protected NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        protected NoteTemplateHelper NoteTemplateHelper { get; set; } = default!;

        [Inject]
        protected ProgramGetter ProgramGetter { get; set; } = default!;

        [Inject]
        protected ProgramSetter ProgramSetter { get; set; } = default!;

        [Inject]
        protected SourceHelper SourceHelper { get; set; } = default!;
        public List<Note> Notes { get; set; } = default!;
        public List<string> NoteTemplateTitles { get; set; } = default!;
        public string Instructions { get; set; } = default!;
        public bool UseItem { get; set; }

        public async Task Save() {
            Layout.RemoveDirty();
            ProgramItem.NoteList = Notes;
            _ = await ProgramSetter.SetProgram(ProgramItem);
            await Layout.Log(CategoryType.Program, FieldType.NotesList, ProgramItem);
            await Layout.AddMessage("Program saved successfully.");
        }

        protected override async Task OnInitializedAsync() {
            var sourceCode = await Layout.CheckSource();
            var id = await Layout.GetCachedId();
            if (string.IsNullOrWhiteSpace(id)) {
                NavigationManager.NavigateTo("/");
            }
            ProgramItem = await ProgramGetter.GetProgram(id);
            var sidebar = await SourceHelper.DoesSourceUseItem(sourceCode, CategoryType.Credential) ? SidebarEnum.ProgramWithCredential : SidebarEnum.Program;
            Layout.SetSidebar(sidebar, ProgramItem.Title);
            Notes = ProgramItem.NoteList?.ToList() ?? [];
            NoteTemplateTitles = [.. (await NoteTemplateHelper.GetNoteTemplatesAsync(sourceCode, CategoryType.Program)).Select(nt => nt.Title).Distinct().OrderBy(s => s)];
            var fieldItems = await FieldManager.GetMergedFieldItems(sourceCode, new ProgramGroup(), FieldType.NotesList);
            Instructions = fieldItems.FirstOrDefault()?.Description ?? "";
            UseItem = fieldItems.FirstOrDefault()?.ShowItem ?? true;
            await base.OnInitializedAsync();
        }

        public async Task SetDirty() {
            Layout.SetDirty();
        }
    }
}
