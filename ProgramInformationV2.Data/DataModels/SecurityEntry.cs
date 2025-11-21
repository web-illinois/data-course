using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProgramInformationV2.Data.DataModels {

    public class SecurityEntry : BaseDataItem {

        public SecurityEntry() {
        }

        public SecurityEntry(string netId, int sourceId) {
            Email = TransformName(netId);
            IsActive = true;
            IsOwner = false;
            IsPublic = false;
            SourceId = sourceId;
        }

        public string DepartmentTag { get; set; } = "";
        public string Email { get; set; } = "";

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        public bool IsFullAdmin { get; set; }
        public bool IsOwner { get; set; }
        public bool IsPublic { get; set; }
        public bool IsRestricted { get; set; }

        public string Notes => IsActive && !IsRestricted ? "" : !IsActive ? " (not active)" : " (restricted)";
        public string RestrictedIds { get; set; } = "";
        public virtual Source? Source { get; set; }
        public int? SourceId { get; set; }

        public static string TransformName(string netid) => (netid.EndsWith("@illinois.edu") ? netid : netid + "@illinois.edu").ToLowerInvariant();
    }
}