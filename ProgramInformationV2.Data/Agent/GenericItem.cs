namespace ProgramInformationV2.Data.Agent {
    public class GenericItem {
        public string Title { get; set; } = "";
        public string Url { get; set; } = "";
        public string Description { get; set; } = "";
        public string Details { get; set; } = "";
        public string Summary { get; set; } = "";
        public List<string> SkillList { get; set; } = new List<string>();
    }
}
