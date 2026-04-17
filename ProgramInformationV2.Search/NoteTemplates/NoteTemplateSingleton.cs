using ProgramInformationV2.Search.Models;

namespace ProgramInformationV2.Search.NoteTemplates {
    public class NoteTemplateSingleton(INoteTemplateLoad NoteTemplateLoader) {
        private INoteTemplateLoad _noteTemplateLoader = NoteTemplateLoader;
        private List<NoteTemplateStorageItem>? _noteTemplates { get; set; }

        public async Task<IEnumerable<Note>> MergeCredentialNotes(Credential credential) {
            var successful = await CheckNoteTemplates();
            if (successful && _noteTemplates != null) {
                var baseCredentialType = CredentialType.None;
                if (credential.CredentialType.IsUndergraduateDegree()) {
                    baseCredentialType = CredentialType.Base_Undergraduate;
                } else if (credential.CredentialType.IsGraduateDegree()) {
                    baseCredentialType = CredentialType.Base_Graduate;
                } else if (credential.CredentialType.IsCertificate()) {
                    baseCredentialType = CredentialType.Base_Certificate;
                }
                var noteTemplatesFromStorage = _noteTemplates.Where(nt => nt.CategoryType == NoteTemplateTypes.Credentials
                    && nt.Source == credential.Source
                    && (nt.CredentialType == credential.CredentialType || nt.CredentialType == CredentialType.None || nt.CredentialType == baseCredentialType)
                    && (nt.FormatType == credential.FormatType || nt.FormatType == FormatType.None)
                    && (credential.DepartmentList.Contains(nt.DepartmentType) || nt.DepartmentType == "")
                    && (credential.SkillList.Contains(nt.SkillType) || nt.SkillType == "")
                    && (credential.TagList.Contains(nt.TagType) || nt.TagType == "")).OrderBy(nt => nt.Order);

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
                return FilterBlankNotes(returnValue);
            }
            return FilterBlankNotes(credential.NoteList);
        }

        public async Task<IEnumerable<Note>> MergeCourseNotes(Course course) {
            var successful = await CheckNoteTemplates();
            if (successful && _noteTemplates != null) {
                var noteTemplatesFromStorage = GetNoteTemplatesPrivate(NoteTemplateTypes.Courses, course);
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
                return FilterBlankNotes(returnValue);
            }
            return FilterBlankNotes(course.NoteList);
        }

        public async Task<IEnumerable<Note>> MergeProgramNotes(Program program) {
            var successful = await CheckNoteTemplates();
            if (successful && _noteTemplates != null) {
                var noteTemplatesFromStorage = GetNoteTemplatesPrivate(NoteTemplateTypes.Programs, program);
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
                return FilterBlankNotes(returnValue);
            }
            return FilterBlankNotes(program.NoteList);
        }

        public bool ResetNoteTemplate() {
            _noteTemplates = null;
            return _noteTemplates == null;
        }

        public async Task<IEnumerable<NoteTemplateStorageItem>> GetNoteTemplates() {
            _noteTemplates = await _noteTemplateLoader.LoadNoteTemplates();
            return _noteTemplates;
        }

        private async Task<bool> CheckNoteTemplates() {
            _noteTemplates ??= await _noteTemplateLoader.LoadNoteTemplates();
            return _noteTemplates != null;
        }

        private IEnumerable<NoteTemplateStorageItem> GetNoteTemplatesPrivate(NoteTemplateTypes noteTemplateTypes, BaseTaggableObject baseObject) =>
            _noteTemplates == null ? [] : _noteTemplates.Where(nt => nt.CategoryType == noteTemplateTypes
                && nt.Source == baseObject.Source
                && (baseObject.DepartmentList.Contains(nt.DepartmentType) || nt.DepartmentType == "")
                && (baseObject.SkillList.Contains(nt.SkillType) || nt.SkillType == "")
                && (baseObject.TagList.Contains(nt.TagType) || nt.TagType == "")).OrderBy(nt => nt.Order);

        private static IEnumerable<Note> FilterBlankNotes(IEnumerable<Note> notes) => notes == null ? [] : notes.Where(n => n.Title != "" && (n.Description != "" || n.LinkText != ""));
    }
}
