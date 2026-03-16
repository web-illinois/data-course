namespace ProgramInformationV2.Search.Models {
    public class NoteTemplateStorageItem : Note {
        public int Id { get; set; }
        public string Source { get; set; } = "";
        public NoteTemplateTypes CategoryType { get; set; }
        public CredentialType CredentialType { get; set; }
        public FormatType FormatType { get; set; }
        public string DepartmentType { get; set; } = "";
        public string SkillType { get; set; } = "";

        public string TagType { get; set; } = "";


        public void Merge(NoteTemplateStorageItem other) {
            if (other != null && other.Title == Title) {
                Description = other.Description;
                LinkText = other.LinkText;
                LinkUrl = other.LinkUrl;
            }
        }
    }
}
