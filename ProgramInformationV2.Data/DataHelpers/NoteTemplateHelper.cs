using Microsoft.EntityFrameworkCore;
using ProgramInformationV2.Data.DataContext;
using ProgramInformationV2.Data.DataModels;

namespace ProgramInformationV2.Data.DataHelpers {
    public class NoteTemplateHelper(ProgramRepository programRepository) {
        private readonly ProgramRepository _programRepository = programRepository;

        public async Task<List<NoteTemplate>> GetNoteTemplatesAsync(string source, CategoryType categoryType) {
            return await _programRepository.ReadAsync(c => c.NoteTemplates.Include(nt => nt.Source).Where(nt => nt.Source != null && nt.Source.Code == source && nt.CategoryType == categoryType).OrderBy(t => t.Order).ToList());
        }

        public async Task<NoteTemplate?> GetNoteTemplateByIdAsync(int id) {
            return await _programRepository.ReadAsync(c => c.NoteTemplates.FirstOrDefault(t => t.Id == id));
        }

        public async Task<int> SaveNoteTemplate(NoteTemplate noteTemplate) {
            noteTemplate.IsActive = true;
            noteTemplate.LastUpdated = DateTime.Now;
            noteTemplate.Title = noteTemplate.Title.Trim();
            return noteTemplate.Id == 0 ? await _programRepository.CreateAsync(noteTemplate) : await _programRepository.UpdateAsync(noteTemplate);
        }

        public async Task<int> DeleteNoteTemplate(NoteTemplate noteTemplate) => await _programRepository.DeleteAsync(noteTemplate);

        public async Task<List<NoteTemplate>> GetAllNoteTemplatesAsync() => await _programRepository.ReadAsync(c => c.NoteTemplates.Include(nt => nt.Source).Where(nt => nt.IsActive).ToList());
    }
}
