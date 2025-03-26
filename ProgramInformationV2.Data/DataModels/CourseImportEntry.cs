using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgramInformationV2.Data.DataModels {

    public class CourseImportEntry : BaseDataItem {
        public string CourseNumber { get; set; } = "";

        public DateTime? DateImported { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        public bool IncludeSections { get; set; } = false;
        public bool IncludeTitleAndDescriptionOnly { get; set; } = false;
        public string Log { get; set; } = "";
        public string Rubric { get; set; } = "";
        public virtual Source? Source { get; set; }
        public int SourceId { get; set; }

        public string UrlPattern { get; set; } = "";
    }
}