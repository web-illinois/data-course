using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgramInformationV2.Data.DataModels {

    public class TagSource : BaseDataItem {

        private static readonly Dictionary<TagType, string> _translator = new() {
            { TagType.None, "" },
            { TagType.Department, "departmentList" },
            { TagType.Skill, "skillList" },
            { TagType.Tag, "tagList" },
            { TagType.Length, "lengthList" }
        };

        [NotMapped]
        public bool EnabledBySource { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        [NotMapped]
        public string OldTitle { get; set; } = "";

        public int Order { get; set; }

        public virtual Source? Source { get; set; }

        public int SourceId { get; set; }

        public TagType TagType { get; set; }

        [NotMapped]
        public string TagTypeSourceName => _translator[TagType];

        public string Title { get; set; } = "";
    }
}