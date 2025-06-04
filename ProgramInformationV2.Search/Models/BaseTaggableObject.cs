using OpenSearch.Client;

namespace ProgramInformationV2.Search.Models {

    public abstract class BaseTaggableObject : BasePublicObject {

        [Keyword]
        public IEnumerable<string> DepartmentList { get; set; } = default!;

        public IEnumerable<Link> LinkList { get; set; } = default!;

        [Keyword]
        public IEnumerable<string> SkillList { get; set; } = default!;

        [Keyword]
        public IEnumerable<string> TagList { get; set; } = default!;

        public static string ProcessTagName(string tag) => tag.Replace("\"", "");

        internal void ProcessLists() {
            TagList = TagList == null ? [] : TagList.Select(ProcessTagName).ToList();
            DepartmentList = DepartmentList == null ? [] : DepartmentList.Select(ProcessTagName).ToList();
            SkillList = SkillList == null ? [] : SkillList.Select(ProcessTagName).ToList();
        }
    }
}