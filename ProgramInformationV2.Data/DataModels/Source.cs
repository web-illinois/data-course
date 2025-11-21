using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgramInformationV2.Data.DataModels {

    public class Source : BaseDataItem {
        public string ApiSecretCurrent { get; set; } = "";
        public DateTime? ApiSecretLastChanged { get; set; }
        public string ApiSecretPrevious { get; set; } = "";
        public string BaseUrl { get; set; } = "";

        public string Code { get; set; } = "";

        public string CreatedByEmail { get; set; } = "";

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        public bool IsTest { get; set; } = false;
        public bool RequestDeletion { get; set; }
        public string RequestDeletionByEmail { get; set; } = "";
        public string Title { get; set; } = "";
        public string UrlTemplate { get; set; } = "";
        public bool UseCourses { get; set; }
        public bool UseCredentials { get; set; }
        public bool UsePrograms { get; set; }
        public bool UseRequirementSets { get; set; }
        public bool UseSections { get; set; }
    }
}