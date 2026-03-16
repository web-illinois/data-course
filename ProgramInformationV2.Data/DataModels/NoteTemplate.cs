using ProgramInformationV2.Search.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgramInformationV2.Data.DataModels {
    public class NoteTemplate : BaseDataItem {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }
        public CategoryType CategoryType { get; set; }
        public CredentialType CredentialTypeCriteria { get; set; }
        public string DepartmentCriteria { get; set; } = "";
        public string Description { get; set; } = "";
        public FormatType FormatTypeCriteria { get; set; }
        public string LinkUrl { get; set; } = "";
        public string LinkText { get; set; } = "";
        public int Order { get; set; }
        public virtual Source? Source { get; set; }
        public int? SourceId { get; set; }
        public string SkillCriteria { get; set; } = "";
        public string TagCriteria { get; set; } = "";
        public string Title { get; set; } = "";

        public string TitleInternal() {
            var criteria = DepartmentCriteria + SkillCriteria + TagCriteria + (CredentialTypeCriteria == CredentialType.None ? "" : CredentialTypeCriteria) + (FormatTypeCriteria == FormatType.None ? "" : FormatTypeCriteria);
            return $"{Title} ({(string.IsNullOrWhiteSpace(criteria) ? "all" : criteria)})";
        }

        public NoteTemplateStorageItem ConvertToStorageItem() => new() {
            CredentialType = CredentialTypeCriteria,
            CategoryType = (NoteTemplateTypes)(int)CategoryType,
            DepartmentType = DepartmentCriteria,
            Description = Description,
            FormatType = FormatTypeCriteria,
            LinkText = LinkText,
            LinkUrl = LinkUrl,
            Source = Source != null ? Source.Code : "",
            SkillType = SkillCriteria,
            TagType = TagCriteria,
            Title = Title,
            Id = Id
        };
    }
}
