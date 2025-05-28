using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgramInformationV2.Data.DataModels {

    public class FacultyName : BaseDataItem {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        public string Name { get; set; } = "";

        public string NetId { get; set; } = "";

        public string ProfileUrl { get; set; } = "";

        public virtual Source? Source { get; set; }
        public int SourceId { get; set; }
    }
}