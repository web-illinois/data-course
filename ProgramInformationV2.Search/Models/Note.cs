namespace ProgramInformationV2.Search.Models {
    public class Note {

        public string Description { get; set; } = "";
        public string DescriptionHtml { get; set; } = "";

        public string Title { get; set; } = "";
        public string LinkUrl { get; set; } = "";
        public string LinkText { get; set; } = "";

        public void Merge(Note other) {
            if (other != null && other.Title == Title) {
                Description = other.Description;
                DescriptionHtml = other.DescriptionHtml;
                LinkText = other.LinkText;
                LinkUrl = other.LinkUrl;
            }
        }
    }
}
