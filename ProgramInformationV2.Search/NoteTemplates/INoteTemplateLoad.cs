using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Search.NoteTemplates {
    public interface INoteTemplateLoad {
        Task<List<NoteTemplateStorageItem>> LoadNoteTemplates();
    }
}
