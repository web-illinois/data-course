using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Search.NoteTemplates {
    public class NoteTemplateSingleton(INoteTemplateLoad NoteTemplateLoader) {
        private INoteTemplateLoad _noteTemplateLoader = NoteTemplateLoader;
        private List<NoteTemplateStorageItem>? _noteTemplates { get; set; }

        public async Task<IEnumerable<Note>> MergeCredentialNotes(Credential credential) {
            var successful = await CheckNoteTemplates();
            if (successful && _noteTemplates != null) {
                var noteTemplatesFromStorage = _noteTemplates.Where(nt => nt.CategoryType == NoteTemplateTypes.Credentials
                    && nt.Source == credential.Source
                    && (nt.CredentialType == credential.CredentialType || nt.CredentialType == CredentialType.None)
                    && (nt.FormatType == credential.FormatType || nt.FormatType == FormatType.None)
                    && (credential.DepartmentList.Contains(nt.DepartmentType) || nt.DepartmentType == "")
                    && (credential.SkillList.Contains(nt.SkillType) || nt.SkillType == "")
                    && (credential.TagList.Contains(nt.TagType) || nt.TagType == ""));
                foreach (var noteTemplate in noteTemplatesFromStorage.GroupBy(nt => nt.Title)) {
                    if (noteTemplate.Count() > 1) {
                        noteTemplate.First().Merge(noteTemplate.Last());
                        foreach (var extraNoteTemplate in noteTemplate.Skip(1)) {
                            extraNoteTemplate.Title = "";
                        }
                    }
                }
                var returnValue = noteTemplatesFromStorage.Where(nt => nt.Title != "").Select(nt => (Note)nt).ToList();
                foreach (var originalNote in credential.NoteList ?? []) {
                    if (returnValue.Select(nt => nt.Title).Contains(originalNote.Title)) {
                        returnValue.First(nt => nt.Title == originalNote.Title).Merge(originalNote);
                    } else {
                        returnValue.Add(originalNote);
                    }
                }
                return returnValue;
            }
            return credential.NoteList ?? [];
        }

        public async Task<IEnumerable<Note>> MergeCourseNotes(Course course) {
            var successful = await CheckNoteTemplates();
            if (successful && _noteTemplates != null) {
                var noteTemplatesFromStorage = _noteTemplates.Where(nt => nt.CategoryType == NoteTemplateTypes.Courses
                    && nt.Source == course.Source
                    && (course.DepartmentList.Contains(nt.DepartmentType) || nt.DepartmentType == "")
                    && (course.SkillList.Contains(nt.SkillType) || nt.SkillType == "")
                    && (course.TagList.Contains(nt.TagType) || nt.TagType == ""));
                foreach (var noteTemplate in noteTemplatesFromStorage.GroupBy(nt => nt.Title)) {
                    if (noteTemplate.Count() > 1) {
                        noteTemplate.First().Merge(noteTemplate.Last());
                        foreach (var extraNoteTemplate in noteTemplate.Skip(1)) {
                            extraNoteTemplate.Title = "";
                        }
                    }
                }
                var returnValue = noteTemplatesFromStorage.Where(nt => nt.Title != "").Select(nt => (Note)nt).ToList();
                foreach (var originalNote in course.NoteList ?? []) {
                    if (returnValue.Select(nt => nt.Title).Contains(originalNote.Title)) {
                        returnValue.First(nt => nt.Title == originalNote.Title).Merge(originalNote);
                    } else {
                        returnValue.Add(originalNote);
                    }
                }
                return returnValue;
            }
            return course.NoteList ?? [];
        }

        public async Task<IEnumerable<Note>> MergeProgramNotes(Program program) {
            var successful = await CheckNoteTemplates();
            if (successful && _noteTemplates != null) {
                var noteTemplatesFromStorage = _noteTemplates.Where(nt => nt.CategoryType == NoteTemplateTypes.Programs
                    && nt.Source == program.Source
                    && (program.DepartmentList.Contains(nt.DepartmentType) || nt.DepartmentType == "")
                    && (program.SkillList.Contains(nt.SkillType) || nt.SkillType == "")
                    && (program.TagList.Contains(nt.TagType) || nt.TagType == ""));
                foreach (var noteTemplate in noteTemplatesFromStorage.GroupBy(nt => nt.Title)) {
                    if (noteTemplate.Count() > 1) {
                        noteTemplate.First().Merge(noteTemplate.Last());
                        foreach (var extraNoteTemplate in noteTemplate.Skip(1)) {
                            extraNoteTemplate.Title = "";
                        }
                    }
                }
                var returnValue = noteTemplatesFromStorage.Where(nt => nt.Title != "").Select(nt => (Note)nt).ToList();
                foreach (var originalNote in program.NoteList ?? []) {
                    if (returnValue.Select(nt => nt.Title).Contains(originalNote.Title)) {
                        returnValue.First(nt => nt.Title == originalNote.Title).Merge(originalNote);
                    } else {
                        returnValue.Add(originalNote);
                    }
                }
                return returnValue;
            }
            return program.NoteList ?? [];
        }

        public bool ResetNoteTemplate() {
            _noteTemplates = null;
            return _noteTemplates == null;
        }

        private async Task<bool> CheckNoteTemplates() {
            _noteTemplates ??= await _noteTemplateLoader.LoadNoteTemplates();
            return _noteTemplates != null;
        }
    }
}
