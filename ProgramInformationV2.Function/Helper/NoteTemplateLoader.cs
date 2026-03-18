using ProgramInformationV2.Data.DataHelpers;
using ProgramInformationV2.Search.Models;
using ProgramInformationV2.Search.NoteTemplates;

namespace ProgramInformationV2.Function.Helper {
    public class NoteTemplateLoader(NoteTemplateHelper noteTemplateHelper) : INoteTemplateLoad, INoteTemplateConvert {
        private readonly NoteTemplateHelper _noteTemplateHelper = noteTemplateHelper;
        public async Task<List<NoteTemplateStorageItem>> LoadNoteTemplates() => [.. (await _noteTemplateHelper.GetAllNoteTemplatesAsync()).Select(nt => nt.ConvertToStorageItem())];

        public string ConvertToHtml(string s) => NoteTemplateHelper.ConvertMarkdownToHtml(s);
    }
}
